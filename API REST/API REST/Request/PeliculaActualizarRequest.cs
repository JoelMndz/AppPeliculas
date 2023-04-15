namespace API_REST.Request
{
    public record PeliculaActualizarRequest(int Id, string Titulo, int Anio, string Director, int IdGenero);
}
