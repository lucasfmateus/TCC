﻿using UI.Service.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Models;

namespace UI.Service.Controller
{
    public class ParkedController : ServiceBase
    {
        public async Task<List<ParkedCar>> GetParkedCarsAsync()
        {
            try
            {
                var request = await client.GetAsync("parked/GetBusySlots");

                return await request.Content.ReadAsAsync<List<ParkedCar>>();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ParkedCar> NewParkedAsync(Car car)
        {
            var myContent = JsonConvert.SerializeObject(car);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("parked/NewParked/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<ParkedCar>();

                }
                catch (Exception)
                {
                    return null;
                }
            }

        }

    }
}
