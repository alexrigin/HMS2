using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Diagnostics;
using MahApps.Metro.Controls;
using HMS.DataRecords;
using HMS.Tools;
using HMS.Managers;

namespace HMS.View
{
    /// <summary>
    /// Логика взаимодействия для AddSimpleArticleWindow.xaml
    /// </summary>
    public partial class SimpleNewArticleWindow : MetroWindow, INotifyPropertyChanged, IDataErrorInfo
    {
        private ObservableCollection<ArticleRecord> _articles;


		private int _errors = 0;
		private string _name;
		private string _number;
		private string _nominalDiameter;
		private string _minDiameter;
		private string _maxDiameter;
		private string _nominalHeight;
		private string _minHeight;
		private string _maxHeight;
		private string _nominalSeamerHeight;
		private string _minSeamerHeight;
		private string _maxSeamerHeight;
        
       

        public SimpleNewArticleWindow(ObservableCollection<ArticleRecord> articles)
        {
            InitializeComponent();
            this.DataContext = this;
            _articles = articles; 
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


		private void Confirm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = _errors == 0;
			e.Handled = true;
		}

		private void Confirm_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try {
				ArticleRecord newArticle = new ArticleRecord() {
					Id = DataManager.GetMaxArticleId() + 1,
					Name = ArticleName,
					Number = ArticleNumber,
					NominalHeight = NominalHeight.ToDouble(),
					NominalSeamerHeight = NominalSeamerHeight.ToDouble(),
					NominalDiameter = NominalDiameter.ToDouble(),
					MaxHeight = MaxAHeight.ToDouble(),
					MaxSeamerHeight = MaxSeamerHeight.ToDouble(),
					MaxDiameter = MaxDiameter.ToDouble(),
					MinHeight = MinAHeight.ToDouble(),
					MinSeamerHeight = MinSeamerHeight.ToDouble(),
					MinDiameter = MinDiameter.ToDouble()
				};
				//Debug.WriteLine(newArticle.ToSqlString());


				DataManager.InsertIntoArticles(newArticle); // добавляем запись в БД
				_articles.Add(newArticle); // загружаем новые данные в коллекцию
				this.Close();
			} catch (SQLiteException ex) {
				MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}

		private void Validation_Error(object sender, ValidationErrorEventArgs e)
		{
			if (e.Action == ValidationErrorEventAction.Added)
				_errors++;
			else
				_errors--;
		}

		public string this[string columnName]
        {
            get
            {

				if (columnName.Equals("ArticleName")) {
					if (string.IsNullOrEmpty(ArticleName))
						return "Данное поле не может быть пустым";
				}

				if (columnName.Equals("ArticleNumber")) {
					if (string.IsNullOrEmpty(ArticleNumber))
						return "Данное поле не может быть пустым";
					else if (IsNameInArticles()) {
						return "Данный номер уже зарегистрирован";
					}
				}

				if (columnName.Equals("NominalHeight"))
                {
					if (string.IsNullOrEmpty(NominalHeight))
						return "Данное поле не может быть пустым";
					else if (!NominalHeight.IsDouble()) 
						return "Вводимое значение должно быть числом";
				}

				if (columnName.Equals("MinAHeight")) {
					if (string.IsNullOrEmpty(MinAHeight))
						return "Данное поле не может быть пустым";
					else if (!MinAHeight.IsDouble())
						return "Вводимое значение должно быть числом";
					//else if (MinAHeight.ToDouble() >= NominalHeight.ToDouble())
					//	return "Минимальное значение должно быть меньше номинального";
				}

				if (columnName.Equals("MaxAHeight")) {
					if (string.IsNullOrEmpty(MaxAHeight))
						return "Данное поле не может быть пустым";
					else if (!MaxAHeight.IsDouble())
						return "Вводимое значение должно быть числом";
					//else if (MaxAHeight.ToDouble() <= NominalHeight.ToDouble())
					//	return "Максимальное значение должно быть больше номинального";
				}

				if (columnName.Equals("NominalSeamerHeight")) {
					if (string.IsNullOrEmpty(NominalSeamerHeight))
						return "Данное поле не может быть пустым";
					else if (!NominalSeamerHeight.IsDouble())
						return "Вводимое значение должно быть числом";
				}

				if (columnName.Equals("MinSeamerHeight")) {
					if (string.IsNullOrEmpty(MinSeamerHeight))
						return "Данное поле не может быть пустым";
					else if (!MinSeamerHeight.IsDouble())
						return "Вводимое значение должно быть числом";
					//else if (MinSeamerHeight.ToDouble() >= NominalSeamerHeight.ToDouble())
					//	return "Минимальное значение должно быть меньше номинального";
				}

				if (columnName.Equals("MaxSeamerHeight")) {
					if (string.IsNullOrEmpty(MaxSeamerHeight))
						return "Данное поле не может быть пустым";
					else if (!MaxSeamerHeight.IsDouble())
						return "Вводимое значение должно быть числом";
					//else if (MaxSeamerHeight.ToDouble() <= NominalSeamerHeight.ToDouble())
					//	return "Максимальное значение должно быть больше номинального";
				}

				if (columnName.Equals("NominalDiameter")) {
					if (string.IsNullOrEmpty(NominalDiameter))
						return "Данное поле не может быть пустым";
					else if (!NominalDiameter.IsDouble())
						return "Вводимое значение должно быть числом";
				}

				if (columnName.Equals("MaxDiameter")) {
					if (string.IsNullOrEmpty(MaxDiameter))
						return "Данное поле не может быть пустым";
					else if (!MaxDiameter.IsDouble())
						return "Вводимое значение должно быть числом";
					//else if (MinDiameter.ToDouble() >= NominalDiameter.ToDouble())
					//	return "Минимальное значение должно быть меньше номинального";
				}

				if (columnName.Equals("MinDiameter")) {
					if (string.IsNullOrEmpty(MinDiameter))
						return "Данное поле не может быть пустым";
					else if (!MinDiameter.IsDouble())
						return "Вводимое значение должно быть числом";
					//else if (MaxDiameter.ToDouble() >= NominalDiameter.ToDouble())
					//	return "Максимальное значение должно быть больше номинального";
				}

				return null;
            }
        }

        public string Error { get { return string.Empty; } }

		private bool IsNameInArticles()
		{
			foreach(ArticleRecord article in _articles) {
				if (article.Number.ToUpper().Equals(ArticleNumber.ToUpper()))
					return true;
			}
			return false;
		}

		public string NominalDiameter
		{
			get
			{
				return _nominalDiameter;
			}

			set
			{
				if (_nominalDiameter != value) {
					_nominalDiameter = value;
					OnPropertyChanged("NominalDiameter");
				}
			}
		}
		public string MinDiameter
		{
			get
			{
				return _minDiameter;
			}

			set
			{
				if (_minDiameter != value) {
					_minDiameter = value;
					OnPropertyChanged("MinDiameter");
				}
				
			}
		}
		public string MaxDiameter
		{
			get
			{
				return _maxDiameter;
			}

			set
			{
				if (_maxDiameter != value) {
					_maxDiameter = value;
					OnPropertyChanged("MaxDiameter");
				}
			}
		}

		public string NominalHeight
		{
			get { return _nominalHeight; }
			set
			{
				if (_nominalHeight != value) {
					_nominalHeight = value;
					OnPropertyChanged("NominalHeight");
				}
			}
		}
		public string MinAHeight
		{
			get
			{
				return _minHeight;
			}

			set
			{
				if (_minHeight != value) {
					_minHeight = value;
					OnPropertyChanged("MinAHeight");
				}
			}
		}
		public string MaxAHeight
		{
			get
			{
				return _maxHeight;
			}

			set
			{
				if (_maxHeight != value) {
					_maxHeight = value;
					OnPropertyChanged("MaxAHeight");
				}
			}
		}
	
		public string NominalSeamerHeight
		{
			get
			{
				return _nominalSeamerHeight;
			}

			set
			{
				_nominalSeamerHeight = value;
			}
		}
		public string MinSeamerHeight
		{
			get
			{
				return _minSeamerHeight;
			}

			set
			{
				_minSeamerHeight = value;
			}
		}
		public string MaxSeamerHeight
		{
			get
			{
				return _maxSeamerHeight;
			}

			set
			{
				_maxSeamerHeight = value;
			}
		}

		public string ArticleName
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}

		public string ArticleNumber
		{
			get
			{
				return _number;
			}

			set
			{
				_number = value;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

		private void Number_KeyDown(object sender, KeyEventArgs e)
		{
			if (Number.Text.Length == 0 && e.Key == Key.Space) {
				e.Handled = true;
			} else {
				e.Handled = false;
			}
		}
	}
}
