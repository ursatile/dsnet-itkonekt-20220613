using System;
using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class VehicleGraphType : ObjectGraphType<Vehicle> {
        public VehicleGraphType() {
            Name = "vehicle";
            Field(v => v.Year).Description("What year was this vehicle first registered?");
            Field(v => v.Registration).Description("The registration (licence plate) of this vehicle");
            Field(v => v.Color).Description("What color is this car?");
            Field(v => v.VehicleModel,
                nullable: false,
                type: typeof(ModelGraphType)
            ).Description("What model of vehicle is this?");
        }
    }
}
