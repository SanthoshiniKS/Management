using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Management.Models
{
    public class Employee
    {
        [Key]
        public int Id {  get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId {  get; set; }

        [Required]
        [ForeignKey("City")]
        public int CityId {  get; set; }

        [Required]
        [Range(10000,5000000,ErrorMessage="CTC must be between 10,000 and 50,00,000")]
        public decimal CTC {  get; set; }

        [Required]
        [StringLength(100)]
        public string Position {  get; set; }

        public int? ManagerID { get; set; }

        [ValidateNever]
        public  Department Department { get; set; }

        [ValidateNever]
        public City City { get; set; }
    }
}
