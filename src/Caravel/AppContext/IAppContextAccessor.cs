namespace Caravel.AppContext
{
    /// <summary>
    /// Provides access to the current <see cref="AppContext"/>, if one is available.
    /// </summary>
    public interface IAppContextAccessor
    {
        /// <summary>
        /// Gets or sets the current <see cref="AppContext"/>. Returns <see langword="null" /> if there is no active <see cref="AppContext" />.
        /// </summary>
        AppContext Context { get; set; }
    }
}