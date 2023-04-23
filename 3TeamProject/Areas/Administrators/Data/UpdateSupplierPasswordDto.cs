using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Administrators.Data
{
	public class UpdateSupplierPasswordDto
	{
        [Required, MinLength(8), PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password"), PasswordPropertyText]
        public string ComfirmPassword { get; set; } = string.Empty;
    }
}

