using System;
using System.Threading;

namespace Caravel.AppContext
{
    /// <summary>
    /// Provides an implementation of <see cref="IAppContextAccessor" /> based on the current execution context.
    ///
    /// This implementation is based on: https://github.com/dotnet/aspnetcore/blob/master/src/Http/Http/src/HttpContextAccessor.cs 
    /// </summary>
    public class AppContextAccessor : IAppContextAccessor
    {
        private static readonly AsyncLocal<AppContextHolder> AppContextCurrent = new AsyncLocal<AppContextHolder>();

        /// <summary>
        /// Gets or sets the current <see cref="AppContext"/>. Returns <see langword="null" /> if there is no active <see cref="AppContext" />.
        /// </summary>
        public AppContext Context
        {
            get => AppContextCurrent?.Value?.Context ?? new AppContext(string.Empty);
            set
            {
                var holder = AppContextCurrent.Value;

                if (holder != null)
                {
                    // Clear current AppContext trapped in the AsyncLocals, as its done.
                    holder.Context = null;
                }

                if (value != null)
                {
                    // Use an object indirection to hold the AppContext in the AsyncLocal,
                    // so it can be cleared in all ExecutionContexts when its cleared.
                    AppContextCurrent.Value = new AppContextHolder {Context = value};
                }
            }
        }

        private class AppContextHolder
        {
            public AppContext? Context;
        }
    }
}