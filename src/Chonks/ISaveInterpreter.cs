namespace Chonks {
    public interface ISaveInterpreter {
        /// <summary>
        /// Processes chunks from a save file.
        /// </summary>
        /// <param name="chunks">The chunks.</param>
        void ProcessChunks(SaveChunk[] chunks);

        /// <summary>
        /// Applies any modifications made to the chunks.
        /// </summary>
        /// <param name="chunks">The chunks.</param>
        /// <returns></returns>
        SaveChunk[] ApplyModifications(SaveChunk[] chunks);

        /// <summary>
        /// Determines whether this instance is dirty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </returns>
        bool IsDirty();
    }
}
