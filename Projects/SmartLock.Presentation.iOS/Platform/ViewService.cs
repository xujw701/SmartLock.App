using System;
using System.Collections.Generic;
using SmartLock.Presentation.iOS.Views;
using SmartLock.Presentation.Core;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.ViewService;
using UIKit;

namespace SmartLock.Presentation.iOS.Platform
{
    public class ViewService : ViewServiceBase
    {
        private UINavigationController _modalView;

        private static Dictionary<Type, Func<object, UIViewController>> _viewDictionary = new Dictionary<Type, Func<object, UIViewController>>();
        private static Dictionary<Type, Func<object, UIViewController>> _dialogViewDictionary = new Dictionary<Type, Func<object, UIViewController>>();

        public ViewService(IMessageBoxService messageBoxService) : base(messageBoxService)
        {
            /*
             *          View Registration
             *          - All views (with the exception of custom view) must be registered with their associated controller here
             *          
             */

            _viewDictionary.Add(typeof(LoginController), vc => new LoginView(vc as LoginController));

            _viewDictionary.Add(typeof(MainController), vc => new MainView(vc as MainController));
            _viewDictionary.Add(typeof(HomeController), vc => new HomeView(vc as HomeController));
            _viewDictionary.Add(typeof(KeyboxesController), vc => new MyLockView(vc as KeyboxesController));
            _viewDictionary.Add(typeof(ListingController), vc => new LogsView(vc as ListingController));
            _viewDictionary.Add(typeof(SettingController), vc => new SettingView(vc as SettingController));

            _viewDictionary.Add(typeof(NearbyController), vc => new PairingView(vc as NearbyController));
        }

        public override void Pop()
        {
            if (_modalView != null)
            {

                if (_modalView.ViewControllers.Length == 1)
                {
                    AppDelegate.NavigationController.DismissModalViewController(true);
                    _modalView = null;
                }
                else
                {
                    _modalView.PopViewController(true);
                }

                return;
            }

            AppDelegate.NavigationController.PopViewController(true);
        }

        public override void PopToRoot()
        {
            AppDelegate.NavigationController.PopToRootViewController(true);
        }

        protected override void PushViewFor<T>(T viewController, bool animated = true)
        {
            var viewType = typeof(T);

            // Check for view registration
            if (!_viewDictionary.ContainsKey(viewType) && !_dialogViewDictionary.ContainsKey(viewType))
            {
                throw new Exception("View for type " + viewType + " not found.");
            }

            // Display registered view (as normal or dialog)

            if (_dialogViewDictionary.ContainsKey(viewType))
            {
                ShowDialog(viewType, viewController, animated);
                return;
            }

            var view = _viewDictionary[viewType](viewController);

            if (_modalView != null)
            {
                _modalView.NavigationController.PushViewController(view, animated);
            }
            else
            {
                AppDelegate.NavigationController.PushViewController(view, animated);
            }
        }

        private void ShowDialog<T>(Type viewType, T viewController, bool animated)
        {
            var view = _dialogViewDictionary[viewType](viewController);
            
            if (_modalView == null)
            {
                _modalView = new UINavigationController(view);
                AppDelegate.NavigationController.PresentModalViewController(_modalView, animated);
            }
            else
            {
                _modalView.PushViewController(view, animated);
            }
        }

        public static UIViewController InstanstantiateView<T>(T viewController)
        {
            var viewType = viewController.GetType();

            // Check for view registration
            if (!_viewDictionary.ContainsKey(viewType) && !_dialogViewDictionary.ContainsKey(viewType))
            {
                throw new Exception("View for type " + viewType + " not found.");
            }

            var view = _viewDictionary[viewType](viewController);

            return view;
        }

        public override Type ResolveViewImplementation<TView>()
        {
            throw new NotImplementedException();
        }

        public override Type ResolveViewImplementation(Type viewType)
        {
            throw new NotImplementedException();
        }
    }
}
