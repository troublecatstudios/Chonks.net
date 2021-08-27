namespace Chonks {
    public interface ISaveManager {
        /// <summary>
        /// Makes a new snapshot.
        /// </summary>
        void MakeSnapshot();

        /// <summary>
        /// Applies the most recently created snapshot to a container.
        /// </summary>
        /// <param name="container">The container.</param>
        void ApplySnapshot(SaveContainer container);

        /// <summary>
        /// Loads a snapshot from a container.
        /// </summary>
        /// <param name="container">The container.</param>
        void LoadSnapshot(SaveContainer container);
    }
}
