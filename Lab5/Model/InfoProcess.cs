using System;

using System.ComponentModel;
using System.Diagnostics;

using System.Management;
using System.Runtime.CompilerServices;

using System.Windows;

namespace Lab5Butenko.Model
{
    internal class InfoProcess:INotifyPropertyChanged
    {

		#region
		private Process _process;
		private string _name;
		private int _id;
		private bool _isActive;
		private int _numberOfThreads;
		private string _userName;
		private int _numberOfModules;
		private DateTime _date;
		private string _startDate;
		private string _path;
		private double _cpu;
		private double _ram;
		

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
		#endregion
		public string Name
		{ get
			{
				return _name;
			}
				 private set
			{
				_name = value;
				OnPropertyChanged();
			}
		}
		public string UserName
		{
			get
			{
				return _userName;
			}
			private set
			{
				_userName = value;
				OnPropertyChanged();
			}
		}
		public string Path
		{
			get
			{
				return _path;
			}
			private set
			{
				_path = value;
				OnPropertyChanged();

			}
		}
		public int NumberOfModules
		{
			get
			{
				return _numberOfModules;
				
			}
			private set
			{
				_numberOfModules = value;
				OnPropertyChanged();

			}
		}
		public double CPU
		{
			get
			{
				return _cpu;
			}
			private set
			{
				_cpu = value;
				OnPropertyChanged();
			}
		}
		public double RAM
		{
			get
			{
				return _ram;
			}
			private set
			{
				_ram = value;
				OnPropertyChanged();
			}
		}
		public int NumberOfThreads
		{
			get
			{
				return _numberOfThreads;
			}
			private set
			{
				_numberOfThreads = value;
				OnPropertyChanged();
			}
		}
		public int Id
		{
			get
			{
				return _id;
			}
			private set
			{
				_id= value;
				OnPropertyChanged();
			}
		}
		public string StartDate
		{
			get
			{
				return _startDate;
			}
			private set
			{
				_startDate = value;
				OnPropertyChanged();
			}
		}
		public DateTime Date
		{
			get
			{
				return _date;
			}
			private set
			{
				_date = value;
				OnPropertyChanged();
			}
		}
		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			private set
			{
				_isActive = value;
				OnPropertyChanged();
			}
		}

		internal InfoProcess(Process proc)
		{
			_process = proc;
			GetCPUAndRam();
			Name = _process.ProcessName;
			IsActive = _process.Responding;
			Id = _process.Id;
			GetTime();
			UserName = GetTheUserName();
			GetNumberOfThreads();
			GetNumberOfModules();
			GetPath();
		    
		}
		public void UpdateProcess()
		{
			GetCPUAndRam();
			IsActive = _process.Responding;
		}

			private string GetTheUserName(){

			string query = "Select * From Win32_Process Where ProcessID = " + Id;
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
			ManagementObjectCollection processList = searcher.Get();
			foreach (ManagementObject obj in processList)
			{
				string[] args = new string[] { string.Empty, string.Empty };
				int returnVal = 0;
				try {
					returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", args));
				}
				catch(Exception )
				{
					returnVal = 0;
				}
				 
				if (returnVal == 0)
				{
					return args[1] + "\\" + args[0];
				}
			}

			return "No owner";
		}
		private  void GetCPUAndRam()
		{



			PerformanceCounter theCPUCounter = new PerformanceCounter("Process", "% Processor Time", _process.ProcessName);
				try
				{
					theCPUCounter.NextValue();
			
					CPU = theCPUCounter.NextValue();
				}
				catch (Exception)
				{
					CPU = 0;
				}
			
			var ram = new PerformanceCounter("Process", "Private Bytes", _process.ProcessName, true);
		
			try
			{
				RAM = Math.Round(ram.NextValue() / 1024 / 1024, 2);
			}
			catch (Exception)
			{
				RAM = 0;
			}
		}
		private  void GetTime()
		{
			
			try
			{
				Date = _process.StartTime;
				StartDate = Date.ToLongDateString();
			}
			catch (Exception )
			{
				Date = new DateTime(0001, 01, 01);
				StartDate = "Access denied";
			}
		}
		private void GetPath()
		{

			try
			{
				Path= _process.MainModule.FileName;
			}
			catch (Exception )
			{
				Path = "Access denied";
			}
		}
		private void GetNumberOfModules()
		{

			try
			{
				NumberOfModules = _process.Modules.Count;
			}
			catch (Exception )
			{
				NumberOfModules = 0;
			}
		}
		private void GetNumberOfThreads()
		{

			try
			{
				NumberOfThreads = _process.Threads.Count;
			}
			catch (Exception )
			{
				NumberOfThreads = 0;
			}
		}
		public static void TerminateProcess(Process process)
		{
			try
			{
				process.Kill();
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

	}
}
