using System;
using System.Data.SqlClient;
using WpfApp1.Model;

namespace WpfApp1.Models
{
    /// <summary>
    /// Clase utilitaria para operaciones ORM y manejo de errores de base de datos.
    /// </summary>
    class Orm
    {
        /// <summary>
        /// Contexto de base de datos de la aplicación.
        /// </summary>
        public static MingsEventsEntities db = new MingsEventsEntities();

        /// <summary>
        /// Devuelve un mensaje de error amigable a partir de una excepción SQL.
        /// </summary>
        /// <param name="sqlException">Excepción SQL capturada.</param>
        /// <returns>Mensaje de error descriptivo.</returns>
        public static string ErrorMessage(SqlException sqlException)
        {
            string message = "";

            switch (sqlException.Number)
            {
                case 2:
                    message = "The server is not operational.";
                    break;
                case 547:
                    message = "The record cannot be deleted because it has related records.";
                    break;
                case 4060:
                    message = "Could not connect to the database.";
                    break;
                case 18456:
                    message = "Login failed.";
                    break;
                case 2601:
                    message = "A record with the same value already exists.";
                    break;
                default:
                    message = sqlException.Number + " - " + sqlException.Message;
                    break;
            }

            return message;
        }

        /// <summary>
        /// Devuelve un mensaje de error a partir de una excepción general.
        /// </summary>
        /// <param name="ex">Excepción capturada.</param>
        /// <returns>Mensaje de error descriptivo.</returns>
        internal static string ErrorMessage(Exception ex)
        {
            string message = "";
            /*
            switch (ex.GetType().Name)
            {
                case "SqlException":
                    SqlException sqlException = (SqlException)ex;
                    message = ErrorMessage(sqlException);
                    break;
                case "InvalidOperationException":
                    message = "Invalid operation.";
                    break;
                case "ArgumentNullException":
                    message = "Null argument.";
                    break;
                default:
                    message = ex.Message;
                    break;
            }
            */
            return message;
            // throw new NotImplementedException();
        }
    }
}