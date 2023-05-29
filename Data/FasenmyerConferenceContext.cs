using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FasenmyerConference.Models;

namespace FasenmyerConference.Data
{
    public class FasenmyerConferenceContext : DbContext
    {
        public FasenmyerConferenceContext (DbContextOptions<FasenmyerConferenceContext> options)
            : base(options)
        {
        }

        public DbSet<FasenmyerConference.Models.Presentations> Presentations { get; set; } = default!;

        public DbSet<FasenmyerConference.Models.Keynote_Speaker> Keynote_Speaker { get; set; }

        public DbSet<FasenmyerConference.Models.Schedule> Schedule { get; set; }

        public DbSet<FasenmyerConference.Models.Spotlight_Sponsor> Spotlight_Sponsor { get; set; }

        public DbSet<FasenmyerConference.Models.Conference> Conference { get; set; }

        public DbSet<FasenmyerConference.Models.HomePage>? HomePage { get; set; }

        //public DbSet<FasenmyerConference.Models.LoginViewModel> UserAdmin { get; set; } = default!;
    }
}
