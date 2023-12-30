using BradyTask.BusinessLogic.Contracts;
using BradyTask.BusinessLogic.Helpers;
using BradyTask.BusinessLogic.Models.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BradyTask.BusinessLogic
{
    public class FileProcessingService : IFileProcessingService
    {
        #region Properties
        private bool ProgramRunning = true;
        private int FilesHandled = 1;
        private string inputFilePath;
        private PathConfiguration _paths;
        private ILogger<FileProcessingService> _logger;
        #endregion

        #region Constructors
        public FileProcessingService(IOptions<PathConfiguration> paths, ILogger<FileProcessingService> logger)
        {
            _paths = paths?.Value;
            _logger = logger;
        }
        #endregion

        #region Public methods
        public void Process()
        {
            _logger.LogInformation($"{nameof(FileProcessingService)} started");
            CheckExistingFiles();
            ReadDirectoryThread();
            _logger.LogInformation($"{nameof(FileProcessingService)} ended");
        }
        #endregion

        #region Private methods
        private void ReadDirectoryThread()
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

        private void CheckExistingFiles()
        {
            FileHelper.EnsureDirectoryExists(_paths.InputFolderPath);
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

        private void ProcessFile(object fileToProcess)
        {
            bool isProcessed = false;
            inputFilePath = fileToProcess.ToString();
            try
            {
                Console.WriteLine($"Processing file: {Path.GetFileName(inputFilePath)}");
                _logger.LogInformation($"Processing of the file: {Path.GetFileName(inputFilePath)} started");

                // Make sure the ReferenceData directory and file exist, if not, create a new folder and input the default ReferenceData.xml file
                FileHelper.EnsureDirectoryExists(_paths.ReferenceDataFolderPath);

                // In case the folder exists and has no xml data, enter the default xml file
                if (!File.Exists(_paths.ReferenceDataFolderPath + _paths.ReferenceDataFileName))
                    FileHelper.CreateDefaultReferenceFile(_paths.ReferenceDataFolderPath);

                // Deserialize ReferenceData into C# object
                var referenceData = XmlHandling.DeserializeReferenceData(_paths.ReferenceDataFolderPath, _paths.ReferenceDataFileName);

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
