using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using HMS.DataRecords;
using HMS.Managers;

namespace HMS.View
{

	/// <summary>
	/// Логика взаимодействия для MakeMeasure.xaml
	/// </summary>
	public partial class MakeMeasureMode : UserControl
	{
		private GridLength _articlesPanelWidth;
		private bool _isArticlesPanelOpen;

		public MakeMeasureMode(ObservableCollection<ArticleRecord> articles)
		{
			InitializeComponent();
			this.DataContext = this; 
			articles_lv.ItemsSource = articles; // init articles data grid

			CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(articles_lv.ItemsSource);
			view.Filter = ArticlesSearch;

			Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted; // подписываемся на событие, чтобы сохранить пользовательские настройки
		}


		private void ArticlesPanelBtn_Click(object sender, RoutedEventArgs e)
		{
			if (ArticlesPanelBtn.IsLoaded == true) {
				if (ArticlesPanelBtn.IsChecked == true) {
					ArticlesPanel.Visibility = Visibility.Visible;
					gs.Visibility = Visibility.Visible;
					column1.Width = _articlesPanelWidth;
					_isArticlesPanelOpen = true;
				} else {
					_articlesPanelWidth = column1.Width;
					column1.Width = GridLength.Auto;
					ArticlesPanel.Visibility = Visibility.Collapsed;
					gs.Visibility = Visibility.Collapsed;

					_isArticlesPanelOpen = false;
					//column3.Width = new GridLength(100, GridUnitType.Star);
				}
			}

		}

		private void LoadSettings()
		{
			_isArticlesPanelOpen = Properties.Settings.Default.IsArticlesPanelOpen;
			_articlesPanelWidth = Properties.Settings.Default.ArticlesPanelWidth;
			//if(_articlesPanelWidth.Equals(GridLength.Auto))
			_articlesPanelWidth = new GridLength(0.2, GridUnitType.Star);

			if (_isArticlesPanelOpen) {
				column1.Width = _articlesPanelWidth;
				ArticlesPanel.Visibility = Visibility.Visible;
				gs.Visibility = Visibility.Visible;
				ArticlesPanelBtn.IsChecked = true;
			} else {
				ArticlesPanel.Visibility = Visibility.Collapsed;
				gs.Visibility = Visibility.Collapsed;
				column1.Width = GridLength.Auto;
			}
			Debug.WriteLine("Settings loaded");
			Debug.WriteLine("Width={0}, {1}", column1.Width, _articlesPanelWidth);
		}

		private void SaveSettings()
		{
			Properties.Settings.Default.ArticlesPanelWidth = _articlesPanelWidth;
			Properties.Settings.Default.IsArticlesPanelOpen = _isArticlesPanelOpen;
			Properties.Settings.Default.Save();
			Debug.WriteLine("Settings have been saved");
		}


		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			LoadSettings();
		}

		private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
		{
			SaveSettings();
		}




		private async void articles_dg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			MeasuringControl currentMeasurementControl = (MeasurementsGroupBox.Content as MeasuringControl);
			if (currentMeasurementControl == null || !currentMeasurementControl.IsStarted || currentMeasurementControl.IsFinished) // если измерение не было выбрано\не было начато\ завершилось
			{
				MeasurementsGroupBox.Content = new MeasuringControl((articles_lv.SelectedValue as ArticleRecord));
				return;
			}
			
			if (!currentMeasurementControl.IsFinished) {
				var mySettings = new MetroDialogSettings() {
					AffirmativeButtonText = "Продолжить",
					NegativeButtonText = "Отмена",
					ColorScheme = MetroDialogColorScheme.Theme
				};
			
				MessageDialogResult result = await (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("Предупреждение!", 
					string.Format("Измерение еще не было заверешено!\nВы уверены, что хотите продолжить?"), 
					MessageDialogStyle.AffirmativeAndNegative,
					mySettings);
			
				if (result != MessageDialogResult.Negative) {
					MeasurementsGroupBox.Content = new MeasuringControl(articles_lv.SelectedValue as ArticleRecord);
					return;
				}
			}
		}

		private void gs_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
		{
			_articlesPanelWidth = new GridLength(column1.Width.Value, column1.Width.GridUnitType);
			Debug.WriteLine("Width={0}, {1}", column1.Width, _articlesPanelWidth);

		}

		private ICommand textBoxButtonCmd;

		public ICommand TextBoxButtonCmd
		{
			get
			{
				return this.textBoxButtonCmd ?? (this.textBoxButtonCmd = new SimpleCommand {
					CanExecuteDelegate = x => true,
					ExecuteDelegate = x => {
						if (x is TextBox) {

						}
					}
				});
			}
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(articles_lv.ItemsSource).Refresh();
		}

		private bool ArticlesSearch(object item)
		{
			if (string.IsNullOrEmpty(SearchBox.Text))
				return true;
			else
				return ((item as ArticleRecord).Name.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0  ||
					(item as ArticleRecord).Number.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
		}


	}
}

// TO DO
// Разобраться с GridLenght при дефолтных настройках.
// 