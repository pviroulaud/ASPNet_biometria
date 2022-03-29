using System;
using System.Collections.Generic;

namespace WebSRVC_Core.Modelo
{
    public partial class Persona
    {
        public Persona()
        {
            ImagenesPersona = new HashSet<ImagenesPersona>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string TrainData { get; set; }
        public int? Edad { get; set; }

        public ICollection<ImagenesPersona> ImagenesPersona { get; set; }
    }
}
