using System.Drawing;
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
        [TestMethod]
        public void OperationDelete()
        {
            var Json = @"{""ops"":[{""insert"":""Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."",""attributes"":{""background"":""#556271"",""color"":""#7b8898""}}]}";
            var Delta = JsonSerializer.Deserialize<Delta>(Json);
            var Delta2 = new Delta { Operations = new() { new DeleteOperation { Value = 1 } } };
            var Result = Delta.Compose(Delta2);
        }
        [TestMethod]
        public void OperationRetainAttribute()
        {
            var Json = @"{""ops"":[{""insert"":""Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."",""attributes"":{""background"":""#556271"",""color"":""#7b8898""}}]}";
            var Delta = JsonSerializer.Deserialize<Delta>(Json);
            var Op = new RetainOperation { Value = 1 };
            Op.AddString("align", "center");
            var Delta2 = new Delta { Operations = new() { new RetainOperation { Value = 16 }, Op, new RetainOperation { Value = 16 } } };
            var Result = Delta.Compose(Delta2);
        }
        [TestMethod]
        public void Combine()
        {
            var D1 = JsonSerializer.Deserialize<Delta>(File.ReadAllText("./Files/json01.json"));
            var D2 = JsonSerializer.Deserialize<Delta>(File.ReadAllText("./Files/json02.json"));
            var D3 = D1.Compose(D2);
        }
    }
}