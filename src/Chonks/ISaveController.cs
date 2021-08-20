namespace Chonks {
    public interface ISaveController {
        ISaveStore[] GetSaveStores();
        void RegisterSaveStore(ISaveStore store);
        void UnregisterSaveStore(ISaveStore store);

        ISaveInterpreter[] GetSaveInterpreters();
        void RegisterSaveInterpreter(ISaveInterpreter interpreter);
        void UnregisterSaveInterpreter(ISaveInterpreter interpreter);
    }
}
