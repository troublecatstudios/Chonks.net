namespace Chonks {
    public interface ISaveManager {
        void MakeSnapshot();
        void ApplySnapshot(SaveContainer container);
        void LoadSnapshot(SaveContainer container);
    }
}
