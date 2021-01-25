namespace Backend.Core.Entities
{
    public class Inscripcion
    {
        public int IdInscripcion { get; set; }
        public string Descripcion { get; set; }
        public int IdMateria { get; set; }
        public int IdEstudiante { get; set; }
    }
}
