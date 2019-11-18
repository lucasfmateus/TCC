using Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Service.Base;

namespace UI.Service.Controller
{
    public class ClassificationController : ServiceBase
    {
        public async Task<KeyValuePair<Car, decimal>> GetCassification(string image)
        {
            try
            {
                var myContent = JsonConvert.SerializeObject(image);

                using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))
                using (client)
                {
                    try
                    {
                        var request = await classificationClient.PostAsync("Classification/Classificate", stringContent);
                        var result = await request.Content.ReadAsStringAsync();
                        return await request.Content.ReadAsAsync<KeyValuePair<Car, decimal>>();
                    }
                    finally
                    {
                        ParkingClientReset();
                    }
                }



            }
            finally
            {
                ClassificationClientReset();
            }
        }
    }
}
