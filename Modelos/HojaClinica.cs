using System;

namespace Veterinaria_Web.Modelos
{
    // Representa una atención de la tabla HOJAS_CLINICAS.
    public class HojaClinica
    {
        public int ID_HOJA { get; set; }

        public DateTime FECHA_ATENCION { get; set; }

        public string SINTOMAS { get; set; }

        public string DIAGNOSTICO { get; set; }

        public string TRATAMIENTO { get; set; }

        // Relaciona la atención con la mascota atendida.
        public int ID_MASCOTA { get; set; }

        // AUDITORÍA DE CREACIÓN
        public DateTime FECHA_ADICION { get; set; }

        public string ADICIONADO_POR { get; set; }

        // AUDITORÍA DE MODIFICACIÓN
        public DateTime? FECHA_MODIFICACION { get; set; }

        public string MODIFICADO_POR { get; set; }
    }
}