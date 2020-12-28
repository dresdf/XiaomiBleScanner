using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XiaomiBleScanner.Models;

namespace XiaomiBleScanner
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public ObservableCollection<MijiaTempSensor> Devices = new ObservableCollection<MijiaTempSensor>();


        // RecyclerView instance 
        RecyclerView deviceRecyclerView;

        // Layout manager that lays out each card in the RecyclerView:
        RecyclerView.LayoutManager deviceLayoutManager;

        // Adapter that accesses the data set:
        DevicesAdapter mAdapter;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            ScanBle();

        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            ScanBle();

        }

        public async void ScanBle()
        {
            MijiaTempSensorScanService mService = new MijiaTempSensorScanService();
            Devices = await mService.ScanMijia(Devices);

            // Get RecyclerView layout:
            deviceRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            // Use the built-in linear layout manager:
            deviceLayoutManager = new LinearLayoutManager(this);
            // Plug the layout manager into the RecyclerView:
            deviceRecyclerView.SetLayoutManager(deviceLayoutManager);

            // Create an adapter for the RecyclerView, and pass it the
            // data set to manage:
            mAdapter = new DevicesAdapter(Devices);

            // Plug the adapter into the RecyclerView:
            deviceRecyclerView.SetAdapter(mAdapter);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if(id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }



        //permissions plugin
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async Task<bool> RequestPermissions()
        {
            if(DeviceInfo.Platform != DevicePlatform.Android)
                return true;

            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();

                if(status != PermissionStatus.Granted)
                {
                    var requestStatus = await Permissions.RequestAsync<Permissions.LocationAlways>();
                    return requestStatus == PermissionStatus.Granted;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception)
            {
                return false;
                //Something went wrong
            }
        }

    }//end of class


}//end of namespace
