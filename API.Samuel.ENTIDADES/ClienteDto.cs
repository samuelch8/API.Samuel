namespace API.Samuel.ENTIDADES
{
    // Cliente del estudio fotográfico Enfoque
    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public string? Telefono { get; set; }
        public string? Provincia { get; set; }
        public int IdTipoSesion { get; set; }
        public string? NombreTipoSesion { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
