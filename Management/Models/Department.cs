using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Management.Models
{
    public class Department
    {
        [Key]
        public int Id {  get; set; }

        [Required]
        [StringLength(100)]
        public string Name {  get; set; }
        
    }
}
