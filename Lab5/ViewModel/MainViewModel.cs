using Lab5Butenko.Tools;
using Lab5Butenko.Tools.Managers;
using Lab5Butenko.Tools.Navigation;
using System.Windows;

namespace Lab5Butenko.ViewModels
{
	class MainViewModel : BaseViewModel, ILoaderOwner, INavigatable
	{
		#region Fields
		private Visibility _loaderVisibility = Visibility.Hidden;
		private bool _isControlEnabled = true;
		#endregion

		#region Properties
		public Visibility LoaderVisibility
		{
			get { return _loaderVisibility; }
			set
			{
				_loaderVisibility = value;
				OnPropertyChanged();
			}
		}
		public bool IsControlEnabled
		{
			get { return _isControlEnabled; }
			set
			{
				_isControlEnabled = value;
				OnPropertyChanged();
			}
		}
		#endregion

		internal MainViewModel()
		{
			LoaderManager.Instance.Initialize(this);

		}
	}

}

