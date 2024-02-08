namespace SM.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.ItemMenu")]
    public partial class ItemMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ItemMenu()
        {
            ItemMenuRols = new HashSet<ItemMenuRol>();
        }

        [Key]
        public Guid IdItemMenu { get; set; }

        public Guid IdMenu { get; set; }

        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }

        public bool EsActivo { get; set; }

        public DateTime FechaRegistro { get; set; }

        [Required]
        [StringLength(20)]
        public string CreadoPor { get; set; }

        [StringLength(100)]
        public string Ruta { get; set; }

        [StringLength(50)]
        public string Pagina { get; set; }

        [StringLength(50)]
        public string Icono { get; set; }

        public bool? Ocultar { get; set; }

        public int? Posicion { get; set; }

        public virtual Menu Menu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemMenuRol> ItemMenuRols { get; set; }
    }
}
