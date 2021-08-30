using System;
using System.Collections.Generic;

namespace Chonks.Depots {
    public class InMemorySaveDepot : ISaveDepot {
        private readonly List<SaveContainer> _containers = new List<SaveContainer>();
        private readonly Dictionary<string, SaveChunk[]> _chunks = new Dictionary<string, SaveChunk[]>();

        public SaveContainer[] ListSaves() => _containers.ToArray();

        public bool TryLoadSave(string name, out SaveChunk[] chunks, out Exception ex) {
            ex = null;
            chunks = new SaveChunk[0];
            if (_chunks.TryGetValue(name, out chunks)) {
                return true;
            }
            return false;
        }

        public bool TryClearSave(string name, out Exception ex) {
            ex = null;
            if (!_chunks.ContainsKey(name)) {
                return true;
            }
            _chunks.Remove(name);
            return true;
        }

        public bool TryWriteSave(string name, SaveChunk[] chunks, out Exception ex) {
            ex = null;
            if (!_chunks.ContainsKey(name)) {
                _chunks.Add(name, chunks);
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
