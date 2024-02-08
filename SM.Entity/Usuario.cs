namespace SM.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SEGURIDAD.Usuario")]
    public partial class Usuario
    {
        [Key]
        public Guid IdUsuario { get; set; }

        public Guid IdRol { get; set; }

        [Column("Usuario")]
        [Required]
        [StringLength(20)]
        public string Usuario1 { get; set; }

        [Required]
        [StringLength(20)]
        public string Contrasena { get; set; }

        public bool EsActivo { get; set; }

        public DateTime FechaRegistro { get; set; }

        [Required]
        [StringLength(20)]
        public string CreadoPor { get; set; }

        public int PaisPorDefecto { get; set; }

        [StringLength(10)]
        public string UltimaConexion { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Apellido { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(30)]
        public string Cedula { get; set; }

        [StringLength(100)]
        public string Direccion { get; set; }

        public virtual Rol Rol { get; set; }
    }
}
