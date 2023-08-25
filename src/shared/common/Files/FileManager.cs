using System.Data;
using System.Net;
using System.Text;
using Common.DependencyInjection.Interfaces;
using Common.Extensions;
using Common.Logging;
using MassTransit.Futures.Contracts;

namespace Common.Files;

public class FileManager : IFileManager, IScoped
{
    private readonly ILoggerAdapter<FileManager> _logger;

    public FileManager(ILoggerAdapter<FileManager> logger)
    {
        _logger = logger;
    }

    public string ReadFileAllText(string path)
    {
        _logger.LogDebug("Trying to read file from path: {FilePath}", path);
        if (path.IsNullOrEmpty())
            throw new ArgumentNullException(path, "File path can not be null or empty");
        if (!File.Exists(path))
        {
            _logger.LogError("Failed to read file, file not found in : {FilePath}", path);
            throw new FileNotFoundException("file not found");
        }

        return File.ReadAllText(path);
    }

    public bool WriteFileText(string path, string content)
    {
        try
        {
            CreateDirectoryIfNotExistFromFilePath(path);
            using var sw = new StreamWriter(path, true);
            sw.Write(content);
            sw.Dispose();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private static void CreateDirectoryIfNotExistFromFilePath(string path)
    {
        var directoryPath = Path.GetDirectoryName(path);
        if (!directoryPath.IsNullOrEmpty() && !Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
    }

    public bool WriteFileText1(string path, string content)
    {
        try
        {
            if (path.IsNullOrEmpty())
                throw new ArgumentNullException(path, "File path can't be null or empty");

            var directoryPath = Path.GetDirectoryName(path);
            if (!directoryPath.IsNullOrEmpty() && !Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            if (Exists(path))
                throw new DuplicateNameException("File already exist");

            using FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            // Convert the text to write to a byte array
            byte[] bytesToWrite = Encoding.UTF8.GetBytes(content);

            // Write the bytes to the file
            stream.Write(bytesToWrite, 0, bytesToWrite.Length);
            // File.WriteAllText(path, content, Encoding.UTF8);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't write file to path :{FilePath}", path);
            throw;
        }
    }

    public bool TryWriteFileText(string path, string content, out string newPath)
    {
        newPath = string.Empty;
        var filename = GetFilename(path).Split('.')[0];
        var extension = GetFilename(path).Split('.')[1];
        var directoryPath = GetDirectoryName(path);
        CreateDirectoryIfNotExist(directoryPath);
        string[] files = Directory.GetFiles(directoryPath, $"{filename}*.{extension}");
        string baseName = Path.Combine(GetDirectoryName(path), $"{filename}_");
        int i = 0;
        do
        {
            filename = baseName + ++i + $".{extension}";
        } while (files.Contains(filename));

        newPath = filename;
        return WriteFileText(filename, content);
    }

    public bool Exists(string path)
    {
        _logger.LogDebug("Trying to read file from path: {FilePath}", path);
        if (path.IsNullOrEmpty())
            throw new ArgumentNullException(path, "File path can't be null or empty");

        return File.Exists(path);
    }

    public string GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    public string GetFilename(string path)
    {
        return Path.GetFileName(path);
    }

    public IEnumerable<string> GetFiles(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            _logger.LogWarning("Directory not found : {DirectoryPath}", directoryPath);
            return Enumerable.Empty<string>().ToList();
        }

        return Directory.GetFiles(directoryPath);
    }

    private void CreateDirectoryIfNotExist(string path)
    {
        if (!path.IsNullOrEmpty() && !Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    private void CreateDirectoryIfNotExistByFilePath(string path)
    {
        var directoryPath = Path.GetDirectoryName(path);
        if (!directoryPath.IsNullOrEmpty() && !Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
    }
}