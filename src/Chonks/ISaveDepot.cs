namespace Chonks {
    public interface ISaveDepot {
        SaveContainer[] ListSaves();
        bool TryLoadSave(string name, out SaveChunk[] chunks);
        bool TryWriteSave(string name, SaveChunk[] chunks);
    }
}
