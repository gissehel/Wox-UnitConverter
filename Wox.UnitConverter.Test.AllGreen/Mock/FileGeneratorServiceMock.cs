using Wox.UnitConverter.Core.Service;

namespace Wox.UnitConverter.Test.AllGreen.Mock
{
    public class FileGeneratorServiceMock : IFileGeneratorService
    {
        public FileGeneratorMock LastFileGenerator { get; set; }

        public IFileGenerator CreateGenerator(string path)
        {
            LastFileGenerator = new FileGeneratorMock(path);
            return LastFileGenerator;
        }
    }
}