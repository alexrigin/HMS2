using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Input;
using HMS.DataRecords;
using HMS.Managers;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;

using PropertyValues = System.Collections.Generic.KeyValuePair<System.DateTime, System.Double>;
using Microsoft.Research.DynamicDataDisplay.Common;

namespace HMS.View
{
	/// <summary>
	/// Логика взаимодействия для PropertyTab.xaml
	/// </summary>

	public partial class ParameterControl : UserControl, INotifyPropertyChanged
	{
		private ObservableCollection<PropertyValues> _batch = new ObservableCollection<PropertyValues>();
		private ObservableCollection<PropertyValues> _lastMeasurements;
		private ObservableDataSource<PropertyValues> _graphMeasurements;
		private ArticleRecord _article = null;

		private bool _isStarted = false;
		private bool _isFinished = false;
		private bool _isActive = false;
		private bool _isDataRelevant = false;
		private int _batchNumber = DataManager.GetMaxBatchNumber() + 1; // номер партии в БД
		private DateTime _date; // дата начала замера партии

		private string _parameterName;
		private string _parameterKey;
		private ParameterType _parameterType;

		private double _minValue;
		private double _maxValue;
		private double _nominalValue;
		private double _measuredValue;
		private double _difference;

		private int _recordsCount; // количество сделанных измерений

		private BackgroundWorker _bwloader = new BackgroundWorker();    //загрузка данных из бд
		private MitutoyoRS232InputDevice _inputDevice = new MitutoyoRS232InputDevice();

		private DispatcherTimer _updateMeasuredValueTimer;

		private ParameterControl()
		{
			InitializeComponent();
			this.DataContext = this;
		}

		public ParameterControl(ArticleRecord article, ParameterType type, int batchNumber) : this()
		{
			Article = article;
			_parameterType = type;
			InitPropertiesByType(type);
			BatchNumber = batchNumber;
			NewMeasurementsDataGrid.ItemsSource = _batch;
			InitUpdateMeasuredValueTimer();

			_inputDevice.DataReceived += inputDevice_DataReceived;
			_inputDevice.ErrorReceived += inputDevice_ErrorReceived;

			_bwloader.DoWork += bwloader_DoWork;
			_bwloader.RunWorkerCompleted += bwloader_RunWorkerCompleted;


			//MeasuringValueBG1.Minimum = MeasuringValueBG2.Minimum = _nominalValue;
			//MeasuringValueBG2.Maximum = _maxValue + 3;
			//_difference = _nominalValue - _minValue;
			//MeasuringValueBG1.Maximum = _nominalValue + _difference +3;
			MeasuringValueBG1.Minimum = MeasuringValueBG2.Minimum = _nominalValue;
			MeasuringValueBG2.Maximum = 14;
			_difference = _nominalValue - _minValue;
			MeasuringValueBG1.Maximum = 14;

			_bwloader.RunWorkerAsync();
		}

		

		#region Initialization

		private void InitPropertiesByType(ParameterType type)
		{
			switch (type) {
				case ParameterType.Height: {
						SetParameterProperties("Высота", ParameterType.Height.ToString(), Article.NominalHeight, Article.MinHeight, Article.MaxHeight);
					}
					break;
				case ParameterType.SeamerHeight: {
						SetParameterProperties("Высота закатки", ParameterType.SeamerHeight.ToString(), Article.NominalSeamerHeight,
							Article.MinSeamerHeight, Article.MaxSeamerHeight);
					}
					break;
				case ParameterType.Diameter: {
						SetParameterProperties("Диаметр", ParameterType.Diameter.ToString(), Article.NominalDiameter, Article.MinDiameter, Article.MaxDiameter);
					}
					break;
				case ParameterType.Weight:
					ParameterName = "Weight";
					throw new Exception("System does not support that type of property");
				//break;
				default:
					break;
			}
		}

		private void SetParameterProperties(string name, string key, double nominalValue, double minValue, double maxValue)
		{
			ParameterName = name;
			ParameterKey = key;
			_nominalValue = nominalValue;
			_minValue = minValue;
			_maxValue = maxValue;
			NewDGColumnName.Header = name;
			LastDGColumnName.Header = name;
		}

		//todo записать ошибку в лог и проинформировать пользователя о проблеме
		private void bwloader_DoWork(object sender, DoWorkEventArgs e)
		{
			try { 
			_lastMeasurements = new ObservableCollection<PropertyValues>(DataManager.GetLastMeasurements(ParameterKey, Article, 100));
			_graphMeasurements = new ObservableDataSource<PropertyValues>(_lastMeasurements);
			}
			catch (SQLiteException ex) {
				Debug.WriteLine(ex.Message);
			}
			Thread.Sleep(2000);
		}

		private void bwloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			InitUIControlsItemsSource();
			LastMeasurementsDataGrid.Visibility = Visibility.Visible;
			plotter.Visibility = Visibility.Visible;
			LoadLastDGProgressRing.IsActive = false;
			LoadGraphProgressRing.IsActive = false;
		}

		private void InitUIControlsItemsSource()
		{
			InitLastMeasurementsDG();
			InitGraphDataSource();
		}

		private void InitLastMeasurementsDG()
		{
			LastMeasurementsDataGrid.ItemsSource = _lastMeasurements;
			if (_lastMeasurements.Count != 0) {
				LastMeasurementsDataGrid.ScrollIntoView(_lastMeasurements.Last());
			}
		}
		private void InitGraphDataSource()
		{
			_graphMeasurements.SetXMapping(x => dateAxis.ConvertToDouble((DateTime)x.Key));
			_graphMeasurements.SetYMapping(y => (double)y.Value);
			//graphData.AddMapping(ShapeElementPointMarker.ToolTipTextProperty, y => string.Format("Значение: {0}; Дата: {1}", y.Value, y.Key));
			//plotter.Viewport.Restrictions.Add(new PhysicalProportionsRestriction { ProportionRatio = 100 });

			//plotter.AddLineGraph(
			//	_graphMeasurements,
			//	new Pen(Brushes.DarkViolet, 3),
			//	new SampleMarker(),
			//	new PenDescription(this.ParameterName)
			//);

			plotter.AddLineGraph(
				_graphMeasurements,
				new Pen(Brushes.DarkViolet, 3),
				new CircleElementPointMarker {
					Size = 7,
					Brush = Brushes.Black,
					Fill = Brushes.DarkBlue
				},
				new PenDescription(this.ParameterName)
			);
			plotter.FitToView();
		}

		private void InitUpdateMeasuredValueTimer()
		{
			_updateMeasuredValueTimer = new DispatcherTimer();
			_updateMeasuredValueTimer.Interval = TimeSpan.FromMilliseconds(500);
			_updateMeasuredValueTimer.Tick += _updateMeasuredValueTimer_Tick;
		}

		

		#endregion

		#region Measuring

		private void StartMeasuring()
		{
			if (!_inputDevice.IsConnected) {
				string deviceId;
				switch (_parameterType) {
					case ParameterType.Diameter:
						deviceId = Properties.Settings.Default.DiameterDevice;
						break;
					case ParameterType.Height:
						deviceId = Properties.Settings.Default.HeightDevice;
						break;
					case ParameterType.SeamerHeight:
						deviceId = Properties.Settings.Default.SeamerHeightDevice;
						break;
					default:
						deviceId = "COM1";
						break;
				}
				_inputDevice.Connect(deviceId);

				_updateMeasuredValueTimer.Start();
			}
		}

		private void _updateMeasuredValueTimer_Tick(object sender, EventArgs e)
		{
			
			if (!_inputDevice.IsConnected) 
				{
					string deviceId;
					switch (_parameterType) {
						case ParameterType.Diameter:
							deviceId = Properties.Settings.Default.DiameterDevice;
							break;
						case ParameterType.Height:
							deviceId = Properties.Settings.Default.HeightDevice;
							break;
						case ParameterType.SeamerHeight:
							deviceId = Properties.Settings.Default.SeamerHeightDevice;
							break;
						default:
							deviceId = "COM1";
							break;
					}
					_inputDevice.Connect(deviceId);
				}

				_inputDevice.TriggerDataRead();
		}

		//TODO оптимизировать
		private void inputDevice_DataReceived(object sender, MitutoyoRS232InputDevice.DataEventArgs e)
		{
			if (!IsDataRelevant) {
				IsDataRelevant = true;
			}
			MeasuredValue = e.NumberRead;
			//Debug.WriteLine(e.NumberRead);
			Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () {
				UpdateValueScale();
			} );
			
		}

		private void inputDevice_ErrorReceived(object sender, EventArgs e)
		{
			if (IsDataRelevant) {
				IsDataRelevant = false;
				CommandManager.InvalidateRequerySuggested();
			}
		}

		//TODO оптимизировать
		private void UpdateValueScale()
		{
			if (_measuredValue > _nominalValue) {
				
				MeasuringValueBG1.Value = _nominalValue;
				MeasuringValueBG2.Value = _measuredValue;

			} else {

				

				MeasuringValueBG2.Value = _nominalValue;
				MeasuringValueBG1.Value = MeasuringValueBG1.Maximum - _measuredValue + _difference;
				//Debug.WriteLine("Messdf" + _measuredValue);
				//Debug.WriteLine("Messdf" + _measuredValue + _difference);
				//Debug.WriteLine("Messdf" + MeasuringValueBG1.Value);
			}
		}

		#endregion

		#region PublicMethods
		public void ActivateMeasuring()
		{
			if (!_isActive) {
				_isActive = true;
				StartMeasuring();
			}
		}

		public void DisactivateMeasuring()
		{
			if (_isActive) {
				_isActive = false;
				if (_inputDevice.IsConnected)
					_inputDevice.Disconnect();
				_updateMeasuredValueTimer.Stop();
			}
		}

		//TODO добавить текст "Завершено" в заголовок групбокса
		public void Finish()
		{
			IsFinished = true;
			// добавить текст "Завершено" в заголовок групбокса
		}

		//TODO Раскоментить
		public bool MakeMeasurement(int maxCountNumber, int startIndex)
		{
			//if (IsDataRelevant) {

				double value = MeasuredValue; // here we get the value from device
				int measurementId;

				try {
					if (!IsStarted) { Date = DateTime.Now; }
					DateTime time = DateTime.Now;
					PropertyValues pv = new PropertyValues(time, value);

					if (RecordsCount < maxCountNumber) {
						measurementId = startIndex + RecordsCount;
						DataManager.UpdateMeasurementRecord(ParameterKey, pv, measurementId);

					} else {
						measurementId = DataManager.GetMaxMeasurementId() + 1;
						DataManager.InsertIntoMeasurements(ParameterKey, Article.Id, Date, pv, BatchNumber, measurementId);
					}
					RecordsCount++;
					_batch.Add(pv);
					NewMeasurementsDataGrid.ScrollIntoView(_batch.Last());
					AddToLastMeasurements(pv);
					AddToGraphCollection(pv);
				}
				catch (SQLiteException ex) {
					MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}
				return true;
			//}
			//return false;
		}


		#endregion

		#region Assistant
		private void AddToGraphCollection(PropertyValues pv)
		{

			DateTime date = pv.Key;
			DateTime date1 = pv.Key;
			DateTime date2 = new DateTime();
			Point p1 = new Point(dateAxis.ConvertToDouble(date), pv.Value);
			_graphMeasurements.AppendAsync(Dispatcher, pv);
			
			double yMin = MinValue * 0.9;
			double yMax = MaxValue * 1.1;
			double xMin = dateAxis.ConvertToDouble(date.AddHours(-8));
			double xMax = dateAxis.ConvertToDouble(date);
			plotter.Viewport.Visible = new DataRect(xMin, yMin, xMax - xMin, yMax - yMin);
		}
		private void AddToLastMeasurements(PropertyValues values)
		{
			_lastMeasurements.Add(values);
			LastMeasurementsDataGrid.ScrollIntoView(_lastMeasurements.Last());
		}

		#endregion

		#region UIEvents
		private void NewMeasurementsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = (e.Row.GetIndex() + 1).ToString();
		}
		private void LastMeasurementsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = (e.Row.GetIndex() + 1).ToString();
		}
		#endregion

		#region Properties

		public ObservableCollection<PropertyValues> Batch { get { return _batch; } }
		public string ParameterName
		{
			get
			{
				return _parameterName;
			}
			private set
			{
				if (_parameterName != value) {
					_parameterName = value;
					OnPropertyChanged("PropertyName");
				}
			}
		}
		public int RecordsCount
		{
			get
			{
				return _recordsCount;
			}
			private set
			{
				if (_recordsCount != value) {
					_recordsCount = value;
					OnPropertyChanged("RecordsCount");
				}
			}
		}

		public string ParameterKey { get { return _parameterKey; } private set { _parameterKey = value; } }
		public ArticleRecord Article { get { return _article; } private set { _article = value; } }
		public bool IsFinished { get { return _isFinished; } private set { _isFinished = value; } }
		public bool IsStarted { get { return _isStarted; } private set { _isStarted = value;  } }
		public bool IsActive { get { return _isActive; } private set {_isActive = value; } }
		public bool IsDataRelevant
		{
			get
			{
				return _isDataRelevant;
			}
			private set
			{
				if (_isDataRelevant != value) {
					_isDataRelevant = value;
					OnPropertyChanged("IsDataRelevant");
					Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () {
						CommandManager.InvalidateRequerySuggested();
					});
				}
			}
		}
		public int BatchNumber { get { return _batchNumber; } private set { _batchNumber = value; } }
		public DateTime Date { get { return _date; } private set { _date = value; } }
	
		public double MinValue { get { return _minValue; } }
		public double MaxValue { get { return _maxValue; } }
		public double NominalValue { get { return _nominalValue; } }
		public double MeasuredValue
		{
			get
			{
				return _measuredValue;
			}

			set
			{
				if (_measuredValue != value) {
					_measuredValue = value;
					OnPropertyChanged("MeasuredValue");
				}
			}
		}

		#endregion

		#region OnPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//Debug.WriteLine("InputDevice"+_inputDevice.Port.CDHolding);
			if (!_inputDevice.IsConnected) {
				_inputDevice.Connect("COM3");
			}
			_inputDevice.TriggerDataRead(textbox.Text);
			//Debug.WriteLine("InputDevice" + _inputDevice.Port.CDHolding);

		}
	}

	//TODO претендент на удаление
	public class SampleMarker : ShapeElementPointMarker
	{
		public override UIElement CreateMarker()
		{
			Canvas result = new Canvas() {
				Width = 10,
				Height = Size
			};

			result.Width = Size;
			result.Height = Size;
			result.Background = Brush;
			if (ToolTipText != String.Empty) {
				ToolTip tt = new ToolTip();
				tt.Content = ToolTipText;
				result.ToolTip = tt;
			}
			return result;
		}

		public override void SetPosition(UIElement marker, Point screenPoint)
		{
			Canvas.SetLeft(marker, screenPoint.X - Size / 2);
			Canvas.SetTop(marker, screenPoint.Y - Size / 2);
		}

	}
}