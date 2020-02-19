using CommandLine;

namespace qr_generator
{
    internal class Program
    {
        private static void Main(string[] args) => Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed<CommandLineOptions>(Application.Start);
    }
}
