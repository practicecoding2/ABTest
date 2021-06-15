using System;
using Xunit;
using CustomerWebApi.Controllers;
using CustomerWebApi.DataPattern;
using Microsoft.AspNetCore.Mvc;
using CustomerWebApi.Models;
using System.Collections.Generic;

namespace CustomerWebApi.Tests
{
	public class CustomerControllerTest
	{

		CustomerController _controller;
		ICustomerRepository _respository;
		public CustomerControllerTest()
		{
			_respository = new CustomerRepository();
			_controller = new CustomerController(_respository);
		}


        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.Get().Result;
            // Assert
            Assert.IsType<OkObjectResult>(okResult);
        }
        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.Get().Result as OkObjectResult;
            // Assert
            var items = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(5, items.Count);
        }

        [Fact]
        public void GetById_UnknownCustomerId_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _controller.Get(101);
            // Assert
            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }
        [Fact]
        public void GetById_ExistingCustomerId_ReturnsOkResult()
        {
          
            // Act
            var okResult = _controller.Get(1001);
            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }
        [Fact]
        public void GetById_ExistingCustomerIdPassed_ReturnsRightItem()
        {
            var customerId = 1001;
            // Act
            var okResult = _controller.Get(1001).Result as OkObjectResult;
            // Assert
            Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(customerId, (okResult.Value as Customer).Id);
        }

        [Fact]
        public void Add_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var customerNameMissing = new Customer()
            {
                LastName = "Last",
                Email = "abc@test.com",
                PhoneNumber = "1223363739", 
                Occupation = "Occupation"

            };
            _controller.ModelState.AddModelError("FirstName", "Required");
            // Act
            var badResponse = _controller.Post(customerNameMissing);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }
        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var customer = new Customer()
            {
                FirstName ="first",
                LastName = "Last",
                Email = "abc@test.com",
                PhoneNumber = "1223363739",
                Occupation = "Occupation"

            };
            // Act
            var createdResponse = _controller.Post(customer);
            // Assert
            Assert.IsType<OkObjectResult>(createdResponse.Result);
        }
        [Fact]
        public void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var customer = new Customer()
            {
                FirstName = "TestFirst",
                LastName = "Last",
                Email = "abc@test.com",
                PhoneNumber = "1223363739",
                Occupation = "Occupation"

            };
            // Act
            var createdResponse = _controller.Post(customer).Result as OkObjectResult; ;
            //// Assert
            Assert.IsType<Customer>(createdResponse.Value);
            Assert.Equal("TestFirst",  (createdResponse.Value as Customer).FirstName);
        }


        [Fact]
        public void Update_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            var customerNameMissing = new Customer()
            {
                LastName = "Last",
                Email = "abc@test.com",
                PhoneNumber = "1223363739",
                Occupation = "Occupation"

            };
            _controller.ModelState.AddModelError("FirstName", "Required");
            // Act
            var badResponse = _controller.Put(customerNameMissing);
            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse.Result);
        }
        [Fact]
        public void Update_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var customer = new Customer()
            {
                Id = 1001,
                FirstName = "first",
                LastName = "Last",
                Email = "abc@test.com",
                PhoneNumber = "1223363739",
                Occupation = "Occupation"

            };
            // Act
            var createdResponse = _controller.Put(customer);
            // Assert
            Assert.IsType<OkObjectResult>(createdResponse.Result);
        }
        [Fact]
        public void Update_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            // Arrange
            var customer = new Customer()
            {
                Id = 1001,
                FirstName = "TestFirst",
                LastName = "Last",
                Email = "abc@test.com",
                PhoneNumber = "1223363739",
                Occupation = "Occupation"

            };
            // Act
            var createdResponse = _controller.Post(customer).Result as OkObjectResult; ;
            //// Assert
            Assert.IsType<Customer>(createdResponse.Value);
            Assert.Equal("TestFirst", (createdResponse.Value as Customer).FirstName);
        }


        [Fact]
        public void Remove_NotExistingCustomerIdPassed_ReturnsNotFoundResponse()
        {
            // Arrange
            var notExistingCustomerId = 1;
            // Act
            var badResponse = _controller.Delete(notExistingCustomerId);
            // Assert
            Assert.IsType<NotFoundObjectResult>(badResponse);
        }
        [Fact]
        public void Remove_ExistingCustomerIdPassed_ReturnsOkResult()
        {
            // Arrange
            var existingCustomerId = 1001;
            // Act
            var okResponse = _controller.Delete(existingCustomerId);
            // Assert
            Assert.IsType<OkResult>(okResponse);
        }
       

    }
}
