using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Oakton;

namespace AsymmetricKeys
{
    public class PemToXmlConvertParameters
    {
        [Description("The file name with private key (Default in PEM format)")]
        public string PrivateKeyFile { get; set; }

        [Description("Optional set if the input private key file is in .Net RSA Xml parameters format")]
        public bool XmlFlag { get; set; } = false;

        [Description("The file name to save the private key in RSA XML Params")]
        [FlagAlias('r')]
        public string XMLPrivateKeyFileFlag { get; set; }

        [Description("Optional file name to save the public key in RSA XML Params")]
        [FlagAlias('p')]
        public string XMLPublicKeyFileFlag { get; set; }
    }

    [Description("Convert a private key file to the RSA XML equivalent")]
    public class ToXmlCommand : OaktonAsyncCommand<PemToXmlConvertParameters>
    {
        public ToXmlCommand()
        {
            Usage("Converts the PEM private file to RSA XML Params").Arguments(x => x.PrivateKeyFile);
        }

        public override async Task<bool> Execute(PemToXmlConvertParameters arguments)
        {
            try
            {
                var rsaAlgorithm = RSA.Create();
                if (arguments.XmlFlag)
                {
                    rsaAlgorithm.FromXmlString(ReadXmlKeyFromFile(arguments.PrivateKeyFile));
                }
                else
                {
                    rsaAlgorithm.ImportFromPem(ReadPemKeyFromFile(arguments.PrivateKeyFile));
                }

                if (!string.IsNullOrWhiteSpace(arguments.XMLPrivateKeyFileFlag))
                {
                    SaveXmlKeyToFile(rsaAlgorithm.ToXmlString(true), arguments.XMLPrivateKeyFileFlag);
                }

                if (!string.IsNullOrWhiteSpace(arguments.XMLPublicKeyFileFlag))
                {
                    SaveXmlKeyToFile(rsaAlgorithm.ToXmlString(false), arguments.XMLPublicKeyFileFlag);
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }

            return true;
        }

        private void SaveXmlKeyToFile(string xmlString, string fileName)
        {
            var onlyFileName = Path.GetFileNameWithoutExtension(fileName);
            File.WriteAllText($"{onlyFileName}.xml", xmlString);
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