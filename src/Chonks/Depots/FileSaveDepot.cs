using System;
using System.IO;
using System.Linq;

namespace Chonks.Depots {
    public class FileSaveDepot : ISaveDepot {
        private static readonly SaveContainer[] _emptySaves = new SaveContainer[0];
        private string _workingDirectoryPath;

        public FileSaveDepot(string workingDirectoryPath) {
            _workingDirectoryPath = workingDirectoryPath;
        }

        public SaveContainer[] ListSaves() {
            EnsureWorkingDirectory();
            var files = Directory.GetFiles(_workingDirectoryPath);
            if (files.Length > 0) {
                return files.Select(f => new SaveContainer() {
                    Name = Path.GetFileNameWithoutExtension(f),
                    CreatedAt = File.GetCreationTimeUtc(f),
                    LastUpdatedAt = File.GetLastWriteTimeUtc(f)
                }).ToArray();
            }
            return _emptySaves;
        }

        public bool TryLoadSave(string name, out SaveChunk[] chunks) {
            throw new NotImplementedException();
        }

        public bool TryWriteSave(string name, SaveChunk[] chunks) {
            throw new NotImplementedException();
        }

        internal void Cleanup() {
            if (Directory.Exists(_workingDirectoryPath)) {
                Directory.Delete(_workingDirectoryPath, true);
            }
        }

        private void EnsureWorkingDirectory() {
            if (!Directory.Exists(_workingDirectoryPath)) {
                Directory.CreateDirectory(_workingDirectoryPath);
            }
        }
    }
}
