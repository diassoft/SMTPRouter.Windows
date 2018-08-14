using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPRouter.Windows.Configuration
{
    /// <summary>
    /// Represents the IP Addresses configuration to be used inside App.Config files
    /// </summary>
    /// <remarks>IP Addresses must be setup to either accept or reject incoming messages</remarks>
    /// <example>
    /// This is an example of a valid IPAddressElement:
    /// <code>
    /// <![CDATA[
    ///   <add address="10.0.0.10" />
    /// ]]>
    /// </code>
    /// </example>
    public sealed class IPAddressElement : ConfigurationElement
    {
        /// <summary>
        /// The IP Address
        /// </summary>
        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get { return (string)this["address"]; }
        }

        /// <summary>
        /// Returns a string with the information regarding the IP Address
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Address........: {this.Address}";
        }

    }
}
