using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using HMS.Tools;

namespace HMS.Managers
{

	//	serialPort = new SerialPort(portName);
	//	serialPort.Open();
	//   object p = serialPort.BaseStream.GetType().GetField("commProp", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(serialPort.BaseStream);
	//	Int32 bv = (Int32)p.GetType().GetField("dwSettableBaud", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(p);

	class RS232InputDevice : INotifyPropertyChanged
	{
		internal class DataEventArgs : EventArgs
		{
			public Double NumberRead { get; set; }
			public string StringRead { get; set; }
			public string RawData { get; set; }
		}

		SerialPort port = new SerialPort();
		public event EventHandler<DataEventArgs> DataReceived;
		public event EventHandler EofReceived;
		
		#region PublicMethods
		// todo записать в логи	ошибки
		public void Connect(string portName)
		{
			if (!this.port.IsOpen) {
				try {
					port.BaudRate = 9600;
					port.StopBits = StopBits.One;
					port.Handshake = Handshake.None;
					port.DataBits = 8;
					port.Parity = Parity.None;
					port.PortName = portName;
					port.DtrEnable = true;
					port.RtsEnable = true;
					port.Encoding = Encoding.ASCII;
					//port.ReadBufferSize = 16;
					//port.ReadTimeout = 100;
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
			if (port.IsOpen) {
				port.Close();
				OnPropertyChanged("isConnected");
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

		public void TriggerDataRead(string s)
		{
			try {
				port.WriteLine(s);
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
			throw new NotImplementedException();
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
					Debug.WriteLine("EofRecieved");
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
					//var raw = port.ReadTo(port.NewLine);
					Debug.WriteLine("Row is: " + raw);
					if (raw.StartsWith("01A")) {
						var s = raw.Substring(3, 9);
						if (DataReceived != null) {
							var args = new DataEventArgs {
								NumberRead = s.ToDouble(),
								StringRead = s,
								RawData = raw,
							};
							DataReceived(this, args); // start event
						}
					} else if(raw.StartsWith("9")) {

					}
					
				}
			}
			catch (ArgumentOutOfRangeException ex) {
				
				Debug.WriteLine(ex.StackTrace);
				//throw;
			}
			catch (System.IO.IOException ex) {
				Debug.WriteLine(ex.StackTrace);
				//throw;
			}
		}

		private void ProcessEof()
		{
			if (EofReceived != null) {
				EofReceived(this, null);
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

		public SerialPort Port
		{
			get
			{
				return port;
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
