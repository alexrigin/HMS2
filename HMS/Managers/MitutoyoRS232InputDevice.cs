using System;
using System.IO.Ports;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Tools;
using System.Threading;

namespace HMS.Managers
{
	class MitutoyoRS232InputDevice : InputDevice, INotifyPropertyChanged
	{
		internal class DataEventArgs : EventArgs
		{
			public Double NumberRead { get; set; }
			public string StringRead { get; set; }
			public string RawData { get; set; }
		}

		internal class ErrorEventArgs : EventArgs
		{
			public string ErrorData { get; set; }
		}

		SerialPort port = new SerialPort();
		public event EventHandler<DataEventArgs> DataReceived;
		public event EventHandler ErrorReceived;

		#region PublicMethods
		// todo записать в логи	ошибки
		public void Connect(string portName)
		{
			if (!this.port.IsOpen) {
				try {
					port.BaudRate = 9600;
					port.StopBits = StopBits.One;
					port.Handshake = Handshake.XOnXOff;
					port.DataBits = 8;
					port.Parity = Parity.None;
					port.PortName = portName;
					port.DtrEnable = true;
					port.RtsEnable = true;
					//port.ReadTimeout = 500;
					port.NewLine = "\r";

					port.DataReceived += Port_DataReceived;
					port.ErrorReceived += Port_ErrorReceived;
					port.PinChanged += Port_PinChanged;

					port.Open();
					OnPropertyChanged("IsConnected");
				}
				catch (Exception ex) {
					Debug.WriteLine(ex.Message);
					//throw;
				}
			}
		}
		public void Disconnect()
		{
			try {
				if (port.IsOpen) {
					port.Close();
					OnPropertyChanged("IsConnected");
				}
			}
			catch (Exception ex) {
				Debug.WriteLine(ex.Message);

			}
		}


		public void TriggerDataRead(string s)
		{
			try {
				port.WriteLine(s);
			}
			catch (Exception e) {
				Debug.WriteLine(e.Message);
			}
		}

		public void TriggerDataRead()
		{
			try {
				port.WriteLine("1");
			}
			catch (Exception e) {
				Debug.WriteLine(e.Message);
			}
		}

		#endregion

		#region EventsHandling
		//TODO реализовать
		private void Port_PinChanged(object sender, SerialPinChangedEventArgs e)
		{

			try {
				Debug.WriteLine(port.IsOpen);
				if (port.IsOpen) {
					//port.Close();
					//OnPropertyChanged("IsConnected");
				}
			}
			catch (Exception ex) {
				Debug.WriteLine(ex.Message);

			}
		}
		//TODO реализовать при необходимости
		private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
			Debug.WriteLine("Port_ErrorReceived");
		}
		private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			switch (e.EventType) {
				case SerialData.Chars:
					Debug.WriteLine("DataRecieved");
					ProcessData();
					break;
				case SerialData.Eof:
					break;
				default:
					break;
			}
		}
		//TODO записать в логи ошибки
		private void ProcessData()
		{
			Thread.Sleep(100);
			try {
				if (port.IsOpen) {
					var raw = port.ReadExisting();
					//Debug.WriteLine("Row is: " + raw);
					if (raw.StartsWith("01A")) {
						Debug.WriteLine("Row is: " + raw);
						var s = raw.Substring(3, 9);
						if (DataReceived != null) {
							var args = new DataEventArgs {
								NumberRead = s.ToDouble(),
								StringRead = s,
								RawData = raw,
							};
							DataReceived(this, args); // start event
						}
					} else if (raw.StartsWith("9")) {
						Debug.WriteLine("Row is: " + raw);
						if (ErrorReceived != null) {
							var args = new ErrorEventArgs {
								ErrorData = raw
							};
							ErrorReceived(this, args);
						}
					}
				}
			}
			catch (ArgumentOutOfRangeException ex) {
				Debug.WriteLine(ex.Message);
				//throw;
			}
			catch (System.IO.IOException ex) {
				Debug.WriteLine(ex.Message);
				//throw;
			}
		}


		#endregion

		#region Properties
		public bool IsConnected
		{
			get
			{
				return port.IsOpen;
			}
		}
		#endregion

		#region NotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
