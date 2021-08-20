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
            if (_depot.TryLoadSave(container.Name, out var chunks, out _)) {
                _snapshot = chunks;
                ProcessSnapshotUpdate();
            }
        }

        public void ApplySnapshot(SaveContainer container) {
            if (_snapshot == null) return;
            var snapshot = ProcessInterpreters(_snapshot);
            if (_depot.TryWriteSave(container.Name, snapshot, out _)) {
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
                        chunks.Add(chunk.Name, chunk);
                    }
                    chunk.AddToChunk(id, state.Data);
                    chunks[chunk.Name] = chunk;
                }
            }

            _snapshot = chunks.Select((pair) => pair.Value).ToArray();

            foreach (var interpreter in _controller.GetSaveInterpreters()) {
                interpreter.ProcessChunks(_snapshot);
            }
        }

        private SaveChunk[] ProcessInterpreters(SaveChunk[] chunks) {
            foreach (var interpreter in _controller.GetSaveInterpreters()) {
                if (!interpreter.IsDirty()) continue;
                chunks = interpreter.ApplyModifications(chunks);
            }
            return chunks;
        }

        private void ProcessSnapshotUpdate() {
            foreach (var store in _controller.GetSaveStores()) {
                var id = store.GetStoreIdentifier();
                foreach (var chunk in _snapshot) {
                    if (chunk.Data.TryGetValue(id, out var json)) {
                        var segment = new ChunkDataSegment(json);
                        store.ProcessChunkData(chunk.Name, segment);
                    }
                }
            }

            foreach (var interpreter in _controller.GetSaveInterpreters()) {
                interpreter.ProcessChunks(_snapshot);
            }
        }
    }
}
