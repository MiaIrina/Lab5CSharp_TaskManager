using Lab5Butenko.Model;
using Lab5Butenko.Tools;
using Lab5Butenko.Tools.Managers;
using Lab5Butenko.Tools.Navigation;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using System.Threading;


namespace Lab5Butenko.ViewModel
{
	internal class ThreadsViewModel:BaseViewModel
	{
		#region
		private Thread _workingThreadFirst;
		private CancellationToken _token;
		private CancellationTokenSource _tokenSource;
		private ProcessThreadCollection _threads;
		private ObservableCollection<MyThread> _infoThreads;
		private Process _process;
		private int _idOfProcess;
		private RelayCommand<object> _backCommand;
		private string _totalInfo = "";

		#endregion
		
		public string TotalInfo
		{
			get
			{
				return _totalInfo;
			}
			private set
			{
				_totalInfo = value;
				OnPropertyChanged();

			}
		}
		public int IdOfProcess
		{
			get
			{
				return _idOfProcess;
			}
			private set
			{
				_idOfProcess = value;
				OnPropertyChanged();

			}
		}
		public ProcessThreadCollection Threads
		{
			get
			{
				return _threads;
			}
			set
			{
				_threads = value;
				OnPropertyChanged();
			}

		}
		public ObservableCollection<MyThread> InfoThreads
		{
			get
			{
				return _infoThreads;
			}
			set
			{
				_infoThreads = value;
				OnPropertyChanged();
			}
		}
		internal ThreadsViewModel(Process process)
		{
			_process = process;
			IdOfProcess = process.Id;
			
			TotalInfo = "Id  of process:" + IdOfProcess;
			_tokenSource = new CancellationTokenSource();
			_token = _tokenSource.Token;
			StartWorkingThread();

		}
		internal void StopWorkingThread()
		{
			_tokenSource.Cancel();
			_workingThreadFirst.Join(2000);
			_workingThreadFirst.Abort();
			_workingThreadFirst = null;
		
		}
		private void StartWorkingThread()
		{
			_workingThreadFirst = new Thread(WorkingThreadProcessFirst);
			
			_workingThreadFirst.Start();
		
		}

		private void WorkingThreadProcessFirst()
		{
			int i = 0;
			while (!_token.IsCancellationRequested)
			{
				if (InfoThreads == null)
					LoaderManager.Instance.ShowLoader();
				InfoThreads = new ObservableCollection<MyThread>(RefreshThreads());
				
				LoaderManager.Instance.HideLoader();


				for (int j = 0; j < 10; j++)
				{
					Thread.Sleep(200);
					if (_token.IsCancellationRequested)
						break;
				}
				if (_token.IsCancellationRequested)
					break;
				i++;

			}
		}
		public RelayCommand<object> BackCommand
		{
			get
			{
				return _backCommand ?? (_backCommand = new RelayCommand<object>(o=>
						   NavigationManager.Instance.Navigate(ViewType.TaskManager)));
			}
		}
		private List<MyThread> RefreshThreads()
		{
			Threads = _process.Threads;
			List<MyThread> newThreads = new List<MyThread>();
			foreach(ProcessThread thr in Threads)
			{
				newThreads.Add(new MyThread(thr));
			}
			return newThreads;
		}
	}
}
