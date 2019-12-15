using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class KeyboxHistoryView : ViewBase<IKeyboxHistoryView>, IKeyboxHistoryView
    {
        private ImageView _btnBack;

        private RecyclerView _rvHistoryList;

        private KeyboxHistoryAdapter _adapter;

        public event Action BackClick;
        public event Action<KeyboxHistory> KeyboxSetOut;

        protected override int LayoutId => Resource.Layout.View_KeyboxHistory;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            _rvHistoryList = FindViewById<RecyclerView>(Resource.Id.rvHistoryList);

            _btnBack.Click += (s, e) => BackClick?.Invoke();
        }

        public void Show(List<KeyboxHistory> keyboxHistories)
        {
            if (_adapter == null)
            {
                _adapter = new KeyboxHistoryAdapter(keyboxHistories, KeyboxSetOut);
                _rvHistoryList.SetLayoutManager(new LinearLayoutManager(this));
                _rvHistoryList.SetAdapter(_adapter);
            }
            else
            {
                _adapter.KeyboxHistories = keyboxHistories;
                _adapter.NotifyDataSetChanged();
            }
        }
    }
}