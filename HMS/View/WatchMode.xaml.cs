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
using HMS.DataRecords;
using HMS.DataProviders;
using HMS.DataVirtualization;
using HMS.Managers;

namespace HMS.View
{
    /// <summary>
    /// Логика взаимодействия для WatchMode.xaml
    /// </summary>
    public partial class WatchMode : UserControl
    {
        private BatchesProvider batchesProvider;

        public WatchMode()
        {
            InitializeComponent();

            articles_dg.ItemsSource = DataManager.Instance.ArticlesList; // init articles data grid

            batchesProvider = new BatchesProvider(1000, 0);
            AsyncVirtualizingCollection<BatchRecord> batchesList = new AsyncVirtualizingCollection<BatchRecord>(batchesProvider, 1000, 3000);
            measurements_dg.ItemsSource = batchesList;
        }

        private void FiltersPanelBtn_Click(object sender, RoutedEventArgs e)
        {
            FiltersPanel.Visibility = Visibility.Visible;
            gs.Visibility = Visibility.Visible;
            //lb_articles.ItemsSource = Articles;

            if (FiltersPanelBtn.IsLoaded == true)
            {
                if (FiltersPanelBtn.IsChecked == true)
                {
                    FiltersPanel.Visibility = Visibility.Visible;
                    gs.Visibility = Visibility.Visible;
                }
                else
                {
                    FiltersPanel.Visibility = Visibility.Collapsed;
                    gs.Visibility = Visibility.Collapsed;
                    column1.Width = GridLength.Auto;

                }
            }
        }
    }
}
