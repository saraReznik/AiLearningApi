using Dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;       
using System.Threading.Tasks;

namespace BL.Models
{
    public class BLPrompt
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // מפתח רץ
        public int Id { get; set; }

        [Required] 
        public int UserId { get; set; }

        [Required] 
        public int CategoryId { get; set; }

        [Required] 
        public int SubCategoryId { get; set; }

        [Required(ErrorMessage = "Prompt text is required.")] 
        [StringLength(500, ErrorMessage = "Prompt cannot exceed 500 characters.")] 
        public string Prompt1 { get; set; } = null!;

       
        public string? Response { get; set; } 

        public DateTime? CreatedAt { get; set; }

    }

}

