using Chonks.Depots;
using Chonks.SaveManagement;
using UnityEngine;

namespace Chonks.Unity {
    [AddComponentMenu("Chonks/Unity/Unity Save Manager")]
    public class UnitySaveManager : UnitySingleton<UnitySaveManager> {
        private ISaveDepot _depot;
        private ISaveController _controller = new DefaultSaveController();
        private ISaveManager _manager;

        public void Register(ISaveStore store) => _controller.RegisterSaveStore(store);

        public void Unregister(ISaveStore store) => _controller.UnregisterSaveStore(store);

        public void Register(ISaveInterpreter interpreter) => _controller.RegisterSaveInterpreter(interpreter);

        public void Unregister(ISaveInterpreter interpreter) => _controller.UnregisterSaveInterpreter(interpreter);

        public void MakeSnapshot() => _manager.MakeSnapshot();

        public void ApplySnapshot(string saveName) => _manager.ApplySnapshot(new SaveContainer() { Name = saveName });

        protected override void SingletonAwake() {
            _depot = new InMemorySaveDepot();
            _manager = new DefaultSaveManager(_controller, _depot);
        }
    }
}
