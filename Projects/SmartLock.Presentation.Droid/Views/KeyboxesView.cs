using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using SmartLock.Model.BlueToothLe;
using SmartLock.Presentation.Core.Views;
using SmartLock.Presentation.Droid.Adapters;
using SmartLock.Presentation.Droid.Views.ViewBases;
using System;
using System.Collections.Generic;

namespace SmartLock.Presentation.Droid.Views
{
    [Activity(Theme = "@style/SmartLockTheme.NoActionBar")]
    public class KeyboxesView : FragmentView<IKeyboxesView>, IKeyboxesView
    {
        private RecyclerView _rvKeyboxList;

        private KeyboxAdapter _adapter;

        public event Action<Keybox> KeyboxClicked;

        protected override int LayoutId => Resource.Layout.View_Keyboxes;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _rvKeyboxList = _view.FindViewById<RecyclerView>(Resource.Id.rvKeyboxList);

            return _view;
        }
        public void Show(List<Keybox> keyboxes)
        {
            if (_adapter == null)
            {
                _adapter = new KeyboxAdapter(keyboxes, KeyboxClicked);
                _rvKeyboxList.SetLayoutManager(new LinearLayoutManager(Context));
                _rvKeyboxList.SetAdapter(_adapter);
            }
            else
            {
                _adapter.Keyboxes = keyboxes;
                _adapter.NotifyDataSetChanged();
            }
        }
    }
}