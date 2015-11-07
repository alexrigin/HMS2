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
using System.Diagnostics;
using System.Windows.Threading;
using HMS.DataVirtualization;
using HMS.DataProviders;
using HMS.DataRecords;
using HMS.Managers;
using System.Data.SQLite;
using HMS.Tools;

namespace HMS
{
    /// <summary>
    /// Логика взаимодействия для DataGrid.xaml
    /// </summary>
    public partial class DataGrid : Window
    {
        private ArticlesProvider articlesProvider;
        private BatchesProvider batchesProvider;
        private string buff;
        public DataGrid()
        {
            InitializeComponent();

            //CollectionViewSource itemCollectionViewSouce;
            //itemCollectionViewSouce = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            //itemCollectionViewSouce.Source = list;


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();

            cb.ItemsSource = DBManager.ExecuteArticlesToListForComboBox("SELECT number from articles order by number;",new SQLiteConnection(Properties.Settings.Default.DBConnectionString));
        }



        private void timer_Tick(object sender, EventArgs e)
        {
            tbMemory.Text = string.Format("{0:0.00} MB", GC.GetTotalMemory(true) / 1024.0 / 1024.0);
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {

            articlesProvider = new ArticlesProvider(1000,0);
            AsyncVirtualizingCollection<ArticleRecord> articlesList = new AsyncVirtualizingCollection<ArticleRecord>(articlesProvider, 1000, 3000);
            DataGrid1.ItemsSource = articlesList;
            //DataGrid1.DataContext = articlesList;
            //DBManager.FillTempBD2();
            //DataGrid1.ItemsSource = DBManager.ExecuteDataSet("SELECT * FROM articles", new SQLiteConnection(Properties.Settings.Default.DBConnectionString)).Tables["articles"].DefaultView;
            //DBManager.CreateDB();

            //SQLRequest r = new SQLRequest("id,name,poebota", "articles a,batches b", "", "SORT BY id", "LIMIT 1,3");
            //Debug.WriteLine(r.SqlRequestString());
            //r.AddFilter("id>100");
            //r.AddFilter("lesha=mydak");
            //r.AddFilter("3aeb=na4alcya");
            //Debug.WriteLine(r.SqlRequestString());
            //r.RemoveFilter("id>100");
            //Debug.WriteLine(r.SqlRequestString());
            //r.RemoveFilter("3aeb=na4alcya");
            //Debug.WriteLine(r.SqlRequestString());
            //r.RemoveFilter("lesha=mydak");
            //Debug.WriteLine(r.SqlRequestString());
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            batchesProvider = new BatchesProvider(1000, 0);
            AsyncVirtualizingCollection<BatchRecord> batchesList = new AsyncVirtualizingCollection<BatchRecord>(batchesProvider, 1000, 3000);
            DataGrid2.ItemsSource = batchesList;
        }

        private void Поиск_TextChanged(object sender, TextChangedEventArgs e)
        {
            articlesProvider.AddFilter(string.Format("name like '%{0}%'",Поиск.Text));
            AsyncVirtualizingCollection<ArticleRecord> articlesList = new AsyncVirtualizingCollection<ArticleRecord>(articlesProvider, 1000, 3000);
            DataGrid1.ItemsSource = articlesList;
        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buff = cb.Items.CurrentItem.ToString();
            batchesProvider.AddFilter(string.Format("number like '{0}'",cb.SelectedItem.ToString()));
            AsyncVirtualizingCollection<BatchRecord> batchesList = new AsyncVirtualizingCollection<BatchRecord>(batchesProvider, 1000, 3000);
            DataGrid2.ItemsSource = batchesList;
        }

        void ShowHideDetails(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility =
                      row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }
    }
}
