
using System;

public class Usuario
{
    // IDENTIFICADOR
    public int Id { get; set; }

    // DATOS DEL USUARIO
    public string NombreUsuario { get; set; }

    // CONTRASEÑA PROTEGIDA
    public string ClaveHash { get; set; }

    // ESTADO: A = ACTIVO, I = INACTIVO
    public string Estado { get; set; }

    // ROL: ADMIN O USUARIO
    public string Rol { get; set; }

    // AUDITORIA
    public string AdicionadoPor { get; set; }

    public DateTime FechaAdicion { get; set; }

    public string ModificadoPor { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
