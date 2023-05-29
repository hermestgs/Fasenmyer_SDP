using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace FasenmyerConference.Models
{
    public class Spotlight_Sponsor
    {
        [Key]
        public string? Id { get; set; }
        public string? Sponsor_Name { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string? Details { get; set; }
        //public byte[]? Logo { get; set; }


    }
}
