using System;

using System.Diagnostics;


namespace Lab5Butenko.Model
{
	
	internal class Module
	{
		
	#region
	
	private string _name;
	private string _path;
	#endregion
		
		public string Name
		{
			get
			{
				return _name;
			}
			private set
			{
				_name =value;
			}

		}
		public string  Path
		{
			get
			{
				return _path;
			}
			private set
			{
				_path = value;
			}

		}

		internal Module(ProcessModule mod)
		{
			Name = mod.ModuleName;
			try
			{
				Path = mod.FileName;
			}
			catch (Exception)
			{
				Path = "No access";
			}
		}
	}
}
