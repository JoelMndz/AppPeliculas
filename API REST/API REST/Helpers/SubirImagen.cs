using System.IO;

namespace API_REST.Helpers
{
    public class SubirImagen
    {
        public static async Task<string> SubirImagenLocalmente(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            var stream = new MemoryStream(imageBytes);
            var nombre = $"{Guid.NewGuid()}.jpg";
            string ruta = $"./public/imagenes/{nombre}";
            await using var fileStream = new FileStream(ruta, FileMode.Create);
            await stream.CopyToAsync(fileStream);
            return $"public/imagenes/{nombre}";
        }
    }
}
