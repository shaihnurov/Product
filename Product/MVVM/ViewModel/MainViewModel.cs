using CommunityToolkit.Mvvm.Input;
using Product.Core;
using Product.MVVM.Model;
using Product.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Product.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public DelegateCommand HomeViewCommand { get; set; }
        public DelegateCommand EditProductViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public EditProductViewModel EditProductVM { get; set; }

        private object? _cuttentView;

        public object? CurrentView
        {
            get { return _cuttentView; }
            set 
            { 
                _cuttentView = value;   
                OnPropertyChanged();
            }
        }
        public ICommand ExitApplicationCommand { get; private set; }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }
        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            EditProductVM = new EditProductViewModel();

            CurrentView = HomeVM;

            HomeViewCommand = new DelegateCommand(o =>
            {
                CurrentView = HomeVM;
            });

            EditProductViewCommand = new DelegateCommand(o =>
            {
                CurrentView = EditProductVM;
            });

            ExitApplicationCommand = new RelayCommand(ExitApplication);
        }
    }
}
