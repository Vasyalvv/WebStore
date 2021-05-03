using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Interfaces.TestApi;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiControllerTests
    {
        [TestMethod]
        public void Index_Returns_View_with_Values()
        {
            var expected_values = new string[] { "1", "2", "3" };
            var values_service_moq=new Mock<IValuesService>();
            values_service_moq
                .Setup(service => service.Get())
                .Returns(expected_values);

            var controller = new WebApiController(values_service_moq.Object);

            var result = controller.Index();

            var view_result = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

            Assert.Equal(expected_values.Length, model.Count());

            values_service_moq.Verify(service => service.Get());
            values_service_moq.VerifyNoOtherCalls();
        }

    }
}
