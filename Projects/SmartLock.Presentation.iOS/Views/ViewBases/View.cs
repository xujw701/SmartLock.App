using System;
using System.Linq;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Controls;
using CoreGraphics;
using UIKit;

namespace SmartLock.Presentation.iOS.Views.ViewBases
{
    /// <summary>
    /// Abstract view class which implements basic functionaliy for the app.
    /// </summary>
    public abstract class View<TView> : UIViewController, IView where TView : class, IView
    {
        private readonly ViewController<TView> _controller;

        private bool _isBusy;
        private LoadingOverlay _loadingOverlay;

        protected View(ViewController<TView> controller, string nibName) : base(nibName, null)
        {
            _controller = controller;
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

        public bool DisplayTitle
        {
            get => !NavigationController.NavigationBarHidden;
            set
            {
                NavigationController.SetNavigationBarHidden(!value, true);
            }
        }

        public string Subtitle { get; set; }

        /// <summary>
        /// Raised when the view will unload.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Detmermines if a full screen busy indicator when <see cref="IsBusy"/> is set on the view.
        /// Otherwise the view must handle is busy on its own.
        /// </summary>
        protected bool FullscreenIsBusy { get; set; } = true;

        protected virtual void OnIsBusyChanged()
        {
            if (!FullscreenIsBusy) return;

            // SetupFullScreenIsBusy was called, so display the full screen overlay
            if (IsBusy)
            {
                if (_loadingOverlay == null)
                {
                    var view = AppDelegate.NavigationController.View;
                    _loadingOverlay = new LoadingOverlay(new CGRect(0, 0, view.Frame.Width, view.Frame.Height), string.Empty);
                    view.AddSubview(_loadingOverlay);
                }
            }
            else
            {
                if (_loadingOverlay != null)
                {
                    _loadingOverlay.Hide();
                    _loadingOverlay = null;
                }
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _controller.SetView(this as TView);

            DisplayTitle = false;
        }
        
        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (NavigationController?.ViewControllers.Contains(this) == false)
            {
                Closed?.Invoke(this, new EventArgs());
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _controller.NotifyViewWillShow(this as TView);
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.LightContent;
        }
    }
}
