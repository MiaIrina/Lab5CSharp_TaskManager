

using Lab5Butenko.Tools.Navigation;
using Lab5Butenko.View;
using System;
using System.Diagnostics;

namespace Lab5Butenko.Tools.Navigation
{
	internal class InitializationNavigationModel : BaseNavigationModel
	{
		public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
		{

		}

		protected override void InitializeView(ViewType viewType)
		{
			switch (viewType)
			{

				case ViewType.TaskManager:
					ViewsDictionary.Add(viewType, new TaskView());
					break;
			
				default:
					throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
			}
		}

		protected override void InitializeView(ViewType viewType, Process proc)
		{
			switch (viewType)
			{




				case ViewType.Threads:
					ViewsDictionary.Add(viewType, new ThreadsView(proc));
					break;
				case ViewType.Modules:
					ViewsDictionary.Add(viewType, new ModuleView(proc));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
			}
		}
	}
	
}

