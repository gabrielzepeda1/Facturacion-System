namespace SM.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.sys_usuario")]
    public partial class sys_usuario
    {
        [Key]
        public int consecutivo_usuario { get; set; }

        [Required]
        [StringLength(50)]
        public string usuario { get; set; }

        [Required]
        [StringLength(200)]
        public string contrasenia { get; set; }

        [StringLength(30)]
        public string email { get; set; }

        public int? pais_default { get; set; }

        public int? cod_empresa { get; set; }

        [StringLength(10)]
        public string cod_puesto { get; set; }

        public int cod_rol { get; set; }

        public bool? es_administrador { get; set; }

        public bool activo { get; set; }

        public DateTime Fecha_Registro { get; set; }

        public DateTime? ultima_conexion { get; set; }

        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(50)]
        public string apellido { get; set; }

        [StringLength(15)]
        public string telefono { get; set; }

        [StringLength(30)]
        public string cedula_ruc { get; set; }

        [StringLength(50)]
        public string direccion { get; set; }
    }
}
