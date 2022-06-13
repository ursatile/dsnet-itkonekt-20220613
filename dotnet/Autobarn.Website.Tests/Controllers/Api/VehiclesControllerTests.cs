using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Controllers.api;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace Autobarn.Website.Tests.Controllers.Api {
    public class VehiclesControllerTests {

        [Fact]
        public async Task Vehicles_GET_Returns_OK() {
            var db = new FakeDatabase();
            var c = new VehiclesController(db);
            var result = c.Get();
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Vehicles_GET_Returns_Vehicles() {
            var db = new FakeDatabase();
            var c = new VehiclesController(db);
            var okResult = c.Get() as OkObjectResult;
            var hal = okResult.Value;
            var type = hal.GetType();
            var links = type.GetProperty("_links").GetValue(hal) as IDictionary<string,object>;
            var linkType = links["next"].GetType();
            var href = linkType.GetProperty("href").GetValue(links["next"]) as string;
            href.ShouldStartWith("/api/vehicles");
        }



        public class FakeDatabase : IAutobarnDatabase {
            private const int VEHICLE_COUNT = 20;
            public int CountVehicles() => VEHICLE_COUNT;

            public IEnumerable<Vehicle> ListVehicles() {
                for (var i = 0; i < VEHICLE_COUNT; i++) {
                    yield return new Vehicle { Registration = $"TEST{i}" };
                }
            }

            public IEnumerable<Manufacturer> ListManufacturers() {
                throw new System.NotImplementedException();
            }

            public IEnumerable<Model> ListModels() {
                throw new System.NotImplementedException();
            }

            public Vehicle FindVehicle(string registration) {
                throw new System.NotImplementedException();
            }

            public Model FindModel(string code) {
                throw new System.NotImplementedException();
            }

            public Manufacturer FindManufacturer(string code) {
                throw new System.NotImplementedException();
            }

            public void CreateVehicle(Vehicle vehicle) {
                throw new System.NotImplementedException();
            }

            public void UpdateVehicle(Vehicle vehicle) {
                throw new System.NotImplementedException();
            }

            public void DeleteVehicle(Vehicle vehicle) {
                throw new System.NotImplementedException();
            }
        }
    }
}
