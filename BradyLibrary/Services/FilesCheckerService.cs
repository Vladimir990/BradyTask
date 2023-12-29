using BradyTask.BusinessLogic.Contracts;
using BradyTask.BusinessLogic.Helpers;
using BradyTask.BusinessLogic.Models.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BradyTask.BusinessLogic.Services
{
    public class FilesCheckerService : IFilesCheckerService
    {
        #region Properties
        private bool ProgramRunning = true;
        private int FilesHandled = 1;
        private string inputFilePath;
        private PathConfiguration _paths;
        private ILogger<FilesCheckerService> _logger;
        #endregion

        #region Constructors
        public FilesCheckerService(IOptions<PathConfiguration> paths, ILogger<FilesCheckerService> logger)
        {
            _paths = paths?.Value;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public void ReadDirectoryThread()
        {
            Console.WriteLine("Looking for a new files...");
            while (ProgramRunning)
            {
                DirectoryInfo DirInfo = new DirectoryInfo(_paths.InputFolderPath);
                foreach (FileInfo FInfo in DirInfo.GetFiles("*.xml"))
                {
                    Thread t = new Thread(new ParameterizedThreadStart(ProcessFile));
                    t.Start(FInfo.FullName);
                    while (t.ThreadState != System.Threading.ThreadState.Stopped)
                    {
                        Thread.Sleep(5);
                    }
                }
            }
        }

        public void CheckExistingFiles()
        {
            FileHelper.EnsureDirectoryExists(_paths.OutputFolderPath);
            // Get all files in the folder
            string[] files = Directory.GetFiles(_paths.InputFolderPath);

            // Process existing files
            foreach (string existingFile in files)
            {
                Console.WriteLine($"Existing file detected: {Path.GetFileName(existingFile)}");
                _logger.LogInformation($"Existing file detected: {Path.GetFileName(existingFile)}");
                ProcessFile(existingFile);
            }
        }
        #endregion

        #region Private methods
        private void ProcessFile(object fileToProcess)
        {
            bool isProcessed = false;
            inputFilePath = fileToProcess.ToString();
            try
            {
                Console.WriteLine($"Processing file: {Path.GetFileName(inputFilePath)}");
                _logger.LogInformation($"Processing of the file: {Path.GetFileName(inputFilePath)} started");

                // Deserialize ReferenceData into C# object
                var referenceData = XmlHandling.DeserializeReferenceData(_paths.ReferenceDataPath);

                // Deserialize Input into C# object
                var generationReport = XmlHandling.DeserializeInput(inputFilePath);

                Console.WriteLine($"File: {Path.GetFileName(inputFilePath)} successfully deserialized");
                Console.WriteLine("Calculation started");
                var output = Calculations.Calculate(generationReport, referenceData);
                Console.WriteLine("Output successfully created");
                FileHelper.GenerateOutputXml(output, inputFilePath, _paths.OutputFolderPath);

                _logger.LogInformation($"File: {Path.GetFileName(inputFilePath)} successfully processed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
                _logger.LogError(ex.Message);
            }

            while (!isProcessed)
            {
                FileHelper.EnsureDirectoryExists(_paths.ProcessedFilesFolderPath);
                try
                {
                    string ProcessedFilesDir = string.Format($"{_paths.ProcessedFilesFolderPath}\\{Path.GetFileNameWithoutExtension(inputFilePath)}_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xml");
                    File.Move(inputFilePath, ProcessedFilesDir);
                    Console.WriteLine(FilesHandled + " - File " +
                                      inputFilePath.ToString() + " processed\n");
                    FilesHandled++;
                }
                catch
                {
                    isProcessed = false;
                }
                finally
                {
                    isProcessed = true;
                }
            }
        }
        #endregion
    }
}
