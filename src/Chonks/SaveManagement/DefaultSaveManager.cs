using Chonks.Depots;
using System.Collections.Generic;
using System.Linq;

namespace Chonks.SaveManagement {
    public class DefaultSaveManager : ISaveManager {
        private readonly ISaveController _controller;
        private readonly ISaveDepot _depot;

        private SaveChunk[] _snapshot;

        public DefaultSaveManager(ISaveController controller = null, ISaveDepot depot = null) {
            _controller = controller ?? new DefaultSaveController();
            _depot = depot ?? new InMemorySaveDepot();
        }

        public void LoadSnapshot(SaveContainer container) {
            if (_depot.TryLoadSave(container.Name, out var chunks)) {
                _snapshot = chunks;
                ProcessSnapshotUpdate();
            }
        }

        public void ApplySnapshot(SaveContainer container) {
            if (_snapshot == null) return;
            if (_depot.TryWriteSave(container.Name, _snapshot)) {
                ProcessSnapshotUpdate();
            }
        }

        public void MakeSnapshot() {
            var chunks = new Dictionary<string, SaveChunk>();
            foreach (var store in _controller.GetSaveStores()) {
                var id = store.GetStoreIdentifier();
                var states = store.CreateSaveStates();
                foreach (var state in states) {
                    SaveChunk chunk;
                    if (!chunks.TryGetValue(state.ChunkName, out chunk)) {
                        chunk = new SaveChunk(state.ChunkName);
                    }
                    chunk.AddToChunk(id, state.Data);
                }
            }

            _snapshot = chunks.Select((pair) => pair.Value).ToArray();

            foreach (var interpreter in _controller.GetSaveInterpreters()) {
                interpreter.ProcessChunks(_snapshot);
            }
        }

        private void ProcessSnapshotUpdate() {
            var states = new List<SaveState>();
            foreach (var store in _controller.GetSaveStores()) {
                states.Clear();
                var id = store.GetStoreIdentifier();
                foreach (var chunk in _snapshot) {
                    if (chunk.Data.TryGetValue(id, out var json)) {
                        states.Add(new SaveState() { ChunkName = chunk.Name, Data = json });
                    }
                }
                store.ProcessStates(states.ToArray());
            }

            foreach (var interpreter in _controller.GetSaveInterpreters()) {
                interpreter.ProcessChunks(_snapshot);
            }
        }
    }
}
