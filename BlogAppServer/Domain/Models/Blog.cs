using BlogApp.Server.Domain.Modelos;

namespace BlogApp.Server.Domain.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public string Link {  get; set; } = string.Empty;
        public string AutorId { get; set; } = string.Empty;
        public UsuarioAplicacion Autor { get; set; } = default!;
    }
}
