using CommandLine;

namespace qr_generator
{
    public class CommandLineOptions
    {
        [Option('r', "regex", Required = false, HelpText = "Set input regex mask to generate.")]
        public string InputRegex { get; set; } = string.Empty;

        [Option('o', "outputDir", Required = false, HelpText = "Set output directory for qr codes")]
        public string OutputDirectory { get; set; }

        [Option('l', "level", Required = false, HelpText = "Set ECC level for qr encoding")]
        public string Level { get; set; } = string.Empty;
    }
}