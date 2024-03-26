using System.Text.Json;

namespace DeltaNET.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BaseJson()
        {
            var Json = @"{""ops"":[{""retain"":1,""attributes"":{""align"":null}}]}";
            var Delta = JsonSerializer.Deserialize<Delta>(Json);
        }
    }
}