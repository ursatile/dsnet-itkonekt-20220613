namespace Messages;
public class Greeting {
    public string Name { get; set; } = "World";
    public int Number { get; set; }
    public string MachineName { get; set; } = Environment.MachineName;
    public DateTimeOffset SentAt { get; set; } = DateTimeOffset.UtcNow;
}
