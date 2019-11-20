using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FruitBasketTestApp.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTest
    {
        [TestMethod()]
        public void Index_PopulatesView_ReturnsView()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index(true) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        

        [TestMethod]
        public void Details_PopulatesView_ReturnsView()
        {
            // Arrange
            var controller = new HomeController();
            // Act
            var result = controller.Details(1) as ViewResult;
            // Act
            Assert.IsNotNull(result);


        }

        [TestMethod()]
        public void TestAboutView()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void TestContactView()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}

