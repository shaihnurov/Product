using Product.Core;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using Product.MVVM.Model;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace Product.MVVM.ViewModel
{
    class NewProductViewModel : ObservableObject
    {
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

        private UClassType _selectedUClassType;
        public UClassType SelectedUClassType
        {
            get { return _selectedUClassType; }
            set
            {
                if (_selectedUClassType != value)
                {
                    _selectedUClassType = value;
                    OnPropertyChanged(nameof(SelectedUClassType));

                    NewItem.UClass = value;
                }
            }
        }

        private ProductsDB _newItem = new ProductsDB();
        public ProductsDB NewItem
        {
            get { return _newItem; }
            set
            {
                _newItem = value;
                OnPropertyChanged("NewItem");
            }
        }

        private ObservableCollection<UClassType> _uClassValues;
        public ObservableCollection<UClassType> UClassValues
        {
            get { return _uClassValues; }
            set
            {
                _uClassValues = value;
                OnPropertyChanged(nameof(UClassValues));
            }
        }
        public ICommand AddItemCommand { get; set; }

        private ICommand? _closeWindowCommand;
        public ICommand ClickBack
        {
            get
            {
                return _closeWindowCommand ??= new RelayCommand<object>(CloseWindow);
            }
        }

        private void CloseWindow(object? obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }

        public NewProductViewModel()
        {
            NewItem = new ProductsDB();
            AddItemCommand = new RelayCommand(AddNewItem);
            UClassValues = new ObservableCollection<UClassType>((IEnumerable<UClassType>)Enum.GetValues(typeof(UClassType)));
        }

        private void AddNewItem()
        {
            using (var db = new ApplicationContext())
            {
                db.ProductsDBs.Add(NewItem);
                db.SaveChanges();
            }
            NewItem = new ProductsDB();
            WeakReferenceMessenger.Default.Send(new ItemAddedMessage());
        }

        public class ItemAddedMessage
        {
            // От сюда передадим данные из бд
        }
    }
}
