using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXA.DPSET.Framework.Common.SFTP
{
    public class Connection
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int ConnectionTimeOutMiliSeconds { get; set; }
        public int IdleTimeoutMiliSeconds { get; set; }
    }
}