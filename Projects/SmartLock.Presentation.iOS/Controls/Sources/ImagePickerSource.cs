using System;
using System.Collections.Generic;
using Foundation;
using SmartLock.Model.Models;
using SmartLock.Presentation.iOS.Controls.Cells;
using UIKit;

namespace SmartLock.Presentation.iOS.Controls.Sources
{
    public class ImagePickerSource : UICollectionViewSource
    {
        private readonly List<Cache> _attachments;
        private readonly Action _addClicked;
        private readonly Action<Cache> _itemSelected;
        private readonly Action<Cache> _itemDeleted;

        public ImagePickerSource(List<Cache> attachments, Action addClicked, Action<Cache> itemSelected, Action<Cache> itemDeleted)
        {
            _attachments = attachments;

            _addClicked = addClicked;
            _itemSelected = itemSelected;
            _itemDeleted = itemDeleted;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return _attachments.Count + 1;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (ImageCollectionViewCell)collectionView.DequeueReusableCell(ImageCollectionViewCell.Key, indexPath);

            if (indexPath.Row == _attachments.Count)
            {
                cell.ConfigureAddButton();
            }
            else
            {
                cell.ConfigureImage(_attachments[indexPath.Row], _itemDeleted);
            }

            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (indexPath.Row == _attachments.Count)
            {
                _addClicked?.Invoke();
            }
            else
            {
                _itemSelected?.Invoke(_attachments[indexPath.Row]);
            }
        }
    }
}
