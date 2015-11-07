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
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using HMS.Managers;
using HMS.DataProviders;
using HMS.DataRecords;
using HMS.DataVirtualization;


namespace HMS.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BatchesProvider batchesProvider;
        public MainWindow()
        {
            InitializeComponent();
            articlesLb.ItemsSource = DBManager.ExecuteArticlesToListForComboBox("SELECT name FROM articles", new SQLiteConnection(Properties.Settings.Default.DBConnectionString));

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();

            batchesProvider = new BatchesProvider(1000, 0);
            AsyncVirtualizingCollection<BatchRecord> batchesList = new AsyncVirtualizingCollection<BatchRecord>(batchesProvider, 1000, 3000);
            DataGrid2.ItemsSource = batchesList;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            tbMemory.Text = string.Format("{0:0.00} MB", GC.GetTotalMemory(true) / 1024.0 / 1024.0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         

        }
    }
}
