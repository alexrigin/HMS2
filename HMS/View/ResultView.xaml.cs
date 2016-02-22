using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using HMS.DataRecords;

namespace HMS.View
{
	/// <summary>
	/// Логика взаимодействия для ResultView.xaml
	/// </summary>
	public partial class ResultView : UserControl
	{

		private List<MeasurementRecord> _batch = new List<MeasurementRecord>();
		public ResultView(List<MeasurementRecord> _batch)
		{
			InitializeComponent();
			ResultDG.ItemsSource = _batch;
		}

		private void LastMeasurementsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = (e.Row.GetIndex() + 1).ToString();
		}
	}
}
