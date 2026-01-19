using System;
using System.Collections.Generic;
using System.Linq;
using System;

public class ValidadorCedulaEcuatoriana
{
    public static bool ValidarCedula(string cedula)
    {
        // 1. Verificar longitud y que solo contenga números
        if (string.IsNullOrEmpty(cedula) || cedula.Length != 10)
            return false;

        if (!long.TryParse(cedula, out _))
            return false;

        // 2. Validar provincia
        int provincia = int.Parse(cedula.Substring(0, 2));
        if (provincia < 1 || provincia > 24)
            return false;

        // 3. Validar tercer dígito
        int tercerDigito = int.Parse(cedula.Substring(2, 1));
        if (tercerDigito > 5)
            return false;

        // 4. Calcular dígito verificador
        int suma = 0;
        for (int i = 0; i < 9; i++)
        {
            int digito = int.Parse(cedula[i].ToString());

            if (i % 2 == 0)
            {
                digito *= 2;
                if (digito > 9)
                    digito -= 9;
            }

            suma += digito;
        }

        int digitoVerificadorCalculado = (10 - (suma % 10)) % 10;
        int digitoVerificadorReal = int.Parse(cedula[9].ToString());

        return digitoVerificadorCalculado == digitoVerificadorReal;
    }
}

