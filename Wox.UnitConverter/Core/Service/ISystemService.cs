namespace Wox.UnitConverter.Core.Service
{
    public interface ISystemService
    {
        string ApplicationDataPath { get; }

        void OpenUrl(string url);
    }
}