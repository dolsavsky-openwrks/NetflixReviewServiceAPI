using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NetflixReviewSericeAPI.Controllers;
using NetflixReviewSericeAPI.Interfaces;
using NetflixReviewSericeAPI.Models;
using NUnit.Framework;

namespace Controllers.Tests
{
    [TestFixture]
    public class ShowsControllerTests
    {
        private Fixture fixture = new Fixture();
        private Mock<IShowsService> showsService;
        private Mock<ILogger<ShowsController>> logger;
        private ShowsController showsController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            showsService = new Mock<IShowsService>();
            logger = new Mock<ILogger<ShowsController>>();

            showsController = new ShowsController(logger.Object, showsService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            showsService.Invocations.Clear();
        }

        #region Get

        [Test]
        public void Get_ShowsServiceReturnsNull_ResultIsError()
        {
            //Arrange
            var expectedResult = new APIResponse { ResponseStatusCode = ResponseStatusCode.Error, Message = "Failed to get shows!" };
            showsService.Setup(x => x.GetAll()).Returns(() => null);

            //Act
            var result = showsController.Get();

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<APIResponse>(result, "IsInstanceOf");
                Assert.AreEqual(expectedResult.ResponseStatusCode, result.ResponseStatusCode, "ResponseStatusCode");
                Assert.IsTrue(result.Message.Contains(expectedResult.Message), "Message");
            });
        }

        [Test]
        public void Get_ShowsServiceReturnsData_ResultIsSuccess()
        {
            //Arrange
            var shows = fixture.CreateMany<Show>();
            var expectedResult = new APIResponse { ResponseStatusCode = ResponseStatusCode.Success };

            showsService.Setup(x => x.GetAll()).Returns(shows);

            //Act
            var result = showsController.Get();

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<APIResponse>(result, "IsInstanceOf");
                Assert.AreEqual(expectedResult.ResponseStatusCode, result.ResponseStatusCode, "ResponseStatusCode");
                Assert.AreEqual(shows.Count(), ((IEnumerable<Show>)result.Data).Count(), "Shows count");
            });
        }

        #endregion
    }
}
