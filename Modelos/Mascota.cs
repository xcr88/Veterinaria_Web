using System;

namespace Veterinaria_Web.Modelos
{
    // Representa los datos de la tabla MASCOTAS.
    public class Mascota
    {
        public int ID_MASCOTA { get; set; }

        public string NOMBRE { get; set; }

        public DateTime FECHA_NACIMIENTO { get; set; }

        public string SEXO { get; set; }

        public decimal PESO { get; set; }

        public string ALERGIAS { get; set; }

        // Relaciona la mascota con su propietario.
        public int ID_PROPIETARIO { get; set; }

        // AUDITORÍA DE CREACIÓN
        public DateTime FECHA_ADICION { get; set; }

        public string ADICIONADO_POR { get; set; }

        // AUDITORÍA DE MODIFICACIÓN
        public DateTime? FECHA_MODIFICACION { get; set; }

        public string MODIFICADO_POR { get; set; }
    }
}