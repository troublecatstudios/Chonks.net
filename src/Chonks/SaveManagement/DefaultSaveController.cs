using System.Collections.Generic;

namespace Chonks.SaveManagement {
    public class DefaultSaveController : ISaveController {
        private readonly List<ISaveStore> _stores = new List<ISaveStore>();
        private readonly List<ISaveInterpreter> _interpreters = new List<ISaveInterpreter>();

        public ISaveInterpreter[] GetSaveInterpreters() => _interpreters.ToArray();

        public ISaveStore[] GetSaveStores() => _stores.ToArray();

        public void RegisterSaveInterpreter(ISaveInterpreter interpreter) {
            if (!_interpreters.Contains(interpreter)) _interpreters.Add(interpreter);
        }

        public void RegisterSaveStore(ISaveStore store) {
            if (!_stores.Contains(store)) _stores.Add(store);
        }

        public void UnregisterSaveInterpreter(ISaveInterpreter interpreter) => _interpreters.Remove(interpreter);

        public void UnregisterSaveStore(ISaveStore store) => _stores.Remove(store);
    }
}
