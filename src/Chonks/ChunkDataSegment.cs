using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Chonks {
    public class ChunkDataSegment {
        private readonly Dictionary<string, object> _container = new Dictionary<string, object>();
        public ChunkDataSegment() { }
        public ChunkDataSegment(string json) {
            _container = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        public string ToJson() {
            return JsonConvert.SerializeObject(_container);
        }

        public Type CheckType(string key) => ContainsKey(key) ? _container[key].GetType() : null;

        public bool ContainsKey(string key) => _container.ContainsKey(key);

        public bool TryAdd(string key, object value) {
#if NETSTANDARD2_0
            if (_container.ContainsKey(key)) return false;
            _container.Add(key, value);
            return true;
#else
            return _container.TryAdd(key, value);
#endif
        }

        public bool TryGetInt(string key, out int integer) {
            integer = -1;
            if (_container.TryGetValue(key, out var value)) {
                integer = Convert.ToInt32(value);
                return true;
            }
            return false;
        }

        public T Get<T>(string key) {
            if (!ContainsKey(key)) {
                throw new KeyNotFoundException($"The key {key} was not found in the collection.");
            }

            object box;
            T check = default;

            // int is a difficult conversion since the default
            // for newtonsoft.json is Int64
            if (check is int) {
                box = Convert.ToInt32(_container[key]);
                return (T)box;
            }

            return (T)_container[key];
        }

        public bool TryGetBool(string key, out bool boolean) {
            boolean = false;
            if (_container.TryGetValue(key, out var value)) {
                boolean = (bool)value;
                return true;
            }
            return false;
        }

        public bool TryGetString(string key, out string stringValue) {
            stringValue = null;
            if (_container.TryGetValue(key, out var value)) {
                stringValue = value.ToString();
                return true;
            }
            return false;
        }
    }
}
