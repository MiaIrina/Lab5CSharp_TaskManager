using Lab5Butenko.Tools.Managers;
using Lab5Butenko.Tools.Navigation;
using Lab5Butenko.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;


namespace Lab5
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IContentOwner
	{

		public ContentControl ContentControl => _contentControl;


		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainViewModel();
			InitializeApplication();
		}
		private void InitializeApplication()
		{
			NavigationManager.Instance.Initialize(new InitializationNavigationModel(this));
			NavigationManager.Instance.Navigate(ViewType.TaskManager);
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			StationManager.CloseApp();
		}
	}
}
