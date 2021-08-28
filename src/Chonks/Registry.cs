using System.Collections.Generic;

namespace Chonks {
    public class Registry<T> : IRegistry<T> {
        private readonly List<T> _items = new List<T>();

        public event ItemRegistered<T> OnItemRegistered;

        public T[] List() {
            Prune();
            return _items.ToArray();
        }

        public void Register(T instance) {
            if (!_items.Contains(instance)) {
                _items.Add(instance);
                OnItemRegistered?.Invoke(instance);
            }
        }

        private void Prune() {
            int idx = 0;
            while (idx < _items.Count) {
                if (_items[idx] == null) {
                    _items.RemoveAt(idx);
                    continue;
                }
                idx++;
            }
        }

        public void Unregister(T instance) {
            _items.Remove(instance);
        }
    }
}
