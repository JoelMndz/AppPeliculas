namespace API_REST.Request
{
    public record PeliculaCrearRequest(string Titulo, int Anio, string Director, string FotoBase64, int IdGenero, int IdUsuario);
}
