using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Model.Managment
{
    /// <summary>
    /// Clase ORM para operaciones relacionadas con roles de usuario.
    /// </summary>
    public static class RoleOrm
    {
        /// <summary>
        /// Obtiene una lista con los nombres de todos los roles.
        /// </summary>
        /// <returns>Lista de nombres de roles.</returns>
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

        /// <summary>
        /// Obtiene el ID de un rol a partir de su nombre.
        /// </summary>
        /// <param name="roleName">Nombre del rol.</param>
        /// <returns>ID del rol, o 0 si no se encuentra.</returns>
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

        /// <summary>
        /// Obtiene el nombre de un rol a partir de su ID.
        /// </summary>
        /// <param name="roleId">ID del rol.</param>
        /// <returns>Nombre del rol, o cadena vacía si no se encuentra.</returns>
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
    }
}
