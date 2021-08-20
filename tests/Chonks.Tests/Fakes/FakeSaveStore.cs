using System.Collections.Generic;
using System.Linq;

namespace Chonks.Tests.Fakes {
    public class FakeSaveStore : ISaveStore {
        private static int _counter = 0;
        private SaveState[] _states;

        private Dictionary<string, ChunkDataSegment> _chunkData = new Dictionary<string, ChunkDataSegment>();

        public FakeSaveStore(params SaveState[] states) {
            _states = states;
            _counter++;
        }

        public FakeSaveStore() {
            _counter++;
        }

        public SaveState[] States => _states;
        public Dictionary<string, ChunkDataSegment> ChunkData => _chunkData;

        public List<SaveState> CreateSaveStates() => _states.ToList();

        public string GetStoreIdentifier() => $"FAKE-{_counter}";

        public void ProcessChunkData(string chunkName, ChunkDataSegment data) {
            if (_chunkData.ContainsKey(chunkName)) {
                _chunkData.Remove(chunkName);
            }
            _chunkData.Add(chunkName, data);
        }
    }
}
