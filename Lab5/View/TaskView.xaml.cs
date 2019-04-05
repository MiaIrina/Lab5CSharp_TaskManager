
using Lab5Butenko.Tools.Navigation;
using Lab5Butenko.ViewModel;
using System.Windows.Controls;


namespace Lab5Butenko.View
{
	/// <summary>
	/// Логика взаимодействия для TaskView.xaml
	/// </summary>
	public partial class TaskView : UserControl,INavigatable
	{
		public TaskView()
		{
			InitializeComponent();
			DataContext = new TaskViewModel();
		}
	}
}
