namespace SMTPRouter.Windows.ServiceHost
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.smtpRoutingServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.smtpRoutingServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // smtpRoutingServiceProcessInstaller
            // 
            this.smtpRoutingServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.smtpRoutingServiceProcessInstaller.Password = null;
            this.smtpRoutingServiceProcessInstaller.Username = null;
            // 
            // smtpRoutingServiceInstaller
            // 
            this.smtpRoutingServiceInstaller.Description = "A SMTP Server that listens to messages and route it to other SMTPs based on Routi" +
    "ng Rules";
            this.smtpRoutingServiceInstaller.DisplayName = "SMTP Routing Service";
            this.smtpRoutingServiceInstaller.ServiceName = "SMTPRoutingService";
            this.smtpRoutingServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.smtpRoutingServiceProcessInstaller,
            this.smtpRoutingServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller smtpRoutingServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller smtpRoutingServiceInstaller;
    }
}