using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyWebApiApp.IntegrationTests
{
    [TestClass]
    [TestCategory("Integration")]
    public class IntegrationTest1
    {
        private const string webApiBaseUrl = "http://webapi/api";
        [TestMethod]
        public async Task ShouldRetrieveValuesThatWereInsertedBefore()
        {
            using (var client = new HttpClient())
            {
                var expectedValue = "aspitalia";
                var valuesEndpoint = $"{webApiBaseUrl}/values";

                //Inviamo vere e proprie richieste HTTP per inserire un valore e recuperare l'elenco dei valori esistenti
                //Trattandosi di un integration test, il valore verrà scritto dalla WebAPI nel database e recuperato da lì
                await client.PostAsync(valuesEndpoint, new StringContent(JsonConvert.SerializeObject(expectedValue), Encoding.UTF8, "application/json"));
                var response = await client.GetStringAsync(valuesEndpoint);
                var values = JsonConvert.DeserializeObject(response) as JArray;

                Assert.IsNotNull(values);
                Assert.AreEqual(1, values.Count);
                Assert.AreEqual(expectedValue, values[0].Value<string>("value"));
            }
        }
    }
}