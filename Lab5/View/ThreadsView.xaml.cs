using Lab5Butenko.Tools.Navigation;
using Lab5Butenko.ViewModel;

using System.Diagnostics;

using System.Windows.Controls;


namespace Lab5Butenko.View
{
	/// <summary>
	/// Логика взаимодействия для ThreadsView.xaml
	/// </summary>
	public partial class ThreadsView : UserControl,INavigatable
	{
		public ThreadsView(Process proc)
		{
			InitializeComponent();
			DataContext = new ThreadsViewModel(proc);
		}
	}
}
