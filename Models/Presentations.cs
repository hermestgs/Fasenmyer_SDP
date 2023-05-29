using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace FasenmyerConference.Models
{
    public class Presentations
    {
 
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? PName { get; set; }
        public string? Time { get; set; }
        public string? Sponsor { get; set; }
        public string? Student1 { get; set; }
        public string? Student2 { get; set; }
        public string? Student3 { get; set; }
        public string? Student4 { get; set; }
        public string? Major { get; set; }
        public string? Room { get; set; }
        public string? Advisor { get; set; }
    }
}
