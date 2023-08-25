using Common.Files;
using Common.Logging;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace CommonTest;

public class FileManagerTest
{
    private readonly IFileManager _sut;
    private readonly ILoggerAdapter<FileManager> _logger=Substitute.For<ILoggerAdapter<FileManager>>();
    public FileManagerTest()
    {
        _sut = new FileManager(_logger);
    }
    
    [Fact]
    public void ReadFileAllText_ShouldThrowException_WhenPathIsNullOrEmpty()
    {
      var readAllText=()=>  _sut.ReadFileAllText("");
      readAllText.Should().Throw<ArgumentNullException>();
    }  
    
    [Fact]
    public void ReadFileAllText_ShouldThrowException_WhenFileIsNotExist()
    {
      var readAllText=()=>  _sut.ReadFileAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"file.txt"));
      readAllText.Should().Throw<FileNotFoundException>();
    }  
    
    [Fact]
    public void ReadFileAllText_ReturnFileContent_WhenFileIsExist()
    {
      var readAllText= _sut.ReadFileAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"sample","file.txt"));
      readAllText.Should().Contain("Welcome");

    }   
    [Fact]
    public void WriteFileText_ReturnTrue_FileCreatedSuccessfully()
    {
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString(), "file.txt");
      var readAllText= _sut.WriteFileText(path,"this is test file to try writing file method ...");
      readAllText.Should().BeTrue();

    }  
    [Fact]
    public void TryWriteFileText_ReturnTrue_FileCreatedSuccessfully()
    {
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString(), "file.txt");
      var readAllText= _sut.TryWriteFileText(path,"this is test file to try writing file method ...",out var newPath);
       // readAllText= _sut.TryWriteFileText(path,"this is test file to try writing file method ...",out var newPath);
      readAllText.Should().BeTrue();

    }   
    [Fact]
    public void GetDirectoryName_ReturnDirectoryPath_WhenValidFilePath()
    {
      var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TesDirectory");
      var path = Path.Combine(directory, "file.txt");
      var directoryPath= _sut.GetDirectoryName(path);
      directoryPath.Should().Be(directory);

    }    [Fact]
    public void GetFilename_ReturnFilename_WhenValidFilePath()
    {
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TesDirectory", "file.txt");
      var directoryPath= _sut.GetFilename(path);
      directoryPath.Should().Be("file.txt");

    }
}