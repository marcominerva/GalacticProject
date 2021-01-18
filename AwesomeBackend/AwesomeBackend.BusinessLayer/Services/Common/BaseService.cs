using AwesomeBackend.DataAccessLayer;
using Microsoft.Extensions.Logging;

namespace AwesomeBackend.BusinessLayer.Services.Common
{
    public abstract class BaseService
    {
        protected IApplicationDbContext DataContext { get; }

        protected ILogger Logger { get; }

        public BaseService(IApplicationDbContext dataContext, ILogger logger)
        {
            DataContext = dataContext;
            Logger = logger;
        }
    }
}
