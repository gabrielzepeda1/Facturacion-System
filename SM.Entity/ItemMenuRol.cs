namespace SM.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.ItemMenuRol")]
    public partial class ItemMenuRol
    {
        [Key]
        public Guid IdItemMenuRol { get; set; }

        public Guid IdItemMenu { get; set; }

        public Guid IdRol { get; set; }

        public bool EsActivo { get; set; }

        public DateTime FechaRegistro { get; set; }

        [Required]
        [StringLength(20)]
        public string CreadoPor { get; set; }

        public virtual ItemMenu ItemMenu { get; set; }

        public virtual Rol Rol { get; set; }
    }
}
