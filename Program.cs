using ImageMagick;

namespace Maestro_Converter
{
    internal static class Program
    {
        private static readonly string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".heic", ".bmp", ".tiff"];

        [STAThread]
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new fmMain());
            }
            else if (args.Length > 0)
            {
                if (args[0] == "jpg" || args[0] == "png")
                {
                    string extension = args[0];
                    string filePath = args[1];
                    await Convert(filePath, extension);
                }
                else
                {
                    await ConvertMultipleToPdf(args);
                }
            }
        }

        private static async Task Convert(string originalFilePath, string targetExtension)
        {
            try
            {
                string originalExtension = Path.GetExtension(originalFilePath).ToLower();

                if (!allowedExtensions.Contains(originalExtension))
                {
                    return;
                }

                using MagickImage image = new(originalFilePath);
                image.Format = GetMagickFormat(targetExtension);

                if (originalExtension == ".png")
                {
                    image.BackgroundColor = MagickColors.White;
                    image.Alpha(AlphaOption.Remove);

                }

                await image.WriteAsync(GetUniqueImagePath(originalFilePath, targetExtension));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось конвертировать в {targetExtension}.\n{ex.Message}", "Ошибка конвертирования", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private static async Task ConvertMultipleToPdf(string[] filePaths)
        {
            try
            {
                using MagickImageCollection collection = [];
                foreach (string filePath in filePaths)
                {
                    string filePathExtension = Path.GetExtension(filePath).ToLower();

                    if (!allowedExtensions.Contains(filePathExtension))
                    {
                        continue;
                    }

                    if (File.Exists(filePath))
                    {
                        MagickImage image = new(filePath)
                        {
                            Format = MagickFormat.Jpg,
                        };
                        collection.Add(image);
                    }
                }

                if (collection.Count > 0)
                {
                    await collection.WriteAsync(GetUniqueImagePath(filePaths[0], "pdf"));
                }
                else
                {
                    throw new Exception("Нет изображений для конвертации.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось создать PDF.\n{ex.Message}", "Ошибка при создании PDF", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private static MagickFormat GetMagickFormat(string targetExtension)
        {
            return targetExtension switch
            {
                "jpg" => MagickFormat.Jpg,
                "png" => MagickFormat.Png,
                _ => MagickFormat.Jpg,
            };
        }

        private static string GetUniqueImagePath(string originalFilePath, string targetExtension)
        {
            string directory = Path.GetDirectoryName(originalFilePath) ?? Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFilePath);
            string baseFileName = $"{fileNameWithoutExtension} (converted)";
            string newImagePath = Path.Combine(directory, $"{baseFileName}.{targetExtension}");

            int counter = 1;
            while (File.Exists(newImagePath))
            {
                newImagePath = Path.Combine(directory, $"{baseFileName} ({counter}).{targetExtension}");
                counter++;
            }

            return newImagePath;
        }
    }
}