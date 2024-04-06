﻿using System.ComponentModel.DataAnnotations;

namespace RESTApi.Models
{
    public class Customer
    {
		[Required(ErrorMessage ="Id is required")]
		public int Id { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Age is required")]
        [Range(18, 90, ErrorMessage ="Age should be 18 to 90" )]
        public int Age {  get; set; }
    }
}
