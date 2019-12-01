using System;
using System.Linq;
using System.Reflection;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using SmartLock.Infrastructure;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Droid.Views.ViewBases
{
    public abstract class FragmentContainerView<TViewInterface> : ViewBase<TViewInterface> where TViewInterface : class, IView
    {
        protected abstract int FragmentContainerId { get; }

        /// <summary>
        ///  Displays a fragment for a controller on the view.
        /// </summary>
        protected void DisplayFragment<TView>(ViewController<TView> controller) where TView : class, IView
        {
            var fragment = LoadFragment(controller);

            SupportFragmentManager.BeginTransaction()
                                  .Replace(FragmentContainerId, fragment)
                                  .Commit();
        }


        protected Fragment LoadFragment<TView>(ViewController<TView> controller) where TView : class, IView
        {

            var viewImplementation = IoC.Resolve<IViewService>().ResolveViewImplementation(controller.GetType());
            var constructor = viewImplementation.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Length == 0) ?? throw new Exception("Tab view must have empty constructor");

            var instance = Activator.CreateInstance(viewImplementation);

            if (instance is FragmentView<TView> fragment)
            {
                fragment.SetController(controller);
                return fragment;
            }

            throw new Exception("Tab must inherit from " + typeof(FragmentView<TView>));
        }
    }
}