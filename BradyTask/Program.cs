using BradyTask;
using Microsoft.Extensions.Configuration;

class Program
{
    public static IConfiguration configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();

    public static string inputFolderPath = configuration["Paths:InputFolderPath"];
    public static string outputFolderPath = configuration["Paths:OutputFolderPath"];
    public static string referenceDataFilePath = configuration["Paths:ReferenceDataPath"];
    public static string inputFilePath;

    static void Main()
    {
        Console.WriteLine($"Cheking if files already exist at location: {inputFolderPath}");
        Console.WriteLine();
        CheckExistingFiles(inputFolderPath);

        using (var watcher = new FileSystemWatcher(inputFolderPath))
        {
            // Subscribe to events
            watcher.Created += OnFileCreated;

            // Start watching the folder
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press 'q' to quit the application.");
            while (Console.Read() != 'q') ;
        }
    }

    private static void CheckExistingFiles(string folderPath)
    {
        // Get all files in the folder
        string[] files = Directory.GetFiles(folderPath);

        // Process existing files
        foreach (string existingFile in files)
        {
            Console.WriteLine($"Existing file detected: {Path.GetFileName(existingFile)}");
            inputFilePath = existingFile;
            ProcessFile(inputFilePath);
        }
    }

    private static void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        // Handle the newly created file
        inputFilePath = e.FullPath;
        Console.WriteLine($"New file detected: {inputFilePath}");

        // Process the new file
        ProcessFile(inputFilePath);
    }

    private static void ProcessFile(string filePath)
    {
        try
        {
            Console.WriteLine($"Processing file: {Path.GetFileName(filePath)}");

            // Deserialize ReferenceData into C# object
            var referenceData = XmlHandling.DeserializeReferenceData(referenceDataFilePath);

            // Deserialize Input into C# object
            var generationReport = XmlHandling.DeserializeInput(filePath);

            Console.WriteLine($"File: {Path.GetFileName(filePath)} successfully deserialized");
            Console.WriteLine("Calculation started");
            var output = Calculations.Calculate(generationReport, referenceData);
            Console.WriteLine("Output successfully created");
            FileHelper.GenerateOutputXml(output, inputFilePath, outputFolderPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file: {ex.Message}");
        }
    }
}
