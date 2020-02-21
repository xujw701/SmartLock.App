using System;
using System.Linq;
using System.Reflection;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Infrastructure;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Core.ViewService;

namespace SmartLock.Presentation.Droid.Views.ViewBases
{
    public abstract class FragmentView<TViewImplementation> : Fragment, IView where TViewImplementation : class, IView
    {
        public event EventHandler Closed;

        public bool DisplayTitle { get; set; }

        public string Subtitle { get; set; }

        public ViewController<TViewImplementation> ViewController => _controller;

        protected virtual int LayoutId => throw new System.Exception("Invalid layout set");

        protected virtual bool BlockBackPress => false;

        protected View _view;

        private bool _isBusy;

        private ViewController<TViewImplementation> _controller;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;

                if (_view != null)
                {
                    var progressOverlay = _view.FindViewById<FrameLayout>(Resource.Id.progressOverlay);
                    if (progressOverlay != null)
                    {
                        progressOverlay.Visibility = _isBusy ? ViewStates.Visible : ViewStates.Gone;
                    }

                    //var swipeRefreshLayout = _view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
                    //if (swipeRefreshLayout != null)
                    //{
                    //    swipeRefreshLayout.Refreshing = _isBusy;
                    //}
                }
            }
        }

        public string Title
        {
            get
            {
                //var title = _view.FindViewById<TextView>(Resource.Id.toolbarTitle);
                //if (title != null) return title.Text;
                return "";
            }
            set
            {
                var title = "";// _view.FindViewById<TextView>(Resource.Id.toolbarTitle);
                if (title != null)
                {
                    //title.Text = value;
                }
            }
        }

        protected Fragment LoadFragment<TView>(ViewController<TView> controller) where TView : class, IView
        {
            var viewImplementation = IoC.Resolve<IViewService>().ResolveViewImplementation(controller.GetType());
            var constructor = viewImplementation.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.GetParameters().Length == 0) ?? throw new Exception(typeof(TView) + " view must have empty constructor");

            var instance = Activator.CreateInstance(viewImplementation);

            if (instance is FragmentView<TView> fragment)
            {
                fragment.SetController(controller);
                return fragment;
            }

            throw new Exception("Tab must inherit from " + typeof(FragmentView<TView>));
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            // Inflate view and let the controller know that the view is ready once setup
            _view = inflater.Inflate(LayoutId, null);

            _controller.SetView(this as TViewImplementation);

            return _view;
        }

        public override void OnViewStateRestored(Bundle savedInstanceState)
        {
            base.OnViewStateRestored(savedInstanceState);

            _controller.NotifyViewWillShow(this as TViewImplementation);
        }

        public void SetController(ViewController<TViewImplementation> controller)
        {
            var implementation = this as TViewImplementation;

            if (implementation == null)
            {
                throw new Exception("Frament must inherit " + typeof(TViewImplementation));
            }

            _controller = controller;
        }
    }
}