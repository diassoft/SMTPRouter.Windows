# SmtpRouter.Windows Components - v1.0.0.4
[![nuget](https://img.shields.io/nuget/v/SmtpRouter.Windows.svg)](https://www.nuget.org/packages/SmtpRouter.Windows/) 
![GitHub release](https://img.shields.io/github/release/diassoft/SmtpRouter.Windows.svg)
![NuGet](https://img.shields.io/nuget/dt/SmtpRouter.Windows.svg)
![license](https://img.shields.io/github/license/diassoft/SMTPRouter.Windows.svg)

The SMTP Router is a intermediate SMTP server useful to intercept messages and route it to another smtp.

Refer to the [SMTP Router](https://github.com/diassoft/SMTPRouter) repository to get more information about the project.

This repository contains additional components for the SMTPRouter project. Those components are only to be used on .NET Framework Implementations, therefore, they are Windows only. Check the [In this repository](#in-this-repository) section to see what is distributed with this component.

>If you wish to contact the creator of this component, you can make it thru the [Nuget.org](https://www.nuget.org/packages/SmtpRouter.Windows/) page or by email [olavodias@gmail.com](mailto:olavodias@gmail.com).

## In this repository

* [SMTP Router Documentation](https://diassoft.github.io/SMTPRouter)
* [What is Distributed](#what-is-distributed)
* [How To Use It](#how-to-use-it)
    * [Prerequisites](#prerequisites)
    * [Configuration File Elements](#configuration-file-elements) 
    * [Sections](#configuration-sections)
    * [Smtp Hosting Configuration](#smtp-hosting-configuration)
    * [Destination Smtps Configuration](#destination-smtps-configuration)
    * [Routing Rulkes Configuration](#routing-rules-configuration)
    * [Parsing the configuration file](#parsing-the-configuration-file)
* [Using as a Windows Service](#using-as-a-windows-service)

## What is Distributed

The components distributed on this project are:

| Type | Usage |
| :--- | :--- |
| `SmtpConnectionElement` | A `ConfigurationElement` that allow custom tags to be added to configuration files such as `App.Config`, `Machine.Config` or `Web.Config` files. |
| `SmtpConnectionCollection` | A `ConfigurationElementCollection` that allow multiple instances of `SmtpConnectionElement` to be added in a configuration file. |
| `SmtpConnectionsSection` | A `ConfigurationSection` that allow the creation of a `<SmtpConnections/>` tag in the configuration file. |
| `RoutingRuleElement` | A `ConfigurationElement` that allow custom tags to be added to configuration files such as `App.Config`, `Machine.Config` or `Web.Config` files. |
| `RoutingRuleCollection` | A `ConfigurationElementCollection` that allow multiple instances of `RoutingRuleElement` to be added in a configuration file. |
| `RoutingRulesSection` | A `ConfigurationSection` that allow the creation of a `<RoutingRules/>` tag in the configuration file. |
| `ConfigFileParser` | A helper class that will parse a configuration file and return the data in a understandable way |

Those components are listed inside the `SMTPRouter.Windows.Configuration` namespace. 

For more information, refer to the [SMTPRouter.Windows.Configuration Namespace](https://diassoft.github.io/SMTPRouter/html/N_SMTPRouter_Windows_Configuration.htm) on the documentation.

This repository also contains a project named **System.Windows.ServiceHost** which shows an implementation of the `SMTPRouter` component as a windows service. 
You can install it and configure the `App.Config` file in very few steps to have a intermediate Smtp routing messages for you.

## How To Use It

### Prerequisites

When you choose to use the [SmtpRouter.Windows Nuget Package](https://www.nuget.org/packages/SmtpRouter.Windows/), you will also be required to install the following packages:

| Package | Nuget Link | Author |
| :--- | ---------- | :--- |
| SMTPRouter | [![nuget](https://img.shields.io/nuget/v/SMTPRouter.svg)](https://www.nuget.org/packages/SMTPRouter/) | olavodias |
| SmtpServer| [![nuget](https://img.shields.io/nuget/v/SmtpServer.svg)](https://www.nuget.org/packages/SmtpServer/) | cosullivan |
| MimeKit | [![nuget](https://img.shields.io/nuget/v/MimeKit.svg)](https://www.nuget.org/packages/MimeKit/) | jstedfast |
| MailKit | [![nuget](https://img.shields.io/nuget/v/MailKit.svg)](https://www.nuget.org/packages/MailKit/) | jstedfast |

### Configuration File Elements

In order to use the `SMTPRouter.Windows` configuration file helper, you need to add a few extra tags to the configuration file of your application. The parameters are **case sensitive**.

#### Configuration Sections
First, configure the sections that will be diplayed for configuration:
```xml
<configSections>
    <section name="SmtpRouterConfiguration"
             type="SMTPRouter.Windows.Configuration.NameValueSection, SMTPRouter.Windows"/>
    <section name="SmtpConfiguration"
             type="SMTPRouter.Windows.Configuration.SmtpConnectionsSection, SMTPRouter.Windows"/>
    <section name="RoutingRulesConfiguration"
             type="SMTPRouter.Windows.Configuration.RoutingRulesSection, SMTPRouter.Windows"/>
</configSections>
```

#### Smtp Hosting Configuration
Now you can create a `<SmtpRouterConfiguration>` element, which is a `NameValueCollection`.

```xml
<!-- Configuration for the Current SMTP Server -->
<SmtpRouterConfiguration>
  <add name="Host" value="localhost"/>
  <add name="Port" value="25"/>
  <add name="QueuePath" value="C:\\SMTPRouter\\Queues"/>
  <add name="MessageLifespan-Minutes" value="15"/>
  <add name="MessagePurgeLifespan-Days" value="90"/>
</SmtpRouterConfiguration>
```

Please note the `MessageLifespan` and `MessagePurgeLifespan` parameters. They have a `-Minutes` and `-Days` at the end. 
This syntax allows the time span to be setup differently for any `TimeSpan` variable and it is the Standard on all **Diassoft** developments.
 
The valid values are: 
* Days
* Hours
* Minutes
* Seconds
* Milliseconds

Those values are used to call the corresponding static method of the `TimeSpan` class that creates a time span. 
If the user sets it to `-Minutes`, the system will create the span using the syntax below:

```cs
TimeSpan t = TimeSpan.FromMinutes([Parameter Value]);
```

#### Destination Smtps Configuration
Now, setup the SMTPs where the messages could be routed to, by using the `<SmtpConfiguration>` tag and adding one or many `SmtpConnections` inside the `<SmtpConnections>` tag.

```xml
<!-- Destination SMTPs -->
<SmtpConfiguration>
  <SmtpConnections>
    <add key="gmail"
         description="Gmail SMTP Server"
         host="smtp.gmail.com"
         port="587"
         requiresAuthentication="true"
         user="user"
         password="pwd"
         secureSocketOption="1"/>
    <add key="hotmail"
         description="Hotmail SMTP Server"
         host="smtp.live.com"
         port="587"
         requiresAuthentication="true"
         user="user"
         password="pwd"
         secureSocketOption="1"/>
  </SmtpConnections>
</SmtpConfiguration>
```

The `secureSocketOption` attribute accept the following values:

| Value | Description | Usage |
| :--- | :--- | :--- |
| `0` | `None` | No SSL or TLS encryption should be used |
| `1` | `Auto` | The system will decide whether to use SSL or TLS |
| `2` | `SslOnConnect` | The connection should use SSL or TLS encryption immediately |
| `3` | `StartTls` | Elevates the connection to use TLS encryption immediatelly after reading the greeting a server capabilities |
| `4` | `StartTlsWhenAvailable` | Elevates the connection to use TLS encryption immediatelly after reading the greeting a server capabilities, but only if the server supports that |

For more information about the parameters available, refer to the [SmtpConfigurationElement](https://diassoft.github.io/SMTPRouter/html/T_SMTPRouter_Windows_Configuration_SmtpConnectionElement.htm) on the documentation.

#### Routing Rules Configuration
Now, setup the Routing Rules to be checked when processing a message. Use the `<RoutingRulesConfiguration>` tag and add one or many `RoutingRule` inside the `<RoutingRules>` tag.

```xml
<RoutingRulesConfiguration>
  <RoutingRules>
    <add executionSequence="10"
         type="SMTPRouter.Models.MailFromDomainRoutingRule, SMTPRouter"
         params="Domain=gmail.com"
         smtpkey="gmail" />
    <add executionSequence="20"
         type="SMTPRouter.Models.MailFromDomainRoutingRule, SMTPRouter"
         params="Domain=hotmail.com;"
         smtpkey="hotmail" />
    <add executionSequence="30"
         type="SMTPRouter.Models.MailFromRegexMatchRoutingRule, SMTPRouter"
         params="RegexExpression=\A[Uu](\d{5})\z"
         smtpkey="hotmail" />
  </RoutingRules>
</RoutingRulesConfiguration>
```

A Routing Rule is a class that derives from the `RoutingRule` base class. Therefore, the system cannot tell what kind of routing rules to expect. 
You have to inform the `type` attribute as the example below, with the Full Name and the Assembly name.

The table below explain how to setup the XML properly:

| Attribute | Usage |
| :--- | :--- |
| `executionSequence` | The Priority of the Rule. The lower the number is, the higher the priority is. |
| `type` | A string containing the full type corresponding to the Routing Rule to be instantiated. It will use the `Activator` class to instantiate dynamically the Routing Rule. **The type must drive from the `RoutingRule` class** |
| `smtpkey` | A key for the `SmtpConnection` that will be used when the rule matches. This is defined on the `<SmtpConfiguration>` section. |
| `params` | Additional parameters that only belong to the specific routing rule. |

When using the `params` attribute, use the following syntax: `[PropertyName]=[PropertyValue]`. You can add multiple parameters and separate they using a semicolon `;`.

> The `PropertyName` is case sensitive. When reading it with the `ConfigFileParser` object, the system will validate if the `PropertyName` has a setter and, if not, an `Exception` will thrown.

#### Parsing the configuration file

Now you can use the `ConfigFileParser` to read the configuration file and return the properties you need.

```cs
// Creates a new parser and uses the App.Config for the current assembly (ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None))
ConfigFileParser helper = new ConfigFileParser();
```

You can also pass a `System.Configuration.Configuration` object to the constructor.

```cs
// Forces the Configuration to come from a different Configuration object
ConfigFileParser helper = new ConfigFileParser(myConfig));
```

The `ConfigFileParser` will return the information you need on its Properties and Collections. 

| Property | Usage |
| :--- | :--- |
| `SmtpHost` | Contains the configuration for the Smtp Host Server |
| `QueuePath` | Contains the folder where all the queues will be created |
| `MessageLifespan` | A `TimeSpan` that defines how long a message lasts until the system decides it will no longer try to resend it if it keeps failing to send |
| `MessagePurgeLifespan` | A `TimeSpan` that defines how long a message lasts until it is considered obsolete. The `Router` component of the `SMTPRouter` will purge the records automatically. |
| `SmtpConnections` | A `Dictionary<string, SmtpConfiguration>` showing all the Smtp Connections setup on the `<SmtpConfiguration>` section |
| `RoutingRules` | A `List<RoutingRule>` showing all the rules setup on the `<RoutingRulesConfiguration>` section |

For more information, refer to the [ConfigFileParser](https://diassoft.github.io/SMTPRouter/html/T_SMTPRouter_Windows_Configuration_ConfigFileParser.htm) on the documentation.

## Using as a Windows Service

This repository contains a project named **System.Windows.ServiceHost**, which is a **Windows Service** implementation of the `SMTPRouter.Server` object.

You can use it as it is. All you need to do is to install it and configure the rules on the `App.Config` file, discussed on the section [Configuration File Elements](#configuration-file-elements).

> *We are working on a page to perform the management of the SMTP Routing Service*