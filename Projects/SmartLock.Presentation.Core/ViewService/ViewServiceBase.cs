using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SmartLock.Infrastructure;
using SmartLock.Model.Services;
using SmartLock.Model.Views;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;

namespace SmartLock.Presentation.Core.ViewService
{
    /// <summary>
    /// Platform agnostic view service implementation which will manage pushing views, creating view controllers
    /// and displaying any errors.
    /// Parent classes must implement the platform specific loading, pushing and popping of views.
    /// </summary>
    public abstract class ViewServiceBase : IViewService
    {
        private readonly IMessageBoxService _messageBoxService;

        protected ViewServiceBase(IMessageBoxService messageBoxService)
        {
            _messageBoxService = messageBoxService;
        }

        /// <summary>
        /// Pop the current view off the screen/stack.
        /// </summary>
        public abstract void Pop();

        /// <summary>
        /// Pop to the root view.
        /// </summary>
        public abstract void PopToRoot();

        /// <summary>
        /// Create and push the view assosciated with <see cref="TViewController"/>.
        /// </summary>
        /// <typeparam name="TViewController">The type of view controller we wish to push.</typeparam>
        /// <param name="viewController">The instance of the view controller of type <see cref="TViewController"/></param>
        /// <param name="animated">Whether or not to animate the view push.</param>
        protected abstract void PushViewFor<TViewController>(TViewController viewController, bool animated = true) where TViewController : class;

        /// <summary>
        /// Create and push the view controller using the view service
        /// Displays an error to the user if this fails for whatever reason, to avoid a crash.
        /// </summary>
        public void Push<TViewController>(Action<TViewController> viewControllerCreated = null, bool animated = true) where TViewController : class
        {
            try
            {
                var vc = CreateViewController<TViewController>();
                viewControllerCreated?.Invoke(vc);
                PushViewFor(vc, animated);
            }
            catch (Exception exception)
            {
                // TODO Log?
                _messageBoxService.ShowMessage("Error loading view", exception.Message);
            }
        }

        public abstract Type ResolveViewImplementation<TView>() where TView : class, IView;
        public abstract Type ResolveViewImplementation(Type viewType);

        private TViewController CreateViewController<TViewController>() where TViewController : class
        {
            var controllerType = typeof(TViewController);
            var constructor = controllerType.GetTypeInfo().DeclaredConstructors.First();
            var parameters = constructor.GetParameters();

            TViewController vc;

            if (!parameters.Any())
            {
                vc = Activator.CreateInstance(controllerType) as TViewController;
            }
            else
            {
                vc = constructor.Invoke(ResolveParameters(parameters).ToArray()) as TViewController;
            }

            return vc;
        }

        private static IEnumerable<object> ResolveParameters(IEnumerable<ParameterInfo> parameters)
        {
            return parameters
                .Select(p => IoC.Resolve(p.ParameterType))
                .ToList();
        }

        /// <summary>
        /// Holder for registered views
        /// </summary>
        private class ViewServiceEntry
        {
            public Type Controller { get; set; }

            public Type ViewImplementation { get; set; }
        }
    }
}
