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
using HMS.DataRecords;
using HMS.Managers;

namespace HMS.View
{

    /// <summary>
    /// Логика взаимодействия для MakeMeasure.xaml
    /// </summary>
    public partial class MakeMeasureMode : UserControl
    {

        public MakeMeasureMode()
        {
            InitializeComponent();

            articles_dg.ItemsSource = DataManager.Instance.ArticlesList; // init articles data grid
            LoadSettings();
        }

        private void ArticlesPanelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ArticlesPanelBtn.IsLoaded == true)
            {
                if (ArticlesPanelBtn.IsChecked == true)
                {
                    ArticlesPanel.Visibility = Visibility.Visible;
                    gs.Visibility = Visibility.Visible;
                    
                }
                else
                {
                    ArticlesPanel.Visibility = Visibility.Collapsed;
                    gs.Visibility = Visibility.Collapsed;
                    column1.Width = new GridLength(0.001,GridUnitType.Star);
                    column2.Width = GridLength.Auto;
                    column3.Width = new GridLength(100, GridUnitType.Star);
                }
            }

        }

        private void LoadSettings()
        {
            column1.Width = Properties.Settings.Default.ArticlesPanelWidth;
            Debug.WriteLine("Setting loaded");
            Debug.WriteLine("Width={0}", column1.Width);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        private void articles_dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataManager.Instance.CurrentArticle = articles_dg.SelectedValue as ArticleRecord;

            MessageBox.Show(String.Format("Ебать ты лох {0}", DataManager.Instance.CurrentArticle.ToString()));
        }
    }
}

//private void MainArea_MouseEnter(object sender, MouseEventArgs e)
//{
//    if (ArticlesPanelBtn.Visibility == Visibility.Visible)
//    {
//        ArticlesPanel.Visibility = Visibility.Collapsed;
//        gs.Visibility = Visibility.Collapsed;
//        column0.Width = GridLength.Auto;
//    }
//}
//
//private void ArticlesPanelPin_Click(object sender, RoutedEventArgs e)
//{
//    if (ArticlesPanelBtn.Visibility == Visibility.Collapsed)
//    {
//        ArticlesPanel.Visibility = Visibility.Collapsed;
//        ArticlesPanelBtn.Visibility = Visibility.Visible;
//        gs.Visibility = Visibility.Collapsed;
//        //pane1PinImage.Source = new BitmapImage(new Uri("pinHorizontal.gif", UriKind.Relative));
//    }
//    else
//    {
//        ArticlesPanelBtn.Visibility = Visibility.Collapsed;
//        //pane1PinImage.Source = new BitmapImage(new Uri("pin.gif", UriKind.Relative));
//    }
//}
