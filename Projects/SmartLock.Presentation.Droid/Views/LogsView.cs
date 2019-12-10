using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar")]
    public class LogsView : ViewBase<ILogsView>, ILogsView
    {
        private RecyclerView _rvBleRecordList;
        private LockboxRecordAdapter _adapter;

        protected override int LayoutId => Resource.Layout.View_Logs;
        protected override bool SwipeRefresh => false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _rvBleRecordList = FindViewById<RecyclerView>(Resource.Id.rvBleRecordList);
        }

        public void Show(List<LockboxRecord> lockboxRecords)
        {
            if (_adapter == null)
            {
                _adapter = new LockboxRecordAdapter(lockboxRecords);
                _rvBleRecordList.SetLayoutManager(new LinearLayoutManager(this));
                _rvBleRecordList.SetAdapter(_adapter);
            }
            else
            {
                _adapter.LockboxRecords = lockboxRecords;
                _adapter.NotifyDataSetChanged();
            }
        }
    }
}