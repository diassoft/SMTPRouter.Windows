using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPRouter.Windows.Configuration
{
    /// <summary>
    /// Represents a Collection of IP Addresses to be used on App.Config files
    /// </summary>
    [ConfigurationCollection(typeof(IPAddressElement))]
    public sealed class IPAddressCollection: ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="IPAddressElement"/>
        /// </summary>
        /// <returns>A new <see cref="IPAddressElement"/></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new IPAddressElement();
        }

        /// <summary>
        /// Returns the Key of the <see cref="IPAddressElement"/>
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>An <see cref="int"/> containing the key of the <see cref="IPAddressElement"/></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IPAddressElement)(element)).Address;
        }

        /// <summary>
        /// Returns the <see cref="IPAddressElement"/> index based
        /// </summary>
        /// <param name="idx">The index to search for</param>
        /// <returns>The <see cref="IPAddressElement"/> found according to the <paramref name="idx"/></returns>
        public IPAddressElement this[int idx]
        {
            get { return (IPAddressElement)BaseGet(idx); }
            set
            {
                if (BaseGet(idx) != null)
                {
                    BaseRemoveAt(idx);
                }
                BaseAdd(idx, value);
            }
        }

        /// <summary>
        /// Retrieves the Index of an element in the collection
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>An intenger with the index of the given element</returns>
        public int IndexOf(IPAddressElement element)
        {
            return BaseIndexOf(element);
        }

        /// <summary>
        /// Adds an element to the collection
        /// </summary>
        /// <param name="element">The element to be added</param>
        public void Add(IPAddressElement element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// Adds an element to the collection
        /// </summary>
        /// <param name="element">The element to be added</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        /// <summary>
        /// Removes an element from the collection
        /// </summary>
        /// <param name="element">The element to be removed</param>
        public void Remove(IPAddressElement element)
        {
            int idx = BaseIndexOf(element);
            if (idx > 0)
            {
                BaseRemoveAt(idx);
            }
        }

        /// <summary>
        /// Removes an element from the collection based on its index
        /// </summary>
        /// <param name="index">The index of the element</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// Clears the entire collection
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }

    }
}
