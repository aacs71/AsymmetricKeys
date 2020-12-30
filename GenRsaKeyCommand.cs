using System;
using System.Threading.Tasks;
using Oakton;

namespace AsymmetricKeys
{
    public class GenRsaKeyParameters
    {
        [Description("Optional set a key length for the key. By default 2048 bits")]
        public int KeyLengthFlag { get; set; } = 2048;

        [Description("Optional set if the export is in .Net RSA Xml parameters format")]
        public bool XmlFlag { get; set; } = false;

        [Description("The file name where key is saved")]
        public string PrivateKeyFile { get; set; }

        [Description("Optional file name to save the public key")]
        public string PublicKeyFileFlag { get; set; }
    }

    [Description("Generate RSA Key in PEM format")]
    public class GenRsaKeyCommand : OaktonAsyncCommand<GenRsaKeyParameters>
    {
        public GenRsaKeyCommand()
        {
            Usage("Generate a new RSA Private Key").Arguments(x => x.PrivateKeyFile);
        }

        public override async Task<bool> Execute(GenRsaKeyParameters arguments)
        {
            try
            {
                RsaGeneratorFactory.Create(arguments.XmlFlag)
                                   .WithKeyLength(arguments.KeyLengthFlag)
                                   .SavePrivateKeyTo(arguments.PrivateKeyFile)
                                   .SavePublicKeyTo(arguments
                                       .PublicKeyFileFlag) // The file only be created is a value is assigned
                                   .GenerateAndSave();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }

            return true;
        }
    }
}