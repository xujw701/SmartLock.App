using System;
using System.Linq;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.ViewControllers;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.iOS.Controls.Cells;
using SmartLock.Presentation.iOS.Controls.TableSupport;
using SmartLock.Presentation.iOS.Support;
using SmartLock.Presentation.iOS.Views.ViewBases;
using UIKit;

namespace SmartLock.Presentation.iOS.Views
{
    public partial class KeyboxPlaceUpdateView : View<IKeyboxPlaceUpdateView>, IKeyboxPlaceUpdateView
    {
        public event Action BackClick;
        public event Action<byte[]> AttachmentAdded;
        public event Action<Cache> AttachmentClicked;
        public event Action<Cache> AttachmentDeleted;
        public event Action<Property> SubmitClick;

        private Property _property;

        public KeyboxPlaceUpdateView(KeyboxPlaceUpdateController controller) : base(controller, "KeyboxPlaceUpdateView")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            EtName.EditingChanged += (s, e) =>
            {
                if (_property != null) _property.PropertyName = EtName.Text;
            };

            EtAddress.EditingChanged += (s, e) =>
            {
                if (_property != null) _property.Address = EtAddress.Text;
            };

            EtArea.EditingChanged += (s, e) =>
            {
                if (_property != null) _property.FloorArea = double.Parse(EtArea.Text);
            };

            EtPrice.EditingChanged += (s, e) =>
            {
                if (_property != null) _property.Price = EtPrice.Text;
            };

            EtData.EditingChanged += (s, e) =>
            {
                if (_property != null) _property.Notes = EtData.Text;
            };

            IvBack.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                BackClick?.Invoke();
            }));

            BtnSubmit.TouchUpInside += (s, e) => SubmitClick?.Invoke(_property);

            ConfigureUI();

            SetupPicker();
        }

        public void Show(Keybox keybox, Property property)
        {
            _property = property;

            LblName.Text = keybox.KeyboxName;
            LblBattery.Text = keybox.BatteryLevelString;
            EtName.Text = keybox.KeyboxName;
            EtAddress.Text = property.Address;
            EtArea.Text = property.FloorArea.HasValue ? property.FloorArea.Value.ToString() : string.Empty;
            EtPrice.Text = property.Price;
            EtData.Text = property.Notes;

            if (property.Bedrooms.HasValue) EtBedroom.Text = $"{property.Bedrooms.Value} Bedrooms";
            if (property.Bathrooms.HasValue) EtBathroom.Text = $"{property.Bathrooms.Value} Bathrooms";
            switch (property.Price.ToString())
            {
                case "Auction":
                    SelectPriceOption("Auction", true);
                    break;
                case "Negotiation":
                    SelectPriceOption("Negotiation", true);
                    break;
                default:
                    SelectPriceOption("Input Amount", true);
                    break;
            }

            ConfigueImagePicker();
        }

        private void ConfigureUI()
        {
            var contentWidth = UIScreen.MainScreen.Bounds.Width - 40;

            HeaderView.WidthAnchor.ConstraintEqualTo(contentWidth).Active = true;

            ShadowHelper.AddShadow(EtName);
            ShadowHelper.AddShadow(EtAddress);
            ShadowHelper.AddShadow(EtArea);
            ShadowHelper.AddShadow(EtBedroom);
            ShadowHelper.AddShadow(EtBathroom);
            ShadowHelper.AddShadow(EtPriceOption);
            ShadowHelper.AddShadow(EtPrice);
            ShadowHelper.AddShadow(EtData);
        }

        private void SetupPicker()
        {
            UIHelper.SetupPicker(EtBedroom, new[]
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
            }, (itemString) =>
            {
                if (_property != null)
                {
                    itemString = itemString.Replace(" Bedroom", "");
                    itemString = itemString.Replace("Bedroom", "");
                    itemString = itemString.Replace("s", "");

                    if (!string.IsNullOrEmpty(itemString) && double.TryParse(itemString, out double itemCount))
                    {
                        _property.Bedrooms = itemCount;
                    }
                }
            });

            UIHelper.SetupPicker(EtBathroom, new[]
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
            }, (itemString) =>
            {
                if (_property != null)
                {
                    itemString = itemString.Replace(" Bathroom", "");
                    itemString = itemString.Replace("Bathroom", "");
                    itemString = itemString.Replace("s", "");

                    if (!string.IsNullOrEmpty(itemString) && double.TryParse(itemString, out double itemCount))
                    {
                        _property.Bathrooms = itemCount;
                    }
                }
            });

            UIHelper.SetupPicker(EtPriceOption, new[]
            {
               "Auction",
                "Negotiation",
                "Input Amount",
            }, (itemString) =>
            {
                SelectPriceOption(itemString);
            });
        }

        private void ConfigueImagePicker()
        {
            var source = new TableSupportSource();

            source.AppendTableSupportCellGroup(new TableSupportCellGroup(new TableSupportCell[]
            {
                new ImagePickerCell(_property.PropertyResource.Where(pr => !pr.ToDelete).Select(pr => pr.Image).Union(_property.ToUploadResource).ToList(), ChooseCaptureMethod, AttachmentClicked)
            }));

            ImagePickerTableView.Source = source;
            ImagePickerTableView.ReloadData();
        }

        private void SelectPriceOption(string optionString, bool isInit = false)
        {
            EtPriceOption.Text = optionString;

            if (optionString.Equals("Input Amount"))
            {
                EtPrice.Hidden = false;

                if (isInit) EtPrice.Text = _property.Price;
                else EtPrice.Text = "0";
            }
            else
            {
                EtPrice.Hidden = true;
                if (_property != null)
                {
                    _property.Price = optionString;
                }
            }
        }

        #region Caches

        [Export("imagePickerController:didFinishPickingMediaWithInfo:")]
        public void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            var isImage = false;

            switch (info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image":
                    isImage = true;
                    break;
                case "public.video":
                    Console.WriteLine("Video selected");
                    break;
            }

            if (isImage)
            {
                // get the original image
                var originalImage = info[UIImagePickerController.OriginalImage] as UIImage;
                if (originalImage != null)
                {
                    using (NSData imageData = originalImage.AsJPEG(0.8f)) // TODO: Quality settings? 
                    {
                        var myByteArray = new byte[imageData.Length];

                        System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));

                        AttachmentAdded?.Invoke(myByteArray);
                    }
                }
                else
                {
                    throw new Exception("Image is blank");
                }
            }

            picker.DismissModalViewController(true);

            AppDelegate.SetWhiteStatusBar();
        }

        [Export("imagePickerControllerDidCancel:")]
        public void Canceled(UIImagePickerController picker)
        {
            picker.DismissModalViewController(true);
            AppDelegate.SetWhiteStatusBar();
        }

        private void ChooseCaptureMethod()
        {
            var actionSheetAlert = UIAlertController.Create(null, null, UIAlertControllerStyle.ActionSheet);
            actionSheetAlert.AddAction(UIAlertAction.Create("Take a photo", UIAlertActionStyle.Default, (action) =>
            {
                CreatePicker(true);
            }));
            actionSheetAlert.AddAction(UIAlertAction.Create("Add existing photo", UIAlertActionStyle.Default, (action) =>
            {
                CreatePicker(false);
            }));
            actionSheetAlert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (action) => { }));

            var presentationPopover = actionSheetAlert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            PresentViewController(actionSheetAlert, true, null);
        }

        private void CreatePicker(bool takePhoto)
        {
            // Show the native image picker controller
            var imagePicker = new UIImagePickerController
            {
                Delegate = this,
                SourceType =
                    takePhoto
                        ? UIImagePickerControllerSourceType.Camera
                        : UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(takePhoto ? UIImagePickerControllerSourceType.Camera : UIImagePickerControllerSourceType.PhotoLibrary)
            };
            AppDelegate.SetBlackStatusBar();
            NavigationController.PresentViewController(imagePicker, true, AppDelegate.SetBlackStatusBar);
        }

        #endregion
    }
}