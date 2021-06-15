using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerWebApi.DataPattern;
using CustomerWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
	{
        private ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        /// <summary>
		/// An endpoint used to retrieve all Customer records from the datastore. 
		/// </summary>
		/// <returns>A collection of Customer records.</returns>
        [HttpGet]
        public ActionResult<Customer> Get()
        {
            return Ok(_customerRepository.Customers);
        }

        /// An endpoint used to retrieve a single Customer record based off of the records unique identifier(s). Accessed by issuing a GET 

        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest("Id value should not be null");
            }

            if (_customerRepository.Customers.ToList().Exists(c => c.Id == id) == false)
            {
                return NotFound("Customer not available with Id");
            }

            var customer = _customerRepository.Customers.Where(c => c.Id == id).FirstOrDefault();

            return Ok(customer);
        }

        //An endpoint used to create new Customer records.Accessed by issuing a POST request to the following

        [HttpPost]
        public ActionResult<Customer> Post([FromBody] Customer customer)
        {
            
            
            Stack<int> myStack = new Stack<int>();
            if (ModelState.IsValid == false)
            {
                return BadRequest("Invalid Payload");
            }
            else if (customer.Id != 0)
            {
                return NotFound("Id is not required ");
            }
            else
            {
                var cust = _customerRepository.AddCustomer(customer);

                return Ok(cust);
            }

          
        }

        //An endpoint used to update Customer record based off of the records unique identifier(s).Accessed by issuing a PUT request to the following

        [HttpPut]
        public ActionResult<Customer> Put([FromBody] Customer customer)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest("Invalid Payload");
            }
            else if (_customerRepository.Customers.ToList().Exists(c => c.Id == customer.Id) == false)
            {
                return NotFound("there is no customer available with id " + customer.Id);
            }
            else
            {
                var cust = _customerRepository.UpdateCustomer(customer);

                return Ok(cust);
            }
        }

        /// An endpoint used to delete customer records . Accessed by issuing a DELETE request.
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
             if (_customerRepository.Customers.ToList().Exists(c => c.Id == id) == false)
            {
                return NotFound("there is no customer available with id " + id);
            }
            else
            {
                _customerRepository.DeleteCustomer(id);
                return Ok();
            }
        }





    }
}
