using Product.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using Product.MVVM.View;
using Product.Core;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;
using System;
using CommunityToolkit.Mvvm.Messaging;
using static Product.MVVM.ViewModel.NewProductViewModel;

namespace Product.MVVM.ViewModel
{
    internal class EditProductViewModel : ObservableObject
    {
        ApplicationContext db = new ApplicationContext();

        private ObservableCollection<ProductsDB> _dataList;
        public ObservableCollection<ProductsDB> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                OnPropertyChanged(nameof(DataList));
            }
        }

        private ProductsDB? _selectedItem;
        public ProductsDB? SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                }
            }
        }

        public ICommand DeleteCommand { get; }
        public static ICommand ClickAdd
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    NewProduct newProduct = new();

                    Window owner = Application.Current.MainWindow;
                    newProduct.Owner = owner;
                    newProduct.ShowDialog();
                });
            }
        }
        public ICommand EditBaseClick
        {
            get
            {
                return new DelegateCommand(o =>
                {
                    EditProductDBClick();
                });
            }
        }

        public EditProductViewModel()
        {
            _dataList = new ObservableCollection<ProductsDB>();
            db.Database.EnsureCreated();
            db.ProductsDBs.Load();
            LoadDataFromDatabase();

            DeleteCommand = new DelegateCommand(DeleteItemFromDB);

            WeakReferenceMessenger.Default.Register<ItemAddedMessage>(this, (r, m) =>
            {
                LoadDataFromDatabase();
            });
        }

        private void LoadDataFromDatabase()
        {
            DataList = new ObservableCollection<ProductsDB>(db.ProductsDBs.ToList());
            OnPropertyChanged("DataList");
        }

        private void EditProductDBClick()
        {
            foreach (var item in _dataList)
            {
                var existingItem = db.ProductsDBs.FirstOrDefault(o => o.Id == item.Id);

                if (existingItem != null)
                {
                    existingItem.Name = item.Name;
                    existingItem.UClass = item.UClass;
                    existingItem.Proteins = item.Proteins;
                    existingItem.Fats = item.Fats;
                    existingItem.Carbohy = item.Carbohy;
                    existingItem.Kilocalories = item.Kilocalories;
                }
            }
            db.SaveChanges();
        }

        private void DeleteItemFromDB(object parameter)
        {
            try
            {
                if (SelectedItem != null)
                {
                    db.ProductsDBs.Remove(SelectedItem);
                    db.SaveChanges();
                    
                    DataList.Remove(SelectedItem);
                }
            }
            catch(Exception)
            {
                MessageBox.Show(Application.Current.MainWindow,"Вы не выбрали элемент для удаления", "Error in database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}