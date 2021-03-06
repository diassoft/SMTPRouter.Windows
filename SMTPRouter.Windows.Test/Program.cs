﻿using SMTPRouter.Models;
using SMTPRouter.Windows.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPRouter.Windows.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Decide what to test
            Console.WriteLine("===================================================================================");
            Console.WriteLine("Test SMTPRouter.Windows (.NET Framework Version)");
            Console.WriteLine("===================================================================================");
            Console.WriteLine();
            Console.WriteLine("1 - Read Setup from App.Config File");
            Console.WriteLine();
            Console.Write("Enter your selection --> ");

            string answer = Console.ReadLine();

            if (answer == "1")
            {
                TestServerWithConfigurationFile();
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to shutdown...");
            Console.ReadLine();
        }

        private static void TestServerWithConfigurationFile()
        {
            try
            {
                // Use the Parser class to procss the App.Config file
                ConfigFileParser helper = new ConfigFileParser();

                // List SMTP Configurations Found
                Console.WriteLine("SMTP Connections");
                Console.WriteLine("--------------------------------------------------------------");
                foreach (var c in helper.SmtpConnections)
                {
                    Console.WriteLine($"Key.......: {c.Key}");
                    Console.WriteLine($"Value.....: {c.Value.ToString()}");
                    Console.WriteLine($"            Host.................: {((SmtpConfiguration)c.Value).Host}");
                    Console.WriteLine($"            Port.................: {((SmtpConfiguration)c.Value).Port.ToString()}");
                    Console.WriteLine($"            Authentication.......: {((SmtpConfiguration)c.Value).RequiresAuthentication.ToString()}");
                    Console.WriteLine($"            Use SSL..............: {((SmtpConfiguration)c.Value).UseSSL.ToString()}");
                    Console.WriteLine($"            User.................: {((SmtpConfiguration)c.Value).User}");
                    Console.WriteLine($"            Password.............: {((SmtpConfiguration)c.Value).Password}");
                    Console.WriteLine($"            Secure Socket........: {((SmtpConfiguration)c.Value).SecureSocketOption}");
                    Console.WriteLine($"            Active Connections...: {((SmtpConfiguration)c.Value).ActiveConnections}");
                    Console.WriteLine($"            Grouping Option......: {((SmtpConfiguration)c.Value).GroupingOption}");

                    Console.WriteLine();
                }

                // List RoutingRules Found
                Console.WriteLine();
                Console.WriteLine("Routing Rules");
                Console.WriteLine("--------------------------------------------------------------");
                foreach (var r in helper.RoutingRules)
                {
                    Console.WriteLine($"Sequence....: {r.ExecutionSequence.ToString()}");
                    Console.WriteLine($"SmtpKey.....: {r.SmtpConfigurationKey}");
                    Console.WriteLine($"Type........: {r.GetType().ToString()}");

                    if (r is MailFromDomainRoutingRule)
                        Console.WriteLine($"Domain......: {((MailFromDomainRoutingRule)r).Domain}");
                    //else if (r is MailFromRegexMatchRoutingRule)
                    //    Console.WriteLine($"Expression..: {((MailFromRegexMatchRoutingRule)r).RegexExpression}");

                    Console.WriteLine();
                }

                // List Accepted IP Addresses
                Console.WriteLine("Accepted IP Addresses");
                Console.WriteLine("--------------------------------------------------------------");
                foreach (var c in helper.AcceptedIPAddresses)
                    Console.WriteLine($"IP........: {c}");

                Console.WriteLine();

                // List Rejected IP Addresses
                Console.WriteLine("Rejected IP Addresses");
                Console.WriteLine("--------------------------------------------------------------");
                foreach (var c in helper.RejectedIPAddresses)
                    Console.WriteLine($"IP........: {c}");

                Console.WriteLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error using the configuration file to instantiate a Server");
                Console.WriteLine($"Error...........: {e.Message}");
                Console.WriteLine($"Stack Trace.....: {e.StackTrace}");
            }
        }

    }
}
