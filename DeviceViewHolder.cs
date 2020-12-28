using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace XiaomiBleScanner
{
    // Implement the ViewHolder pattern: each ViewHolder holds references
    // to the UI components within the CardView 
    // that is displayed in a row of the RecyclerView:
    public class DeviceViewHolder : RecyclerView.ViewHolder
    {
        public TextView DeviceId { get; private set; }
        public TextView DeviceName { get; private set; }
        public TextView DeviceTemperature { get; private set; }
        public TextView DeviceHumidity { get; private set; }
        public TextView DeviceBattery { get; private set; }
        public TextView DeviceLastUpdated { get; private set; }


        // Get references to the views defined in the CardView layout.
        public DeviceViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            DeviceId = itemView.FindViewById<TextView>(Resource.Id.deviceIdTextView);
            DeviceName = itemView.FindViewById<TextView>(Resource.Id.deviceNameTextView);
            DeviceTemperature = itemView.FindViewById<TextView>(Resource.Id.deviceTemperatureTextView);
            DeviceHumidity = itemView.FindViewById<TextView>(Resource.Id.deviceHumidityTextView);
            DeviceBattery = itemView.FindViewById<TextView>(Resource.Id.deviceBatteryTextView);
            DeviceLastUpdated = itemView.FindViewById<TextView>(Resource.Id.deviceLastUpdatedTextView);
        }
    }
}