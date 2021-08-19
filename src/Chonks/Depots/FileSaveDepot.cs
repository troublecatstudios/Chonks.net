using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chonks.Depots {
    public class FileSaveDepot : ISaveDepot {
        private static readonly SaveContainer[] _emptySaves = new SaveContainer[0];
        private string _workingDirectoryPath;

        private const string CHUNK_START_MARKER = "";
        private const string CHUNK_END_MARKER = "===";
        private const string CHUNK_META_END_MARKER = "---";
        private const string CHUNK_META_NAME = "name:";
        private const string CHUNK_META_ID = "uid:";

        private List<Func<string, SaveChunk, SaveChunk>> _metaDataOperators = new List<Func<string, SaveChunk, SaveChunk>>() {
            (line, chunk) => {
                if (line.StartsWith(CHUNK_META_ID)) {
                    chunk.Id = new Guid(line.Replace(CHUNK_META_ID, "").Trim());
                }
                return chunk;
            },
            (line, chunk) => {
                if (line.StartsWith(CHUNK_META_NAME)) {
                    chunk.Name = line.Replace(CHUNK_META_NAME, "").Trim();
                }
                return chunk;
            }
        };

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

        public bool TryLoadSave(string name, out SaveChunk[] chunks, out Exception ex) {
            EnsureWorkingDirectory();
            ex = null;
            chunks = new SaveChunk[0];
            try {
                var readChunks = new List<SaveChunk>();
                using (var fs = new FileStream(Path.Combine(_workingDirectoryPath, $"{name}.sav"), FileMode.Open, FileAccess.Read)) {
                    using (var reader = new StreamReader(fs)) {
                        string? line = "";
                        var chunk = new SaveChunk();
                        var inChunk = false;
                        var inMetaData = false;
                        while (line != null) {
                            line = reader.ReadLine();
                            if (!inChunk && line == CHUNK_START_MARKER) {
                                chunk = new SaveChunk();
                                inChunk = true;
                                inMetaData = true;
                                continue;
                            }

                            if (inChunk && line == CHUNK_META_END_MARKER) {
                                inMetaData = false;
                                continue;
                            }

                            if (line == CHUNK_END_MARKER) {
                                readChunks.Add(chunk);
                                inChunk = false;
                                continue;
                            }

                            if (inChunk) {
                                if (inMetaData) {
                                    foreach (var op in _metaDataOperators) {
                                        chunk = op(line, chunk);
                                    }
                                } else {
                                    var key = line.Split(':')[0] + ":";
                                    var json = line.Replace(key, "").Trim();
                                    chunk.Data.Add(key.Replace(":", ""), json);
                                }
                                continue;
                            }
                        }
                    }
                }
                chunks = readChunks.ToArray();
                return true;
            } catch (Exception e) {
                ex = e;
                return false;
            }
        }

        public bool TryWriteSave(string name, SaveChunk[] chunks, out Exception ex) {
            EnsureWorkingDirectory();
            ex = null;
            try {
                using (var fs = new FileStream(Path.Combine(_workingDirectoryPath, $"{name}.sav"), FileMode.OpenOrCreate, FileAccess.Write)) {
                    using (var writer = new StreamWriter(fs)) {
                        foreach (var chunk in chunks) {
                            writer.WriteLine(CHUNK_START_MARKER);
                            writer.WriteLine($"uid: {chunk.Id}");
                            writer.WriteLine($"name: {chunk.Name}");
                            writer.WriteLine(CHUNK_META_END_MARKER);
                            foreach (var segment in chunk.Data) {
                                writer.WriteLine($"{segment.Key}: {segment.Value}");
                            }
                            writer.WriteLine(CHUNK_END_MARKER);
                        }
                    }
                }
                return true;
            } catch (Exception e) {
                ex = e;
                return false;
            }
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
