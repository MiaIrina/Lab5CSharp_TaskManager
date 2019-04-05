using Lab5Butenko.Model;
using Lab5Butenko.Tools;
using Lab5Butenko.Tools.Managers;
using Lab5Butenko.Tools.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Diagnostics;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lab5Butenko.ViewModel
{
	internal class TaskViewModel : BaseViewModel
	{
		private RelayCommand<object> _sortCommand;
		private RelayCommand<object> _showThreadsCommand;
		private RelayCommand<object> _showModulesCommand;
		private RelayCommand<object> _stopCommand;
		private RelayCommand<object> _openFolderCommand;
		private int _sortBy;
		private bool _ascending=true;
		private string _updateMetaString;
		private string _updateAllString;
		private bool _descending;
		private Thread _workingThreadFirst;
		private Thread _workingThreadSecond;
		private CancellationToken _token;
		private CancellationTokenSource _tokenSource;
		private ObservableCollection<InfoProcess> _processes;
		
		private Process[] _pro;
		private InfoProcess _selected;
		private List<Process> _proInfo;
		
		private List<Process> ProInfo
		{
			get
			{
				return _proInfo;
			}
			set
			{
				_proInfo = value;
				OnPropertyChanged();
			}
		}
		
		public bool Ascending {
			get
			{
				return _ascending;
			}
			set
			{
				_ascending = value;
				OnPropertyChanged();
			}

		}
		public string UpdateMetaString
		{
			get
			{
				return _updateMetaString;
			}
			private set
			{
				_updateMetaString = value;
				OnPropertyChanged();
			}
		}
		public string UpdateAllString
		{
			get
			{
				return _updateAllString;
			}
			private set
			{
				_updateAllString = value;
				OnPropertyChanged();
			}
		}
		public bool Descending
		{
			get
			{
				return _descending;
			}
			set
			{
				_descending = value;
				OnPropertyChanged();
			}

		}

		public InfoProcess Selected {

			get
			{
				return _selected;
			}
			set
			{
				_selected = value;
				OnPropertyChanged();
			}
		}
		public int SortBy
		{
			get
			{
				return _sortBy;
			}
			set
			{
				_sortBy = value;
				OnPropertyChanged();
			}
		}


		public ObservableCollection<InfoProcess> Processes
		{
			get
			{
				return _processes;
			}
			set
			{
				_processes = value;
				OnPropertyChanged();
			}
		}



		public RelayCommand<object> ShowModulesCommand
		{
			get
			{
				return _showModulesCommand ?? (_showModulesCommand = new RelayCommand<object>(
						   ShowModules, CanExecuteActionOfProcess));
			}
		}
		public RelayCommand<object> ShowThreadsCommand
		{
			get
			{
				return _showThreadsCommand ?? (_showThreadsCommand = new RelayCommand<object>(
						   ShowThreads, CanExecuteActionOfProcess));
			}
		}
		private void ShowThreads(object obj)
		{
			NavigationManager.Instance.Navigate(ViewType.Threads,Process.GetProcessById(Selected.Id));
		}
		private void ShowModules(object obj)
			
		{
			
				if (Selected.NumberOfModules != 0)
				{
					NavigationManager.Instance.Navigate(ViewType.Modules, Process.GetProcessById(Selected.Id));
				}
				else
				{
					MessageBox.Show("No access to see modules");
				}
			
		}
		public RelayCommand<object> StopProcessCommand
		{
			get
			{
				return _stopCommand ?? (_stopCommand = new RelayCommand<object>(
						   StopCommand, CanExecuteActionOfProcess));
			}
		}

		private  void StopCommand(object obj)
		{
			InfoProcess.TerminateProcess(Process.GetProcessById(Selected.Id));
			Processes = new ObservableCollection<InfoProcess>(RefreshProcesses());
			Sort();
			Selected = null;
			
		
		}

		public RelayCommand<object> OpenFolderCommand
		{
			get
			{
				return _openFolderCommand ?? (_openFolderCommand = new RelayCommand<object>(
						   OpenDirectory, CanExecuteActionOfProcess));
			}
		}
		public RelayCommand<object> SortCommand
		{
			get
			{
				return _sortCommand ?? (_sortCommand = new RelayCommand<object>(o=>Sort()));
			}
		}

		private async void OpenDirectory(object obj)
		{
			await Task.Run(() =>
			{

try
			{
				Process process = Process.GetProcessById(Selected.Id);
				string path = "/select, \"" + process.MainModule.FileName + "\"";
				Process.Start("explorer.exe", path);
					Thread.Sleep(100);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			});
			
		}

		private List<InfoProcess> RefreshProcesses()
		{
			Dictionary<int,InfoProcess> result;

			if (Processes == null)
			{
            result=new Dictionary<int,InfoProcess>();
			}
			else
			{
				result = GetDictionaryProcess(Processes.ToList());
			}
			
				_pro = Process.GetProcesses();
			Dictionary<int, InfoProcess> temp =result;
			Dictionary<int, Process> tempProc = GetDictionaryProcess(_pro.ToList());
			for (int i = 0; i < _pro.Length; i++)
			{
				if (!temp.Keys.ToList().Contains(_pro[i].Id))
				{
					result.Add(_pro[i].Id,new InfoProcess(_pro[i]));
				}
				


			}
			temp = result;
			for (int i = 0; i < tempProc.Count; i++)
			{
				if (!tempProc.Keys.ToList().Contains(temp.Keys.ToList()[i]))
				{
					result.Remove(temp.Keys.ToList()[i]);
				}

			}
			return result.Values.ToList();
				
	

			
			
		}
		internal TaskViewModel()
		{
		
			_tokenSource = new CancellationTokenSource();
			_token = _tokenSource.Token;
			StartWorkingThread();

			StationManager.StopThreads += StopWorkingThread;

		}


	
		private void StartWorkingThread()
		{
			_workingThreadFirst = new Thread(WorkingThreadProcessFirst);
			_workingThreadSecond = new Thread(WorkingThreadProcessSecond);
			_workingThreadFirst.Start();
			_workingThreadSecond.Start();
		}

		private  void WorkingThreadProcessFirst()
		{
			int i = 0;
			while (!_token.IsCancellationRequested)
			{
				if (Processes == null)
				{
					LoaderManager.Instance.ShowLoader();
					
				}
				else
				{
					UpdateAllString = "Refresh all list....";
				}
				
				
				Processes = new ObservableCollection<InfoProcess>(RefreshProcesses());
				Sort();
				LoaderManager.Instance.HideLoader();
				UpdateAllString = "";

				for (int j = 0; j < 10; j++)
				{
					Thread.Sleep(500);
					if (_token.IsCancellationRequested)
						break;
				}
				if (_token.IsCancellationRequested)
					break;
				i++;
				
			}
		}
		private void WorkingThreadProcessSecond()
		{
			int i = 0;
			while (!_token.IsCancellationRequested)
			{
				if (Processes != null)
				{
					UpdateMetaString = "Refresh metadate....";
					UpdateMetaDate();
					Sort();
					UpdateMetaString = "";
				}
				
				for (int j = 0; j < 4; j++)
				{
					Thread.Sleep(500);
					if (_token.IsCancellationRequested)
						break;
				}
				if (_token.IsCancellationRequested)
					break;
				i++;

			}
		}
		internal void StopWorkingThread()
		{
			_tokenSource.Cancel();
			_workingThreadFirst.Join(2000);
			_workingThreadFirst.Abort();
			_workingThreadFirst = null;
			_workingThreadSecond.Join(2000);
			_workingThreadSecond.Abort();
			_workingThreadSecond = null;
		
		}

		private void UpdateMetaDate()
		{
			

	
		    foreach(InfoProcess i in Processes)
			{
				i.UpdateProcess();

			}
			
		}
		private Dictionary<int,InfoProcess> GetDictionaryProcess(List<InfoProcess> process)
		{
			Dictionary<int, InfoProcess> dictionary = new Dictionary<int,InfoProcess>();
			foreach (InfoProcess p in process)
			{
				dictionary.Add(p.Id, p);
			}
			return dictionary;
		}
		private Dictionary<int, Process> GetDictionaryProcess(List<Process> process)
		{
			Dictionary<int, Process> dictionary = new Dictionary<int, Process>();
			foreach (Process p in process)
			{
				dictionary.Add(p.Id, p);
			}
			return dictionary;
		}

		private bool CanExecuteActionOfProcess(object obj)
		{
			return Selected != null;
		}
		private void Sort()
		{
			switch(SortBy)
			{
				
				case 0:
					SortByName();
					break;
				case 1:
					SortByUserName();
					break;
				case 2:
					SortByCPU();
					break;
				case 3:
					SortByRAM();
					break;
				case 4:
					SortById();
					break;
				case 5:
					SortByNumberOfThreads();
					break;
				case 6:
					SortByDate();
					break;
				case 7:
					SortByPath();
					break;



			}
		}
		private void SortById()
		{
			if (Ascending)
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderBy(process => process.Id));
			}
			else
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderByDescending(process => process.Id));

			}
		}
		private void SortByName()
		{
			if (Ascending)
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderBy(process => process.Name));
			}
			else
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderByDescending(process => process.Name));

			}

		}
		private void SortByUserName()
		{
			if (Ascending)
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderBy(process => process.UserName));
			}
			else
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderByDescending(process => process.UserName));

			}
		}
		private void SortByNumberOfThreads()
		{
			if (Ascending)
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderBy(process => process.NumberOfThreads));
			}
			else
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderByDescending(process => process.NumberOfThreads));

			}
		}
		private void SortByPath()
		{

			if (Ascending)
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderBy(process => process.Path));
			}
			else
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderByDescending(process => process.Path));

			}
		}
		private void SortByCPU()
		{
			if (Ascending)
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderBy(process => process.CPU));
			}
			else
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderByDescending(process => process.CPU));

			}
		}
		private void SortByRAM()
		{
			if (Ascending)
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderBy(process => process.RAM));
			}
			else
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderByDescending(process => process.RAM));

			}
		}
		private void SortByDate()
		{
			if (Ascending)
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderBy(process => process.Date));
			}
			else
			{
				Processes = new ObservableCollection<InfoProcess>(Processes.OrderByDescending(process => process.Date));

			}
		}


	}
}
