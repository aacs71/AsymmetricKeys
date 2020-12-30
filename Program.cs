using System.Reflection;
using System.Threading.Tasks;
using Oakton;

namespace AsymmetricKeys
{
    internal class Program
    {
        private static Task<int> Main(string[] args)
        {
            var executor = CommandExecutor.For(_ => { _.RegisterCommands(typeof(Program).GetTypeInfo().Assembly); });

            return executor.ExecuteAsync(args);
        }
    }
}