namespace PhoneProject.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Email")]
    public class Email
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
