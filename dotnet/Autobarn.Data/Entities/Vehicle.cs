using Newtonsoft.Json;

#nullable disable

namespace Autobarn.Data.Entities {
    public partial class Vehicle {
        private Model vehicleModel;
        public string Registration { get; set; }
        [JsonIgnore]
        public string ModelCode { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }

        [JsonIgnore]
        public virtual Model VehicleModel {
            get => vehicleModel;
            set {
                vehicleModel = value;
                ModelCode = value.Code;
            }
        }
    }
}
