namespace Chonks {

    public delegate void ItemRegistered<T>(T item);

    public interface IRegistry<T> {
        /// <summary>
        /// Occurs when a new item is registered.
        /// </summary>
        event ItemRegistered<T> OnItemRegistered;

        /// <summary>
        /// Lists all the registered items.
        /// </summary>
        /// <returns></returns>
        T[] List();

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Register(T instance);

        /// <summary>
        /// Unregisters the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Unregister(T instance);
    }
}
