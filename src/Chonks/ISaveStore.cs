using System;
using System.Collections.Generic;

namespace Chonks {
    [Serializable]
    public class SaveState {
        /// <summary>
        /// Gets or sets the name of the chunk.
        /// </summary>
        /// <value>
        /// The name of the chunk.
        /// </value>
        public string ChunkName { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public object Data { get; set; }
    }

    public interface ISaveStore {
        /// <summary>
        /// Gets the store identifier.
        /// </summary>
        /// <returns></returns>
        string GetStoreIdentifier();

        /// <summary>
        /// Gets the save states for each chunk.
        /// </summary>
        /// <returns></returns>
        List<SaveState> GetSaveStates();
        /// <summary>
        /// Loads the chunk data.
        /// </summary>
        /// <param name="chunkName">Name of the chunk.</param>
        /// <param name="data">The data.</param>
        /// <param name="onAllChunksLoadedCallback">The callback (if any) to call when all chunks have been loaded. Note: this is not called if the store is recieving chunk data outside of a snapshot creation.</param>
        void LoadChunkData(string chunkName, ChunkDataSegment data, out Action onAllChunksLoadedCallback);
    }
}
