namespace Chonks {
    public interface ISaveInterpreter {
        void ProcessChunks(SaveChunk[] chunks);
        SaveChunk[] ApplyModifications(SaveChunk[] chunks);
        bool IsDirty();
    }
}
