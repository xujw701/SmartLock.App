using System;
using CoreGraphics;
using SmartLock.Infrastructure;
using SmartLock.Model.Services;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class LoginView : TableView<ILoginView>, ILoginView
    {
        private IBlueToothLeService BlueToothLeService => IoC.Resolve<IBlueToothLeService>();

        public LoginView(LoginController controller) : base(controller)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TableView.TableHeaderView = CreateHeader();
            TableView.ReloadData();
        }

        private UIView CreateHeader()
        {
            var headerView = new UIView(new CGRect(0, 0, TableView.Frame.Width, 300));

            var connectButtonRect = new CGRect(0, 30, TableView.Frame.Width, 20);
            var unlockButtonRect = new CGRect(0, 60, TableView.Frame.Width, 60);
            var lockButtonRect = new CGRect(0, 90, TableView.Frame.Width, 60);
            var batteryButtonRect = new CGRect(0, 120, TableView.Frame.Width, 60);

            var connectButton = CreateButton(connectButtonRect, "Connect", () => { BlueToothLeService.StartScanningForDevicesAsync(); }, true);
            var unlockButton = CreateButton(unlockButtonRect, "Unlock", () => { BlueToothLeService.SetLock(false); }, true);
            var lockButton = CreateButton(lockButtonRect, "Lock", () => { BlueToothLeService.SetLock(true); }, true);
            var batteryButton = CreateButton(batteryButtonRect, "Battery Level", () => { BlueToothLeService.GetBatteryLevel(); }, true);

            headerView.AddSubview(connectButton);
            headerView.AddSubview(unlockButton);
            headerView.AddSubview(lockButton);
            headerView.AddSubview(batteryButton);

            return headerView;
        }

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
    }
}

