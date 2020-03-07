using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.iOS.Controls.Sources;
using SmartLock.Presentation.iOS.Controls.TableSupport;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Cells
{
    public class ImagePickerCell : TableSupportCell, IProvidesHeight
    {
        public nfloat Height => _attachments.Count == 0 ? 140 : (nfloat)Math.Ceiling((_attachments.Count + 1) / 3f) * 140;

        private readonly List<Cache> _attachments;
        private readonly Action _addClicked;
        private readonly Action<Cache> _itemSelected;

        protected override string CellReuseIdentifier => "ImagePickerCell";

        public ImagePickerCell(List<Cache> attachments, Action addClicked, Action<Cache> itemSelected)
        {
            _attachments = attachments;
            _addClicked = addClicked;
            _itemSelected = itemSelected;
        }

        protected override UITableViewCell CreateCell()
        {
            return ImagePickerTableViewCell.Create();
        }

        protected override void UpdateCell(TableSupportSource source, UITableView tableView, UITableViewCell cell, NSIndexPath path)
        {
            if (cell is ImagePickerTableViewCell imagePickerTableViewCell)
            {
                imagePickerTableViewCell.Configure(Height, _attachments, _addClicked, _itemSelected);
            }
        }

        private class ImagePickerTableViewCell : UITableViewCell
        {
            public static readonly NSString Key = new NSString("ImagePickerTableViewCell");

            private UICollectionView _collectionView;

            internal static ImagePickerTableViewCell Create()
            {
                return new ImagePickerTableViewCell();
            }

            private ImagePickerTableViewCell()
            {
                ContentView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
                ContentView.AutosizesSubviews = true;
            }

            public void Configure(nfloat height, List<Cache> attachments, Action addClicked, Action<Cache> itemSelected)
            {
                if (_collectionView == null)
                {
                    _collectionView = CreateCollectionView(height);
                    _collectionView.RegisterClassForCell(typeof(ImageCollectionViewCell), ImageCollectionViewCell.Key);
                    ContentView.AddSubview(_collectionView);
                }

                _collectionView.Source = new ImagePickerSource(attachments, addClicked, itemSelected);
                _collectionView.ReloadData();
            }

            private UICollectionView CreateCollectionView(nfloat height)
            {
                var layout = new UICollectionViewFlowLayout();
                var spacing = layout.MinimumInteritemSpacing;
                var itemSize = UIScreen.MainScreen.Bounds.Width / 3 - spacing * 2;
                layout.ItemSize = new CGSize(itemSize, itemSize);

                var result = new UICollectionView(new CGRect(spacing, spacing, UIScreen.MainScreen.Bounds.Width - spacing * 2, height - spacing * 2), layout);
                result.ContentInset = result.ScrollIndicatorInsets =
                        new UIEdgeInsets(spacing, spacing, spacing, spacing);
                result.BackgroundColor = UIColor.White;
                result.ScrollEnabled = false;
                return result;
            }
        }
    }
}
