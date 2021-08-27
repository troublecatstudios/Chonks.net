using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Chonks {
    public class ChunkDataSegment {
        private readonly Dictionary<string, object> _container = new Dictionary<string, object>();
        private readonly string _json;
        private JObject _jobject;
        public ChunkDataSegment() { }
        public ChunkDataSegment(string json) {
            _container = SerializationUtility.DeserializeObject<Dictionary<string, object>>(json);
            _json = json;
            _jobject = JObject.Parse(json);
        }

        public string ToJson() => _json;

        public T As<T>() => SerializationUtility.DeserializeObject<T>(_json);

        public Type CheckType(string key) => ContainsKey(key) ? _container[key].GetType() : null;

        public bool ContainsKey(string key) => _container.ContainsKey(key);

        public T Get<T>(string key) {
            if (!ContainsKey(key)) {
                throw new KeyNotFoundException($"The key {key} was not found in the collection.");
            }

            if (_jobject.ContainsKey(key)) {
                var json = _jobject[key].ToString(Newtonsoft.Json.Formatting.None);
                return SerializationUtility.DeserializeObject<T>(json);
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

    }
}
