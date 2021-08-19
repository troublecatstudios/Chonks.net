using System;
using System.Collections.Generic;

namespace Chonks.Depots {
    public class InMemorySaveDepot : ISaveDepot {
        private readonly List<SaveContainer> _containers = new List<SaveContainer>();
        private readonly Dictionary<string, SaveChunk[]> _chunks = new Dictionary<string, SaveChunk[]>();

        public SaveContainer[] ListSaves() {
            return _containers.ToArray();
        }

        public bool TryLoadSave(string name, out SaveChunk[] chunks) {
            chunks = new SaveChunk[0];
            if (_chunks.TryGetValue(name, out chunks)) {
                return true;
            }
            return false;
        }

        public bool TryWriteSave(string name, SaveChunk[] chunks) {
            if (_chunks.TryAdd(name, chunks)) {
                _containers.Add(new SaveContainer() { Name = name, CreatedAt = DateTime.UtcNow, LastUpdatedAt = DateTime.UtcNow });
                return true;
            } else {
                if (_chunks.TryGetValue(name, out _)) {
                    for (var i = 0; i < _containers.Count; i++) {
                        var container = _containers[i];
                        if (container.Name == name) {
                            container.LastUpdatedAt = DateTime.UtcNow;
                            break;
                        }
                    }

                    _chunks[name] = chunks;
                    return true;
                }
            }
            return false;
        }
    }
}
