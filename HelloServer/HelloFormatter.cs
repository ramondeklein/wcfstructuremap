namespace HelloServer
{
    // ReSharper disable once UnusedMember.Global
    public class HelloFormatter : IHelloFormatter
    {
        public string Format(string input) => $"Hello {input}";
    }
}