using Chonks.Depots;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chonks.SaveManagement {
    public class DefaultSaveManager : ISaveManager {
        private readonly ISaveDepot _depot;

        private readonly IRegistry<ISaveStore> _storeRegistry;
        private readonly IRegistry<ISaveInterpreter> _interpreterRegistry;

        private SaveChunk[] _snapshot;

        public DefaultSaveManager(IRegistry<ISaveStore> storeRegistry = null, IRegistry<ISaveInterpreter> interpreterRegistry = null, ISaveDepot depot = null) {
            _depot = depot ?? new InMemorySaveDepot();
            _storeRegistry = storeRegistry ?? new Registry<ISaveStore>();
            _interpreterRegistry = interpreterRegistry ?? new Registry<ISaveInterpreter>();

            _interpreterRegistry.OnItemRegistered += _interpreterRegistry_OnItemRegistered;
            _storeRegistry.OnItemRegistered += _storeRegistry_OnItemRegistered;
        }

        private void _storeRegistry_OnItemRegistered(ISaveStore item) {
            // notify any newly registered stores of data in the save for them
            var id = item.GetStoreIdentifier();
            if (_snapshot == null) return;
            foreach (var chunk in _snapshot) {
                if (chunk.Data.TryGetValue(id, out var json)) {
                    var segment = new ChunkDataSegment(json);
                    item.LoadChunkData(chunk.Name, segment, out _);
                }
            }
        }

        private void _interpreterRegistry_OnItemRegistered(ISaveInterpreter item) {
            if (_snapshot == null) return;
            item.ProcessChunks(_snapshot);
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
            foreach (var store in _storeRegistry.List()) {
                var id = store.GetStoreIdentifier();
                var states = store.GetSaveStates();
                foreach (var state in states) {
                    if (state == null) continue;
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

            foreach (var interpreter in _interpreterRegistry.List()) {
                interpreter.ProcessChunks(_snapshot);
            }
        }

        private SaveChunk[] ProcessInterpreters(SaveChunk[] chunks) {
            foreach (var interpreter in _interpreterRegistry.List()) {
                if (!interpreter.IsDirty()) continue;
                chunks = interpreter.ApplyModifications(chunks);
            }
            return chunks;
        }

        private void ProcessSnapshotUpdate() {
            var postChunkLoadCallbacks = new List<Action>();
            foreach (var store in _storeRegistry.List()) {
                var id = store.GetStoreIdentifier();
                foreach (var chunk in _snapshot) {
                    if (chunk.Data.TryGetValue(id, out var json)) {
                        var segment = new ChunkDataSegment(json);
                        Action callback = null;
                        store.LoadChunkData(chunk.Name, segment, out callback);

                        if (callback != null) postChunkLoadCallbacks.Add(callback);
                    }
                }
            }

            foreach (var callback in postChunkLoadCallbacks) {
                callback?.Invoke();
            }

            foreach (var interpreter in _interpreterRegistry.List()) {
                interpreter.ProcessChunks(_snapshot);
            }
        }
    }
}
