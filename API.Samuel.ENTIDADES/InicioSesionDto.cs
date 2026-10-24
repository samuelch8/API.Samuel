namespace API.Samuel.ENTIDADES
{
    public class InicioSesionDto
    {
        public int IdCliente { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public string? NombreCompleto { get; set; }
    }
}
