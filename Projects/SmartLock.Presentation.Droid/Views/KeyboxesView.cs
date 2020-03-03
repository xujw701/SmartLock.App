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

        private View _btnMine;
        private View _btnOthers;
        private ImageView _ivMine;
        private ImageView _ivOthers;

        private RecyclerView _rvKeyboxList;

        private KeyboxAdapter _adapter;

        public event Action<Keybox> KeyboxClicked;
        public event Action PlaceKeyboxClicked;
        public event Action<bool> TabClicked;
        public event Action Refresh;

        protected override int LayoutId => Resource.Layout.View_Keyboxes;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _context = ViewBase.CurrentActivity ?? container.Context;

            _btnPlaceLock = _view.FindViewById<Button>(Resource.Id.btnPlaceLock);

            _btnMine = _view.FindViewById<View>(Resource.Id.btnMine);
            _btnOthers = _view.FindViewById<View>(Resource.Id.btnOthers);
            _ivMine = _view.FindViewById<ImageView>(Resource.Id.ivMine);
            _ivOthers = _view.FindViewById<ImageView>(Resource.Id.ivOthers);

            _rvKeyboxList = _view.FindViewById<RecyclerView>(Resource.Id.rvKeyboxList);

            _btnPlaceLock.Click += (s, e) =>
            {
                PlaceKeyboxClicked?.Invoke();
            };

            _btnMine.Click += (s, e) =>
            {
                TabClicked?.Invoke(true);
                UpdateUI(true);
            };

            _btnOthers.Click += (s, e) =>
            {
                TabClicked?.Invoke(false);
                UpdateUI(false);
            };

            return _view;
        }

        public override void OnResume()
        {
            base.OnResume();

            Refresh?.Invoke();
        }

        public void Show(List<Keybox> keyboxes, bool placeLockButtonEnabled)
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                UpdatePlaceLockButton(placeLockButtonEnabled);

                _adapter = new KeyboxAdapter(keyboxes, KeyboxClicked);
                _rvKeyboxList.SetLayoutManager(new LinearLayoutManager(_context));
                _rvKeyboxList.SetAdapter(_adapter);
            });
        }

        public void UpdatePlaceLockButton(bool enabled)
        {
            ViewBase.CurrentActivity.RunOnUiThread(() =>
            {
                _btnPlaceLock.Background = _context.GetDrawable(enabled ? Resource.Drawable.rounded_rectangle_add_lock : Resource.Drawable.rounded_rectangle_add_lock_disabled);
            });
        }

        private void UpdateUI(bool mine)
        {
            _ivMine.Visibility = mine ? ViewStates.Visible : ViewStates.Invisible;
            _ivOthers.Visibility = !mine ? ViewStates.Visible : ViewStates.Invisible;
        }
    }
}