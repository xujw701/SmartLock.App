using System;
using SmartLock.Model.Views;

namespace SmartLock.Model.Services
{
    /// <summary>
    /// Platform specific, fundamental app view service which handles the view stack and creation of views.
    /// </summary>
    public interface IViewService
    {
        /// <summary>
        /// Creates a view controller and associated view (platform implemented), pushing the view on to the view stack.
        /// </summary>
        /// <typeparam name="TViewController">The type of view controller to create. Must have a public constructor!</typeparam>
        /// <param name="viewControllerCreated">Action to call when the view controller is created, called before the view is pushed on to the stack.</param>
        /// <param name="animated">Whether or not to animate the view push.</param>
        void Push<TViewController>(Action<TViewController> viewControllerCreated = null, bool animated = true) where TViewController : class;

        /// <summary>
        /// Pops the current view off the view stack.
        /// </summary>
        void Pop();

        /// <summary>
        /// Pops back to the root view controller.
        /// </summary>
        void PopToRoot();

        void Resume();

        Type ResolveViewImplementation<TView>() where TView : class, IView;

        Type ResolveViewImplementation(Type viewType);
    }
}