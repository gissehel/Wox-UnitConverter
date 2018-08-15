using System;

namespace Wox.UnitConverter.Core.Service
{
    public interface IFileReader : IDisposable
    {
        string ReadLine();
    }
}