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


        //retorna todos os carros registrados
        [Route("GetCars/")]
        [HttpGet]
        public List<Car> GetAllCar()
        {
            var p = db.Cars.Include(x => x.Model)
                           .Include(x => x.Model.Manufacturer)
                           .Include(x => x.Type)
                           .ToList();
            return p;
        }


        [Route("GetAdress/")]
        [HttpGet]
        public async Task<List<string>> GetAddressFolder()
        {
            return await service.AddressFolderAsync();
        }

        //retorna o carro, sendo passado o id como parametro 
        [Route("GetCarById/")]
        [HttpGet]
        public Car GetCarById([FromQuery]string id)
        {
            var p = db.Cars.Include(x => x.Model)
               .Include(x => x.Type)
               .Where(x => x.Id == id)
               .FirstOrDefault();

            return p;
        }

        //retorna todos os modelos registrados no banco
        [Route("GetModels/")]
        [HttpGet]
        public List<Model> GetAllModels()
        {
            var p = db.Models.Include(x => x.Manufacturer)
                             .ToList();
            return p;
        }

        //retorna o modelo, sendo passado o nome como parametro 
        [Route("GetModelByName")]
        [HttpGet]
        public Model GetModelsByName([FromQuery]string name)
        {
            var p = db.Models.Include(x => x.Manufacturer)
                             .Where(x => x.Name == name)
                             .FirstOrDefault();
            return p;
        }

        //retorna todas as fabricantes registradas do banco 
        [Route("GetManufactures/")]
        [HttpGet]
        public List<Manufacturer> GetAllManufactures()
        {
            var p = db.Manufacturers.ToList();
            return p;
        }

        //retorna todos os tipos registrados do banco 
        [Route("GetTypes/")]
        [HttpGet]
        public List<Core.Models.Type> GetAllTypes()
        {
            var p = db.Types.ToList();
            return p;
        }

        //retorna a fabricante, sendo passado o nome como parametro 
        [Route("GetManufacturesByName")]
        [HttpGet]
        public Manufacturer GetManufacturesByName([FromQuery] string name)
        {
            var p = db.Manufacturers.Where(x => x.Name == name).FirstOrDefault();
            return p;
        }

        //retorna o tipo, sendo passado o nome como parametro 
        [Route("GetTypeByName")]
        [HttpGet]
        public Core.Models.Type GetTypeByName([FromQuery] string name)
        {
            var p = db.Types.Where(x => x.Name == name).FirstOrDefault();
            return p;
        }

        //retorna o modelo, sendo passado o nome como parametro 
        [Route("GetModelByNameAsync")]
        [HttpGet]
        public Model GetModelById([FromQuery] string name)
        {
            var p = db.Models.Where(x => x.Name == name).FirstOrDefault();
            return p;
        }

        //adiciona uma nova fabricante no banco
        [Route("NewManufacture/")]
        [HttpPost]
        public async Task<bool> AsyncAddManufacture([FromBody] Manufacturer manufacturer)
        {
            try
            {
                db.Manufacturers.Add(manufacturer);
                await db.SaveChangesAsync();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        //adiciona um novo modelo carro no banco utilizando uma fabricante ja existente
        [Route("NewModel/")]
        [HttpPost]
        public async Task<bool> AsyncAddModel([FromBody] Model model)
        {
            var x = await service.RegisterNewModel(model);
            if (x)
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
        public async Task<bool> AsyncAddCar([FromBody] Car car)
        {
            var x = await service.RegisterNewCar(car);
            if (x)
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
        public async Task<bool> AsyncAddType([FromBody] Core.Models.Type type)
        {
            var x = await service.RegisterNewType(type);
            if (x)
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
                var c = db.Set<Car>().Where(x => x.Id == carId).FirstOrDefault();

                if(c == null)
                {
                    throw new Exception("Carro não encontrado no Banco.");
                }
                
                var p = db.Set<ParkedCar>().Where(x => x.Car.Id == c.Id).AsNoTracking().ToList();

                if (p != null)
                {
                    throw new Exception("Já possuiu um registro desse carro estacionado.");
                }

                db.Cars.Remove(c);
                
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
                var p = db.Set<Model>().Where(x => x.Id == modelId).FirstOrDefault();

                if (p == null)
                {
                    throw new Exception("Modelo não encontrado no Banco.");
                }

                var c = db.Set<Car>().Where(x => x.Model.Id == p.Id).AsNoTracking().ToList();

                if(c != null)
                {
                    throw new Exception("Não foi possivel deletar este modelo. Existe " + c.Count() + " veículo(s) com esse modelo.");
                }

                db.Models.Remove(p);
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
                var p = db.Set<Manufacturer>().Where(x => x.Id == manufactureId).FirstOrDefault();

                if (p == null)
                {
                    throw new Exception("Fabricante não encontrada no Banco.");
                }

                var m = db.Set<Model>().Where(x => x.Manufacturer.Id == manufactureId).AsNoTracking().ToList();

                if (m != null)
                {
                    throw new Exception("Não foi possivel deletar esta fabricante. Existe " + m.Count() + " modelo(s) com essa fabricante.");
                }
                db.Manufacturers.Remove(p);
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
                var p = db.Set<Core.Models.Type>().Where(x => x.Id == typeId).FirstOrDefault();

                if (p == null)
                {
                    throw new Exception("Tipo não encontrado no Banco.");
                }

                var c = db.Set<Car>().Where(x => x.Type.Id == p.Id).AsNoTracking().ToList();

                if (c != null)
                {
                    throw new Exception("Não foi possivel deletar este tipo. Existe " + c.Count() + " veículo(s) com esse tipo.");
                }

                var s = db.Set<SlotType>().Where(x => x.TypeId == typeId).AsNoTracking().ToList();

                if (s != null)
                {
                    throw new Exception("Não foi possivel deletar este tipo. Existe " + c.Count() + " Slots(s) com esse tipo.");
                }

                db.Types.Remove(p);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }

    }
}
