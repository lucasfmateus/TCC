﻿using Classification.Services;
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
            db.InitializeDatabase();
        }

        [Route("Train")]
        [HttpGet]
        public async Task Train()
        {
            await classificationService.TrainAsync();
        }

        [Route("Classificate")]
        [HttpGet]
        public async Task<KeyValuePair<Car, decimal>> GetClassification([FromQuery] string image)
        {
            return await classificationService.ClassificateAsync(image);
        }        
    }
}
