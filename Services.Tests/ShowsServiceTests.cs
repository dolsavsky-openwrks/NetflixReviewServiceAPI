using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using NetflixReviewSericeAPI.Interfaces;
using NetflixReviewSericeAPI.Models;
using NetflixReviewSericeAPI.NetflixAPI;
using NetflixReviewSericeAPI.Services;
using NUnit.Framework;

namespace Services.Tests
{
    [TestFixture]
    public class ShowsServiceTests
    {
        private Fixture fixture = new Fixture();
        private Mock<INetflixAPIService> netflixAPIService;
        private Mock<IReviewService> reviewService;
        private ShowsService showsService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            netflixAPIService = new Mock<INetflixAPIService>();
            reviewService = new Mock<IReviewService>();

            showsService = new ShowsService(netflixAPIService.Object, reviewService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            netflixAPIService.Invocations.Clear();
            reviewService.Invocations.Clear();
        }

        #region MyRegion

        [Test]
        public void GetAll_TokenResponseIsNull_ResultIsNull()
        {
            //Arrange
            netflixAPIService.Setup(x => x.GetToken()).Returns(() => null);

            //Act
            var result = showsService.GetAll();

            //Assert
            Assert.IsNull(result);

            netflixAPIService.Verify(x => x.GetShows(It.IsAny<TokenResponse>()), Times.Never);
            reviewService.Verify(x => x.GetReviews(It.IsAny<IEnumerable<Show>>()), Times.Never);
        }

        [Test]
        public void GetAll_ServiceResponseIsNull_GetReviewsCalledNever()
        {
            //Arrange
            var token = fixture.Create<TokenResponse>();

            netflixAPIService.Setup(x => x.GetToken()).Returns(token);
            netflixAPIService.Setup(x => x.GetShows(It.IsAny<TokenResponse>())).Returns(() => null);

            //Act
            var result = showsService.GetAll();

            //Assert
            Assert.IsNull(result);

            netflixAPIService.Verify(x => x.GetShows(It.IsAny<TokenResponse>()), Times.Once);
            reviewService.Verify(x => x.GetReviews(It.IsAny<IEnumerable<Show>>()), Times.Never);
        }

        [Test]
        public void GetAll_ServiceResponseHasNoData_GetReviewsCalledNever()
        {
            //Arrange
            var token = fixture.Create<TokenResponse>();
            var showsResponse = fixture.Build<ShowsPaginatedResponse>().Without(x => x.Data).Create();

            netflixAPIService.Setup(x => x.GetToken()).Returns(token);
            netflixAPIService.Setup(x => x.GetShows(It.IsAny<TokenResponse>())).Returns(showsResponse);

            //Act
            var result = showsService.GetAll();

            //Assert
            Assert.IsNull(result);

            netflixAPIService.Verify(x => x.GetShows(It.IsAny<TokenResponse>()), Times.Once);
            reviewService.Verify(x => x.GetReviews(It.IsAny<IEnumerable<Show>>()), Times.Never);
        }

        [Test]
        public void GetAll_ServiceResponseHasEmptyList_GetReviewsCalledNever()
        {
            //Arrange
            var token = fixture.Create<TokenResponse>();
            var data = fixture.CreateMany<Show>(0);
            var showsResponse = fixture.Build<ShowsPaginatedResponse>().With(x => x.Data, data).Create();

            netflixAPIService.Setup(x => x.GetToken()).Returns(token);
            netflixAPIService.Setup(x => x.GetShows(It.IsAny<TokenResponse>())).Returns(showsResponse);

            //Act
            var result = showsService.GetAll();

            //Assert
            Assert.IsNull(result);

            netflixAPIService.Verify(x => x.GetShows(It.IsAny<TokenResponse>()), Times.Once);
            reviewService.Verify(x => x.GetReviews(It.IsAny<IEnumerable<Show>>()), Times.Never);
        }

        [Test]
        public void GetAll_ServiceReturnsShows_GetReviewsCalledOnce()
        {
            //Arrange
            var token = fixture.Create<TokenResponse>();
            var data = fixture.Build<Show>().Without(x => x.Reviews).CreateMany();
            var showsResponse = fixture.Build<ShowsPaginatedResponse>().With(x => x.Data, data).Create();
            var showsWithReviews = fixture.CreateMany<Show>();

            netflixAPIService.Setup(x => x.GetToken()).Returns(token);
            netflixAPIService.Setup(x => x.GetShows(It.IsAny<TokenResponse>())).Returns(showsResponse);
            reviewService.Setup(x => x.GetReviews(It.IsAny<IEnumerable<Show>>())).Returns(showsWithReviews);

            //Act
            var result = showsService.GetAll();

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<IEnumerable<Show>>(result, "IsInstanceOf");
                Assert.AreEqual(showsWithReviews.Count(), result.Count(), "Count");
            });

            netflixAPIService.Verify(x => x.GetShows(It.IsAny<TokenResponse>()), Times.Once);
            reviewService.Verify(x => x.GetReviews(It.IsAny<IEnumerable<Show>>()), Times.Once);
        }

        #endregion
    }
}
