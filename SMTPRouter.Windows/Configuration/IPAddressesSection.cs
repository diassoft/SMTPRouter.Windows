using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPRouter.Windows.Configuration
{
    /// <summary>
    /// Represents a <see cref="ConfigurationSection"/> where multiple IP Addresses can be setup
    /// </summary>
    /// <remarks>The syntax must be followed precisely otherwise the system will not accept the routing rule</remarks>
    /// <example>
    /// This is how the App.Config must be setup in order to be processed by this class:
    /// <code>
    /// <![CDATA[
    ///  <IPAddressesConfiguration>
    ///    <IPAddresses>
    ///      <add address="10.0.0.10" />
    ///      <add address="10.0.0.10" />
    ///    </IPAddresses>
    ///  </IPAddressesConfiguration>
    /// ]]>
    /// </code>
    /// </example>
    public sealed class IPAddressesSection : ConfigurationSection
    {
        /// <summary>
        /// A collection of routing rules to be set in the configuration file
        /// </summary>
        [ConfigurationProperty(nameof(IPAddresses), IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(IPAddressCollection),
                                 AddItemName = "add",
                                 ClearItemsName = "clear",
                                 RemoveItemName = "remove")]
        public IPAddressCollection IPAddresses
        {
            get
            {
                IPAddressCollection ipAddressesCollection = (IPAddressCollection)base[nameof(IPAddresses)];

                return ipAddressesCollection;
            }

            set
            {
                IPAddressCollection ipAddressesCollection = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSection"/>
        /// </summary>
        /// <remarks>The system automatically adds one empty element to the collection</remarks>
        public IPAddressesSection()
        {
            IPAddresses.Add(new IPAddressElement());
        }

    }
}
