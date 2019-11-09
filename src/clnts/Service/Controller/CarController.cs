using UI.Service.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Models;

namespace UI.Service.Controller
{
    public class CarController : ServiceBase
    {
        public async Task<List<string>> GetAllAdressAsync()
        {
            try
            {
                var request = await client.GetAsync("car/GetAdress");

                return await request.Content.ReadAsAsync<List<string>>();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            try
            {
                var request = await client.GetAsync("car/GetCars");

                return await request.Content.ReadAsAsync<List<Car>>();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Model>> GetAllModelsAsync()
        {
            try
            {
                var request = await client.GetAsync("car/GetModels");

                return await request.Content.ReadAsAsync<List<Model>>();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Manufacturer>> GetAllManufacturesAsync()
        {
            try
            {
                var request = await client.GetAsync("car/GetManufactures");

                return await request.Content.ReadAsAsync<List<Manufacturer>>();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Manufacturer> NewManufactureAsync(Manufacturer manufacturer)
        {
            var myContent = JsonConvert.SerializeObject(manufacturer);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("parked/NewManufacture/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<Manufacturer>();

                }
                catch (Exception)
                {
                    return null;
                }
            }

        }

        public async Task<Model> NewModelAsync(Model model)
        {
            var myContent = JsonConvert.SerializeObject(model);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("parked/NewModel/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<Model>();

                }
                catch (Exception)
                {
                    return null;
                }
            }

        }
        
        public async Task<Car> NewCarAsync(Car car)
        {
            var myContent = JsonConvert.SerializeObject(car);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("parked/NewCar/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<Car>();

                }
                catch (Exception)
                {
                    return null;
                }
            }

        }
        
        public async Task<Core.Models.Type> NewCategoryAsync(Core.Models.Type category)
        {
            var myContent = JsonConvert.SerializeObject(category);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("car/NewCarCategory/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<Core.Models.Type> ();

                }
                catch (Exception)
                {
                    return null;
                }
            }

        }

    }
}
