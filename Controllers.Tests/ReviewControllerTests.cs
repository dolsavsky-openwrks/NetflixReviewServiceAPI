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
    public class ReviewControllerTests
    {
        private Fixture fixture = new Fixture();
        private Mock<IReviewService> reviewService;
        private Mock<ILogger<ReviewController>> logger;
        private ReviewController reviewController;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            reviewService = new Mock<IReviewService>();
            logger = new Mock<ILogger<ReviewController>>();

            reviewController = new ReviewController(reviewService.Object, logger.Object);
        }

        [TearDown]
        public void TearDown()
        {
            reviewService.Invocations.Clear();
        }

        #region Post

        [TestCase(0)]
        [TestCase(-2)]
        [TestCase(6)]
        public void Post_RatingIsOutOfRange_ResultIsError(int rating)
        {
            //Arrange
            var request = fixture.Build<Review>().With(x => x.Rating, rating).Create();
            var expectedResult = new APIResponse 
            { 
                ResponseStatusCode = ResponseStatusCode.Error, 
                Message = "Rating is out of range." 
            };

            //Act
            var result = reviewController.Post(request);

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<APIResponse>(result, "IsInstanceOf");
                Assert.AreEqual(expectedResult.ResponseStatusCode, result.ResponseStatusCode, "ResponseStatusCode");
                Assert.IsTrue(result.Message.Contains(expectedResult.Message), "Message");
            });

            reviewService.Verify(x => x.Add(It.IsAny<Review>()), Times.Never);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        
        public void Post_InvalidDescription_ResultIsError(string description)
        {
            //Arrange
            var request = fixture.Build<Review>().With(x => x.Description, description).With(x => x.Rating, 2).Create();
            var expectedResult = new APIResponse 
            { 
                ResponseStatusCode = ResponseStatusCode.Error,
                Message = "Description must be between 1 and 255 characters." 
            };

            //Act
            var result = reviewController.Post(request);

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<APIResponse>(result, "IsInstanceOf");
                Assert.AreEqual(expectedResult.ResponseStatusCode, result.ResponseStatusCode, "ResponseStatusCode");
                Assert.IsTrue(result.Message.Contains(expectedResult.Message), "Message");
            });

            reviewService.Verify(x => x.Add(It.IsAny<Review>()), Times.Never);
        }

        [Test]
        public void Post_LongDescription_ResultIsError()
        {
            //Arrange
            var request = fixture.Build<Review>().With(x => x.Description, new string('a', 256)).With(x => x.Rating, 2).Create();
            var expectedResult = new APIResponse
            {
                ResponseStatusCode = ResponseStatusCode.Error,
                Message = "Description must be between 1 and 255 characters."
            };

            //Act
            var result = reviewController.Post(request);

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<APIResponse>(result, "IsInstanceOf");
                Assert.AreEqual(expectedResult.ResponseStatusCode, result.ResponseStatusCode, "ResponseStatusCode");
                Assert.IsTrue(result.Message.Contains(expectedResult.Message), "Message");
            });

            reviewService.Verify(x => x.Add(It.IsAny<Review>()), Times.Never);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Post_ShowIDIsNotValid_ResultIsError(string showID)
        {
            //Arrange
            var request = fixture.Build<Review>().With(x => x.ShowID, showID).With(x => x.Rating, 2).Create();
            var expectedResult = new APIResponse
            {
                ResponseStatusCode = ResponseStatusCode.Error,
                Message = "Show ID is required."
            };

            //Act
            var result = reviewController.Post(request);

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<APIResponse>(result, "IsInstanceOf");
                Assert.AreEqual(expectedResult.ResponseStatusCode, result.ResponseStatusCode, "ResponseStatusCode");
                Assert.IsTrue(result.Message.Contains(expectedResult.Message), "Message");
            });

            reviewService.Verify(x => x.Add(It.IsAny<Review>()), Times.Never);
        }

        [Test]
        public void Post_AddReviewResponseIsFalse_ResultIsError()
        {
            //Arrange
            var request = fixture.Build<Review>().With(x => x.Rating, 3).Create();
            var expectedResult = new APIResponse
            {
                ResponseStatusCode = ResponseStatusCode.Error,
                Message = "Service response is false."
            };

            reviewService.Setup(x => x.Add(It.IsAny<Review>())).Returns(false);

            //Act
            var result = reviewController.Post(request);

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<APIResponse>(result, "IsInstanceOf");
                Assert.AreEqual(expectedResult.ResponseStatusCode, result.ResponseStatusCode, "ResponseStatusCode");
                Assert.IsTrue(result.Message.Contains(expectedResult.Message), "Message");
            });

            reviewService.Verify(x => x.Add(It.IsAny<Review>()), Times.Once);
        }

        [Test]
        public void Post_AddReviewResponseIsTrue_ResultIsSuccess()
        {
            //Arrange
            var request = fixture.Build<Review>().With(x => x.Rating, 3).Create();
            var expectedResult = new APIResponse
            {
                ResponseStatusCode = ResponseStatusCode.Success,
                Message = "Review was added successfully."
            };

            reviewService.Setup(x => x.Add(It.IsAny<Review>())).Returns(true);

            //Act
            var result = reviewController.Post(request);

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<APIResponse>(result, "IsInstanceOf");
                Assert.AreEqual(expectedResult.ResponseStatusCode, result.ResponseStatusCode, "ResponseStatusCode");
                Assert.IsTrue(result.Message.Contains(expectedResult.Message), "Message");
            });

            reviewService.Verify(x => x.Add(It.IsAny<Review>()), Times.Once);
        }

        #endregion
    }
}
