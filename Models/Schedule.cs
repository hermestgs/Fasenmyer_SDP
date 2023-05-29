using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace FasenmyerConference.Models
{
    public class Schedule
    {

        [Key]
        public string? Id { get; set; }
        public string? Time { get; set; }
        
        public string? Event { get; set;}
        public string? Location { get; set; }
        public int Order { get; set; }

    }
}
