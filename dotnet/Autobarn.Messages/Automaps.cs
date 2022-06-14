using AutoMapper;

namespace Autobarn.Messages {
    public class Automaps {
        private static readonly Mapper mapper;
        static Automaps() {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<NewVehicleMessage, NewVehiclePriceMessage>());
            mapper = new Mapper(config);
        }

        public static NewVehiclePriceMessage Map(NewVehicleMessage nvm)
            => mapper.Map<NewVehiclePriceMessage>(nvm);
    }
}
