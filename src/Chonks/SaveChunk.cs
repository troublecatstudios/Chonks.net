using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Chonks {

    [Serializable]
    public struct SaveChunk {
        private readonly static JsonSerializer _serializer = JsonSerializer.Create();
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public SaveChunk(string name) {
            Name = name;
            Id = Guid.NewGuid();
            Data = new Dictionary<string, string>();
        }

        public override int GetHashCode() {
            return $"{Id}-{Name}".GetHashCode();
        }

        public void AddToChunk(string key, object data) {
            var json = JsonConvert.SerializeObject(data);
            Data.Add(key, json);
        }
    }
}
