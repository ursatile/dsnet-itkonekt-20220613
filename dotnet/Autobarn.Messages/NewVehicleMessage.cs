using System;

namespace Autobarn.Messages {
    public class NewVehicleMessage {
        public string Registration { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelName { get; set; }
        public DateTimeOffset ListedAt { get; set; }

        public override string ToString() {
            return $"{Registration},{Year},{Color},{ManufacturerName},{ModelName},{ListedAt:O}";
        }

        public NewVehiclePriceMessage WithPrice(int price, string currencyCode) {
            var result = Automaps.Map(this);
            result.Price = price;
            result.CurrencyCode = currencyCode;
            return result;
        }
    }
}
