
using System;

public class Propietario
{
    public int Id { get; set; }
    public string Identificacion { get; set; }
    public string PrimerNombre { get; set; }
    public string SegundoNombre { get; set; }
    public string PrimerApellido { get; set; }
    public string SegundoApellido { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public string AdicionadoPor { get; set; }
    public DateTime FechaAdicion { get; set; }
    public string ModificadoPor { get; set; }
    public DateTime? FechaModificacion { get; set; }
}
