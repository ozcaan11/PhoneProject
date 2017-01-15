using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PhoneProject.Models
{
    [Table("Type")]
    public sealed class Type
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Type()
        {
            Phones = new HashSet<Phone>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Phone> Phones { get; set; }
    }
}
