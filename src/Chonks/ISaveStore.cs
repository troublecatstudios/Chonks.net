using System.Collections.Generic;

namespace Chonks {
    public class SaveState {
        public string ChunkName { get; set; }
        public object Data { get; set; }
    }

    public interface ISaveStore {
        string GetStoreIdentifier();
        List<SaveState> CreateSaveStates();
        void ProcessChunkData(string chunkName, ChunkDataSegment data);
    }
}
