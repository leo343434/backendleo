using System;

namespace Backend.Core.Entities
{
    public class Estudiante
    {
        public int IdEstudiante { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Ci { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
