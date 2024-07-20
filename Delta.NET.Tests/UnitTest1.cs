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
            Assert.AreEqual(Delta.Operations.Count, 1);
        }
        [TestMethod]
        public void OperationDelete()
        {
            var Json = @"{""ops"":[{""insert"":""Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."",""attributes"":{""background"":""#556271"",""color"":""#7b8898""}}]}";
            var Delta = JsonSerializer.Deserialize<Delta>(Json);
            var Delta2 = new Delta { Operations = new() { new DeleteOperation { Value = 6 } } };
            var Result = Delta.Compose(Delta2);
            Assert.AreEqual(Result.Operations.Count,1);
            Assert.AreEqual(Result.Operations.Last().GetLength(),439);
        }
        [TestMethod]
        public void OperationRetainAttribute()
        {
            var Json = @"{""ops"":[{""insert"":""Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."",""attributes"":{""background"":""#556271"",""color"":""#7b8898""}}]}";
            var Delta = JsonSerializer.Deserialize<Delta>(Json);
            var Op = new RetainOperation { Value = 8 };
            Op.AddString("align", "center");
            var Delta2 = new Delta { Operations = new() { new RetainOperation { Value = 18 }, Op} };
            var Result = Delta.Compose(Delta2);
            Assert.AreEqual(Result.Operations.Count, 3);
            Assert.AreEqual(Result.Operations[1].GetLength(), 8);
        }
        [TestMethod]
        public void Combine()
        {
            var D1 = JsonSerializer.Deserialize<Delta>(File.ReadAllText("./Files/json01.json"));
            var D2 = JsonSerializer.Deserialize<Delta>(File.ReadAllText("./Files/json02.json"));
            var D3 = JsonSerializer.Deserialize<Delta>(File.ReadAllText("./Files/json03.json"));
            var DOut = D1.Compose(D2).Compose(D3);
            Assert.AreEqual(DOut.Operations.Count, 2);
            Assert.AreEqual(((InsertDataString)((InsertOperation)DOut.Operations.Last()).Insert).Value, " ce");
        }
        [TestMethod]
        public void CombineDelete()
        {
            var FileName = "delete";
            var D1 = JsonSerializer.Deserialize<Delta>(File.ReadAllText($"./Files/{FileName}01.json"));
            var D2 = JsonSerializer.Deserialize<Delta>(File.ReadAllText($"./Files/{FileName}02.json"));
            var D3 = JsonSerializer.Deserialize<Delta>(File.ReadAllText($"./Files/{FileName}03.json"));
            var DOut = D1.Compose(D2).Compose(D3);
            Assert.AreEqual(DOut.Operations.Count, 2);
            Assert.AreEqual(((DeleteOperation)DOut.Operations.Last()).Value, 29);
        }
    }
}