namespace SM.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.Rol")]
    public partial class Rol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rol()
        {
            ItemMenuRols = new HashSet<ItemMenuRol>();
            Usuarios = new HashSet<Usuario>();
        }

        [Key]
        public Guid IdRol { get; set; }

        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }

        public bool EsActivo { get; set; }

        public DateTime FechaRegistro { get; set; }

        [Required]
        [StringLength(20)]
        public string CreadoPor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemMenuRol> ItemMenuRols { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
