using System;
using System.Collections.Generic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries {
    public sealed class VehicleQuery : ObjectGraphType {
        private readonly IAutobarnDatabase db;

        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;

            Field<ListGraphType<VehicleGraphType>>("Vehicles",
                "Return all vehicles in the system",
                resolve: GetAllVehicles);

            Field<VehicleGraphType>("Vehicle",
                "Retrieve a single vehicle based on registration plate",
                new QueryArguments(
                    MakeNonNullStringArgument("registration", "The registration (licence plate) of the Vehicle")),
                resolve: GetVehicle);

            Field<ListGraphType<VehicleGraphType>>("VehiclesByColor",
                "Return all vehicles matching a specified color",
                new QueryArguments(
                    MakeNonNullStringArgument("color", "The color of cars you want")),
                resolve: GetVehiclesByColor);
        }

        private object GetVehiclesByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            return db.ListVehicles().Where(
                v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }

        private Vehicle GetVehicle(IResolveFieldContext<object> context) {
            var registration = context.GetArgument<string>("registration");
            return db.FindVehicle(registration);
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg) => db.ListVehicles();
    }
}
