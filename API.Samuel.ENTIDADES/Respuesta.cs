namespace API.Samuel.ENTIDADES
{
    // Formato estándar de todas las respuestas de la API
    public class Respuesta<T>
    {
        public bool Ok { get; set; }
        public string? Mensaje { get; set; }
        public T? ValorRetorno { get; set; }
    }
}
