using System;
using Dal.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class BLUser
    {
        [Required]
        [Key] 
       
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")] 
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [Phone(ErrorMessage = "Invalid phone number format.")] 
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email is required.")] 
        [EmailAddress(ErrorMessage = "Invalid email address format.")] 
        public string Email { get; set; } = null!;

        public string? Role { get; set; }

      
    }
}
