using System;
using SmartLock.Infrastructure;
using SmartLock.Logic;
using SmartLock.Model;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.ViewService;
using SmartLock.Presentation.Core.ViewControllers;

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

        /// <summary>
        /// Registers any app (platform specific) services, such as <see cref="IViewService"/>
        /// </summary>
		protected abstract void RegisterPlatformServices();
	}
}
