using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Controls;
using SmartLock.Presentation.Droid.Support;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class KeyboxPlaceUpdateView : ViewBase<IKeyboxPlaceUpdateView>, IKeyboxPlaceUpdateView, IDialogInterfaceOnClickListener
    {
        private ImageView _btnBack;
        private TextView _tvTitle;

        private TextView _tvName;
        private TextView _tvBatteryStatus;
        private EditText _etLockname;
        private EditText _etAddress;
        private EditText _etArea;
        private EditText _etPrice;
        private EditText _etNotes;

        private Spinner _spinBedroomOption;
        private Spinner _spinBathroomOption;
        private Spinner _spinPriceOption;

        private View _priceLayout;

        private Button _btnPlace;

        private Model.Models.Property _property;

        private ImageGridView _gvImageView;
        private ImagePickerAdapter _imageAdapter;

        public event Action BackClick;
        public event Action<byte[]> AttachmentAdded;
        public event Action<Cache> AttachmentClicked;
        public event Action<Cache> AttachmentDeleted;
        public event Action<Model.Models.Property> SubmitClick;

        protected override int LayoutId => Resource.Layout.View_KeyboxPlace;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            _tvTitle = FindViewById<TextView>(Resource.Id.tvTitle);

            _tvName = FindViewById<TextView>(Resource.Id.tvName);
            _tvBatteryStatus = FindViewById<TextView>(Resource.Id.tvBatteryStatus);

            _etLockname = FindViewById<EditText>(Resource.Id.etLockname);
            _etAddress = FindViewById<EditText>(Resource.Id.etAddress);
            _etArea = FindViewById<EditText>(Resource.Id.etArea);
            _etNotes = FindViewById<EditText>(Resource.Id.etNotes);
            _etPrice = FindViewById<EditText>(Resource.Id.etPrice);

            _spinBedroomOption = FindViewById<Spinner>(Resource.Id.bedroomOption);
            _spinBathroomOption = FindViewById<Spinner>(Resource.Id.bathroomOption);
            _spinPriceOption = FindViewById<Spinner>(Resource.Id.spinPriceOption);

            _priceLayout = FindViewById<View>(Resource.Id.priceLayout);

            _gvImageView = FindViewById<ImageGridView>(Resource.Id.gvImageView);

            _btnPlace = FindViewById<Button>(Resource.Id.btnPlace);

            ConfigueSpinner();

            _etLockname.TextChanged += (s, e) =>
            {
                if (_property != null) _property.PropertyName = _etLockname.Text;
            };

            _etAddress.TextChanged += (s, e) =>
            {
                if (_property != null) _property.Address = _etAddress.Text;
            };

            _etArea.TextChanged += (s, e) =>
            {
                if (_property != null) _property.FloorArea = double.Parse(_etArea.Text);
            };

            _etPrice.TextChanged += (s, e) =>
            {
                if (_property != null) _property.Price = _etPrice.Text;
            };

            _etNotes.TextChanged += (s, e) =>
            {
                if (_property != null) _property.Notes = _etNotes.Text;
            };

            _btnBack.Click += (s, e) => BackClick?.Invoke();

            _btnPlace.Click += (s, e) => SubmitClick?.Invoke(_property);

            _tvTitle.Text = "Update keybox";
            _btnPlace.Text = "Update keybox";
        }

        public void Show(Keybox keybox, Model.Models.Property property)
        {
            _property = property;

            _tvName.Text = keybox.KeyboxName;
            _tvBatteryStatus.Text = keybox.BatteryLevelString;
            _etLockname.Text = keybox.KeyboxName;
            _etAddress.Text = property.Address;
            _etArea.Text = property.FloorArea.HasValue ? property.FloorArea.Value.ToString() : string.Empty;
            _etPrice.Text = property.Price;
            _etNotes.Text = property.Notes;

            if (property.Bedrooms.HasValue) _spinBedroomOption.SetSelection(int.Parse(property.Bedrooms.ToString()));
            if (property.Bathrooms.HasValue) _spinBathroomOption.SetSelection(int.Parse(property.Bathrooms.ToString()));
            switch(property.Price.ToString())
            {
                case "Auction":
                    _spinPriceOption.SetSelection(0);
                    SelectPriceOption("Auction", true);
                    break;
                case "Negotiation":
                    _spinPriceOption.SetSelection(1);
                    SelectPriceOption("Negotiation", true);
                    break;
                default:
                    _spinPriceOption.SetSelection(2);
                    SelectPriceOption("Input Amount", true);
                    break;
            }

            _imageAdapter = new ImagePickerAdapter(this, _property.PropertyResource.Where(pr => !pr.ToDelete).Select(pr => pr.Image).Union(_property.ToUploadResource).ToList(), ChooseCaptureMethod, AttachmentClicked, AttachmentDeleted);
            _gvImageView.Adapter = _imageAdapter;
        }

        private void ConfigueSpinner()
        {
            ConfigueSpinner(_spinBedroomOption, "Bedroom", new List<string>()
            {
                "1 Bedroom",
                "2 Bedrooms",
                "3 Bedrooms",
                "4 Bedrooms",
                "5 Bedrooms",
                "6 Bedrooms",
                "7 Bedrooms",
                "8 Bedrooms",
                "9 Bedrooms"
            });

            _spinBedroomOption.ItemSelected += (s, e) =>
            {
                if (_property != null)
                {
                    var itemString = _spinBedroomOption.SelectedItem.ToString();
                    itemString = itemString.Replace(" Bedroom", "");
                    itemString = itemString.Replace("s", "");
                    _property.Bedrooms = double.Parse(itemString);
                }
            };

            ConfigueSpinner(_spinBathroomOption, "Bathroom", new List<string>()
            {
                "1 Bathroom",
                "2 Bathrooms",
                "3 Bathrooms",
                "4 Bathrooms",
                "5 Bathrooms",
                "6 Bathrooms",
                "7 Bathrooms",
                "8 Bathrooms",
                "9 Bathrooms"
            });

            _spinBathroomOption.ItemSelected += (s, e) =>
            {
                if (_property != null)
                {
                    var itemString = _spinBathroomOption.SelectedItem.ToString();
                    itemString = itemString.Replace(" Bathroom", "");
                    itemString = itemString.Replace("s", "");
                    _property.Bathrooms = double.Parse(itemString);
                }
            };

            ConfigueSpinner(_spinPriceOption, "Price Options", new List<string>()
            {
                "Auction",
                "Negotiation",
                "Input Amount",
            });

            _spinPriceOption.ItemSelected += (s, e) =>
            {
                SelectPriceOption(_spinPriceOption.SelectedItem.ToString());
            };
        }

        private void ConfigueSpinner(Spinner spinner, string title, List<string> items)
        {
            if (!items.Contains(title)) items.Add(title);

            var adapter = new CustomArrayAdapter(this, Resource.Layout.Item_Spinner_Text, items);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

            spinner.Adapter = adapter;
            spinner.SetSelection(items.Count - 1, true);
        }

        private void SelectPriceOption(string optionString, bool isInit = false)
        {
            if (optionString.Equals("Input Amount"))
            {
                _priceLayout.Visibility = ViewStates.Visible;

                if (isInit) _etPrice.Text = _property.Price;
            }
            else
            {
                _priceLayout.Visibility = ViewStates.Gone;
                if (_property != null)
                {
                    _property.Price = optionString;
                }
            }
        }

        #region Attachments

        private const int CameraRequest = 1;
        private const int ChooseFromAlbumRequest = 2;

        private string _tmpFilePath;

        public void ChooseCaptureMethod()
        {
            var granted = RequestPermissions();

            if (granted)
            {
                var builder = new AlertDialog.Builder(this);
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
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                if (requestCode == CameraRequest || requestCode == ChooseFromAlbumRequest)
                {
                    try
                    {
                        var path = requestCode == ChooseFromAlbumRequest ? SelectedFilePathHelper.GetPath(this, data.Data) : _tmpFilePath;
                        var dataByte = ImageHelper.CompressAndRotate(this, path);

                        AttachmentAdded?.Invoke(dataByte);
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
            if (intent.ResolveActivity(PackageManager) != null)
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

            if (intent.ResolveActivity(PackageManager) != null)
            {
                StartActivityForResult(intent, ChooseFromAlbumRequest);
            }
        }

        #endregion
    }
}