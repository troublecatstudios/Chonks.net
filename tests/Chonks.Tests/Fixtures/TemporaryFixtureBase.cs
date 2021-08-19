using System;
using System.Diagnostics;
using System.IO;

namespace Chonks.Tests.Fixtures {
    /// <summary>
    /// Base class that handles cleaning up temporary fixtures when the class is disposed.
    /// </summary>
    public abstract class TemporaryFixtureBase : IDisposable {
        private bool _deleteOnDispose;
        public TemporaryFixtureBase(bool deleteOnDispose) {
            _deleteOnDispose = deleteOnDispose;
        }

        public string Path { get; protected set; }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if (_deleteOnDispose && Directory.Exists(Path)) {
                        Debug.WriteLine($"Deleting {Path}");
                        Directory.Delete(Path, true);
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
    }
}
