using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomerWebApi.Models;
using CustomerWebApi.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CustomerWebApi.DataPattern
{
	public class CustomerJsonRepository : ICustomerRepository
	{
		private List<Customer> _customers = new List<Customer>();
		private object _lock = new object();
		private IWebHostEnvironment _hostEnvironment;

		public CustomerJsonRepository(IWebHostEnvironment hostEnvironment)
		{
			_hostEnvironment = hostEnvironment;
			_customers = LoadCustomers();
			

		}

		public IEnumerable<Customer> Customers => _customers;

		public Customer this[int id] => _customers.Exists(c => c.Id == id) ?
			_customers.Where(c => c.Id == id).FirstOrDefault() : null;


		public Customer AddCustomer(Customer customer)
		{

			if (customer.Id == 0)
			{
				if (_customers.Count > 0)
				{
					var newId = _customers.Max(customer => customer.Id) + 1;
					customer.Id = newId;
				}

				else
				{
					//assign with default CustomerId starts from 1000
					customer.Id = 1000;
				}

				_customers.Add(customer);
			}

			SaveOrUpdateCustomersIntoJsonStore();

			return customer;

		}

		public void DeleteCustomer(int id)
		{
			var customer = _customers.Where(c => c.Id == id).FirstOrDefault();
			_customers.Remove(customer);

			SaveOrUpdateCustomersIntoJsonStore();
		}

		public Customer UpdateCustomer(Customer customer)
		{
			var currentCustomer = _customers.Where(c => c.Id == customer.Id).FirstOrDefault();
			currentCustomer.FirstName = customer.FirstName;
			currentCustomer.LastName = customer.LastName;
			currentCustomer.Email = customer.Email;
			currentCustomer.PhoneNumber = customer.PhoneNumber;
			currentCustomer.Occupation = customer.Occupation;

			SaveOrUpdateCustomersIntoJsonStore();

			return currentCustomer;

		}

		private List<Customer> LoadCustomers()
		{
			var filePath = FileUtility.GetFilePath(_hostEnvironment);

			List<Customer> customers = new List<Customer>();

			var customersContent = FileUtility.GetContent(filePath).Result;
			if (customersContent == string.Empty)
				return customers;

			customers =  JsonConvert.DeserializeObject<List<Customer>>(customersContent);

			return customers;
		}

		

		private void SaveOrUpdateCustomersIntoJsonStore()
		{
			var filePath = FileUtility.GetFilePath(_hostEnvironment);

			var customersContent = JsonConvert.SerializeObject(_customers);

			FileUtility.SaveContent(filePath, customersContent);
			

		}

	}
}
