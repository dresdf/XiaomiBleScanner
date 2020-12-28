using System;
using System.ComponentModel;

namespace XiaomiBleScanner.Models
{
    public class MijiaTempSensor : INotifyPropertyChanged
    {
        private Guid _deviceId;
        private string _name;
        private double _temperature;
        private double _humidity;
        private double _battery;
        private DateTime _lastUpdated;

        public Guid DeviceId
        {
            get => _deviceId;

            set
            {

                _deviceId = value;
                OnPropertyChanged("DeviceId");
            }

        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public double Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        public double Humidity
        {
            get => _humidity;
            set
            {
                _humidity = value;
                OnPropertyChanged("Humidity");
            }
        }

        public double Battery
        {
            get => _battery;
            set
            {
                _battery = value;
                OnPropertyChanged("Battery");
            }
        }

        public DateTime LastUpdated
        {
            get => _lastUpdated;
            set
            {
                _lastUpdated = value;
                OnPropertyChanged("LastUpdated");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

