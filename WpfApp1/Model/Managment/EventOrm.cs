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
        /// Obtiene una lista de todos los eventos.
        /// </summary>
        /// <returns>Lista de objetos <see cref="Event"/>.</returns>
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
        /// Inserta un nuevo evento en la base de datos.
        /// </summary>
        /// <param name="newEvent">El evento a insertar.</param>
        /// <returns>True si la inserción fue exitosa, false en caso contrario.</returns>
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

        /// <summary>
        /// Obtiene un evento por su ID.
        /// </summary>
        /// <param name="eventId">ID del evento.</param>
        /// <returns>El objeto <see cref="Event"/> encontrado, o null si no existe.</returns>
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
        /// Actualiza un evento existente.
        /// </summary>
        /// <param name="updatedEvent">El evento con los datos actualizados.</param>
        /// <returns>True si la actualización fue exitosa, false en caso contrario.</returns>
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
        /// Elimina un evento por su ID.
        /// </summary>
        /// <param name="eventId">ID del evento a eliminar.</param>
        /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
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