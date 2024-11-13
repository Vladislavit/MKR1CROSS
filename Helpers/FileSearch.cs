namespace Helpers;

public static class FileSearch
{
    public static string? FindProjectDirectory(string? currentDirectory)
    {
        while (currentDirectory != null && !Directory.GetFiles(currentDirectory, "*.csproj").Any())
        {
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }

        return currentDirectory;
    }
}