using System;
using System.Collections.Generic;

namespace WebSRVC_Core.Modelo
{
    public partial class ImagenesPersona
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }
        public string Foto { get; set; }
        public DateTime Fecha { get; set; }
        public string Fotocompleta { get; set; }

        public Persona Persona { get; set; }
    }
}
