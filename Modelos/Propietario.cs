using System;

namespace Veterinaria_Web.Modelos
{
    // Representa los datos de la tabla PROPIETARIOS.
    public class Propietario
    {
        public int ID_PROPIETARIO { get; set; }

        public string IDENTIFICACION { get; set; }

        public string PRIMER_NOMBRE { get; set; }

        public string SEGUNDO_NOMBRE { get; set; }

        public string PRIMER_APELLIDO { get; set; }

        public string SEGUNDO_APELLIDO { get; set; }

        public string TELEFONO { get; set; }

        public string CORREO { get; set; }

        // AUDITORÍA DE CREACIÓN
        public DateTime FECHA_ADICION { get; set; }

        public string ADICIONADO_POR { get; set; }

        // AUDITORÍA DE MODIFICACIÓN
        public DateTime? FECHA_MODIFICACION { get; set; }

        public string MODIFICADO_POR { get; set; }
    }
}