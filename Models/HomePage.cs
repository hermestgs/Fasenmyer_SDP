using Microsoft.Identity.Client;
using System;
using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;
using System.Web.Mvc;

namespace FasenmyerConference.Models
{
    // this model will hold all text elements to be added to the home page
    public class HomePage
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string? WelcomeArea { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string? KeynoteIntro { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string? SpotlightIntro { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string? ContactInfo { get; set; }

    }
}
