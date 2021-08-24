using System.Collections.Generic;
using System.Linq;

namespace Chonks.Tests.Fakes {
    public class FakeSaveStore : ISaveStore {
        private static int _counter = 0;
        private SaveState[] _states;

        private string _id;

        private Dictionary<string, ChunkDataSegment> _chunkData = new Dictionary<string, ChunkDataSegment>();

        public FakeSaveStore(params SaveState[] states) : this() {
            _states = states;
        }

        public FakeSaveStore() {
            _id = $"FAKE-{_counter++}";
        }

        public SaveState[] States => _states;
        public Dictionary<string, ChunkDataSegment> ChunkData => _chunkData;

        public List<SaveState> GetSaveStates() => _states.ToList();

        public string GetStoreIdentifier() => _id;

        public void LoadChunkData(string chunkName, ChunkDataSegment data) {
            if (_chunkData.ContainsKey(chunkName)) {
                _chunkData.Remove(chunkName);
            }
            _chunkData.Add(chunkName, data);
        }
    }
}
