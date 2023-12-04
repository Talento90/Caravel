namespace Caravel.ApplicationContext;

/// <summary>
/// Provides access to the current <see cref="ApplicationContext"/>, if one is available.
/// </summary>
public interface IApplicationContextAccessor
{
    /// <summary>
    /// Gets or sets the current <see cref="ApplicationContext"/>. Returns <see langword="null" /> if there is no active <see cref="ApplicationContext" />.
    /// </summary>
    ApplicationContext Context { get; set; }
}