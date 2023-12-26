using BradyTask.Models.Outputs;
using System.Xml.Serialization;

namespace BradyTask
{
    public static class FileHelper
    {
        public static void GenerateOutputXml(GenerationOutput generationReport, string inputFilePath, string outputFolderPath)
        {
            EnsureDirectoryExists(outputFolderPath);
            string newFileName = $"{Path.GetFileNameWithoutExtension(inputFilePath)}-Result{Path.GetExtension(inputFilePath)}";
            string newFilePath = Path.Combine(Path.GetDirectoryName(outputFolderPath), newFileName);
            Console.WriteLine($"Serializing a new file: {newFileName}");

            XmlSerializer serializer = new XmlSerializer(typeof(GenerationOutput));

            using (FileStream stream = new FileStream(newFilePath, FileMode.Create))
            {
                serializer.Serialize(stream, generationReport);
            }

            Console.WriteLine($"{newFileName} successfully created at location: {outputFolderPath}");
            Console.WriteLine();
        }

        static void EnsureDirectoryExists(string directoryPath)
        {
            // Check if the directory exists
            if (!Directory.Exists(directoryPath))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"New directory created: {directoryPath}");
            }
        }
    }
}
