
using Microsoft.EntityFrameworkCore;
using MVC.Interface;
using MVC.Models;
namespace MVC.Data
{
    public class MVC_Context : DbContext
    {
        public MVC_Context(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Course> courses { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Employee> Employees { get; set; }

        //public DbSet<ElectronicCopies> ElectronicCopies { get; set; }
        //public DbSet<OnlineArticle> OnlineArticles { get; set; }
        //public DbSet<VideoMaterial> VideoMaterials { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MVC;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}
