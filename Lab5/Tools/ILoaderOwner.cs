
using System.ComponentModel;
using System.Windows;

namespace Lab5Butenko.Tools
{
	internal interface ILoaderOwner : INotifyPropertyChanged
	{
		Visibility LoaderVisibility { get; set; }
		bool IsControlEnabled { get; set; }
	}
}
