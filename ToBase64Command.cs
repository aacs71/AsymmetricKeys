using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Oakton;

namespace AsymmetricKeys
{
    public class ToBase64Parameters
    {
        [Description("The input file")]
        public string InputFile { get; set; }

        [Description("The output file")]
        public string OutputFile { get; set; }
    }

    [Description("Convert content file to base 64")]
    public class ToBase64Command : OaktonAsyncCommand<ToBase64Parameters>
    {
        public ToBase64Command()
        {
            Usage("Converts the file content to base 64").Arguments(x => x.InputFile, x => x.OutputFile);
        }

        public override async Task<bool> Execute(ToBase64Parameters arguments)
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
                    Convert.ToBase64String(
                        fileEncoding.GetBytes(fileContents)
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