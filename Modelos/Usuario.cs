using System;

namespace Veterinaria_Web.Modelos
{
    // Representa los datos de la tabla USUARIOS.
    public class Usuario
    {
        public int ID_USUARIO { get; set; }

        public string USUARIO { get; set; }

        public string CLAVE { get; set; }

        public string ESTADO { get; set; }

        // AUDITORÍA DE CREACIÓN
        public DateTime FECHA_ADICION { get; set; }

        public string ADICIONADO_POR { get; set; }

        // AUDITORÍA DE MODIFICACIÓN
        public DateTime? FECHA_MODIFICACION { get; set; }

        public string MODIFICADO_POR { get; set; }
    }
}