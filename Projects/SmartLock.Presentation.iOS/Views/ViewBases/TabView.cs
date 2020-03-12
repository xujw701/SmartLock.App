using System;
using System.Linq;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using UIKit;
using SmartLock.Presentation.iOS.Platform;
using SmartLock.Model.Services;
using SmartLock.Model.Views;

namespace SmartLock.Presentation.iOS.Views.ViewBases
{
    public abstract class TabView<TViewInterface> : UITabBarController, ITabView where TViewInterface : class, IView
    {
        private bool _isBusy;
        private ViewController<TViewInterface> _controller;
        private readonly IViewService _viewService;

        protected TabView(ViewController<TViewInterface> controller)
        {
            // UITabBarController will call ViewDidLoad before the constructor for whatever reason
            // So, now, we assume view has loaded

            _controller = controller;

            _controller.SetView(this as TViewInterface);
        }


        /// <summary>
        /// Shows a busy indicator on the view.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnIsBusyChanged();
            }
        }

        /// <summary>
        /// Detmermines if a full screen busy indicator when <see cref="IsBusy"/> is set on the view.
        /// Otherwise the view must handle is busy on its own.
        /// </summary>
        protected bool FullscreenIsBusy { get; set; } = true;
        public bool DisplayTitle { get; set; }
        public string Subtitle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnIsBusyChanged()
        {
            IsBusyChanged?.Invoke(this, new EventArgs());

            if (!FullscreenIsBusy) return;

            // SetupFullScreenIsBusy was called, so display the full screen overlay
            if (IsBusy)
            {
                //if (_loadingOverlay == null)
                //{
                //    var view = AppDelegate.NavigationController.View;
                //    _loadingOverlay = new LoadingOverlay(new CGRect(0, 0, view.Frame.Width, view.Frame.Height), string.Empty);
                //    view.AddSubview(_loadingOverlay);
                //}
            }
            else
            {
                //if (_loadingOverlay != null)
                //{
                //    _loadingOverlay.Hide();
                //    _loadingOverlay = null;
                //}
            }

        }

        /// <inheritdoc />
        public event EventHandler IsBusyChanged;

        /// <inheritdoc />
        public event EventHandler Unloaded;
        public event EventHandler Closed;

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            Unloaded?.Invoke(this, new EventArgs());
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _controller.NotifyViewWillShow(this as TViewInterface);
        }

        /// <summary>
        /// Loads a view implementation for a given view controler.
        /// </summary>
        /// <typeparam name="TView">The type of view to load.</typeparam>
        /// <param name="controller">The controller for the view.</param>
        /// <returns>An instanstantiaed view.</returns>
        protected UIViewController LoadView<TView>(ViewController<TView> controller) where TView : class, IView
        {
            var vc = ViewService.InstanstantiateView(controller);
            return vc;
        }

        public void ShowTabs(params object[] viewControllers)
        {
            ViewControllers = viewControllers.Select(vc => ViewService.InstanstantiateView(vc)).ToArray();
        }

        protected void SetupTab(int index, string title, UIImage image)
        {
            TabBar.Items[index].Title = title;
            TabBar.Items[index].Image = image;
        }

        protected void SetupTab(int index, string title, UIImage image, UIImage selectedImage)
        {
            TabBar.Items[index].Title = title;
            TabBar.Items[index].Image = image;
            TabBar.Items[index].SelectedImage = selectedImage;
        }
    }
}
