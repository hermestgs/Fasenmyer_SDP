using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;
using System;
using MessagePack;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
using System.Web.Mvc;

namespace FasenmyerConference.Models
{
    public class Keynote_Speaker
    {
        [Key]
        public string? Id { get; set; }
        public string? KName { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string? Bio { get; set; }
        //public byte[]? Photo { get; set; }
    }
}
