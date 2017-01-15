using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneProject.Models
{
    [Table("Phone")]
    public class Phone
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Number { get; set; }

        public int? TypeId { get; set; }

        public int? UserId { get; set; }

        public virtual Type Type { get; set; }

        public virtual User User { get; set; }
    }
}
