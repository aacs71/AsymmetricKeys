using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Oakton;

namespace AsymmetricKeys
{
    public class FromBase64Parameters
    {
        [Description("The input file")]
        public string InputFile { get; set; }

        [Description("The output file")]
        public string OutputFile { get; set; }
    }

    [Description("Convert content file from base 64")]
    public class FromBase64Command : OaktonAsyncCommand<FromBase64Parameters>
    {
        public FromBase64Command()
        {
            Usage("Converts the file content from base 64").Arguments(x => x.InputFile, x => x.OutputFile);
        }

        public override async Task<bool> Execute(FromBase64Parameters arguments)
        {
            try
            {
                // https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
                using var reader = new StreamReader(arguments.InputFile, Encoding.Default, true);
                reader.Peek(); 
                var fileEncoding = reader.CurrentEncoding;
                var fileContents = await reader.ReadToEndAsync();
                await File.WriteAllTextAsync(
                    arguments.OutputFile,
                    fileEncoding.GetString(
                        Convert.FromBase64String(fileContents)
                    )
                );
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }

            return true;
        }
    }
}