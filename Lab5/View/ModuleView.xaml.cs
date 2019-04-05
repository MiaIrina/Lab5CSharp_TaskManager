using Lab5Butenko.Tools.Navigation;
using Lab5Butenko.ViewModel;

using System.Diagnostics;

using System.Windows.Controls;

namespace Lab5Butenko.View
{
	/// <summary>
	/// Логика взаимодействия для ModuleView.xaml
	/// </summary>
	public partial class ModuleView : UserControl,INavigatable
	{
		public ModuleView(Process proc)
		{
			InitializeComponent();
			DataContext = new ModuleViewModel(proc);
		}
	}
}
