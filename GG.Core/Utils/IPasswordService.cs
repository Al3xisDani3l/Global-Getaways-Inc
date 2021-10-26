using System;
using System.Collections.Generic;
using System.Text;

namespace GG.Core
{

    /**
     *Author: Alexis Daniel hernandez Gamez
     *Version: 05/08/2021
    */

    /// <summary>
    /// Proporciona funcionalidades de encriptacion y comprobacion de cadenas
    /// </summary>
    public interface IPasswordService
    {
        /// <summary>
        /// Convierte una cadena a una representacion segura e indesifrable.
        /// </summary>
        /// <param name="password">Cadena a encriptar</param>
        /// <returns>Cadena en su representacion encriptada</returns>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        string Hash(string password);
        /// <summary>
        /// Comprueba si una cadena corresponde al texto cifrado
        /// </summary>
        /// <param name="hash">cadena cifrada</param>
        /// <param name="password">Cadena sin cifrar</param>
        /// <returns>Devuelve <seealso cref="true"/> si la cadena corresponde a su representacion cifrada, en caso contrario <see cref="false"/> </returns>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        bool Check(string hash, string password);
    }
}
