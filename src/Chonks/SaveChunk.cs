using System;
using System.Collections.Generic;

namespace Chonks {

    [Serializable]
    public class SaveChunk {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveChunk"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public SaveChunk(string name = "") {
            Name = name;
        }

        public override int GetHashCode() {
            return $"{Id}-{Name}".GetHashCode();
        }


        /// <summary>
        /// Adds the data to the chunk.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        public void AddToChunk(string key, object data) {
            var json = SerializationUtility.SerializeObject(data);
            Data.Add(key, json);
        }
    }
}
