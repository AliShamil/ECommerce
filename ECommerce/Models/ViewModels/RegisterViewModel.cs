﻿using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.ViewModels
{
    public class RegisterViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(5)]
        public string FullName { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
