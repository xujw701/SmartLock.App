using Android.App;
using Android.Content.PM;
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
using System.Linq;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class KeyboxHistoryView : ViewBase<IKeyboxHistoryView>, IKeyboxHistoryView
    {
        private ImageView _btnBack;
        private View _btn7Days;
        private View _btn30Days;
        private View _btnAllDays;
        private ImageView _iv7Days;
        private ImageView _iv30Days;
        private ImageView _ivAllDays;

        private RecyclerView _rvHistoryList;

        private KeyboxHistoryAdapter _adapter;

        private List<KeyboxHistory> _keyboxHistories;

        private int _filter;

        public event Action BackClick;

        protected override int LayoutId => Resource.Layout.View_KeyboxHistory;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _btnBack = FindViewById<ImageView>(Resource.Id.btnBack);
            _btn7Days = FindViewById<View>(Resource.Id.btn7Days);
            _btn30Days = FindViewById<View>(Resource.Id.btn30Days);
            _btnAllDays = FindViewById<View>(Resource.Id.btnAllDays);
            _iv7Days = FindViewById<ImageView>(Resource.Id.iv7Days);
            _iv30Days = FindViewById<ImageView>(Resource.Id.iv30Days);
            _ivAllDays = FindViewById<ImageView>(Resource.Id.ivAllDays);
            _rvHistoryList = FindViewById<RecyclerView>(Resource.Id.rvHistoryList);

            _btn7Days.Click += (s, e) =>
            {
                Show(0);
            };

            _btn30Days.Click += (s, e) =>
            {
                Show(1);
            };

            _btnAllDays.Click += (s, e) =>
            {
                Show(2);
            };

            _btnBack.Click += (s, e) => BackClick?.Invoke();
        }

        public void Show(List<KeyboxHistory> keyboxHistories)
        {
            _keyboxHistories = keyboxHistories;

            Show(0);
        }

        private List<KeyboxHistory> GetFilteredHistories()
        {
            if (_filter == 0)
            {
                return _keyboxHistories.Where(k => (DateTime.Now - k.InOn).TotalDays <= 7).ToList();
            }
            else if (_filter == 1)
            {
                return _keyboxHistories.Where(k => (DateTime.Now - k.InOn).TotalDays <= 30).ToList();
            }

            return _keyboxHistories;
        }

        private void Show(int filter)
        {
            _filter = filter;

            if (_adapter == null)
            {
                _adapter = new KeyboxHistoryAdapter(GetFilteredHistories());
                _rvHistoryList.SetLayoutManager(new LinearLayoutManager(this));
                _rvHistoryList.SetAdapter(_adapter);
            }
            else
            {
                _adapter.KeyboxHistories = GetFilteredHistories();
                _adapter.NotifyDataSetChanged();
            }

            _iv7Days.Visibility = filter == 0 ? ViewStates.Visible : ViewStates.Invisible;
            _iv30Days.Visibility = filter == 1 ? ViewStates.Visible : ViewStates.Invisible;
            _ivAllDays.Visibility = filter == 2 ? ViewStates.Visible : ViewStates.Invisible;
        }
    }
}