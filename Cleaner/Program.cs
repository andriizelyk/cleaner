using System.Diagnostics;

// Ensure at least 1 slot is available
var semaphoreSlots = Math.Max(1, Environment.ProcessorCount - 2); 
// Create a semaphore with slots count based on CPU core count - 2 or at least 1
var semaphore = new Semaphore(semaphoreSlots, semaphoreSlots);

// Check if the user has provided a root folder
if (args.Length < 1)
{
    Console.WriteLine("Please provide the root folder as an argument.");
    return;
}

var rootFolder = args[0];

// Ensure the provided folder exists
if (!Directory.Exists(rootFolder))
{
    Console.WriteLine("The specified folder does not exist.");
    return;
}

// Get all .sln files in the root folder and its subfolders
var slnFiles = Directory.GetFiles(
    rootFolder, 
    "*.sln",
    SearchOption.AllDirectories);

var tasks = new Task[slnFiles.Length];
for (var i = 0; i < slnFiles.Length; i++)
{
    var slnFile = slnFiles[i];
    Console.WriteLine($"Cleaning solution: {slnFile}");
    tasks[i] = Task.Run(() => RunDotnetClean(slnFile));
}

Task.WaitAll(tasks);

Console.WriteLine("All solutions cleaned successfully.");

void RunDotnetClean(string solutionFilePath)
{
    try
    {
        semaphore.WaitOne();    // Wait for a semaphore slot
        
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"clean \"{solutionFilePath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
        process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error while cleaning solution - {solutionFilePath}.");
        Console.WriteLine(e);
    }
    finally
    {
        semaphore.Release();    // Release the semaphore slot
    }
}