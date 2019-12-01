using CoreGraphics;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls
{
    /// <summary>
    /// Loading overlay goes on top of a view to indiate something is loading
    /// </summary>
    public sealed class LoadingOverlay : UIView
    {
        public LoadingOverlay(CGRect frame, string message) : base(frame)
        {
            // configurable bits
            BackgroundColor = UIColor.Black;
            Alpha = 0.4f;
            AutoresizingMask = UIViewAutoresizing.All;

            var labelHeight = 22;
            var labelWidth = Frame.Width - 20;

            if (string.IsNullOrEmpty(message))
            {
                labelWidth = 0;
                labelHeight = 0;
            }

            // derive the center x and y
            var centerX = Frame.Width / 2;
            var centerY = Frame.Height / 2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            var activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
            activitySpinner.Frame = new CGRect(
                centerX - (activitySpinner.Frame.Width / 2),
                centerY - (activitySpinner.Frame.Height / 2) - (labelHeight / 2),
                activitySpinner.Frame.Width,
                activitySpinner.Frame.Height);
            activitySpinner.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(activitySpinner);
            activitySpinner.StartAnimating();

            // create and configure the "Loading Data" label
            var loadingLabel = new UILabel(new CGRect(
                centerX - (labelWidth / 2),
                centerY + 20,
                labelWidth,
                labelHeight
                ));
            loadingLabel.BackgroundColor = UIColor.Clear;
            loadingLabel.TextColor = UIColor.White;
            loadingLabel.Text = message;
            loadingLabel.TextAlignment = UITextAlignment.Center;
            loadingLabel.AutoresizingMask = UIViewAutoresizing.All;
            AddSubview(loadingLabel);

        }

        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public void Hide()
        {
            Animate(
                0.5, // duration
                () => { Alpha = 0; },
                RemoveFromSuperview
            );
        }
    }

}
