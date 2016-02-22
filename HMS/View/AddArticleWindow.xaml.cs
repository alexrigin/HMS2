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
using System.Diagnostics;
using System.Data.SQLite;
using HMS.DataRecords;
using HMS.Tools;
using System.Globalization;
using HMS.Managers;

namespace HMS.View
{
    /// <summary>
    /// Логика взаимодействия для AddArticleWindow.xaml
    /// </summary>
    public partial class AddArticleWindow : Window
    {
        public AddArticleWindow()
        {
            InitializeComponent();
            //DBManager.CreateDB();
            //double h = System.Windows.SystemParameters.PrimaryScreenHeight;
            //double w = System.Windows.SystemParameters.PrimaryScreenWidth;
            //Debug.WriteLine("current_resolution=" + w + "x" + h);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            //ArticleRecord article = new ArticleRecord(NameTextBox.Text,NumberTextBox.Text, 
            //    MinDTextBox.Text.ToDouble(), MaxDTextBox.Text.ToDouble(),
            //    MinHTextBox.Text.ToDouble(), MaxHTextBox.Text.ToDouble(),
            //    MinSHTextBox.Text.ToDouble(), MaxSHTextBox.Text.ToDouble());
            //DBManager.InsertIntoArticles(article, new SQLiteConnection(Properties.Settings.Default.DBConnectionString));

            //Debug.WriteLine(article.ToString());
            //this.Close();
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void CloseBtn2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
