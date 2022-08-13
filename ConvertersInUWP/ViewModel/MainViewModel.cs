using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomerLib;

namespace ConvertersInUWP.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ICustomerRepository _customerRepository;
        private IEnumerable<Customer> _allCustomers;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RemoveCommand))]
        private Customer _selectedCustomer;
        [ObservableProperty]
        private IEnumerable<Customer> _customers;
        [ObservableProperty]
        private string _searchText;

        public MainViewModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository ??
                                  throw new ArgumentNullException("customerRepository");
            GetCustomers();
        }

        private async void GetCustomers()
        {
            _allCustomers = await _customerRepository.GetCustomersAsync();
            Search();
        }


        [RelayCommand]
        private void Add()
        {
            var customer = new Customer();
            _customerRepository.Add(customer);
            SelectedCustomer = customer;
            OnPropertyChanged("Customers");
        }

        [RelayCommand(CanExecute = "HasSelectedCustomer")]
        private void Remove()
        {
            if (SelectedCustomer != null)
            {
                _customerRepository.Remove(SelectedCustomer);
                SelectedCustomer = null;
                OnPropertyChanged("Customers");
            }
        }

        private bool HasSelectedCustomer() => SelectedCustomer != null;

        [RelayCommand]
        private void Save()
        {
            _customerRepository.Commit();
        }

        [RelayCommand]
        public void Search()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
                Customers = _allCustomers.Where(c => ((Customer)c).Country.ToLower().Contains(SearchText.ToLower()));
            else
                Customers = _allCustomers;
            OnPropertyChanged("Customers");
        }
    }
}