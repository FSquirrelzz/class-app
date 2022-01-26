using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(18,MinimumLength = 6,ErrorMessage ="Password must be between 6 to 18 characters")]
        public string Password { get; set; }
    }
}