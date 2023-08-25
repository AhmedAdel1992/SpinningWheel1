namespace Common.Files;

public interface IFileManager
{
   string ReadFileAllText(string path);
   bool WriteFileText(string path, string content);
   bool TryWriteFileText(string path, string content,out string newPath);
   bool Exists(string path);
   string GetDirectoryName(string path);
   string GetFilename(string path);
   IEnumerable<string> GetFiles(string directoryPath);
}