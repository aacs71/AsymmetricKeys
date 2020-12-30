using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Oakton;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace AsymmetricKeys
{
    public class XmlToPemConvertParameters
    {
        [Description("The file name with private key (Default in XML format)")]
        public string PrivateKeyFile { get; set; }

        [Description("Optional set if the input private key file is in PEM format")]
        [FlagAlias('m')]
        public bool PemFlag { get; set; } = false;

        [Description("The file name to save the private key in PEM Format")]
        [FlagAlias('r')]
        public string PEMPrivateKeyFileFlag { get; set; }

        [Description("Optional file name to save the public key in PEM Format")]
        [FlagAlias('p')]
        public string PEMPublicKeyFileFlag { get; set; }
    }

    [Description("Convert a private key file to the RSA XML equivalent")]
    public class ToPemCommand : OaktonAsyncCommand<XmlToPemConvertParameters>
    {
        public ToPemCommand()
        {
            Usage("Converts the XML private file to PEM Format").Arguments(x => x.PrivateKeyFile);
        }

        public override async Task<bool> Execute(XmlToPemConvertParameters arguments)
        {
            try
            {
                var rsaAlgorithm = RSA.Create();
                if (arguments.PemFlag)
                {
                    rsaAlgorithm.ImportFromPem(ReadPemKeyFromFile(arguments.PrivateKeyFile));
                }
                else
                {
                    rsaAlgorithm.FromXmlString(ReadXmlKeyFromFile(arguments.PrivateKeyFile));
                }

                var key = DotNetUtilities.GetRsaKeyPair(rsaAlgorithm);

                if (!string.IsNullOrWhiteSpace(arguments.PEMPrivateKeyFileFlag))
                {
                    SavePemKeyToFile(key.Private, arguments.PEMPrivateKeyFileFlag);
                }

                if (!string.IsNullOrWhiteSpace(arguments.PEMPublicKeyFileFlag))
                {
                    SavePemKeyToFile(key.Public, arguments.PEMPublicKeyFileFlag);
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }

            return true;
        }

        private void SavePemKeyToFile(AsymmetricKeyParameter keyParameter, string fileName)
        {
            using var writer = new StreamWriter(fileName);
            new PemWriter(writer).WriteObject(keyParameter);
        }

        private string ReadXmlKeyFromFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        private string ReadPemKeyFromFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}