using System;




namespace Lab5Butenko.Tools.Managers
{
	internal static class StationManager
	{
		public static event Action StopThreads;



		internal static void Initialize()
		{
		}

		internal static void CloseApp()
		{
		
			StopThreads?.Invoke();
			Environment.Exit(1);
		}
	}
}
