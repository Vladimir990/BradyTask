using BradyTask.BusinessLogic.Contracts;
using BradyTask.BusinessLogic.Models.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BradyTask.BusinessLogic
{
    public class FileProcessingService : IFileProcessingService
    {
        #region Properties
        private PathConfiguration _paths;
        private ILogger<FileProcessingService> _logger;
        private IFilesCheckerService _fileCheckerService;
        #endregion

        #region Constructors
        public FileProcessingService(IOptions<PathConfiguration> paths, ILogger<FileProcessingService> logger, IFilesCheckerService fileCheckerService)
        {
            _paths = paths?.Value;
            _logger = logger;
            _fileCheckerService = fileCheckerService;
        }
        #endregion

        #region Public methods
        public void Process()
        {
            _logger.LogInformation($"{nameof(FileProcessingService)} started");
            _fileCheckerService.CheckExistingFiles();
            _fileCheckerService.ReadDirectoryThread();
            _logger.LogInformation($"{nameof(FileProcessingService)} ended");
        }
        #endregion
    }
}
