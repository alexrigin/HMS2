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
using Xceed.Wpf.DataGrid;
using System.Data.SQLite;

namespace HMS
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1() {
            InitializeComponent();
            //OrdersGrid.ItemsSource = new DataGridCollectionView((DBManager.ExecuteDataSet("SELECT * FROM articles;",new SQLiteConnection("Data Source=temp1.db; Version=3;"))).Tables["articles"].DefaultView);
            //DBManager.CreateTempDB();
            //DBManager.FillTempDB();
        }
    }
}
