using Core.Models;
using Flurl;
using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Parking.API.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            try
            {
                var lablelsDict = new Dictionary<string, string>();

                var cars = db.Cars
                    .Include(x => x.Type)
                    .Include(x => x.Model)
                        .ThenInclude(x => x.Manufacturer)
                    .ToList();

                foreach (var car in cars.OrderBy(x => x.CreateAt))
                {
                    var label = $"{car.Model.Manufacturer.Name}__{car.Model.Name}__{car.Model.Year}__{car.Type.Name}";

                    lablelsDict.Add(label, car.Folder);

                    if (!System.IO.Directory.Exists(car.Folder))
                    {

                    }
                }

                var request = "http://127.0.0.1:5000/"
                    .AppendPathSegment("api/train")
                    .WithHeader("retrain", retrain);

                await request.PostJsonAsync(lablelsDict);

                return lablelsDict;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Classifica uma imagem em um dos carros conhecidos
        /// </summary>
        /// <param name="image">Imagem em base 64</param>
        /// <returns></returns>
        public async Task<ClassificationCar> ClassificateAsync(string image) 
        {
            try
            {
                // Envia a requisição para API da rede neural convolucional
                var request = "http://127.0.0.1:5000"
                    .AppendPathSegment("api/classify")
                    .WithTimeout(TimeSpan.FromSeconds(5));

                var response = await request
                    .PostJsonAsync(image)
                    .ReceiveString();

                string[] separator = { "__" };
                var names = response.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                // Converte os dados obtidos
                var manufacturerName = names[0].ToUpper();
                var modelName = names[1].ToUpper();
                var year = names[2];
                var type = names[3].ToUpper();
                var acc = Decimal.Parse(names[4].Replace(".", ","), new CultureInfo("pt-BR"));

                // Busca pelo respectivo carro no banco de dados
                var car = db.Cars
                    .Include(x => x.Type)
                    .Include(x => x.Model)
                        .ThenInclude(x => x.Manufacturer)
                    .FirstOrDefault(x =>
                        x.Model.Manufacturer.Name.ToUpper().Equals(manufacturerName) &&
                        x.Model.Name.ToUpper().Equals(modelName) &&
                        x.Model.Year.ToString().Equals(year) &&
                        x.Type.Name.Equals(type));

                return new ClassificationCar
                {
                    Car = car,
                    Accuracy = decimal.Round(acc * 100, 2, MidpointRounding.AwayFromZero)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
