using System.Configuration;

namespace FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements
{
    public class ConnectionElement : ConfigurationElement
    {
        [ConfigurationProperty("server", DefaultValue = null, IsRequired = true)]
        public string Server
        {
            get { return (string)base["server"]; }
            set { base["server"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = -1, IsRequired = true)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("user", DefaultValue = null, IsRequired = true)]
        public string User
        {
            get { return (string)base["user"]; }
            set { base["user"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = null, IsRequired = true)]
        public string Password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }

        [ConfigurationProperty("certificatePath", DefaultValue = null, IsRequired = true)]
        public string CertificatePath
        {
            get { return (string)base["certificatePath"]; }
            set { base["certificatePath"] = value; }
        }

        [ConfigurationProperty("useCertificateInsteadPasswordForAuthorization", DefaultValue = false, IsRequired = true)]
        public bool UseCertificateInsteadPasswordForAuthorization
        {
            get { return (bool)base["useCertificateInsteadPasswordForAuthorization"]; }
            set { base["useCertificateInsteadPasswordForAuthorization"] = value; }
        }

        [ConfigurationProperty("connectionTimeOutMiliSeconds", DefaultValue = 15000, IsRequired = true)]
        public int ConnectionTimeOutMiliSeconds
        {
            get { return (int)base["connectionTimeOutMiliSeconds"]; }
            set { base["connectionTimeOutMiliSeconds"] = value; }
        }

        [ConfigurationProperty("idleTimeoutMiliSeconds", DefaultValue = 15000, IsRequired = true)]
        public int IdleTimeoutMiliSeconds
        {
            get { return (int)base["idleTimeoutMiliSeconds"]; }
            set { base["idleTimeoutMiliSeconds"] = value; }
        }
    }
}