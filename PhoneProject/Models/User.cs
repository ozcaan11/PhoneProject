using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PhoneProject.Models
{
    [Table("User")]
    public sealed class User
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Emails = new HashSet<Email>();
            Phones = new HashSet<Phone>();
        }

        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Nickname { get; set; }

        public string Adress { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Email> Emails { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Phone> Phones { get; set; }
    }
}
