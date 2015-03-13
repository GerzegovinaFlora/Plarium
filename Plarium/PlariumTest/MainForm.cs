namespace Plarium
{
    using Plarium.DataAccess;
    using Plarium.Resources;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        private string folderForWork;
        private string fieldForWork;
        private Threads threadProvider;

        public string FolderForWork
        {
            get
            {
                return this.folderForWork;
            }
            set
            {
                if (this.folderForWork != value)
                {
                    this.WorkFolderPath.Text = this.folderForWork = value;
                    this.SetStartEnabled();
                }
            }
        }

        public string FieldForWork
        {
            get
            {
                return this.fieldForWork;
            }
            set
            {
                if (this.fieldForWork != value)
                {
                    this.WorkFolderPath.Text = this.fieldForWork = value;
                    this.SetStartEnabled();
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();
            this.threadProvider = new Threads(this.ResultTreeView);
        }

        private void OnFolderForWorkButtonClick(object sender, EventArgs e)
        {
            this.FolderForWork = this.SelectFolder();
            this.textBox1.Text = FolderForWork;
            if (string.IsNullOrEmpty(this.FolderForWork))
            {
                MessageBox.Show(ProjectResource.WorkFolderInfo);
            }
        }

        private void SetStartEnabled()
        {
            if (!string.IsNullOrEmpty(this.FieldForWork) || !string.IsNullOrEmpty(this.FolderForWork))
            {
                this.StartButton.Enabled = true;
            }
        }

        private void OnStartButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.FolderForWork))
            {
                this.threadProvider.StartThreads(this.FolderForWork);
            }
        }

        private string SelectFolder()
        {
            var result = string.Empty;
            var folderDialog = new FolderBrowserDialog();
            var dialogResult = folderDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(folderDialog.SelectedPath))
            {
                result = folderDialog.SelectedPath;
            }
            return result;
        }
    }
}