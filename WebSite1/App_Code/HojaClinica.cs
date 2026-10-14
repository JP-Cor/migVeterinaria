
using System;

public class HojaClinica
{
    // IDENTIFICADOR
    public int Id { get; set; }

    // DATOS DE LA ATENCION CLINICA
    public DateTime FechaAtencion { get; set; }

    public string Sintomas { get; set; }

    public string Diagnostico { get; set; }

    public string Tratamiento { get; set; }

    // RELACION CON MASCOTAS
    public int MascotaId { get; set; }

    // DATOS PARA MOSTRAR EN PANTALLA
    public string NombreMascota { get; set; }

    public string NombrePropietario { get; set; }

    public string ApellidoPropietario { get; set; }

    // AUDITORIA
    public string AdicionadoPor { get; set; }

    public DateTime FechaAdicion { get; set; }

    public string ModificadoPor { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
