using BradyTask.BusinessLogic.Models.Outputs;
using System.Reflection;
using System.Xml.Serialization;

namespace BradyTask.BusinessLogic.Helpers
{
    public static class FileHelper
    {
        #region Properties
        private static readonly string DEFAULT_REFERENCE_DATA = "ReferenceData.xml";
        private static readonly string DOCUMENTS_FOLDER = "Documents\\";
        #endregion

        #region Public methods
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

        public static void EnsureDirectoryExists(string directoryPath)
        {
            // Check if the directory exists
            if (!Directory.Exists(directoryPath))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"New directory created: {directoryPath}");

                if (directoryPath.Contains("ReferenceData"))
                    CreateDefaultReferenceFile(directoryPath);               
            }
        }

        public static void CreateDefaultReferenceFile(string destinationFolder)
        {
            string sourcePath = Path.Combine(GetSolutionDirectory(), DOCUMENTS_FOLDER, DEFAULT_REFERENCE_DATA);
            string destinationPath = Path.Combine(destinationFolder, DEFAULT_REFERENCE_DATA);
            File.Copy(sourcePath, destinationPath);
            Console.WriteLine($"New default ReferenceData xml file created at location: {destinationFolder}");
        }
        #endregion

        #region Private methods
        static string GetSolutionDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyLocation = assembly.Location;
            return Path.GetDirectoryName(assemblyLocation);
        }
        #endregion
    }
}
