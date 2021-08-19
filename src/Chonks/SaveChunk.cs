using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Chonks {

    [Serializable]
    public class SaveChunk {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();

        public SaveChunk(string name = "") {
            Name = name;
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
