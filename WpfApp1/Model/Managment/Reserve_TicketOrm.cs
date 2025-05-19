using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Model.Managment
{
    /// <summary>
    /// Clase ORM para operaciones relacionadas con reservas de tickets.
    /// </summary>
    public static class Reserve_TicketOrm
    {
        /// <summary>
        /// Obtiene la lista de tickets reservados por un usuario.
        /// </summary>
        /// <param name="userId">ID del usuario.</param>
        /// <returns>Lista de objetos <see cref="Reserve_Ticket"/> asociados al usuario.</returns>
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

        /// <summary>
        /// Obtiene la butaca asociada a un ticket.
        /// </summary>
        /// <param name="ticketId">ID del ticket (corresponde al ID de la butaca).</param>
        /// <returns>Objeto <see cref="Armchair"/> asociado al ticket, o null si no existe.</returns>
        public static Armchair SelectTicketArmchair(int ticketId)
        {
            Armchair armchair = new Armchair();
            try
            {
                armchair = Orm.db.Armchair.FirstOrDefault(r => r.armchair_id == ticketId);
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            return armchair;
        }
    }
}
