using Chonks.Depots;
using Chonks.SaveManagement;
using UnityEngine;

namespace Chonks.Unity {
    [AddComponentMenu("Chonks/Unity/Unity Save Manager")]
    public class UnitySaveManager : UnitySingleton<UnitySaveManager> {
        private ISaveDepot _depot;
        private IRegistry<ISaveStore> _storeRegistry = new Registry<ISaveStore>();
        private IRegistry<ISaveInterpreter> _interpreterRegistry = new Registry<ISaveInterpreter>();
        private ISaveManager _manager;

        public void Register(ISaveStore store) => _storeRegistry.Register(store);

        public void Unregister(ISaveStore store) => _storeRegistry.Unregister(store);

        public void Register(ISaveInterpreter interpreter) => _interpreterRegistry.Register(interpreter);

        public void Unregister(ISaveInterpreter interpreter) => _interpreterRegistry.Unregister(interpreter);

        public void MakeSnapshot() => _manager.MakeSnapshot();

        public void ApplySnapshot(string saveName) => _manager.ApplySnapshot(new SaveContainer() { Name = saveName });

        protected override void SingletonAwake() {
            _depot = new InMemorySaveDepot();
            _manager = new DefaultSaveManager(_storeRegistry, _interpreterRegistry, _depot);
        }
    }
}
