using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chilkat;

namespace FXA.DPSET.Framework.Common.SFTP
{
    public static class ChilkatSftp
    {
        public static SFtp GetConnection(Connection connection)
        {
            if (connection == null)
                throw new InvalidOperationException("Source Connection Is NULL");

            if (string.IsNullOrEmpty(connection.Server))
                throw new InvalidOperationException("Source Connection Server Details Not Found");

            if (string.IsNullOrEmpty(connection.User))
                throw new InvalidOperationException("Source Connection User Details Not Found");

            if (string.IsNullOrEmpty(connection.Password))
                throw new InvalidOperationException("Source Connection Password Details Not Found");

            if (connection.Port <= 0) connection.Port = 22;

            if (connection.ConnectionTimeOutMiliSeconds <= 5000) connection.ConnectionTimeOutMiliSeconds = 5000;

            if (connection.IdleTimeoutMiliSeconds <= 5000) connection.IdleTimeoutMiliSeconds = 5000;


            var sftp = new SFtp();

            var success = sftp.UnlockComponent("Anything for 30-day trial");

            if (success != true)
            {
                throw new InvalidOperationException(sftp.LastErrorText);
            }

            sftp.ConnectTimeoutMs = connection.ConnectionTimeOutMiliSeconds;
            sftp.IdleTimeoutMs = connection.IdleTimeoutMiliSeconds;

            var hostname = connection.Server;
            var port = connection.Port;

            success = sftp.Connect(hostname, port);

            if (success != true)
            {
                throw new InvalidOperationException(sftp.LastErrorText);
            }

            var user = connection.User;
            var password = connection.Password;

            success = sftp.AuthenticatePw(user, password);

            if (success != true)
            {
                throw new InvalidOperationException(sftp.LastErrorText);
            }

            success = sftp.InitializeSftp();

            if (success != true)
            {
                throw new InvalidOperationException(sftp.LastErrorText);
            }

            return sftp;
        }
    }
}
