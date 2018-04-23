using SMTPRouter.Windows.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMTPRouter.Windows.ServiceHost
{
    partial class SMTPRoutingService : ServiceBase
    {
        /// <summary>
        /// The cancellation token to control the asynchronous tasks
        /// </summary>
        public CancellationTokenSource CancellationToken { get; private set; }

        /// <summary>
        /// Initializes the Smtp Routing Service
        /// </summary>
        public SMTPRoutingService()
        {
            InitializeComponent();

            this.CanPauseAndContinue = false;
        }

        /// <summary>
        /// Starts the Service (this method is called by the SCM)
        /// </summary>
        /// <param name="args">Initialization Arguments</param>
        /// <remarks>This service do not treat any argument passed thru the <paramref name="args"/></remarks>
        protected override void OnStart(string[] args)
        {
            try
            {
                // Creates a Cancellation Token
                CancellationToken = new CancellationTokenSource();

                // Load Information from App.Config File
                ConfigFileParser parser = new ConfigFileParser();

                // Initialize Server
                var server = new SMTPRouter.Server(parser.SmtpHost.Host, parser.SmtpHost.Port, parser.SmtpHost.RequiresAuthentication, parser.SmtpHost.UseSSL, "SMTPRouter", parser.QueuePath)
                {
                    MessageLifespan = new TimeSpan(0, 15, 0),
                };

                foreach (var smtp in parser.SmtpConnections)
                    server.DestinationSmtps.Add(smtp.Key, smtp.Value);

                foreach (var routingRule in parser.RoutingRules)
                    server.RoutingRules.Add(routingRule);

                // Initialize Services
                Task.WhenAll(server.StartAsync(CancellationToken.Token)).ConfigureAwait(false);

                // Create Log Entry to inform the service was started
                StringBuilder sb = new StringBuilder();

                // Print Smtp Host
                sb.AppendLine($"{nameof(SMTPRoutingService)} started successfully");
                sb.AppendLine("-----------------------------------------------------------------------------------");
                sb.AppendLine("Smtp Host");
                sb.AppendLine($"   Host..............: {parser.SmtpHost.Host}");
                sb.AppendLine($"   Port..............: {parser.SmtpHost.Port.ToString()}");
                sb.AppendLine($"   Authentication....: {parser.SmtpHost.RequiresAuthentication.ToString()}");
                sb.AppendLine();

                // Print Smtp Connections
                sb.AppendLine("Smtp Connections");

                if (parser.SmtpConnections.Count == 0)
                {
                    sb.AppendLine("   No Smtp Connections Found");
                }
                else
                {
                    foreach (var s in parser.SmtpConnections)
                    {
                        sb.AppendLine($"   Key...............: {s.Value.Key}");
                        sb.AppendLine($"   Description.......: {s.Value.Description}");
                        sb.AppendLine($"   Host..............: {s.Value.Host}");
                        sb.AppendLine($"   Port..............: {s.Value.Port}");
                        sb.AppendLine($"   Use SSL...........: {s.Value.UseSSL}");
                        sb.AppendLine($"   Authentication....: {s.Value.RequiresAuthentication.ToString()}");
                        sb.AppendLine($"   User..............: {s.Value.User}");
                        sb.AppendLine($"   Password..........: {s.Value.Password}");
                    }
                }
                sb.AppendLine();

                // Print Routing Rules
                sb.AppendLine("Routing Rules");

                if (parser.RoutingRules.Count == 0)
                {
                    sb.AppendLine("   No Routing Rules Found");
                }
                else
                {
                    foreach (var r in parser.RoutingRules)
                    {
                        sb.AppendLine($"   Sequence..........: {r.ExecutionSequence}");
                        sb.AppendLine($"   Smtp Key..........: {r.SmtpConfigurationKey}");
                    }
                }

                // Writes to the log
                this.EventLog.WriteEntry(sb.ToString(), EventLogEntryType.SuccessAudit);
            }
            catch (Exception e)
            {
                // Error Starting the Server

                // Log the Error
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"Error starting the {nameof(SMTPRoutingService)}");
                sb.AppendLine("-----------------------------------------------------------------------------------");
                sb.AppendLine($"Exception............: {e.GetType().ToString()}");
                sb.AppendLine($"   Message...........: {e.Message}");
                sb.AppendLine($"   Stack Trace.......: {e.StackTrace}");

                if (e.InnerException != null)
                {
                    sb.AppendLine();
                    sb.AppendLine($"Inner Exception......: {e.InnerException.GetType().ToString()}");
                    sb.AppendLine($"   Message...........: {e.InnerException.Message}");
                    sb.AppendLine($"   Stack Trace.......: {e.InnerException.StackTrace}");
                }

                this.EventLog.WriteEntry(sb.ToString(), EventLogEntryType.Error);

                // Changes the ExitCode to inform the SCM that the service failed
                this.ExitCode = 1;
            }
        }

        /// <summary>
        /// Stops the Service
        /// </summary>
        /// <remarks>This function uses the <see cref="CancellationToken"/> to cancel the asynchronous tasks</remarks>
        protected override void OnStop()
        {
            // Cancel the execution
            if (CancellationToken != null)
                CancellationToken.Cancel();
        }
    }
}
