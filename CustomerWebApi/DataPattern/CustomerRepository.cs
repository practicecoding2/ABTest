using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerWebApi.Models;

namespace CustomerWebApi.DataPattern
{
	public class CustomerRepository : ICustomerRepository
	{
		private List<Customer> _customers = new List<Customer>();

		public CustomerRepository()
		{
			_customers = LoadCustomers();
		}

		public IEnumerable<Customer> Customers =>  _customers;

		public Customer this[int id] => _customers.Exists(c => c.Id == id) ? 
			_customers.Where( c=> c.Id == id).FirstOrDefault() : null;


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

			return customer;
			
		}

		public void DeleteCustomer(int id)
		{
			var customer = _customers.Where(c => c.Id == id).FirstOrDefault();
			_customers.Remove(customer);
		}

		public Customer UpdateCustomer(Customer customer)
		{
			var currentCustomer = _customers.Where(c => c.Id == customer.Id).FirstOrDefault();
			currentCustomer.FirstName = customer.FirstName;
			currentCustomer.LastName = customer.LastName;
			currentCustomer.Email = customer.Email;
			currentCustomer.PhoneNumber = customer.PhoneNumber;
			currentCustomer.Occupation = customer.Occupation;

			return currentCustomer;
				
		}

		private List<Customer> LoadCustomers()
		{

			var cusomterList = new List<Customer>()
			{
				new Customer() { Id =1001 ,FirstName ="Marioe" , LastName ="Johny" , Email = "Marioe.Johny@test.com",
				 Occupation="Business" , PhoneNumber ="1234567890" },

				new Customer() { Id =1002 ,FirstName ="Rich" , LastName ="Mike" , Email = "Rich.Mike@test.com",
				 Occupation="Software Professional" , PhoneNumber ="1234567490" },

				new Customer() { Id =1003 ,FirstName ="Simon" , LastName ="Jerry" , Email = "Simon.Jerry@test.com",
				 Occupation="Software Professional" , PhoneNumber ="1234367490" },

				new Customer() { Id =1004 ,FirstName ="Martin" , LastName ="Tom" , Email = "Martin.Tom@test.com",
				 Occupation="Private Employee" , PhoneNumber ="1214367490" },

				new Customer() { Id =1005 ,FirstName ="Per" , LastName ="Andreson" , Email = "Per.Andreson@test.com",
				 Occupation="Business" , PhoneNumber ="1214367491" },

			};

			cusomterList.ForEach(c => AddCustomer(c));

			return cusomterList;
		}
	}
}
