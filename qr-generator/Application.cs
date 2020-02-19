using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

// ReSharper disable NotAccessedField.Local

namespace qr_generator
{
    internal class Application
    {
        private static Application _instance;

        private readonly QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //  L   M   Q   H
        // 7% 15% 25% 30%
        private const QRCodeGenerator.ECCLevel DefaultLevel = QRCodeGenerator.ECCLevel.H;

        private Application(CommandLineOptions commandLineOptions)
        {
            var outputDirectory = commandLineOptions.OutputDirectory;
            commandLineOptions.OutputDirectory = string.IsNullOrEmpty(outputDirectory) ? "out" : outputDirectory;
            CreateOutputDirectory(commandLineOptions.OutputDirectory);
            GenerateCodes(commandLineOptions);
        }

        public static void Start(CommandLineOptions commandLineOptions) => _instance = new Application(commandLineOptions);

        private static void CreateOutputDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void GenerateCodes(CommandLineOptions commandLineOptions)
        {
            commandLineOptions.Level.TryParseEnum(out QRCodeGenerator.ECCLevel level);


            var regex = commandLineOptions.InputRegex;
            var regexCompiled = new Regex(regex, RegexOptions.Compiled);
            var fails = 0;
            var indexLt = regex.IndexOf('[');
            var indexGt = regex.LastIndexOf('$') + 1;
            var substring = regex.Substring(indexLt, indexGt - indexLt);
            for (var i = 1; i < int.MaxValue; ++i)
            {
                var filename = regex.Replace(substring, i.ToString("000"));

                var isMatch = regexCompiled.IsMatch(filename);
                if (fails > 0 && isMatch)
                {
                    fails = 0;
                }

                if (!isMatch)
                {
                    ++fails;
                    if (fails > 99)
                    {
                        break;
                    }
                    continue;
                }

                var filePath = Path.Combine(commandLineOptions.OutputDirectory, filename + ".png");
                var qrCode = GenerateCode(filename, level);
                qrCode.Save(filePath, ImageFormat.Png);
            }
        }

        private Bitmap GenerateCode(string text, QRCodeGenerator.ECCLevel level)
        {
            var qrCodeData = qrGenerator.CreateQrCode(text, level);
            var qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }
    }
}
