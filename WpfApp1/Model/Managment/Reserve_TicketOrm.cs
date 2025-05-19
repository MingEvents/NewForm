using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Model.Managment
{
    public static class Reserve_TicketOrm
    {
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
