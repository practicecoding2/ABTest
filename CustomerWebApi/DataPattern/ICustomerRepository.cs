using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerWebApi.Models;

namespace CustomerWebApi.DataPattern
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> Customers { get; }
        Customer AddCustomer(Customer customer);
        Customer UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
    }
}
