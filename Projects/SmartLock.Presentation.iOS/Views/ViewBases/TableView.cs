using System;
using System.Linq;
using SmartLock.Presentation.iOS.Controls;
using SmartLock.Presentation.Core.ViewControllers;
using CoreGraphics;
using UIKit;
using SmartLock.Model.Views;

namespace SmartLock.Presentation.iOS.Views.ViewBases
{
    public abstract class TableView<TView> : UITableViewController, IView where TView : class, IView
    {
        private readonly ViewController<TView> _controller;

        private bool _isBusy;
        private LoadingOverlay _loadingOverlay;

        protected TableView(ViewController<TView> controller) : base(UITableViewStyle.Grouped)
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

        /// <summary>
        /// Raised when the view will unload.
        /// </summary>
        public event EventHandler Closed;

        /// Detmermines if a full screen busy indicator when <see cref="IsBusy"/> is set on the view.
        /// Otherwise the view must handle is busy on its own.
        protected bool FullscreenIsBusy { get; set; } = true;


        public bool DisplayTitle
        {
            get => !NavigationController.NavigationBarHidden;
            set
            {
                NavigationController.SetNavigationBarHidden(!value, true);
            }
        }

        protected enum NativeOsFeature
        {
            Enabled,
            Disabled
        }

        protected virtual NativeOsFeature DisablesBackSwipeGesture { get; } = NativeOsFeature.Disabled;

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

            if (DisablesBackSwipeGesture == NativeOsFeature.Disabled
                && NavigationController?.InteractivePopGestureRecognizer != null)
            {
                NavigationController.InteractivePopGestureRecognizer.Enabled = false;
            }

            //if (NavigationController != null) NavigationController.HidesBarsOnSwipe = true;

            TableView.CellLayoutMarginsFollowReadableWidth = false;
        }

        public override void ViewWillDisappear(bool animated)
        {

            base.ViewWillDisappear(animated);

            if (NavigationController?.ViewControllers.Contains(this) == false)
            {
                Closed?.Invoke(this, new EventArgs());
            }
        }

        public string Subtitle
        {
            get => throw new NotSupportedException();
            set => SetSubtitle(value);
        }

        private void SetSubtitle(string value)
        {
            var titleLabel = new UILabel(CGRect.Empty);
            titleLabel.BackgroundColor = UIColor.Clear;
            titleLabel.TextColor = UIColor.Black;
            titleLabel.Font = UIFont.PreferredHeadline;
            titleLabel.Text = Title;
            titleLabel.SizeToFit();
            titleLabel.Frame = new CGRect(0, 0, View.Frame.Width - 100, titleLabel.Frame.Height);
            titleLabel.TextAlignment = UITextAlignment.Center;

            var subTitleLabel = new UILabel(CGRect.Empty);
            subTitleLabel.BackgroundColor = UIColor.Clear;
            subTitleLabel.TextColor = UIColor.DarkGray;
            subTitleLabel.Font = UIFont.PreferredSubheadline;
            subTitleLabel.Text = value;
            subTitleLabel.SizeToFit();
            subTitleLabel.Frame = new CGRect(0, 22, View.Frame.Width - 100, titleLabel.Frame.Height);
            subTitleLabel.TextAlignment = UITextAlignment.Center;

            var twoLineTitleView = new UIView(frame: new CGRect(x: 0, y: 0, width: View.Frame.Width, height: 40));
            twoLineTitleView.AddSubview(titleLabel);
            twoLineTitleView.AddSubview(subTitleLabel);

            NavigationItem.TitleView = twoLineTitleView;
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

        public ViewController<TView> ViewController => _controller;

        protected UIButton CreateButton(CGRect rect, string title, Action clicked, bool textOnly, bool? rightAlign = null)
        {
            var button = new UIButton(rect);
            button.SetTitle(title, UIControlState.Normal);
            button.TouchUpInside += (s, e) => { clicked?.Invoke(); };

            if (textOnly)
            {
                button.SetTitleColor(View.TintColor, UIControlState.Normal);
                button.Font = UIFont.PreferredBody.WithSize(14);
            }
            else
            {
                button.BackgroundColor = View.TintColor;
                button.Layer.CornerRadius = 8;
            }

            if (rightAlign.HasValue)
            {
                button.HorizontalAlignment = rightAlign.Value ? UIControlContentHorizontalAlignment.Trailing : UIControlContentHorizontalAlignment.Leading;
            }

            return button;
        }

        protected UILabel CreateLabel(CGRect rect, string text, int fontSize = 17)
        {
            var label = new UILabel(rect);
            label.Text = text;
            label.Font = label.Font.WithSize(fontSize);
            label.Lines = 0;
            label.LineBreakMode = UILineBreakMode.WordWrap;

            return label;
        }

        protected UIActivityIndicatorView CreateLoadingIndicator(CGRect rect)
        {
            var activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
            activitySpinner.Frame = rect;

            return activitySpinner;
        }
    }
}