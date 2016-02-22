using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SQLite;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using HMS.DataRecords;
using HMS.DataProviders;
using HMS.DataVirtualization;
using HMS.Managers;
using HMS.SQLQuery;

namespace HMS.View
{
    /// <summary>
    /// Логика взаимодействия для WatchMode.xaml
    /// </summary>
    public partial class WatchMode : UserControl
    {
        private BatchesProvider _batchesProvider;


		public WatchMode(ObservableCollection<ArticleRecord> articles)
        {
            InitializeComponent();

            articles_dg.ItemsSource = articles; // init articles_datagrid

			articlesNameTextBox.ItemsSource = articles;
			articlesNumberTextBox.ItemsSource = articles;

			_batchesProvider = new BatchesProvider(1000, 0);
			AsyncVirtualizingCollection<BatchRecord> _batchesList = new AsyncVirtualizingCollection<BatchRecord> (_batchesProvider, 1000, 3000);
            measurements_dg.ItemsSource = _batchesList;
        }

        private void HandleExpandCollapseForRow(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            DataGridRow selectedRow = DataGridRow.GetRowContainingElement(btn);
            if (btn != null && btn.Content.ToString() == "+")
            {
                selectedRow.DetailsVisibility = Visibility.Visible;
                btn.Content = "-";
            }
            else
            {
                selectedRow.DetailsVisibility = Visibility.Collapsed;
                btn.Content = "+";
            }
        }


		private void SearchBtn_Click(object sender, RoutedEventArgs e)
		{
			_batchesProvider.AddFilter(BuildSearchWhereStatement());	
		}

		private List<WhereClause> BuildSearchWhereStatement()
		{
			List<WhereClause>  whereClauses = new List<WhereClause>();
			//if (lb_articlesName.SelectedValue.ToString() != string.Empty)
			//	whereClauses.Add(new WhereClause ("a.name", Comparison.Like, "%" + lb_articlesName.SelectedValue.ToString() + "%"));

			//if (lb_articlesNumber.SelectedValue.ToString() != string.Empty)
			//	whereClauses.Add(new WhereClause("a.number", Comparison.Like, "%"+lb_articlesNumber.SelectedValue.ToString()+"%"));

			if(StartDatePicker.SelectedDate.HasValue)
				whereClauses.Add(new WhereClause("m.date", Comparison.LessOrEquals, StartDatePicker.SelectedDate));
			
			if(EndDatePicker.SelectedDate.HasValue)
				whereClauses.Add(new WhereClause("m.date", Comparison.GreaterOrEquals, EndDatePicker.SelectedDate));

			return whereClauses;
		}

		private void ResetBtn_Click(object sender, RoutedEventArgs e)
		{
			_batchesProvider.ResetFilters();
		}

		private void ExportToExcelBtn_Click(object sender, RoutedEventArgs e)
		{
			SQLSelectQuery query = new SQLSelectQuery(_batchesProvider.Query);
			query.LimitClear();
			query.BuildQuery();
			Debug.WriteLine("SDFDSFSDFSDFSDF"+query.Query);
			IList<BatchRecord> batches = DataManager.ExecuteToList<BatchRecord>(query.Query, 
				new SQLiteConnection(Properties.Settings.Default.DBConnectionString), DataManager.ReadBatch);
			ExportManager.ExportToExcel(batches);
		}
	}
}
