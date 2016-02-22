using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DataRecords
{
    class NotifyMeasurementRecord : MeasurementRecord, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public new double? Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        public new DateTime? HTime
        {
            get { return _hTime; }
            set
            {
                if (_hTime != value)
                {
                    _hTime = value;
                    OnPropertyChanged("HTime");
                }
            }
        }

        public new double? SeamerHeight
        {
            get { return _seamerHeight; }
            set
            {
                if (_seamerHeight != value)
                {
                    _seamerHeight = value;
                    OnPropertyChanged("SeamerHeight");
                }
            }
        }

        public new DateTime? SHTime
        {
            get { return _shTime; }
            set
            {
                if (_shTime != value)
                {
                    _shTime = value;
                    OnPropertyChanged("SHTime");
                }
            }
        }

        public new double? Diameter
        {
            get { return _diameter; }
            set
            {
                if (_diameter != value)
                {
                    _diameter = value;
                    OnPropertyChanged("Diameter");
                }
            }
        }

        public new DateTime? DTime
        {
            get { return _dTime; }
            set
            {
                if (_dTime != value)
                {
                    _dTime = value;
                    OnPropertyChanged("DTime");
                }
            }
        }

    }
}
