using System;

namespace Chonks {
    public struct SaveContainer {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the created at date.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last updated at date.
        /// </summary>
        /// <value>
        /// The last updated at.
        /// </value>
        public DateTime LastUpdatedAt { get; set; }
    }
}
