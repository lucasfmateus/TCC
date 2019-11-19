using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.API.Context;
using Parking.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.API.Controller
{
    [Route("api/[controller]/")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public readonly ParkingContext db;
        public readonly CarService service;

        //Controller: Set das rotas utilizadas para consumo dos metodos
        //Adicionado o contexto que connecta com o Banco de dados 
        //Adicionado O CarService onde eh feito tratamento de alguns dados 

        public CarController(ParkingContext db)
        {
            this.db = db;
            this.service = new CarService(db);
        }


        /// <summary>
        /// Retorna todos os carros cadastrados no banco
        /// </summary>
        /// <returns></returns>
        [Route("GetCars/")]
        [HttpGet]
        public List<Car> GetAllCar()
        {
            var cars = db.Cars.Include(x => x.Model)
                           .Include(x => x.Model.Manufacturer)
                           .Include(x => x.Type)
                           .ToList();
            return cars;
        }

        /// <summary>
        /// Retorna o diretório das imagens de treinamento
        /// </summary>
        /// <returns></returns>
        [Route("GetAdress/")]
        [HttpGet]
        public async Task<List<string>> GetAddressFolder()
        {
            return await service.AddressFolderAsync();
        }

        /// <summary>
        /// Retorna o carro de acordo com o id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("GetCarById/")]
        [HttpGet]
        public Car GetCarById([FromQuery]string id)
        {
            var car = db.Cars.Include(x => x.Model)
               .Include(x => x.Type)
               .Where(x => x.Id == id)
               .FirstOrDefault();

            return car;
        }

        //retorna todos os modelos registrados no banco
        [Route("GetModels/")]
        [HttpGet]
        public List<Model> GetAllModels()
        {
            var models = db.Models.Include(x => x.Manufacturer)
                             .ToList();
            return models;
        }

        //retorna o modelo, sendo passado o nome como parametro 
        [Route("GetModelByName")]
        [HttpGet]
        public Model GetModelsByName([FromQuery]string name)
        {
            var model = db.Models.Include(x => x.Manufacturer)
                             .Where(x => x.Name == name)
                             .FirstOrDefault();
            return model;
        }

        //retorna todas as fabricantes registradas do banco 
        [Route("GetManufactures/")]
        [HttpGet]
        public List<Manufacturer> GetAllManufactures()
        {
            var manufacturers = db.Manufacturers.ToList();
            return manufacturers;
        }

        //retorna todos os tipos registrados do banco 
        [Route("GetTypes/")]
        [HttpGet]
        public List<Core.Models.Type> GetAllTypes()
        {
            var types = db.Types.ToList();
            return types;
        }

        //retorna a fabricante, sendo passado o nome como parametro 
        [Route("GetManufacturesByName")]
        [HttpGet]
        public Manufacturer GetManufacturesByName([FromQuery] string name)
        {
            var manufacturer = db.Manufacturers.Where(x => x.Name == name).FirstOrDefault();

            return manufacturer;
        }

        //retorna o tipo, sendo passado o nome como parametro 
        [Route("GetTypeByName")]
        [HttpGet]
        public Core.Models.Type GetTypeByName([FromQuery] string name)
        {
            var type = db.Types.Where(x => x.Name == name).FirstOrDefault();
            return type;
        }

        //retorna o tipo, sendo passado o nome como parametro 
        [Route("GetTypeById")]
        [HttpGet]
        public Core.Models.Type GetTypeById([FromQuery] string id)
        {
            var type = db.Types.Where(x => x.Id == id).FirstOrDefault();
            return type;
        }

        //retorna o modelo, sendo passado o nome como parametro 
        [Route("GetModelByNameAsync")]
        [HttpGet]
        public Model GetModelById([FromQuery] string name)
        {
            var model = db.Models.Where(x => x.Name == name).FirstOrDefault();

            return model;
        }

        //adiciona uma nova fabricante no banco
        [Route("NewManufacture/")]
        [HttpPost]
        public async Task<bool> AddOrUpdateManufactureAsync([FromBody] Manufacturer manufacturer)
        {
            var result = await service.RegisterOrUpdateNewManufacturer(manufacturer);

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //adiciona um novo modelo carro no banco utilizando uma fabricante ja existente
        [Route("NewModel/")]
        [HttpPost]
        public async Task<bool> AddOrUpdateModelAsync([FromBody] Model model)
        {
            var result = await service.RegisterOrUpdateNewModel(model);

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //adiciona um novo carro no banco utilizando modelos e tipos ja existentes
        [Route("NewCar/")]
        [HttpPost]
        public async Task<bool> AddOrUpdateCarAsync([FromBody] Car car)
        {
            var result = await service.RegisterNewCar(car);

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //adiciona um novo tipo carro no bancos
        [Route("NewCarType/")]
        [HttpPost]
        public async Task<bool> AddOrUpdateTypeAsync([FromBody] Core.Models.Type type)
        {
            var result = await service.RegisterOrUpdateNewType(type);

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Deleta um carro do banco que nao esteja alocado em nenhuma vaga 
        [Route("DeleteCar")]
        [HttpDelete]
        public async Task DeleteCarAsync([FromQuery] string carId)
        {
            try
            {
                var car = db.Set<Car>().Where(x => x.Id == carId).FirstOrDefault();

                if(car == null)
                {
                    throw new Exception("Carro não encontrado no Banco.");
                }
                
                var parkedCar = db.Set<ParkedCar>().Where(x => x.Car.Id == car.Id).AsNoTracking().ToList();

                if (parkedCar != null)
                {
                    throw new Exception("Já possuiu um registro desse carro estacionado.");
                }

                db.Cars.Remove(car);
                
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
            }

        }
        //Deleta um Modelo do Banco que nao tenha nenhum carro utilizando 
        [Route("DeleteModel")]
        [HttpDelete]
        public async Task DeleteModelAsync([FromQuery] string modelId)
        {
            try
            {
                var model = db.Set<Model>().Where(x => x.Id == modelId).FirstOrDefault();

                if (model == null)
                {
                    throw new Exception("Modelo não encontrado no Banco.");
                }

                var cars = db.Set<Car>().Where(x => x.Model.Id == model.Id).AsNoTracking().ToList();

                if(cars != null)
                {
                    throw new Exception("Não foi possivel deletar este modelo. Existe " + cars.Count() + " veículo(s) com esse modelo.");
                }

                db.Models.Remove(model);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }
        //Deleta uma Fabricante do Banco que nao teha nenhum modelo de carro a utilizando
        [Route("DeleteManufacture")]
        [HttpDelete]
        public async Task DeleteManufactureAsync([FromQuery] string manufactureId)
        {
            try
            {
                var manufacturer = db.Set<Manufacturer>().Where(x => x.Id == manufactureId).FirstOrDefault();

                if (manufacturer == null)
                {
                    throw new Exception("Fabricante não encontrada no Banco.");
                }

                var models = db.Set<Model>().Where(x => x.Manufacturer.Id == manufactureId).AsNoTracking().ToList();

                if (models != null)
                {
                    throw new Exception("Não foi possivel deletar esta fabricante. Existe " + models.Count() + " modelo(s) com essa fabricante.");
                }

                db.Manufacturers.Remove(manufacturer);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }

        //Deleta um tipo já cadastrado no banco de dados que nao tenha nenhum carro e slot com o mesmo cadastrado.
        [Route("DeleteType")]
        [HttpDelete]
        public async Task DeleteTypeAsync([FromQuery] string typeId)
        {
            try
            {
                var type = db.Set<Core.Models.Type>().Where(x => x.Id == typeId).FirstOrDefault();

                if (type == null)
                {
                    throw new Exception("Tipo não encontrado no Banco.");
                }

                var cars = db.Set<Car>().Where(x => x.Type.Id == type.Id).AsNoTracking().ToList();

                if (cars != null)
                {
                    throw new Exception("Não foi possivel deletar este tipo. Existe " + cars.Count() + " veículo(s) com esse tipo.");
                }

                var slots = db.Set<SlotType>().Where(x => x.TypeId == typeId).AsNoTracking().ToList();

                if (slots != null)
                {
                    throw new Exception("Não foi possivel deletar este tipo. Existe " + cars.Count() + " Slots(s) com esse tipo.");
                }

                db.Types.Remove(type);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }

    }
}
