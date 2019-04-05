using Lab5Butenko.Model;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab5Butenko.Tools.Navigation
{
	internal abstract class BaseNavigationModel : INavigationModel
	{
		private readonly IContentOwner _contentOwner;
		private readonly Dictionary<ViewType, INavigatable> _viewsDictionary;

		protected BaseNavigationModel(IContentOwner contentOwner)
		{
			_contentOwner = contentOwner;
			_viewsDictionary = new Dictionary<ViewType, INavigatable>();
		}

		protected IContentOwner ContentOwner
		{
			get { return _contentOwner; }
		}

		protected Dictionary<ViewType, INavigatable> ViewsDictionary
		{
			get { return _viewsDictionary; }
		}

		public void Navigate(ViewType viewType)
		{

			if (!ViewsDictionary.ContainsKey(viewType))
			{

				InitializeView(viewType);
			}
			
			ContentOwner.ContentControl.Content = ViewsDictionary[viewType];
		}

		public void Navigate(ViewType viewType, Process proc)
		{
			if (ViewsDictionary.ContainsKey(viewType))
			{

				ViewsDictionary.Remove(viewType);
			}
			InitializeView(viewType,proc);
			ContentOwner.ContentControl.Content = ViewsDictionary[viewType];
		}

		protected abstract void InitializeView(ViewType viewType);
		protected abstract void InitializeView(ViewType viewType,Process proc);
	}
}
