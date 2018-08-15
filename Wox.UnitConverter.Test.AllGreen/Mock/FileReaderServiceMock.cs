using System.Collections.Generic;
using Wox.UnitConverter.Core.Service;

namespace Wox.UnitConverter.Test.AllGreen.Mock
{
    public class FileReaderServiceMock : IFileReaderService
    {
        public Dictionary<string, List<string>> FilesOnFileSystem { get; private set; } = new Dictionary<string, List<string>>();

        public List<string> LastCreatedFile { get; set; } = null;

        public bool FileExists(string path) => FilesOnFileSystem.ContainsKey(path);

        public IFileReader Read(string path)
        {
            if (!FileExists(path))
            {
                return null;
            }
            return new FileReaderMock(path, FilesOnFileSystem[path]);
        }

        public List<string> CreateFile()
        {
            LastCreatedFile = new List<string>();
            return LastCreatedFile;
        }

        public void SaveLastCreatedFileTo(string path)
        {
            FilesOnFileSystem[path] = LastCreatedFile;
            LastCreatedFile = null;
        }
    }
}