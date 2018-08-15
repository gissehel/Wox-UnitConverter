namespace Wox.UnitConverter.Core.Service
{
    public interface IFileReaderService
    {
        bool FileExists(string path);

        IFileReader Read(string path);
    }
}