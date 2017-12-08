using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWebApiApp.Controllers;
using MyWebApiApp.Models;
using NSubstitute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace MyWebApiApp.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task GET_action_of_ValuesController_should_return_previously_POSTed_value()
        {
            //Prepariamo il test
            var testValue = "aspitalia";
            //Creiamo al volo un'implementazione di IValuesRepository...
            var mockValuesRepository = Substitute.For<IValuesRepository>();
            //E ora la "configuriamo" impostando solo il comportamento di cui abbiamo bisogno per questo test
            //In particolare, ci serve tenere un riferimento ai valori aggunti
            var valueList = new List<(int Id, string Value)>();
            //E facciamo l'aggiunta quando il ValuesController invoca il metodo Create sull'oggetto IValuesRepository
            mockValuesRepository.When(mock => mock.Create(Arg.Any<string>())).Do(callInfo => valueList.Add((valueList.Count + 1, callInfo.Arg<string>())));
            //La proprietà All del repository restituirà la lista
            mockValuesRepository.GetAll().Returns(Task.FromResult((IEnumerable<(int Id, string Value)>) valueList));
            //Creiamo il controller
            var controller = new ValuesController(mockValuesRepository);

            //Esercitiamo il nostro controller inserendo un valore...
            await controller.Post(testValue);
            //...e receuperando l'elenco dei valori inseriti
            var results = await controller.Get();

            //Verifichiamo che le aspettative siano rispettate
            //L'elenco di risultati deve contenere un solo valore
            Assert.AreEqual(1, results.Count());
            //E tale valore deve essere "aspitalia"
            Assert.AreEqual(testValue, results.First().Value);
            //Verifichiamo che il metodo Create sia stato invocato
            mockValuesRepository.Received().Create(testValue);
        }

        [TestMethod]
        public async Task POST_with_invalid_data_should_return_a_400_bad_request_response()
        {
            var testValue = string.Empty;
            var mockValuesRepository = Substitute.For<IValuesRepository>();
            var controller = new ValuesController(mockValuesRepository);
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext {
                HttpContext = httpContext
            };
            await controller.Post(testValue);

            Assert.AreEqual(400, httpContext.Response.StatusCode);
        }

        [TestMethod]
        [DataRow("", 400)]
        [DataRow(null, 400)]
        [DataRow("  ", 400)]
        [DataRow("Foo", 200)]
        public async Task POST_should_return_an_appropriate_response(string testValue, int expectedStatusCode)
        {
            var mockValuesRepository = Substitute.For<IValuesRepository>();
            var controller = new ValuesController(mockValuesRepository);
            
            //Ricreiamo un nosto HttpContext
            var httpContext = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            await controller.Post(testValue);

            Assert.AreEqual(expectedStatusCode, httpContext.Response.StatusCode);
        }
    }
}