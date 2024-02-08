using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SM.Entity
{
    public partial class DBFACTURACION : DbContext
    {
        public DBFACTURACION()
            : base("name=DBFACTURACION")
        {
        }

        public virtual DbSet<ItemMenu> ItemMenus { get; set; }
        public virtual DbSet<ItemMenuRol> ItemMenuRols { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<sys_menu_permisos_roles> sys_menu_permisos_roles { get; set; }
        public virtual DbSet<sys_menu_web> sys_menu_web { get; set; }
        public virtual DbSet<sys_menu_web_parent> sys_menu_web_parent { get; set; }
        public virtual DbSet<sys_usuario> sys_usuario { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemMenu>()
                .HasMany(e => e.ItemMenuRols)
                .WithRequired(e => e.ItemMenu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Menu>()
                .HasMany(e => e.ItemMenus)
                .WithRequired(e => e.Menu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.ItemMenuRols)
                .WithRequired(e => e.Rol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rol>()
                .HasMany(e => e.Usuarios)
                .WithRequired(e => e.Rol)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sys_usuario>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<sys_usuario>()
                .Property(e => e.cod_puesto)
                .IsFixedLength();

            modelBuilder.Entity<sys_usuario>()
                .Property(e => e.telefono)
                .IsUnicode(false);

            modelBuilder.Entity<sys_usuario>()
                .Property(e => e.cedula_ruc)
                .IsUnicode(false);

            modelBuilder.Entity<sys_usuario>()
                .Property(e => e.direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.UltimaConexion)
                .IsFixedLength();

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Telefono)
                .IsFixedLength();
        }
    }
}
