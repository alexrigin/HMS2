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
using HMS.DataRecords;
using HMS.DataRecords;
using HMS.DataProviders;
using HMS.DataVirtualization;
using HMS.Managers;

namespace HMS
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private BatchesProvider batchesProvider;

        public Window2()
        {
            InitializeComponent();
            //dg.ItemsSource = DataManager.ExecuteToList<ArticleRecord>("SELECT * FROM articles;", new SQLiteConnection(Properties.Settings.Default.DBConnectionString), GetData);



        }

        private void ArticlesPanelBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (ArticlesPanelBtn.IsLoaded == true)
            //{
            //    if (ArticlesPanelBtn.IsChecked == true)
            //    {
            //        ArticlesPanel.Visibility = Visibility.Visible;
            //        gs.Visibility = Visibility.Visible;
            //        articles_lv.ItemsSource = Articles;//change
            //    }
            //    else
            //    {
            //        ArticlesPanel.Visibility = Visibility.Collapsed;
            //        gs.Visibility = Visibility.Collapsed;
            //        column1.Width = GridLength.Auto;
            //
            //    }
            //}
        }



        public ArticleRecord GetData(SQLiteDataReader r)
        {

            return (new ArticleRecord()
            {
                Id = Convert.ToInt32(r["id"].ToString()),
                Name = r["name"].ToString(),
                Number = r["number"].ToString(),
                //NominalDiameter = Convert.ToDouble(r["nominald"]),
                MinDiameter = Convert.ToDouble(r["mind"]),
                MaxDiameter = Convert.ToDouble(r["maxd"]),
                //NominalHeight = Convert.ToDouble(r["nominalh"]),
                MinHeight = Convert.ToDouble(r["minh"]),
                MaxHeight = Convert.ToDouble(r["maxh"]),
                //NominalSeamerHeight = Convert.ToDouble(r["nominalsh"]),
                MinSeamerHeight = Convert.ToDouble(r["minsh"]),
                MaxSeamerHeight = Convert.ToDouble(r["maxsh"])
            });
        }

        public IList<ArticleRecord> Articles
        {
            get { return (IList<ArticleRecord>)GetValue(ArticlesProperty); }
            set { SetValue(ArticlesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Articles.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArticlesProperty =
            DependencyProperty.Register("Articles", typeof(IList<ArticleRecord>), typeof(Window2), new PropertyMetadata(null));
    }
}
