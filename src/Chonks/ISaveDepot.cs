using System;

namespace Chonks {
    public interface ISaveDepot {
        SaveContainer[] ListSaves();
        bool TryLoadSave(string name, out SaveChunk[] chunks, out Exception ex);
        bool TryWriteSave(string name, SaveChunk[] chunks, out Exception ex);
    }
}
