using System;
using Xunit;
using ASP.Web.Controllers;
using Moq;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using ASP.Web.Web.Configuration;
using Microsoft.AspNetCore.Http;
using ASP.Tests.TestUtilities;
using ASC.Utilities;

namespace ASP.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<HttpContext> mockHttpContext;
        private readonly Mock<IOptions<ApplicationSettings>> optionsMock;
        public HomeControllerTests()
        {
            // Create an instance of Mock IOptions
            optionsMock = new Mock<IOptions<ApplicationSettings>>();
            mockHttpContext = new Mock<HttpContext>();
            // Set FakeSession to HttpContext Session.
            mockHttpContext.Setup(p => p.Session).Returns(new FakeSession());
            // Set IOptions<> Values property to return ApplicationSettings object
            optionsMock.Setup(ap => ap.Value).Returns(new ApplicationSettings
            {
                ApplicationTitle = "ASC"
            });
        }
        [Fact]
        public void HomeController_Index_View_Test()
        {
            // Home controller instantiated with Mock IOptions<> object
            var controller = new HomeController(optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            controller.Index();
            Assert.NotNull(controller.HttpContext.Session.GetSession<ApplicationSettings>("Test"));

        }
        [Fact]
        public void HomeController_Index_NoModel_Test() {
            var controller = new HomeController(optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            // Assert Model for Null
            Assert.Null((controller.Index() as ViewResult).ViewData.Model);
        }
        [Fact]
        public void HomeController_Index_Validation_Test() {
            var controller = new HomeController(optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            // Assert ModelState Error Count to 0
            Assert.Equal(0, (controller.Index() as ViewResult).ViewData.ModelState.ErrorCount);
        }
    }
    }

