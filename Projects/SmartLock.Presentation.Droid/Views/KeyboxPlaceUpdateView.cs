using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Controls;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class KeyboxPlaceUpdateView : ViewBase<IKeyboxPlaceUpdateView>, IKeyboxPlaceUpdateView
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

        public event Action BackClick;
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
                    _spinPriceOption.SetSelection(1);
                    break;
                case "Negotiation":
                    _spinPriceOption.SetSelection(2);
                    break;
                case "Input Amount":
                    _spinPriceOption.SetSelection(3);
                    break;
            }
        }

        private void ConfigueSpinner()
        {
            ConfigueSpinner(_spinBedroomOption, "Bedroom", new List<string>()
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9"
            });

            _spinBedroomOption.ItemSelected += (s, e) =>
            {
                if (_property != null)
                {
                    _property.Bedrooms = double.Parse(_spinBedroomOption.SelectedItem.ToString());
                }
            };

            ConfigueSpinner(_spinBathroomOption, "Bathroom", new List<string>()
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9"
            });

            _spinBathroomOption.ItemSelected += (s, e) =>
            {
                if (_property != null)
                {
                    _property.Bathrooms = double.Parse(_spinBathroomOption.SelectedItem.ToString());
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
                var selectedItem = _spinPriceOption.SelectedItem.ToString();

                if (selectedItem.Equals("Input Amount"))
                {
                    _priceLayout.Visibility = ViewStates.Visible;
                }
                else
                {
                    _priceLayout.Visibility = ViewStates.Gone;
                    if (_property != null)
                    {
                        _property.Price = selectedItem;
                    }
                }
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
    }
}