namespace BradyTask.BusinessLogic.Contracts
{
    public interface IFilesCheckerService
    {
        void ReadDirectoryThread();
        void CheckExistingFiles();
    }
}
