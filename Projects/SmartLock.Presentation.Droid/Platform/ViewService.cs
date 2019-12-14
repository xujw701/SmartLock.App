using System;
using System.Collections.Generic;
using SmartLock.Presentation.Core.ViewService;
using SmartLock.Presentation.Droid.Views;
using Android.Content;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core;
using SmartLock.Presentation.Droid.Views.ViewBases;

namespace SmartLock.Presentation.Droid.Platform
{
	public class ViewService : ViewServiceBase
	{
        public const string PreviousViewTypeKey = "ViewService.From";
        public static object CurrentViewController => ControllerStack[ControllerStack.Count - 1];
        public static List<object> ControllerStack = new List<object>();

        private readonly IMessageBoxService _messageBoxService;
	    private object _rootViewController;

        private readonly Dictionary<Type, Type> _viewcontrollerMappings = new Dictionary<Type, Type>();
        
        public ViewService(IMessageBoxService messageBoxService) : base(messageBoxService)
        {
            _messageBoxService = messageBoxService;

            // Auth
            _viewcontrollerMappings.Add(typeof(LoginController), typeof(LoginView));

            // Main Pages
            _viewcontrollerMappings.Add(typeof(MainController), typeof(MainView));
            _viewcontrollerMappings.Add(typeof(HomeController), typeof(HomeView));
            _viewcontrollerMappings.Add(typeof(KeyboxesController), typeof(KeyboxesView));
            _viewcontrollerMappings.Add(typeof(ListingController), typeof(ListingView));
            _viewcontrollerMappings.Add(typeof(SettingController), typeof(SettingView));
            _viewcontrollerMappings.Add(typeof(NearbyController), typeof(NearbyView));

            _viewcontrollerMappings.Add(typeof(KeyboxDetailController), typeof(KeyboxDetailView));
            _viewcontrollerMappings.Add(typeof(KeyboxDashboardController), typeof(KeyboxDashboardView));
        }

        public override void Pop()
        {
            ControllerStack.RemoveAt(ControllerStack.Count - 1);
            ViewBase.CurrentActivity.Finish();
        }

        public override void PopToRoot()
        {
            ControllerStack.Clear();
            ControllerStack.Add(_rootViewController);

            Intent intent = new Intent(ViewBase.CurrentActivity, typeof(MainView));
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            ViewBase.CurrentActivity.StartActivity(intent);
        }
        
        protected override void PushViewFor<TViewController>(TViewController viewController, bool animated = true)
        {
            ControllerStack.Add(viewController);

            if (_rootViewController == null)
            {
                _rootViewController = viewController;
            }

            if (_viewcontrollerMappings.ContainsKey(viewController.GetType()))
            {
                var viewType = _viewcontrollerMappings[viewController.GetType()];
                var intent = new Intent(ViewBase.CurrentActivity, viewType);
                intent.PutExtra(PreviousViewTypeKey, ViewBase.CurrentActivity.GetType().ToString());
                ViewBase.CurrentActivity?.StartActivity(intent);
            }
            else 
            {
                _messageBoxService.ShowMessage("Navigation error", "View does not exist for controller: " + viewController.GetType());
            }
        }

        public override Type ResolveViewImplementation(Type viewType)
        {
            if (!_viewcontrollerMappings.ContainsKey(viewType))
            {
                throw new Exception(viewType + " has not been registered");
            }

            return _viewcontrollerMappings[viewType];
        }

        public override Type ResolveViewImplementation<TView>()
        {
            return ResolveViewImplementation(typeof(TView));
        }
    }
}