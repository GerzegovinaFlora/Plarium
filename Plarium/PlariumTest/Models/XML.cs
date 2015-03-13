namespace Plarium.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlRootAttribute("XmlItem")]
    public class XML
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public DateTime DateCreated { get; set; }

        [XmlElement]
        public DateTime DateModified { get; set; }

        [XmlElement]
        public DateTime DateLastAccessed { get; set; }

        [XmlElement]
        public string Attributes { get; set; }

        [XmlElement]
        public bool IsCanWrite { get; set; }

        [XmlElement]
        public bool IsCanDelete { get; set; }

        [XmlElement]
        public bool IsHidden { get; set; }

        [XmlElement]
        public long Size { get; set; }

        [XmlElement]
        public string Owner { get; set; }
    }
}
