using System;
using System.Threading.Tasks;
using Oakton;

namespace AsymmetricKeys
{
    public class GenRsaKeysParameters
    {
        [Description("Optional set a key length for the key. By default 2048 bits")]
        public int KeyLengthFlag { get; set; } = 2048;

        [Description("Optional set if the export is in .Net RSA Xml parameters format")]
        public bool XmlFlag { get; set; } = false;

        [Description("The prefix of the file name where keys will be saved")]
        public string FileNamePrefix { get; set; }
    }

    [Description("Generate RSA Keys pair (private and public) and save in PEM format")]
    public class GenRsaKeysCommand : OaktonAsyncCommand<GenRsaKeysParameters>
    {
        public GenRsaKeysCommand()
        {
            Usage("Generate a new RSA Key Pair").Arguments(x => x.FileNamePrefix);
        }

        public override async Task<bool> Execute(GenRsaKeysParameters arguments)
        {
            try
            {
                RsaGeneratorFactory.Create(arguments.XmlFlag)
                                   .WithKeyLength(arguments.KeyLengthFlag)
                                   .SavePrivateKeyTo($"{arguments.FileNamePrefix}-private.pem")
                                   .SavePublicKeyTo($"{arguments.FileNamePrefix}-public.pem")
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