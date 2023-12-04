namespace Caravel.ApplicationContext;

/// <summary>
/// Provides an implementation of <see cref="IApplicationContextAccessor" /> based on the current execution context.
///
/// This implementation is based on: https://github.com/dotnet/aspnetcore/blob/master/src/Http/Http/src/HttpContextAccessor.cs 
/// </summary>
public record ApplicationContextAccessor : IApplicationContextAccessor
{
    private static readonly AsyncLocal<AppContextHolder> AppContextCurrent = new();

    /// <summary>
    /// Gets or sets the current <see cref="ApplicationContext"/>. Returns <see langword="null" /> if there is no active <see cref="ApplicationContext" />.
    /// </summary>
    public ApplicationContext Context
    {
        get => AppContextCurrent?.Value?.Context ?? new Caravel.ApplicationContext.ApplicationContext(string.Empty);
        set
        {
            var holder = AppContextCurrent.Value;

            if (holder != null)
            {
                // Clear current AppContext trapped in the AsyncLocals, as its done.
                holder.Context = null;
            }
            
            // Use an object indirection to hold the AppContext in the AsyncLocal,
            // so it can be cleared in all ExecutionContexts when its cleared.
            AppContextCurrent.Value = new AppContextHolder {Context = value};
        }
    }

    private sealed record AppContextHolder
    {
        public ApplicationContext? Context;
    }
}