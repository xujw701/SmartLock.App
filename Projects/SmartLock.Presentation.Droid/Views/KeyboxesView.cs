using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SmartLock.Model.Models;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class KeyboxesView : FragmentView<IKeyboxesView>, IKeyboxesView
    {
        private Context _context;

        private Button _btnPlaceLock;

        private RecyclerView _rvKeyboxList;

        private KeyboxAdapter _adapter;

        public event Action<Keybox> KeyboxClicked;
        public event Action PlaceKeyboxClicked;

        protected override int LayoutId => Resource.Layout.View_Keyboxes;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _context = _view.Context;

            _btnPlaceLock = _view.FindViewById<Button>(Resource.Id.btnPlaceLock);
            _rvKeyboxList = _view.FindViewById<RecyclerView>(Resource.Id.rvKeyboxList);

            _btnPlaceLock.Click += (s, e) =>
            {
                PlaceKeyboxClicked?.Invoke();
            };

            return _view;
        }

        public void Show(List<Keybox> keyboxes, bool placeLockButtonEnabled)
        {
            UpdatePlaceLockButton(placeLockButtonEnabled);

            if (_adapter == null)
            {
                _adapter = new KeyboxAdapter(keyboxes, KeyboxClicked);
                _rvKeyboxList.SetLayoutManager(new LinearLayoutManager(_context));
                _rvKeyboxList.SetAdapter(_adapter);
            }
            else
            {
                _adapter.Keyboxes = keyboxes;
                _adapter.NotifyDataSetChanged();
            }
        }

        public void UpdatePlaceLockButton(bool enabled)
        {
            _btnPlaceLock.Background = _context.GetDrawable(enabled ? Resource.Drawable.rounded_rectangle_add_lock : Resource.Drawable.rounded_rectangle_add_lock_disabled);
        }
    }
}