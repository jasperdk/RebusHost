using System.Configuration;

namespace RebusHost.Configuration
{
    public class HandlerPathConfigElement : ConfigurationElement
    {
        public HandlerPathConfigElement(string newName,
            string newPath)
        {
            Name = newName;
            Path = newPath;
        }

        public HandlerPathConfigElement()
        {
        }

        public HandlerPathConfigElement(string elementName)
        {
            Name = elementName;
        }

        [ConfigurationProperty("name",
            IsRequired = true,
            IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("path",
            IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
    }
}