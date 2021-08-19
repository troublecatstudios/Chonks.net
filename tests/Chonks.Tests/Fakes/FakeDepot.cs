using System;
using System.Collections.Generic;
using System.Linq;

namespace Chonks.Tests.Fakes {
    public class FakeDepot : ISaveDepot {
        private readonly Dictionary<string, SaveChunk[]> _storage = new Dictionary<string, SaveChunk[]>();

        public Dictionary<string, SaveChunk[]> Storage => _storage;

        public SaveContainer[] ListSaves() => _storage.Select(pair => new SaveContainer() { Name = pair.Key }).ToArray();

        public bool TryLoadSave(string name, out SaveChunk[] chunks, out Exception ex) {
            ex = null;
            if (_storage.TryGetValue(name, out chunks)) {
                return true;
            }
            return false;
        }

        public bool TryWriteSave(string name, SaveChunk[] chunks, out Exception ex) {
            ex = null;
            if (_storage.TryAdd(name, chunks)) {
                return true;
            }
            _storage[name] = chunks;
            return true;
        }
    }
}
