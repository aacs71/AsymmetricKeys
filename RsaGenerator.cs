using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace AsymmetricKeys
{
    public static class RsaGeneratorFactory
    {
        public static IRsaGenerator Create(bool isXmlOutput)
        {
            return isXmlOutput
                ? new NativeRsaGenerator()
                : new BouncyCastleRsaGenerator();
        }
    }

    public interface IRsaGenerator
    {
        IRsaGenerator WithKeyLength(int keyLength);
        IRsaGenerator SavePrivateKeyTo(string fileName);
        IRsaGenerator SavePublicKeyTo(string fileName);
        void GenerateAndSave();
    }

    public abstract class AbstractRsaGenerator : IRsaGenerator
    {
        protected int KeyLength { get; set; }
        protected string PrivateKeyFileName { get; set; }
        protected string PublicKeyFileName { get; set; }

        public IRsaGenerator WithKeyLength(int keyLength)
        {
            KeyLength = keyLength;

            return this;
        }

        public IRsaGenerator SavePrivateKeyTo(string fileName)
        {
            PrivateKeyFileName = fileName;

            return this;
        }

        public IRsaGenerator SavePublicKeyTo(string fileName)
        {
            PublicKeyFileName = fileName;

            return this;
        }

        public abstract void GenerateAndSave();
    }

    public class BouncyCastleRsaGenerator : AbstractRsaGenerator
    {
        public override void GenerateAndSave()
        {
            var rsaGenerator = new RsaKeyPairGenerator();
            rsaGenerator.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), KeyLength));

            var key = rsaGenerator.GenerateKeyPair();

            if (!string.IsNullOrWhiteSpace(PrivateKeyFileName))
            {
                SavePemKeyToFile(key.Private, PrivateKeyFileName);
            }

            if (!string.IsNullOrWhiteSpace(PublicKeyFileName))
            {
                SavePemKeyToFile(key.Public, PublicKeyFileName);
            }
        }

        private void SavePemKeyToFile(AsymmetricKeyParameter keyParameter, string fileName)
        {
            using var writer = new StreamWriter(fileName);
            new PemWriter(writer).WriteObject(keyParameter);
        }
    }

    public class NativeRsaGenerator : AbstractRsaGenerator
    {
        public override void GenerateAndSave()
        {
            var rsaGenerator = RSA.Create(KeyLength);

            if (!string.IsNullOrWhiteSpace(PrivateKeyFileName))
            {
                SaveXmlKeyToFile(rsaGenerator.ToXmlString(true), PrivateKeyFileName);
            }

            if (!string.IsNullOrWhiteSpace(PublicKeyFileName))
            {
                SaveXmlKeyToFile(rsaGenerator.ToXmlString(false), PublicKeyFileName);
            }
        }

        private void SaveXmlKeyToFile(string xmlString, string fileName)
        {
            var onlyFileName = Path.GetFileNameWithoutExtension(fileName);
            File.WriteAllText($"{onlyFileName}.xml", xmlString);
        }
    }
}