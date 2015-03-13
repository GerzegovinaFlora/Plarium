namespace Plarium.DataAccess
{
    using Plarium.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Principal;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    public class Threads
    {
        private TreeView resultTreeView;
        private Thread collectInformation;

        private Thread xmlSaver;
        private EventWaitHandle xmlSaverWaitHandler;
        private object xmlSaverLocker;
        private object fileAccessLocker;
        private Queue<XML> xmlItems;

        private Thread treeCreator;
        private EventWaitHandle treeCreatorWaitHandler;
        private object treeCreatorLocker;
        private Queue<Tree> nodes;

        private string folderForWork;


        public Threads(TreeView sourceTreeView)
        {
            this.resultTreeView = sourceTreeView;
            this.xmlSaverWaitHandler = new AutoResetEvent(false);
            this.xmlSaverLocker = new object();
            this.fileAccessLocker = new object();
            this.xmlItems = new Queue<XML>();
            this.treeCreatorWaitHandler = new AutoResetEvent(false);
            this.treeCreatorLocker = new object();
            this.nodes = new Queue<Tree>();
        }

        public void StartThreads(string folderForWork)
        {
            this.folderForWork = folderForWork;

            this.resultTreeView.Nodes.Clear();
            this.xmlItems.Clear();
            this.nodes.Clear();

            this.collectInformation = new Thread(this.CollectInfo);
            this.collectInformation.IsBackground = true;

            this.xmlSaver = new Thread(this.SaveToXml);
            this.xmlSaver.IsBackground = true;

            this.treeCreator = new Thread(this.AddNodeToTree);
            this.treeCreator.IsBackground = true;

            this.collectInformation.Start(this.folderForWork);
            this.xmlSaver.Start();
            this.treeCreator.Start(this.resultTreeView.Nodes.Add(this.folderForWork));
        }

        // Определяем каталоги
        private void CollectInfo(object folder)
        {
            var folderPath = (string)folder;
            if (!string.IsNullOrEmpty(folderPath))
            {
                // добавляем все файлы с папки в очередь
                var fileNames = Directory.GetFiles(folderPath);
                foreach (string fileName in fileNames)
                {
                    var file = this.GetFileInfo(fileName);
                    this.AddElementToQueues(file);
                }

                //добавляем все папки очередь и рекурсивно вызываем метод для каждой вложенной папки
                var directoryNames = Directory.GetDirectories(folderPath);
                foreach (string directoryName in directoryNames)
                {
                    var directory = GetDirectoryInfo(directoryName);
                    this.AddElementToQueues(directory, true);
                    this.CollectInfo(directoryName);
                }

                //добавляем пустой элемент. Все файлы и папки добавляются в очередь
                lock (this.treeCreatorLocker)
                {
                    var emptyNode = new Tree
                    {
                        Name = string.Empty
                    };

                    this.nodes.Enqueue(emptyNode);
                }
                this.treeCreatorWaitHandler.Set();
            }
        }
      
        // Получаем данные о файлах
        private XML GetFileInfo(string fileName)
        {
            var attributes = File.GetAttributes(fileName);
            var fileInfo = new FileInfo(fileName);

            var xmlItem = new XML()
            {
                Name = fileInfo.Name,
                DateCreated = File.GetCreationTime(fileName),
                DateLastAccessed = File.GetLastAccessTime(fileName),
                DateModified = File.GetLastWriteTime(fileName),
                Attributes = attributes.ToString(),
                IsHidden = (attributes & FileAttributes.Hidden) == FileAttributes.Hidden,
                IsCanWrite = fileInfo.IsReadOnly,
                Size = fileInfo.Length,
                Owner = File.GetAccessControl(fileName).GetOwner(typeof(SecurityIdentifier)).Translate(typeof(NTAccount)).Value
            };

            return xmlItem;
        }

        private XML GetDirectoryInfo(string directoryName)
        {
            var directoryInfo = new DirectoryInfo(directoryName);

            var xmlItem = new XML()
           {
               DateCreated = Directory.GetCreationTime(directoryName),
               DateLastAccessed = Directory.GetLastAccessTime(directoryName),
               DateModified = Directory.GetLastWriteTime(directoryName),
               Attributes = directoryInfo.Attributes.ToString(),
               Name = directoryInfo.Name,
               IsHidden = (directoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden,
               Owner = File.GetAccessControl(directoryName).GetOwner(typeof(SecurityIdentifier)).Translate(typeof(NTAccount)).Value,
           };

            return xmlItem;

        }

        private void AddElementToQueues(XML xmlItem, bool isDirectory = false)
        {
            lock (this.xmlSaverLocker)
            {
                this.xmlItems.Enqueue(xmlItem);
            }
            this.xmlSaverWaitHandler.Set();

            lock (this.treeCreatorLocker)
            {
                var treeNode = new Tree
                {
                    Name = xmlItem.Name,
                    IsDirectory = isDirectory
                };

                this.nodes.Enqueue(treeNode);
            }
            this.treeCreatorWaitHandler.Set();
        }

        private void SaveToXml()
        {
            while (true)
            {
                XML xmlItem = null;

                //проверяем очередь на наличие элементов
                lock (this.xmlSaverLocker)
                {
                    if (this.xmlItems.Count > 0)
                    {
                        xmlItem = this.xmlItems.Dequeue();
                        if (xmlItem == null)
                        {
                            return;
                        }
                    }
                }

                if (xmlItem != null)
                {
                    this.AddElementToXml(xmlItem);
                }
                else
                {
                    //ожидаем новые элементы в очередь
                    this.xmlSaverWaitHandler.WaitOne();
                }
            }
        }

        private void AddElementToXml(XML xmlItem)
        {
            var serializer = new XmlSerializer(typeof(List<XML>));
            var xmlSettings = new XmlWriterSettings();
            List<XML> xmlItems = null;

            lock (this.fileAccessLocker)
            {
                //достаем коллекцию из файла, если файла нет - создаем его и инициализируем пустую коллекцию
                Stream file = null;
                try
                {
                    file = File.OpenRead(folderForWork + ".xml");
                    try
                    {
                        xmlItems = (List<XML>)serializer.Deserialize(file);
                    }
                    catch (InvalidOperationException)
                    {
                        xmlItems = new List<XML>();
                    }
                }
                catch (FileNotFoundException)
                {
                    file = File.Create(folderForWork+".xml");
                    xmlItems = new List<XML>();
                }
                finally
                {
                    file.Close();
                }
            }

            //добавляем новый элемент в коллекцию
            xmlItems.Add(xmlItem);

            //записываем коллекцию в файл
            lock (this.fileAccessLocker)
            {
                using (var file = File.OpenWrite(folderForWork + ".xml"))
                {
                    serializer.Serialize(file, xmlItems);
                }
            }
        }

        private void AddNodeToTree(object parametr)
        {
            while (true)
            {
                Tree treeItem = null;

                lock (this.treeCreatorLocker)
                {
                    if (this.nodes.Count > 0)
                    {
                        treeItem = this.nodes.Dequeue();
                        if (treeItem == null)
                        {
                            return;
                        }
                    }
                }

                if (treeItem != null)
                {
                    if (!string.IsNullOrEmpty(treeItem.Name))
                    {
                        var node = parametr as TreeNode;
                        //если элемент директория
                        //создаем новый узел
                        if (treeItem.IsDirectory)
                        {
                            var newNode = new TreeNode(treeItem.Name);
                            this.resultTreeView.BeginInvoke(new Action(() =>
                            {
                                node.Nodes.Add(newNode);
                            }));
                            this.AddNodeToTree(newNode);
                        }
                        //добавляем элемент
                        else
                        {
                            this.resultTreeView.BeginInvoke(new Action(() =>
                            {
                                node.Nodes.Add(treeItem.Name);
                            }));
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    this.treeCreatorWaitHandler.WaitOne();
                }
            }
        }
    }

    public class Tree
    {
        public string Name { get; set; }

        public bool IsDirectory { get; set; }
    }
}