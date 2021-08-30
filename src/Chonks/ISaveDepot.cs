using System;

namespace Chonks {
    public interface ISaveDepot {
        /// <summary>
        /// Lists the saves.
        /// </summary>
        /// <returns></returns>
        SaveContainer[] ListSaves();

        /// <summary>
        /// Tries to load a save.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="chunks">The chunks.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        bool TryLoadSave(string name, out SaveChunk[] chunks, out Exception ex);

        /// <summary>
        /// Tries to write a save.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="chunks">The chunks.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        bool TryWriteSave(string name, SaveChunk[] chunks, out Exception ex);

        /// <summary>
        /// Tries to the clear save.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        bool TryClearSave(string name, out Exception ex);
    }
}
