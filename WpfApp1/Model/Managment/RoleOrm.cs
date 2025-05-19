using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Model.Managment
{
    public static class RoleOrm
    {
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
    }
}
