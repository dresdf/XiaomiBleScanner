using Android.Support.V7.Widget;
using Android.Views;
using System.Collections.ObjectModel;
using XiaomiBleScanner.Models;

namespace XiaomiBleScanner
{
    // Adapter to connect the data set to the RecyclerView: 
    public class DevicesAdapter : RecyclerView.Adapter
    {
        ObservableCollection<MijiaTempSensor> DevicesList;
        // Create a new CardView (invoked by the layout manager): 
        public override RecyclerView.ViewHolder
            OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.RecyclerItemView, parent, false);

            // Create a ViewHolder to find and hold these view references
            DeviceViewHolder dvh = new DeviceViewHolder(itemView);
            return dvh;
        }

        // Fill in the contents of the card (invoked by the layout manager):
        public override void
            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DeviceViewHolder vh = holder as DeviceViewHolder;

            // Set the TextViews in this ViewHolder's CardView from this position in the data list
            vh.DeviceId.Text = $"ID: {DevicesList[position].DeviceId}";
            vh.DeviceName.Text = $"Name: {DevicesList[position].Name}";
            vh.DeviceTemperature.Text = $"Temperature: {DevicesList[position].Temperature} C";
            vh.DeviceHumidity.Text = $"Humidity: {DevicesList[position].Humidity} %";
            vh.DeviceBattery.Text = $"Battery: {DevicesList[position].Battery} %";
            vh.DeviceLastUpdated.Text = $"Last update: {DevicesList[position].LastUpdated}";

        }

        public DevicesAdapter(ObservableCollection<MijiaTempSensor> inputList)
        {
            DevicesList = inputList;
        }

        // Return the number of items available in the list:
        public override int ItemCount
        {
            get { return DevicesList.Count; }
        }

    }
}