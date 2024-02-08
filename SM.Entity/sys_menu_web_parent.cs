namespace SM.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.sys_menu_web_parent")]
    public partial class sys_menu_web_parent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_menu { get; set; }

        [Required]
        [StringLength(100)]
        public string etiqueta { get; set; }

        public int posicion { get; set; }

        [StringLength(200)]
        public string ruta { get; set; }

        [StringLength(50)]
        public string pagina { get; set; }

        [StringLength(200)]
        public string icono { get; set; }

        public bool ocultar { get; set; }

        public bool activo { get; set; }

        public DateTime? FechaRegistro { get; set; }

        [StringLength(20)]
        public string CreadoPor { get; set; }
    }
}
