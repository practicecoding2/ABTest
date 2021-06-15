using System.ComponentModel.DataAnnotations;

namespace CustomerWebApi.Models
{
	public class Customer
	{
		public int Id { get; set; }

		[Required]
		public string FirstName { get; set; }

		public string LastName { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		public string Occupation { get; set; }

		
	}
}
