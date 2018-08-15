using System.IO;
using Wox.UnitConverter.Core.Service;

namespace Wox.UnitConverter.Service
{
    public class FileReaderService : IFileReaderService
    {
        public bool FileExists(string path) => File.Exists(path);

        public IFileReader Read(string path)
        {
            return new FileReader(path);
        }
    }
}