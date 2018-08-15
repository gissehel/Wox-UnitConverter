using Wox.UnitConverter.Core.Service;

namespace Wox.UnitConverter.Service
{
    public class FileGeneratorService : IFileGeneratorService
    {
        public IFileGenerator CreateGenerator(string path)
        {
            return new FileGenerator(path);
        }
    }
}