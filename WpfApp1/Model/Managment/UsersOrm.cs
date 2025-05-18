using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using WpfApp1.Model;

namespace WpfApp1.Model.Managment
{
    public static class UsersOrm
    {
        // Método para obtener todos los usuarios
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

        // Método para insertar un nuevo usuario
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

        // Método para actualizar un usuario existente
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

        // Método para eliminar un usuario
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

        // ➤ NUEVO MÉTODO: Validar login de usuario
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

        public static List<string> SlectAllRoles()
        {
            List<Role> roles = new List<Role>();
            List<string> rolesName = new List<string>();
            try
            {
                roles = Orm.db.Role.ToList();
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            foreach (Role role in roles)
            {
                rolesName.Add(role.name);
            }
            return rolesName;
        }
        public static int SelectRoleId(string roleName)
        {
            int roleId = 0;
            try
            {
                var role = Orm.db.Role.FirstOrDefault(r => r.name == roleName);
                if (role != null)
                {
                    roleId = role.role_id;
                }
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            return roleId;
        }
        public static string SelectRoleName(int roleId)
        {
            var roleName = "";
            try
            {
                var role = Orm.db.Role.FirstOrDefault(r => r.role_id == roleId);
                if (role != null)
                {
                    roleName = role.name;
                }
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            return roleName;
        }
        public static List<Reserve_Ticket> SelectTicket(int userId)
        {
            List<Reserve_Ticket> tickets = new List<Reserve_Ticket>();
            try
            {
                tickets = Orm.db.Reserve_Ticket.Where(r => r.user_id == userId).ToList();
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            return tickets;
        }
    }
}