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
using System.Windows.Shapes;
using System.Diagnostics;
using MahApps.Metro.Controls;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using HMS.DataRecords;
using HMS.Managers;
using HMS.SQLQuery;
using HMS.View;

namespace HMS.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        private ObservableCollection<ArticleRecord> _articles = new ObservableCollection<ArticleRecord>(DataManager.ExecuteToList<ArticleRecord>("SELECT * FROM articles;",
			new SQLiteConnection(Properties.Settings.Default.DBConnectionString), ReadArticlesFromDB));

		private RelayActionCommand _addNewArticleCommand;

		

		public MainWindow()
        {
            InitializeComponent();
		
            MakeMeasureMode.Children.Add(new MakeMeasureMode(_articles));
            WatchMode.Children.Add(new WatchMode(_articles));

			

			_addNewArticleCommand = new RelayActionCommand();
			AddNewArticleCommand.CanExecuteAction += IsCanExecute;
			AddNewArticleCommand.ExecuteAction += ShowNewArticleWindow;
		
		}

        private void SwitchToMakeMeasureMode_Checked(object sender, RoutedEventArgs e)
        {
            if(SwitchToMakeMeasureMode.IsLoaded==true)
            {
                WatchMode.Visibility = Visibility.Collapsed;
                MakeMeasureMode.Visibility = Visibility.Visible;
            }
        }

		private bool IsCanExecute(object obj)
		{
			return true;
		}

        private void SwitchToWatchMode_Checked(object sender, RoutedEventArgs e)
        {
            MakeMeasureMode.Visibility = Visibility.Collapsed;
            WatchMode.Visibility = Visibility.Visible;
        }

        private void LoadSettings()
        {														 

		}

		private void ShowNewArticleWindow(object obj)
		{
			(new SimpleNewArticleWindow(_articles)).ShowDialog();
		}

        private void SaveSettings()
        {
            //Properties.Settings.Default.ArticlesPanelWidth = MakeMeasureMode.column1.Width;
            Properties.Settings.Default.Save();
            Debug.WriteLine("Setting Saved");
            //Debug.WriteLine("Width={0}",MakeMeasureMode.column1.Width);

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            SaveSettings();
            base.OnClosed(e);
        }

        public static ArticleRecord ReadArticlesFromDB(SQLiteDataReader r)
        {

            return (new ArticleRecord() {
                Id = Convert.ToInt32(r["id"].ToString()),
                Name = r["name"].ToString(),
                Number = r["number"].ToString(),
                NominalDiameter = Convert.ToDouble(r["nominald"]),
                MinDiameter = Convert.ToDouble(r["mind"]),
                MaxDiameter = Convert.ToDouble(r["maxd"]),
                NominalHeight = Convert.ToDouble(r["nominalh"]),
                MinHeight = Convert.ToDouble(r["minh"]),
                MaxHeight = Convert.ToDouble(r["maxh"]),
                NominalSeamerHeight = Convert.ToDouble(r["nominalsh"]),
                MinSeamerHeight = Convert.ToDouble(r["minsh"]),
                MaxSeamerHeight = Convert.ToDouble(r["maxsh"])
            });
        }


        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            await this.ShowInputAsync("", "asd");
        }

		private void MenuItem_Devices_Click(object sender, RoutedEventArgs e)
		{
			new DeviceManagerWindow().Show();
		}

		public RelayActionCommand AddNewArticleCommand
		{
			get
			{
				return _addNewArticleCommand;
			}

			set
			{
				_addNewArticleCommand = value;
			}
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			(new SimpleNewArticleWindow(_articles)).ShowDialog();

		}
	}
}
