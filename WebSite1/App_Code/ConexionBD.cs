
using System;
using System.Configuration;
using System.Data.SqlClient;

public class ConexionBD
{
    public static SqlConnection ObtenerConexion()
    {
        string cadena = ConfigurationManager
            .ConnectionStrings["Conexion"]
            .ConnectionString;

        return new SqlConnection(cadena);
    }
}
