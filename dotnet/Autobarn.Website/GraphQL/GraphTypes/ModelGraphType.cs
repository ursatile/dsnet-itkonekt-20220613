using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class ModelGraphType : ObjectGraphType<Model> {
        public ModelGraphType() {
            Name = "vehicle model";
            Description = "The model of vehicle, e.g. Volkswagen Beetle";
            Field(m => m.Name);
            Field(m => m.Code);
            Field(m => m.Manufacturer,
                    nullable: false,
                    type: typeof(ManufacturerGraphType))
                .Description("The company that manufactures this model of vehicle");
        }
    }
}
