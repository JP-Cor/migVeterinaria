
using System;
using System.Security.Cryptography;

public static class SeguridadClave
{
    private const int Iteraciones = 600000;
    private const int TamanoSal = 16;
    private const int TamanoHash = 32;

    // GENERAR HASH SEGURO
    public static string GenerarHash(string clave)
    {
        if (string.IsNullOrWhiteSpace(clave))
            throw new ArgumentException(
                "La contraseña no puede estar vacía.");

        byte[] sal = new byte[TamanoSal];

        using (RandomNumberGenerator rng =
            RandomNumberGenerator.Create())
        {
            rng.GetBytes(sal);
        }

        byte[] hash;

        using (Rfc2898DeriveBytes pbkdf2 =
            new Rfc2898DeriveBytes(
                clave,
                sal,
                Iteraciones,
                HashAlgorithmName.SHA256))
        {
            hash = pbkdf2.GetBytes(TamanoHash);
        }

        return "PBKDF2-SHA256$" +
            Iteraciones + "$" +
            Convert.ToBase64String(sal) + "$" +
            Convert.ToBase64String(hash);
    }

    // VERIFICAR CONTRASEÑA
    public static bool VerificarClave(
        string clave,
        string hashAlmacenado)
    {
        if (string.IsNullOrEmpty(clave) ||
            string.IsNullOrEmpty(hashAlmacenado))
            return false;

        try
        {
            string[] partes = hashAlmacenado.Split('$');

            if (partes.Length != 4 ||
                partes[0] != "PBKDF2-SHA256")
                return false;

            int iteraciones;

            if (!int.TryParse(partes[1], out iteraciones) ||
                iteraciones < 100000 ||
                iteraciones > 2000000)
                return false;

            byte[] sal = Convert.FromBase64String(partes[2]);
            byte[] hashOriginal =
                Convert.FromBase64String(partes[3]);

            if (sal.Length != TamanoSal ||
                hashOriginal.Length != TamanoHash)
                return false;

            byte[] hashCalculado;

            using (Rfc2898DeriveBytes pbkdf2 =
                new Rfc2898DeriveBytes(
                    clave,
                    sal,
                    iteraciones,
                    HashAlgorithmName.SHA256))
            {
                hashCalculado =
                    pbkdf2.GetBytes(hashOriginal.Length);
            }

            // COMPARACIÓN EN TIEMPO CONSTANTE
            int diferencia = 0;

            for (int i = 0; i < hashOriginal.Length; i++)
            {
                diferencia |=
                    hashOriginal[i] ^ hashCalculado[i];
            }

            return diferencia == 0;
        }
        catch (FormatException)
        {
            return false;
        }
        catch (CryptographicException)
        {
            return false;
        }
    }
}
