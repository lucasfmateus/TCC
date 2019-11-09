using Classification.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parking.API.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classification.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ClassificationController : ControllerBase
    {
        public readonly ParkingContext db;
        public readonly ClassificationService classificationService;

        public ClassificationController(ParkingContext db)
        {
            this.db = db;
            this.classificationService = new ClassificationService(db);
        }

        [Route("Train/")]
        [HttpGet]
        public async void Train()
        {
            classificationService.TrainAsync();
        }

        [Route("Classificate/")]
        [HttpGet]
        public async Task<Car> GetAllCars([FromQuery] string folder)
        {
            return await classificationService.ClassificateAsync(folder);
        }        
    }
}
