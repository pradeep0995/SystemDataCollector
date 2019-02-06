using System;
using System.Configuration;

namespace SystemErrorsDataCollector.Helper
{
    public class MappingElement : ConfigurationElement
    {

        [ConfigurationProperty("Source", IsRequired = true)]
        public String Source
        {
            get
            {
                return (String)this["Source"];
            }
            set { this["Source"] = value; }
        }
        [ConfigurationProperty("EventID", IsRequired = true)]
        public string EventID
        {
            get
            {
                return (String)this["EventID"];
            }
            set { this["EventID"] = value; }
        }
    }

    [ConfigurationCollection(typeof(MappingElement))]
    public class MappingElementCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new MappingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MappingElement)element).EventID;
        }

    }

    public class MappingSection : ConfigurationSection
    {
        [ConfigurationProperty("ExclusionEvents", IsDefaultCollection = true)]
        public MappingElementCollection ExclusionEvents
        {
            get { return (MappingElementCollection)this["ExclusionEvents"]; }
            set { this["ExclusionEvents"] = value; }
        }
    }
}

