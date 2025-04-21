namespace BlogApp.Shared.DTOs
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string AutorId { get; set; } = string.Empty;
        public UsuarioDto Autor { get; set; } = new();
    }
}
