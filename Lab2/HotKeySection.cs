using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Lab2
{
    public class HotKeySection : ConfigurationSection
    {
        [ConfigurationProperty("hotKeys")]
        public HotKeysCollection HotKeys
        {
            get { return (HotKeysCollection)base["hotKeys"]; }
            set { base["hotKeys"] = value; }
        }
    }

    public class HotKeyElement: ConfigurationElement
    {
        [ConfigurationProperty("command")]
        public string Command
        {
            get { return (string) base["command"]; }
            set { base["command"] = value; }
        }

        [ConfigurationProperty("gesture")]
        public string Gesture
        {
            get { return (string) base["gesture"]; }
            set { base["gesture"] = value; }
        }
  
    }
    [ConfigurationCollection(typeof(HotKeyElement), AddItemName ="hotKey")]
    public class HotKeysCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HotKeyElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HotKeyElement)element).Command;
        }
    }
}
