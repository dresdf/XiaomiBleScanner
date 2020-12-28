using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XiaomiBleScanner.Models;

namespace XiaomiBleScanner
{
    class MijiaTempSensorScanService
    {
        const string MIJIA1 = "00000000-0000-0000-0000-582d34351e7f";
        const string MIJIA2 = "00000000-0000-0000-0000-582d3438125e";
        public ObservableCollection<MijiaTempSensor> Devices;
        public async Task<ObservableCollection<MijiaTempSensor>> ScanMijia(ObservableCollection<MijiaTempSensor> existingDevices)
        {
            Devices = existingDevices;
            var ble = CrossBluetoothLE.Current;
            var adapter = CrossBluetoothLE.Current.Adapter;
            adapter.DeviceDiscovered += OnDeviceDiscovered;

            if(ble.IsAvailable && ble.IsOn)
            {
                adapter.ScanMode = ScanMode.LowLatency;
                await adapter.StartScanningForDevicesAsync(null, DeviceFilter);
            }
            await adapter.StopScanningForDevicesAsync();
            return Devices;
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            var device = Devices.FirstOrDefault(d => d.DeviceId == e.Device.Id);
            bool newDevice = false;
            if(device == null)
            {
                device = new MijiaTempSensor
                {
                    DeviceId = e.Device.Id
                };
                newDevice = true;

                switch(device.DeviceId.ToString())
                {
                    case MIJIA1:
                        device.Name = "Mijia Temperature Sensor 1";
                        break;
                    case MIJIA2:
                        device.Name = "Mijia Temperature Sensor 2";
                        break;
                    default:
                        device.Name = e.Device.Name;
                        break;
                }
            }

            foreach(var record in e.Device.AdvertisementRecords)
            {
                if(record.Type == Plugin.BLE.Abstractions.AdvertisementRecordType.ServiceData)
                {
                    string raw = "";
                    foreach(var item in record.Data)
                    {
                        raw += item.ToString() + " ";
                    }

                    var (battery, temperature, humidity) = ReadServiceData(record.Data);

                    if(temperature.HasValue)
                        device.Temperature = temperature.Value;
                    if(humidity.HasValue)
                        device.Humidity = humidity.Value;

                    if(battery > 0)
                    {
                        device.Battery = battery;
                    }
                    device.LastUpdated = DateTime.Now;

                }
            }

            if(newDevice)
            {
                Devices.Add(device);
            }
        }

        private (double battery, double? temperature, double? humidity) ReadServiceData(byte[] data)
        {

            if(data.Length < 14)
                return (-1, null, null);

            double battery = -1;
            double? temp = null;
            double? humidity = null;

            if(data[13] == 0x04) //temp 4
            {
                temp = BitConverter.ToUInt16(new byte[] { data[16], data[17] }, 0) / 10.0;
            }
            else if(data[13] == 0x06) //humidity 6
            {
                humidity = BitConverter.ToUInt16(new byte[] { data[16], data[17] }, 0) / 10.0;
            }
            else if(data[13] == 0x0A) //battery 10
            {
                battery = data[16];
            }
            else if(data[13] == 0x0D) //temp + humidity 13
            {
                temp = BitConverter.ToUInt16(new byte[] { data[16], data[17] }, 0) / 10.0;
                humidity = BitConverter.ToUInt16(new byte[] { data[18], data[19] }, 0) / 10.0;
            }

            return (battery, temp, humidity);
        }

        private bool DeviceFilter(IDevice device)
        {
            if(device.Name?.StartsWith("MJ_HT_V1") ?? false)
            {
                return true;
            }
            return false;
        }
    }
}