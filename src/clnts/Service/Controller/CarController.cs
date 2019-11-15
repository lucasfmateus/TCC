using UI.Service.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace UI.Service.Controller
{
    public class CarController : ServiceBase
    {
        public async Task<List<string>> GetAllAdressAsync()
        {
            try
            {
                var request = await client.GetAsync("Car/GetAdress");

                return await request.Content.ReadAsAsync<List<string>>();

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ParkingClientReset();
            }
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            try
            {
                var request = await client.GetAsync("Car/GetCars");

                return await request.Content.ReadAsAsync<List<Car>>();

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ParkingClientReset();
            }
        }

        public async Task<List<Model>> GetAllModelsAsync()
        {
            try
            {
                var request = await client.GetAsync("Car/GetModels");

                return await request.Content.ReadAsAsync<List<Model>>();

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ParkingClientReset();
            }
        }

        public async Task<List<Manufacturer>> GetAllManufacturesAsync()
        {
            try
            {
                var request = await client.GetAsync("Car/GetManufactures/");
                return await request.Content.ReadAsAsync<List<Manufacturer>>();

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                ParkingClientReset();
            }
        }

        public async Task<List<Core.Models.Type>> GetAllTypesAsync()
        {
            try
            {
                var request = await client.GetAsync("Car/GetTypes");

                return await request.Content.ReadAsAsync<List<Core.Models.Type>>();

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ParkingClientReset();
            }
        }

        public async Task<Manufacturer> GetManufacturesByNameAsync(string name)
        {
            try
            {
                var request = await client.GetAsync("Car/GetManufacturesByName?name="+name);


                return await request.Content.ReadAsAsync<Manufacturer>();

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ParkingClientReset();
            }
        }

        public async Task<Model> GetModelByNameAsync(string name)
        {
            try
            {
                var request = await client.GetAsync("Car/GetModelByName?name=" + name);
                
                return await request.Content.ReadAsAsync<Model>();

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ParkingClientReset();
            }
        }
        public async Task<Core.Models.Type> GetTypeByNameAsync(string name)
        {
            try
            {
                var request = await client.GetAsync("Car/GetTypeByName?name=" + name);

                return await request.Content.ReadAsAsync<Core.Models.Type>();

            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                ParkingClientReset();
            }
        }

        public async Task<bool> NewManufactureAsync(Manufacturer manufacturer)
        {
            var myContent = JsonConvert.SerializeObject(manufacturer);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("Car/NewManufacture/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<bool>();

                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    ParkingClientReset();
                }
            }

        }

        public async Task<bool> NewModelAsync(Model model)
        {
            var myContent = JsonConvert.SerializeObject(model);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("Car/NewModel/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<bool>();

                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    ParkingClientReset();
                }
            }

        }

        public async Task<bool> NewCarAsync(Car car)
        {
            var myContent = JsonConvert.SerializeObject(car);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("Car/NewCar/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<bool>();

                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    ParkingClientReset();
                }
            }

        }

        public async Task<bool> NewTypeAsync(Core.Models.Type type)
        {
            var myContent = JsonConvert.SerializeObject(type);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("Car/NewCarType/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<bool>();

                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    ParkingClientReset();
                }
            }

        }

    }
}
