using System.Configuration;

namespace RebusHost.Configuration
{
    public class RebusHostConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("serviceName",
           DefaultValue = "RebusHost",
           IsKey = false)]
        [StringValidator(InvalidCharacters =
            " ~!@#$%^&*()[]{}/;'\"|\\",
            MinLength = 1, MaxLength = 60)]
        public string ServiceName
        {

            get
            {
                return (string)this["serviceName"];
            }
            set
            {
                this["serviceName"] = value;
            }

        }

        [ConfigurationProperty("displayName",
          DefaultValue = "RebusHost",
          IsKey = false)]
        [StringValidator(InvalidCharacters =
            " ~!@#$%^&*()[]{}/;'\"|\\",
            MinLength = 1, MaxLength = 60)]
        public string DisplayName
        {

            get
            {
                return (string)this["displayName"];
            }
            set
            {
                this["displayName"] = value;
            }

        }


        [ConfigurationProperty("handlerPaths",
            IsDefaultCollection = false)]
        public HandlerPathsCollection HandlerPaths
        {
            get
            {
                var handlerPathsCollection =
                    (HandlerPathsCollection)base["handlerPaths"];
                return handlerPathsCollection;
            }
        }
    }
}