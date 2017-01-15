using System.Data.Entity;

namespace PhoneProject.Models
{
    public class mydb : DbContext
    {
        public mydb()
            : base("name=mydb")
        {
        }

        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
