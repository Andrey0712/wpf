using Budzet.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace Budzet.EFData
{
    public class EFDataContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=91.238.103.51;Port=5743;Database=andreydb;Username=andrey;Password=$544$B77w**G)K$t!Ube22}xa");
        }
    }
}
