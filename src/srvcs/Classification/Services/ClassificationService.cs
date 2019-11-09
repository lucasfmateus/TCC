using Core.Models;
using Flurl;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Parking.API.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Classification.Services
{
    public class ClassificationService
    {
        public readonly ParkingContext db;

        public ClassificationService(ParkingContext db)
        {
            this.db = db;
        }

        /// <summary>
        /// Gera as labels a partir do banco de dados e envia para o treinamento
        /// </summary>
        public async Task<object> TrainAsync(bool retrain = false)
        {
            var lablelsDict = new Dictionary<string, string>();

            var cars = db.Cars
                .Include(x => x.Model)
                    .ThenInclude(x => x.Manufacturer)
                .ToList();

            foreach (var car in cars.OrderBy(x => x.CreateAt))
            {
                var label = $"{car.Model.Manufacturer.Name}__{car.Model.Name}__{car.Model.Year}__{car.Type.Name}";

                lablelsDict.Add(label, car.ImagesDirectory);
            }

            var request = "http://127.0.0.1:5000/"
                .AppendPathSegment("api/train")
                .WithHeader("retrain", retrain);

            await request.PostJsonAsync(lablelsDict);

            return lablelsDict;
        }

        public async Task<Car> ClassificateAsync(string imageDirectory)
        {
            try
            {
                var request = "http://127.0.0.1:5000/"
                    .AppendPathSegment("api/classify")
                    .WithTimeout(TimeSpan.FromSeconds(5));

                var response = await request
                    .PostMultipartAsync(mp => mp
                        .AddFile("FilePath", imageDirectory));

                var result = JsonConvert.DeserializeObject<KeyValuePair<string, decimal>>(await response.Content.ReadAsStringAsync());

                string[] separator = { "__" };
                var names = result.Key.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                var manufacturerName = names[0].ToUpper();
                var modelName = names[1].ToUpper();
                var year = names[2];
                var type = names[3].ToUpper();

                var car = db.Cars.FirstOrDefault(x => 
                x.Model.Manufacturer.Name.ToUpper().Equals(manufacturerName) &&
                x.Model.Name.ToUpper().Equals(modelName) &&
                x.Model.Year.ToString().Equals(year) &&
                x.Type.Name.Equals(type));

                if(car == null)
                {
                    throw new Exception("Veículo não encontrado na base de dados do sistema.");
                }
                else if(result.Value < 70)
                {
                    throw new Exception($"Veículo não reconhecido (certeza: {result.Value}).");
                }

                return car;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
