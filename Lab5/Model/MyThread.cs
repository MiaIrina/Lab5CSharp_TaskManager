using System;
using System.Diagnostics;

namespace Lab5Butenko.Model
{
	internal class MyThread
	{
		#region fields

		private int _id;
		private ThreadState _state;
		private string _startDate;
		
		#endregion

		#region properties

		public int Id
		{
			get
			{
				return _id;
			}
			private set
			{
				_id = value;
			}
		}

		public ThreadState State
		{
			get
			{
				return _state;
			}
			private set
			{
				_state = value;
			}
		}

		public string StartDate
		{
			get { return _startDate; }
			private set
			{
				_startDate= value;
			}
		}
	

		internal MyThread(ProcessThread thr)
		{
			Id = thr.Id;
			State= thr.ThreadState;
			try
			{
				StartDate = thr.StartTime.ToLongDateString();
		    }
			catch(Exception)
			{
				StartDate = "No access";
			}
			
		}

		#endregion

	}
}
