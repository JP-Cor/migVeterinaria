
using System;

public class Mascota
{
    // IDENTIFICADOR
    public int Id { get; set; }

    // DATOS DE LA MASCOTA
    public string Nombre { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string Sexo { get; set; }

    public decimal Peso { get; set; }

    public string Alergias { get; set; }

    // RELACION CON PROPIETARIOS
    public int PropietarioId { get; set; }

    // DATOS DEL PROPIETARIO PARA MOSTRAR EN PANTALLA
    public string IdentificacionPropietario { get; set; }

    public string NombrePropietario { get; set; }

    public string ApellidoPropietario { get; set; }

    // AUDITORIA
    public string AdicionadoPor { get; set; }

    public DateTime FechaAdicion { get; set; }

    public string ModificadoPor { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
