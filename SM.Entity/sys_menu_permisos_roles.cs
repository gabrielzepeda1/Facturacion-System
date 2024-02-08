namespace SM.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.sys_menu_permisos_roles")]
    public partial class sys_menu_permisos_roles
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_menu { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_rol { get; set; }

        public Guid? IdItemMenuRol { get; set; }

        public bool? EsActivo { get; set; }

        public DateTime FechaRegistro { get; set; }

        [StringLength(20)]
        public string CreadoPor { get; set; }
    }
}
