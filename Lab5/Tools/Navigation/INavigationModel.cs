


using System.Diagnostics;

namespace Lab5Butenko.Tools.Navigation
{
	internal enum ViewType
	{
		TaskManager,
		Threads,
		Modules
		
	}

	interface INavigationModel
	{
		void Navigate(ViewType viewType);
		void Navigate(ViewType viewType,Process proc);
	}
}
