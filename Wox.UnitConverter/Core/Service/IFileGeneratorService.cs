namespace Wox.UnitConverter.Core.Service
{
    public interface IFileGeneratorService
    {
        IFileGenerator CreateGenerator(string path);
    }
}