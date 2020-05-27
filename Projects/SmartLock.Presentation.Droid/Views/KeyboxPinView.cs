using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class KeyboxPinView : ViewBase<IKeyboxPinView>, IKeyboxPinView
    {
        private ImageView _btnBack;

        private EditText _etOldPin;
        private EditText _etNewPin1;
        private EditText _etNewPin2;

        private Button _btnSubmit;

        public event Action BackClick;
        public event Action<string, string, string> SubmitClick;
        public event Action<bool> PinChanged;

        protected override int LayoutId => Resource.Layout.View_KeyboxPin;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);

            _etOldPin = FindViewById<EditText>(Resource.Id.etOldPin);
            _etNewPin1 = FindViewById<EditText>(Resource.Id.etNewPin1);
            _etNewPin2 = FindViewById<EditText>(Resource.Id.etNewPin2);

            _btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);

            _btnBack.Click += (s, e) => BackClick?.Invoke();

            _btnSubmit.Click += (s, e) => SubmitClick?.Invoke(_etOldPin.Text, _etNewPin1.Text, _etNewPin2.Text);
        }

        public void OnPinChanged(bool success)
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                PinChanged?.Invoke(success);
            });
        }
    }
}