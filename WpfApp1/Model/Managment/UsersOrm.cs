using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.Models;

namespace WpfApp1.Model.Managment
{
    /// <summary>
    /// Clase ORM para operaciones relacionadas con usuarios.
    /// </summary>
    public static class UsersOrm
    {
        /// <summary>
        /// Obtiene una lista de todos los usuarios.
        /// </summary>
        /// <returns>Lista de objetos <see cref="Users"/>.</returns>
        public static List<Users> SlectAllUsers()
        {
            List<Users> users = new List<Users>();
            try
            {
                users = Orm.db.Users.ToList();
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            return users;
        }

        /// <summary>
        /// Inserta un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="user">El usuario a insertar.</param>
        /// <returns>True si la inserción fue exitosa, false en caso contrario.</returns>
        public static bool InsertUser(Users user)
        {
            try
            {
                Orm.db.Users.Add(user);
                Orm.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        /// <param name="user">El usuario con los datos actualizados.</param>
        /// <returns>True si la actualización fue exitosa, false si el usuario no existe.</returns>
        public static bool UpdateUser(Users user)
        {
            try
            {
                var existingUser = Orm.db.Users.Find(user.user_id);
                if (existingUser != null)
                {
                    existingUser.phone = user.phone;
                    existingUser.password = user.password;
                    existingUser.second_name = user.second_name;
                    existingUser.name = user.name;
                    existingUser.email = user.email;
                    // Actualiza otros campos según sea necesario
                    Orm.db.SaveChanges();
                    return true;
                }
                return false; // Usuario no encontrado
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false si el usuario no existe.</returns>
        public static bool DeleteUser(int userId)
        {
            try
            {
                var user = Orm.db.Users.Find(userId);
                if (user != null)
                {
                    Orm.db.Users.Remove(user);
                    Orm.db.SaveChanges();
                    return true;
                }
                return false; // Usuario no encontrado
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception("El usuario tiene reservas activas");
            }
        }

        /// <summary>
        /// Valida el login de un usuario por nombre de usuario o email y contraseña.
        /// </summary>
        /// <param name="usernameOrEmail">Nombre de usuario o email.</param>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>El objeto <see cref="Users"/> si las credenciales son correctas, o null si no coinciden.</returns>
        public static Users Login(string usernameOrEmail, string password)
        {
            try
            {
                // Busca el usuario por nombre de usuario o email
                var user = Orm.db.Users
                    .FirstOrDefault(u => (u.name == usernameOrEmail || u.email == usernameOrEmail)
                                         && u.password == password);

                return user; // Devuelve el usuario si coincide, sino null
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
        }
    }
}