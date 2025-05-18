using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Model.Managment
{
    public static class EstablishmentOrm
    {
        /// <summary>
        /// Obtiene una lista de todos los establecimientos
        /// </summary>
        public static List<Establishment> SelectAllEstablishments()
        {
            List<Establishment> establishments = new List<Establishment>();
            try
            {
                establishments = Orm.db.Establishment.ToList();
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            return establishments;
        }

        /// <summary>
        /// Inserta un nuevo establecimiento en la base de datos
        /// </summary>
        public static bool InsertEstablishment(Establishment newEstablishment)
        {
            try
            {
                Orm.db.Establishment.Add(newEstablishment);
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
        /// Actualiza un establecimiento existente
        /// </summary>
        public static bool UpdateEstablishment(Establishment updatedEstablishment)
        {
            try
            {
                var existingEstablishment = Orm.db.Establishment.Find(updatedEstablishment.establish_id);

                if (existingEstablishment == null)
                    return false;

                // Actualizamos cada propiedad
                existingEstablishment.name = updatedEstablishment.name;
                existingEstablishment.direction = updatedEstablishment.direction;
                existingEstablishment.capacity = updatedEstablishment.capacity;
                existingEstablishment.city_id = updatedEstablishment.city_id;

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
        /// Elimina un establecimiento por su ID
        /// </summary>
        public static bool DeleteEstablishment(int establishmentId)
        {
            try
            {
                var establishmentToDelete = Orm.db.Establishment.Find(establishmentId);
                if (establishmentToDelete != null)
                {
                    Orm.db.Establishment.Remove(establishmentToDelete);
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
        public static Establishment GetEstablishmentById(int establishmentId)
        {
            Establishment establishment = null;
            try
            {
                establishment = Orm.db.Establishment.FirstOrDefault(e => e.establish_id == establishmentId);
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            return establishment;
        }
        public static List<string> selectAllCities()
        {
            List<City> cities = new List<City>();
            List<string> citiesName = new List<string>();
            try
            {
                cities = Orm.db.City.ToList();
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }

            foreach (var city in cities)
            {
                citiesName.Add(city.name);
            }
            return citiesName;

        }
        public static List<City> getAllCities()
        {
            List<City> cities = new List<City>();
            try
            {
                cities = Orm.db.City.ToList();
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }

            return cities;

        }
        public static int SlectCityId(string cityName)
        {
            int cityId = 0;
            try
            {
                var city = Orm.db.City.FirstOrDefault(c => c.name == cityName);
                if (city != null)
                {
                    cityId = city.city_id;
                }
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
            return cityId;
        }
        // Añade este método a la clase EstablishmentOrm
        public static void InsertArmchairsForEstablishment(int establishId, int numRows, int numColumns)
        {
            try
            {
                for (int row = 1; row <= numRows; row++)
                {
                    for (int col = 1; col <= numColumns; col++)
                    {
                        var armchair = new Armchair
                        {
                            rows = row,
                            columns = col,
                            establish_id = establishId
                        };
                        Orm.db.Armchair.Add(armchair);
                    }
                }
                Orm.db.SaveChanges();
            }
            catch (Exception ex)
            {
                string message = Orm.ErrorMessage(ex);
                throw new Exception(message);
            }
        }
    }
}