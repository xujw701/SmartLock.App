using SmartLock.Infrastructure;
using SmartLock.Logic;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Logic.Services;
using SmartLock.Logic.Environment;
using SmartLock.Model.PushNotification;
using SmartLock.Logic.PushNotification;

namespace SmartLock.Presentation.Core
{
    public abstract class AppCore
	{
	    private bool _started;

	    protected AppCore()
		{
			RegisterServices();
        }

        /// <summary>
        /// Registers common services, except playform services.
        /// </summary>
		private void RegisterServices()
		{
			IoC.Register<IWebService, WebServiceFunctions>();
            IoC.Register<IUserService, UserService>();
            IoC.Register<ICacheManager, CacheManager>();

            IoC.RegisterSingleton<IEnvironmentManager, EnvironmentManager>();
            IoC.RegisterSingleton<IUserSession, UserSession>();

            IoC.RegisterSingleton<IKeyboxService, KeyboxService>();

            IoC.RegisterSingleton<IPushNotificationService, PushNotificationService>();
        }

        /// <summary>
        /// Starts the application by showing the first view.
        /// </summary>
		public void Start()
		{
            if (!_started)
            {
                RegisterPlatformServices();
            }

            _started = true;

            IoC.Resolve<IViewService>().Push<LoginController>();
        }

        public void Resume()
        {
            IoC.Resolve<IViewService>().Push<MainController>();
        }

        /// <summary>
        /// Registers any app (platform specific) services, such as <see cref="IViewService"/>
        /// </summary>
		protected abstract void RegisterPlatformServices();
	}
}
