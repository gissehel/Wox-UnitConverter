using System;

namespace Wox.UnitConverter.Core.Service
{
    public interface IFileGenerator : IDisposable
    {
        IFileGenerator AddLine(string line);

        void Generate();
    }
}