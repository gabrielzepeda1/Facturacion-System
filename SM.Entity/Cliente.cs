namespace SM.Entity
{
    public class Cliente
    {
        public int CodigoCliente { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string CuentaContable { get; set; }
        public decimal LimiteCredito { get; set; }
        public int DiasCredito { get; set; }
        public bool ExcentoImpuestos { get; set; }
        public bool Activo { get; set; }
        public bool Distribuidor { get; set; }
        public bool PersonaJuridica { get; set; }
        public bool Externo { get; set; }
        public int CodigoPais { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoSectorMercado { get; set; }
        public int CodigoVendedor { get; set; }
        public int CodigoUser { get; set; }
        public int CodigoUserUlt { get; set; }

    }
}
