using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using FXA.DPSET.Framework.Common.Properties;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;

namespace FXA.DPSET.Framework.Common.PGP
{
    public static class PGPCrypto
    {
        private static void DecryptFile(FileInfo file, Stream outputStream, FileInfo privateKeyFile, char[] privateKeyPasscodeIn)
        {
            Stream inputStream = File.OpenRead(file.FullName);
            inputStream = PgpUtilities.GetDecoderStream(inputStream);

            try
            {
                var pgpObjectFactory = new PgpObjectFactory(inputStream);

                var pgpObject = pgpObjectFactory.NextPgpObject();
                var pgpEncryptedDataList = pgpObject as PgpEncryptedDataList;

                if (pgpEncryptedDataList == null)
                {
                    throw new InvalidDataException();
                }

                var pgpPrivateKey = default(PgpPrivateKey);
                var pgpPublicKeyEncryptedData = default(PgpPublicKeyEncryptedData);
                var privateKeyStream = (privateKeyFile == null ? ((Stream)new MemoryStream(PGPKeys._private)) : ((Stream)File.OpenRead(privateKeyFile.FullName)));
                var pgpSecretKeyRingBundle = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(privateKeyStream));
                foreach (PgpPublicKeyEncryptedData package in pgpEncryptedDataList.GetEncryptedDataObjects())
                {
                    pgpPrivateKey = ExtractSecretKey(pgpSecretKeyRingBundle, package.KeyId, privateKeyPasscodeIn);

                    if (pgpPrivateKey != null)
                    {
                        pgpPublicKeyEncryptedData = package;
                        break;
                    }
                }
                privateKeyStream.Close();

                if (pgpPrivateKey == null)
                {
                    throw new ArgumentException("Secret key for message not found.");
                }

                var clear = pgpPublicKeyEncryptedData.GetDataStream(pgpPrivateKey);
                var plainPgpObjectFactory = new PgpObjectFactory(clear);
                var pgpMessage = plainPgpObjectFactory.NextPgpObject();
                var pgpCompressedData = pgpMessage as PgpCompressedData;

                if (pgpCompressedData != null)
                {
                    var newPgpObjectFactory = new PgpObjectFactory(pgpCompressedData.GetDataStream());
                    pgpMessage = newPgpObjectFactory.NextPgpObject();
                }

                var pgpLiteralData = pgpMessage as PgpLiteralData;

                if (pgpLiteralData != null)
                {
                    //var outFileName = pgpLiteralData.FileName;

                    //if (outFileName.Length == 0)
                    //{
                    //    outFileName = defaultFileName;
                    //}

                    var unc = pgpLiteralData.GetInputStream();
                    Streams.PipeAll(unc, outputStream);
                    outputStream.Close();
                }
                else if (pgpMessage is PgpOnePassSignatureList)
                {
                    throw new PgpException("encrypted message contains a signed message - not literal data.");
                }

                else
                {
                    throw new PgpException("message is not a simple encrypted file - type unknown.");
                }

                if (pgpPublicKeyEncryptedData.IsIntegrityProtected())
                {
                    Console.Error.WriteLine(!pgpPublicKeyEncryptedData.Verify()
                        ? "Message failed integrity check"
                        : "Message integrity check passed"
                        );
                }
                else
                {
                    Console.Error.WriteLine("No message integrity check");
                }
            }
            catch (PgpException exception)
            {
                var errorMessage = string.Format("{0}\n\n{1}\n\n{2}",
                    exception,
                    (exception.InnerException != null ? exception.InnerException.Message : string.Empty),
                    (exception.InnerException != null ? exception.InnerException.StackTrace : string.Empty));

                Console.Error.WriteLine(errorMessage);
            }
            finally
            {
                inputStream.Close();
            }
        }

        private static void EncryptFile(FileInfo file, Stream outputStream, FileInfo publicKeyFile, bool armor = true, bool withIntegrityCheck = true)
        {
            try
            {
                if (armor) outputStream = new ArmoredOutputStream(outputStream);

                var binaryOut = new MemoryStream();
                var pgpCompressedDataGenerator = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);

                PgpUtilities.WriteFileToLiteralData(pgpCompressedDataGenerator.Open(binaryOut), PgpLiteralData.Binary, file);
                pgpCompressedDataGenerator.Close();

                var publicKeyStream = (publicKeyFile == null ? ((Stream)new MemoryStream(PGPKeys._public)) : ((Stream)File.OpenRead(publicKeyFile.FullName)));
                var publicKey = ReadPublicKey(publicKeyStream);
                publicKeyStream.Close();

                var pgpEncryptedDataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, withIntegrityCheck, new SecureRandom());
                pgpEncryptedDataGenerator.AddMethod(publicKey);

                var bytes = binaryOut.ToArray();
                var streamOut = pgpEncryptedDataGenerator.Open(outputStream, bytes.Length);
                streamOut.Write(bytes, 0, bytes.Length);
                streamOut.Close();
                if (armor) outputStream.Close();
            }
            catch (PgpException exception)
            {
                var errorMessage = string.Format("{0}\n\n{1}\n\n{2}",
                    exception,
                    (exception.InnerException != null ? exception.InnerException.Message : string.Empty),
                    (exception.InnerException != null ? exception.InnerException.StackTrace : string.Empty));

                Console.Error.WriteLine(errorMessage);
            }
        }

        private static PgpPublicKey ReadPublicKey(Stream inputStream)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);

            var pgpPub = new PgpPublicKeyRingBundle(inputStream);

            foreach (PgpPublicKeyRing kRing in pgpPub.GetKeyRings())
            {
                foreach (PgpPublicKey k in kRing.GetPublicKeys())
                {
                    if (k.IsEncryptionKey)
                    {
                        return k;
                    }
                }
            }

            throw new ArgumentException("Can't find encryption key in key ring.");
        }

        private static PgpPrivateKey ExtractSecretKey(PgpSecretKeyRingBundle pgpSec, long keyId, char[] pass)
        {
            var pgpSecKey = pgpSec.GetSecretKey(keyId);

            if (pgpSecKey == null)
            {
                return null;
            }

            return pgpSecKey.ExtractPrivateKey(pass);
        }

        public static FileInfo Encrypt(FileInfo file, bool deleteOriginalFile = true)
        {
            return Encrypt(file, null, deleteOriginalFile);
        }

        public static FileInfo Encrypt(FileInfo file, FileInfo publicKeyFile, bool deleteOriginalFile = true)
        {
            if (file != null)
            {
                var ofilePath = string.Format("{0}.{1}", file.FullName, "pgp");

                if (File.Exists(ofilePath))
                    throw new IOException(string.Format("Target file '{0}' to be created is already exists", ofilePath));

                var ofile = new FileInfo(ofilePath);
                var fos = File.Create(ofile.FullName);
                EncryptFile(file, fos, publicKeyFile);
                fos.Close();

                if (deleteOriginalFile) file.Delete();

                return ofile;
            }

            return null;
        }

        public static FileInfo Decrypt(FileInfo file, bool deleteOriginalFile = true)
        {
            return Decrypt(file, null, null, deleteOriginalFile);
        }

        public static FileInfo Decrypt(FileInfo file, FileInfo privateKeyFile, string passPhrase, bool deleteOriginalFile = true)
        {
            if (file != null)
            {
                var ofilePath = Path.Combine(Path.GetDirectoryName(file.FullName),
                    Path.GetFileNameWithoutExtension(file.Name));

                if (File.Exists(ofilePath))
                    throw new IOException(string.Format("Target file '{0}' to be created is already exists", ofilePath));

                var ofile = new FileInfo(ofilePath);
                var fos = File.Create(ofile.FullName);
                DecryptFile(file, fos, privateKeyFile, (string.IsNullOrEmpty(passPhrase) ? PGPKeys.DefaultPasscode.ToCharArray() : passPhrase.ToCharArray()));
                fos.Close();

                if (deleteOriginalFile) file.Delete();

                return ofile;
            }

            return null;
        }
    }
}