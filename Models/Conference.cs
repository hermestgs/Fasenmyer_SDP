using MessagePack;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;


namespace FasenmyerConference.Models
{
    public class Conference
    {
        [Required]
        public int? Id { get; set; }
        public string? Date { get; set; }
        public string? Sponsor { get; set; }
        public string? Keynote { get; set; }



    }
}
