using Core.Models;
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

        public async Task<Car> GetAllCars(string folder)
        {
            try
            {
                var request = await classificationClient.GetAsync("Car/GetModelByName?folder=" + folder);

                return await request.Content.ReadAsAsync<Car>();

            }
            catch (Exception)
            {

                return null;
            }
            finally
            {
                ClassificationClientReset();
            }
        }
    }
}
