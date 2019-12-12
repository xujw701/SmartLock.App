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
    public class ListingView : FragmentView<IListingView>, IListingView
    {
        private RecyclerView _rvBleRecordList;
        private LockboxRecordAdapter _adapter;

        protected override int LayoutId => Resource.Layout.View_Listing;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            //_rvBleRecordList = _view.FindViewById<RecyclerView>(Resource.Id.rvBleRecordList);

            return _view;
        }

        public void Show(List<LockboxRecord> lockboxRecords)
        {
            //if (_adapter == null)
            //{
            //    _adapter = new LockboxRecordAdapter(lockboxRecords);
            //    _rvBleRecordList.SetLayoutManager(new LinearLayoutManager(Context));
            //    _rvBleRecordList.SetAdapter(_adapter);
            //}
            //else
            //{
            //    _adapter.LockboxRecords = lockboxRecords;
            //    _adapter.NotifyDataSetChanged();
            //}
        }
    }
}