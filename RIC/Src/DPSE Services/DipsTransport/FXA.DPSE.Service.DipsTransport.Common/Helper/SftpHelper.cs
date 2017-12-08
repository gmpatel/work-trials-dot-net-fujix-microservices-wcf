﻿using System;
using System.IO;
using Chilkat;
using FXA.DPSE.Service.DipsTransport.Common.Configuration.Elements;

namespace FXA.DPSE.Service.DipsTransport.Common.Helper
{
    public static class SftpHelper
    {
        public static SFtp GetConnection(ConnectionElement connection)
        {
            if (connection == null)
                throw new InvalidOperationException("Source Connection Is NULL");

            if (string.IsNullOrEmpty(connection.Server))
                throw new InvalidOperationException("Source Connection Server Details Not Found");

            if (string.IsNullOrEmpty(connection.User))
                throw new InvalidOperationException("Source Connection User Details Not Found");

            if (!connection.UseCertificateInsteadPasswordForAuthorization && string.IsNullOrEmpty(connection.Password))
                throw new InvalidOperationException("Source Connection Password Details Not Found");

            if (connection.UseCertificateInsteadPasswordForAuthorization && string.IsNullOrEmpty(connection.CertificatePath))
                throw new InvalidOperationException("Source Connection Authorization Certificate File Path Not Found");
            
            if (connection.UseCertificateInsteadPasswordForAuthorization && !File.Exists(connection.CertificatePath))
                throw new InvalidOperationException("Source Connection Authorization Certificate File Not Exists At The Path Provided");
            
            if (connection.Port <= 0) connection.Port = 22;

            if (connection.ConnectionTimeOutMiliSeconds <= 5000) connection.ConnectionTimeOutMiliSeconds = 5000;

            if (connection.IdleTimeoutMiliSeconds <= 5000) connection.IdleTimeoutMiliSeconds = 5000;

            var sftp = new SFtp();

            var success = sftp.UnlockComponent("FUJXRXSSH_sPfSFYMd3Cze");

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
            
            if (connection.UseCertificateInsteadPasswordForAuthorization)
            {
                var key = new SshKey();
                var privKey = key.LoadText(connection.CertificatePath);

                if (privKey == null)
                {
                    throw new InvalidOperationException(sftp.LastErrorText);
                }

                success = key.FromOpenSshPrivateKey(privKey);

                if (success != true)
                {
                    throw new InvalidOperationException(sftp.LastErrorText);
                }

                success = sftp.AuthenticatePk(user, key);

                if (success != true)
                {
                    throw new InvalidOperationException(sftp.LastErrorText);
                }
            }
            else
            {
                var password = connection.Password;
                
                success = sftp.AuthenticatePw(user, password);

                if (success != true)
                {
                    throw new InvalidOperationException(sftp.LastErrorText);
                }
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