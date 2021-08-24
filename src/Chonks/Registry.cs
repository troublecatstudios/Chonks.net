using System.Collections.Generic;

namespace Chonks {
    public class Registry<T> : IRegistry<T> {
        private readonly List<T> _items = new List<T>();

        public event ItemRegistered<T> OnItemRegistered;

        public T[] List() {
            return _items.ToArray();
        }

        public void Register(T instance) {
            if (!_items.Contains(instance)) {
                _items.Add(instance);
                OnItemRegistered?.Invoke(instance);
            }
        }

        public void Unregister(T instance) {
            _items.Remove(instance);
        }
    }
}
