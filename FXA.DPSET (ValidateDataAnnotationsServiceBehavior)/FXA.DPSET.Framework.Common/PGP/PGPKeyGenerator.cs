using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;

namespace FXA.DPSET.Framework.Common.PGP
{
    public static class PGPKeyGenerator
    {
        public static IList<FileInfo> GenerateKeys(string identity, IDictionary<string, string> info, string passcode, DirectoryInfo directoryToGenerateKeys)
        {
            var keygen = GeneratePgpRingKeys(string.Format("{0} {1}", identity, ((info == null || info.Count <= 0) ? string.Empty : string.Join(" ", info))).Trim(), passcode);

            // Generate Public Key File
            var pkr = keygen.GeneratePublicKeyRing();
            var pkp = Path.Combine(directoryToGenerateKeys.FullName, string.Format("{0}-Public-Key.{1}", identity.Replace(" ", "-"), "pkr"));
            var pubout = new BufferedStream(new FileStream(pkp, FileMode.Create));
            pkr.Encode(pubout);
            pubout.Close();

            // Generate Private Key File
            var skr = keygen.GenerateSecretKeyRing();
            var skp = Path.Combine(directoryToGenerateKeys.FullName, string.Format("{0}-Private-Key.{1}", identity.Replace(" ", "-"), "skr"));
            var secout = new BufferedStream(new FileStream(skp, FileMode.Create));
            skr.Encode(secout);
            secout.Close();

            return new List<FileInfo>
            {
                new FileInfo(pkp),
                new FileInfo(skp)
            };
        }

        private static PgpKeyRingGenerator GeneratePgpRingKeys(string identity, string password)
        {
            var keyRingParams = new KeyRingParams
            {
                Password = password,
                Identity = identity,
                PrivateKeyEncryptionAlgorithm = SymmetricKeyAlgorithmTag.Aes128,
                SymmetricAlgorithms = new SymmetricKeyAlgorithmTag[]
                {
                    SymmetricKeyAlgorithmTag.Aes256,
                    SymmetricKeyAlgorithmTag.Aes192,
                    SymmetricKeyAlgorithmTag.Aes128
                },
                HashAlgorithms = new HashAlgorithmTag[]
                {
                    HashAlgorithmTag.Sha256,
                    HashAlgorithmTag.Sha1,
                    HashAlgorithmTag.Sha384,
                    HashAlgorithmTag.Sha512,
                    HashAlgorithmTag.Sha224,
                }
            };

            var generator = GeneratorUtilities.GetKeyPairGenerator("RSA");
            generator.Init(keyRingParams.RsaParams);

            var masterKeyPair = new PgpKeyPair(PublicKeyAlgorithmTag.RsaSign, generator.GenerateKeyPair(), DateTime.UtcNow);
            Debug.WriteLine("Generated master key with Id {0}", masterKeyPair.KeyId.ToString("x"));

            var masterSubpckGen = new PgpSignatureSubpacketGenerator();
            masterSubpckGen.SetKeyFlags(false, PgpKeyFlags.CanSign | PgpKeyFlags.CanCertify);
            masterSubpckGen.SetPreferredSymmetricAlgorithms(false, (from a in keyRingParams.SymmetricAlgorithms select (int)a).ToArray());
            masterSubpckGen.SetPreferredHashAlgorithms(false, (from a in keyRingParams.HashAlgorithms select (int)a).ToArray());

            var encKeyPair = new PgpKeyPair(PublicKeyAlgorithmTag.RsaGeneral, generator.GenerateKeyPair(), DateTime.UtcNow);
            Debug.WriteLine("Generated encryption key with Id {0}", encKeyPair.KeyId.ToString("x"));

            var encSubpckGen = new PgpSignatureSubpacketGenerator();
            encSubpckGen.SetKeyFlags(false, PgpKeyFlags.CanEncryptCommunications | PgpKeyFlags.CanEncryptStorage);

            masterSubpckGen.SetPreferredSymmetricAlgorithms(false, (from a in keyRingParams.SymmetricAlgorithms select (int)a).ToArray());
            masterSubpckGen.SetPreferredHashAlgorithms(false, (from a in keyRingParams.HashAlgorithms select (int)a).ToArray());
            var keyRingGen = new PgpKeyRingGenerator(PgpSignature.DefaultCertification, masterKeyPair, keyRingParams.Identity, keyRingParams.PrivateKeyEncryptionAlgorithm.Value, keyRingParams.GetPassword(), true, masterSubpckGen.Generate(), null, new SecureRandom());

            keyRingGen.AddSubKey(encKeyPair, encSubpckGen.Generate(), null);

            return keyRingGen;
        }

        private class KeyRingParams
        {
            public RsaKeyGenerationParameters RsaParams { get; set; }
            public string Identity { get; set; }
            public string Password { get; set; }
            public SymmetricKeyAlgorithmTag? PrivateKeyEncryptionAlgorithm { get; set; }
            public SymmetricKeyAlgorithmTag[] SymmetricAlgorithms { get; set; }
            public HashAlgorithmTag[] HashAlgorithms { get; set; }

            public char[] GetPassword()
            {
                return Password.ToCharArray();
            }

            public KeyRingParams()
            {
                RsaParams = new RsaKeyGenerationParameters(BigInteger.ValueOf(0x10001), new SecureRandom(), 2048, 12); //Org.BouncyCastle.Crypto.Tls.EncryptionAlgorithm
            }
        }
    }
}