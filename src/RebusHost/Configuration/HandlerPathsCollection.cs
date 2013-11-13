using System.Configuration;

namespace RebusHost.Configuration
{
    public class HandlerPathsCollection : ConfigurationElementCollection
    {
        public HandlerPathsCollection()
        {
            // Add one handlerPath to the collection.  This is 
            // not necessary; could leave the collection  
            // empty until items are added to it outside 
            // the constructor.
            var handlerPath =
                (HandlerPathConfigElement)CreateNewElement();
            Add(handlerPath);
        }

        public override
            ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return

                    ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override
            ConfigurationElement CreateNewElement()
        {
            return new HandlerPathConfigElement();
        }


        protected override
            ConfigurationElement CreateNewElement(
            string elementName)
        {
            return new HandlerPathConfigElement(elementName);
        }


        protected override object
            GetElementKey(ConfigurationElement element)
        {
            return ((HandlerPathConfigElement)element).Name;
        }


        public new string AddElementName
        {
            get
            { return base.AddElementName; }

            set
            { base.AddElementName = value; }

        }

        public new string ClearElementName
        {
            get
            { return base.ClearElementName; }

            set
            { base.ClearElementName = value; }

        }

        public new string RemoveElementName
        {
            get
            { return base.RemoveElementName; }
        }

        public new int Count
        {
            get { return base.Count; }
        }


        public HandlerPathConfigElement this[int index]
        {
            get
            {
                return (HandlerPathConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public HandlerPathConfigElement this[string Name]
        {
            get
            {
                return (HandlerPathConfigElement)BaseGet(Name);
            }
        }

        public int IndexOf(HandlerPathConfigElement handlerPath)
        {
            return BaseIndexOf(handlerPath);
        }

        public void Add(HandlerPathConfigElement handlerPath)
        {
            BaseAdd(handlerPath);
            // Add custom code here.
        }

        protected override void
            BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
            // Add custom code here.
        }

        public void Remove(HandlerPathConfigElement handlerPath)
        {
            if (BaseIndexOf(handlerPath) >= 0)
                BaseRemove(handlerPath.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
            // Add custom code here.
        }
    }
}