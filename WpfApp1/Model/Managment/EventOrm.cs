using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Model.Managment
{
    public static class EventOrm
    {
        /// <summary>
        /// Obtiene una lista de todos los eventos
        /// </summary>
        public static List<Event> SelectAllEvents()
        {
            List<Event> events = new List<Event>();
            try
            {
                events = Orm.db.Event.ToList();
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            return events;
        }

        /// <summary>
        /// Inserta un nuevo evento en la base de datos
        /// </summary>
        public static bool InsertEvent(Event newEvent)
        {
            try
            {
                Orm.db.Event.Add(newEvent);
                Orm.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
        }
        public static Event GetEventById(int eventId)
        {
            try
            {
                return Orm.db.Event.Find(eventId);
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
        }
        /// <summary>
        /// Actualiza un evento existente
        /// </summary>
        public static bool UpdateEvent(Event updatedEvent)
        {
            try
            {
                var existingEvent = Orm.db.Event.Find(updatedEvent.event_id);

                if (existingEvent == null)
                    return false;

                // Actualizamos cada propiedad
                existingEvent.name = updatedEvent.name;
                existingEvent.price = updatedEvent.price;
                existingEvent.reserved_places = updatedEvent.reserved_places;
                existingEvent.photo = updatedEvent.photo;
                existingEvent.start_date = updatedEvent.start_date;
                existingEvent.end_date = updatedEvent.end_date;
                existingEvent.seating = updatedEvent.seating;
                existingEvent.descripcion = updatedEvent.descripcion;
                existingEvent.establish_id = updatedEvent.establish_id;

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
        /// Elimina un evento por su ID
        /// </summary>
        public static bool DeleteEvent(int eventId)
        {
            try
            {
                var eventToDelete = Orm.db.Event.Find(eventId);
                if (eventToDelete != null)
                {
                    Orm.db.Event.Remove(eventToDelete);
                    Orm.db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
        }

    }
}