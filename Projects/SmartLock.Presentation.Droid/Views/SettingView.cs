using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Support;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingView : FragmentView<ISettingView>, ISettingView, IDialogInterfaceOnClickListener
    {
        private Context _context;

        private ImageView _ivPortrait;
        private TextView _tvName;

        private View _btnProfile;
        private View _btnPassword;
        private View _btnFeedback;
        private Button _btnLogout;

        public event Action<byte[]> PortraitChanged;
        public event Action ProfileClick;
        public event Action PasswordClick;
        public event Action FeedbackClick;
        public event Action LogoutClick;
        public event Action Refresh;

        protected override int LayoutId => Resource.Layout.View_Setting;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _context = ViewBase.CurrentActivity ?? container.Context;

            _ivPortrait = _view.FindViewById<ImageView>(Resource.Id.ivPortrait);
            _tvName = _view.FindViewById<TextView>(Resource.Id.tvName);

            _btnProfile = _view.FindViewById<View>(Resource.Id.btnProfile);
            _btnPassword = _view.FindViewById<View>(Resource.Id.btnPassword);
            _btnFeedback = _view.FindViewById<View>(Resource.Id.btnFeedback);
            _btnLogout = _view.FindViewById<Button>(Resource.Id.btnLogout);

            _ivPortrait.Click += (s, e) => ChooseCaptureMethod();
            _btnProfile.Click += (s, e) => ProfileClick?.Invoke();
            _btnPassword.Click += (s, e) => PasswordClick?.Invoke();
            _btnFeedback.Click += (s, e) => FeedbackClick?.Invoke();
            _btnLogout.Click += (s, e) => LogoutClick?.Invoke();

            return _view;
        }

        public override void OnResume()
        {
            base.OnResume();

            Refresh?.Invoke();
        }

        public void Show(string name, Cache portrait)
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                if (_tvName != null)
                {
                    _tvName.Text = name;
                }

                if (_ivPortrait != null)
                {
                    ImageHelper.SetThumbnail(_ivPortrait, portrait);
                }
            });
        }

        #region Attachments

        private const int CameraRequest = 1;
        private const int ChooseFromAlbumRequest = 2;

        private string _tmpFilePath;

        public void ChooseCaptureMethod()
        {
            var granted = RequestPermissions(_context);

            if (granted)
            {
                var builder = new AlertDialog.Builder(_context);
                builder.SetItems(new string[] { GetString(Resource.String.general_camera), GetString(Resource.String.general_choose_from_album) }, this);
                builder.Show();
            }
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            dialog.Dismiss();

            switch (which)
            {
                case 0:
                    Camera(true);
                    break;
                case 1:
                    ChooseFromAlbum();
                    break;
            }
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (resultCode == (int)Result.Ok)
            {
                if (requestCode == CameraRequest || requestCode == ChooseFromAlbumRequest)
                {
                    try
                    {
                        var path = requestCode == ChooseFromAlbumRequest ? SelectedFilePathHelper.GetPath(_context, data.Data) : _tmpFilePath;
                        var dataByte = ImageHelper.CompressAndRotate(_context, path, 600);

                        PortraitChanged?.Invoke(dataByte);
                    }
                    catch (Exception)
                    {
                    }
                    if (requestCode == CameraRequest)
                    {
                        //Clear tmp photo
                        FileHelper.DeleteFile(_tmpFilePath);

                        _tmpFilePath = null;
                    }
                }
            }
        }

        private void Camera(bool takeImage)
        {
            var intent = new Intent(takeImage ? MediaStore.ActionImageCapture : MediaStore.ActionVideoCapture);
            if (intent.ResolveActivity(_context.PackageManager) != null)
            {
                // Export origin photo
                var tmpFile = FileHelper.GetTmpFile(takeImage);
                _tmpFilePath = tmpFile.AbsolutePath;
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(tmpFile));
                StartActivityForResult(intent, CameraRequest);
            }
        }

        private void ChooseFromAlbum()
        {
            var intent = new Intent(Intent.ActionGetContent, MediaStore.Images.Media.ExternalContentUri);
            string[] mimetypes = { "image/*" };
            intent.PutExtra(Intent.ExtraMimeTypes, mimetypes);

            if (intent.ResolveActivity(_context.PackageManager) != null)
            {
                StartActivityForResult(intent, ChooseFromAlbumRequest);
            }
        }

        #endregion
    }
}