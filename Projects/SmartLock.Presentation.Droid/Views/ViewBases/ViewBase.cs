using System;
using Android.Graphics;
using SmartLock.Presentation.Core.Views;
using Android.Support.V7.App;
using SmartLock.Presentation.Droid.Platform;
using SmartLock.Presentation.Core.ViewControllers;
using Android.Widget;
using Android.Views;
using Android.OS;
using Android.Support.V4.Widget;
using Android;
using Android.Support.V4.Content;
using Android.Content.PM;
using SmartLock.Presentation.Droid.Support;

namespace SmartLock.Presentation.Droid.Views.ViewBases
{
    public static class ViewBase
    {
        public static AppCompatActivity CurrentActivity { get; set; }
    }

    /// <summary>
    /// View base for all activities in the android project
    /// </summary>
	public abstract class ViewBase<TView> : ActivityBase, IView where TView : class, IView
    {
        public event EventHandler Closed;

        public event Action RightButtonClicked;

        protected virtual int LayoutId => throw new System.Exception("Invalid layout set");

        //protected virtual int ToolBarId => Resource.Id.toolbar;

        protected virtual int RightButtonId => 0;

        protected virtual bool BlockBackPress => false;
        protected virtual bool SwipeRefresh => true;

        private bool _isBusy;
        private bool _displayHeader;
        private ViewController<TView> _controller;

        private SwipeRefreshLayout _swipeRefreshLayout;

        public ViewController<TView> ViewController => _controller;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                var progressOverlay = FindViewById<FrameLayout>(Resource.Id.progressOverlay);
                if (progressOverlay != null)
                {
                    progressOverlay.Visibility = _isBusy ? ViewStates.Visible : ViewStates.Gone;
                }

                //_swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
                //if (_swipeRefreshLayout != null)
                //{
                //    _swipeRefreshLayout.Refreshing = _isBusy;
                //}
            }
        }

        public virtual bool DisplayTitle
        {
            get => _displayHeader;
            set
            {
                _displayHeader = value;
                UpdateHeader();
            }
        }

        public new string Title
        {
            get
            {
                //var title = FindViewById<TextView>(Resource.Id.toolbarTitle);
                //if (title != null) return title.Text;
                return SupportActionBar?.Title ?? string.Empty;
            }
            set
            {
                //var title = FindViewById<TextView>(Resource.Id.toolbarTitle);
                //if (title != null)
                //{
                //    title.Text = value;
                //}
                if (SupportActionBar != null)
                {
                    SupportActionBar.Title = value;
                }
            }
        }

        public string Subtitle
        {
            get => SupportActionBar.Subtitle;
            set => SupportActionBar.Subtitle = value;
        }

        protected void UpdateHeader()
        {
            if (DisplayTitle)
            {
                SupportActionBar?.Show();
            }
            else
            {
                SupportActionBar?.Hide();
            }
        }

        public void SetupToolbar()
        {
            // set up the action bar
            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(ToolBarId);

            //if (toolbar != null)
            //{
            //    SetSupportActionBar(toolbar);
            //    SupportActionBar.Title = string.Empty;
            //}

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(LayoutId);

            SetupToolbar();

            Window.SetSoftInputMode(SoftInput.StateHidden);

            //_swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);

            //if (_swipeRefreshLayout != null)
            //{
            //    _swipeRefreshLayout.Enabled = SwipeRefresh;
            //}

            RequestPermissions();
        }

        protected override void OnResume()
        {
            base.OnResume();

            ViewBase.CurrentActivity = this;
        }

        protected override void OnStart()
        {
            ViewBase.CurrentActivity = this;

            base.OnStart();

            if (_controller == null)
            {
                // controller has not been set for this view yet, do it now
                _controller = ViewService.CurrentViewController as ViewController<TView>;
                _controller?.SetView(this as TView);
            }

            _controller?.NotifyViewWillShow(this as TView);
        }

        /// <summary>
        /// toolbar back button handling
        /// </summary>
        /// <returns></returns>
        public override bool OnSupportNavigateUp()
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                // remove fragments
                SupportFragmentManager.PopBackStack();
            }
            else
            {
                Finish();
            }
            return base.OnSupportNavigateUp();
        }

        protected override void OnDestroy()
        {
            Closed?.Invoke(this, null);
            base.OnDestroy();
        }

        public override void OnBackPressed()
        {
            if (BlockBackPress) return;

            base.OnBackPressed();
        }

        protected void SetupHiddenMenu(View view, Action clicked)
        {
            var click = 0;
            view.Clickable = true;
            view.Click += (s, e) =>
            {
                click++;
                if (click % 5 == 0)
                {
                    clicked?.Invoke();
                    click = 0;
                }
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if (RightButtonId > 0)
            {
                MenuInflater.Inflate(Resource.Menu.menu_right_button, menu);
                var toolbarMenuItem = menu.GetItem(0);
                toolbarMenuItem.SetIcon(ImageHelper.GetTintImageDrawable(GetDrawable(RightButtonId), Color.White));
                return true;
            }
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.item_right_button)
            {
                RightButtonClicked?.Invoke();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        #region Permissions
        private const int REQUEST_PERMISSIONS = 1;

        private Action _requestPermissionSuccessAction;

        protected bool RequestPermissions()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.Bluetooth) != (int)Permission.Granted
                || ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothAdmin) != (int)Permission.Granted)
            {
                RequestPermissions(new string[]
                                    {
                                        Manifest.Permission.ReadExternalStorage,
                                        Manifest.Permission.WriteExternalStorage,
                                        Manifest.Permission.Camera,
                                        Manifest.Permission.AccessCoarseLocation,
                                        Manifest.Permission.AccessFineLocation,
                                        Manifest.Permission.Bluetooth,
                                        Manifest.Permission.BluetoothAdmin,
                                    },
                        REQUEST_PERMISSIONS);

                return false;
            }
            else
            {
                return true;
            }
        }

        protected void OnRequestPermissionsResult(int requestCode, string[] permissions, int[] grantResults)
        {
            if (requestCode == REQUEST_PERMISSIONS)
            {
                if (grantResults.Length != 1 || grantResults[0] != (int)Permission.Granted)
                {
                }
            }

            return;
        }
        #endregion
    }
}