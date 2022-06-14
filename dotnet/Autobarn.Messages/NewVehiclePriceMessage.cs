namespace Autobarn.Messages {
    public class NewVehiclePriceMessage : NewVehicleMessage {
        public int Price { get; set; }
        public string CurrencyCode { get; set; }
        public override string ToString() => $"{base.ToString()} ({Price} {CurrencyCode})";
    }
}
