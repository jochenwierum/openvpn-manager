using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using System.ComponentModel;

namespace OpenVPNManagerService
{
    [RunInstaller(true)]
    public sealed class ServiceInstallerProcess : ServiceProcessInstaller
    {
        public ServiceInstallerProcess()
        {
            this.Account = ServiceAccount.LocalSystem;
        }
    }
}
