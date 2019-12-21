using System.Threading;

namespace Caravel.AppContext
{
    public class AppContextAccessor : IAppContext
    {
        private static readonly AsyncLocal<AppContextHolder> AppContextCurrent = new AsyncLocal<AppContextHolder>();

        public AppContext Context
        {
            get => AppContextCurrent?.Value?.Context ?? new AppContext(string.Empty, string.Empty);
            set
            {
                var holder = AppContextCurrent.Value;

                if (holder != null)
                {
                    holder.Context = null;
                }

                if (value != null)
                {
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