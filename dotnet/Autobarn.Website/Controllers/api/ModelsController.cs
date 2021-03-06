using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Autobarn.Messages;
using Autobarn.Website.Models;
using EasyNetQ;

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase {
        private readonly IAutobarnDatabase db;
        private readonly IPubSub pubSub;

        public ModelsController(IAutobarnDatabase db, IPubSub pubSub) {
            this.db = db;
            this.pubSub = pubSub;
        }

        [HttpGet]
        public IActionResult Get() {
            var models = db.ListModels().Select(m => m.ToResource());
            return Ok(models);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound();
            var resource = vehicleModel.ToResource();
            resource._actions = new {
                create = new {
                    href = $"/api/models/{id}",
                    method = "POST",
                    type = "application/json",
                    name = $"Create a new {vehicleModel.Manufacturer.Name} {vehicleModel.Name}"
                }
            };
            return Ok(resource);
        }


        // POST api/vehicles
        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id, [FromBody] VehicleDto dto) {
            var existing = db.FindVehicle(dto.Registration);
            if (existing != default) {
                return Conflict($"Sorry, you can't sell the same car twice and {dto.Registration} is already in our database");
            }
            var vehicleModel = db.FindModel(id);
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel,
            };
            db.CreateVehicle(vehicle);
            await PublishNewVehicleNotification(vehicle);
            return Created($"/api/vehicles/{vehicle.Registration}",
                vehicle.ToResource());
        }

        public async Task PublishNewVehicleNotification(Vehicle vehicle) {
            var message = new NewVehicleMessage {
                Registration = vehicle.Registration,
                Color = vehicle.Color,
                Year = vehicle.Year,
                ManufacturerName = vehicle.VehicleModel.Manufacturer.Name,
                ModelName = vehicle.VehicleModel.Name,
                ListedAt = DateTimeOffset.UtcNow
            };
            await pubSub.PublishAsync(message);
        }
    }
}