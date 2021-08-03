using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Moq;
using NetflixReviewSericeAPI.Models;
using NetflixReviewSericeAPI.Repository;
using NetflixReviewSericeAPI.Services;
using NUnit.Framework;

namespace Services.Tests
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private Fixture fixture = new Fixture();
        private Mock<IReviewRepo> reviewRepo;
        private ReviewService reviewService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            reviewRepo = new Mock<IReviewRepo>();

            reviewService = new ReviewService(reviewRepo.Object);
        }

        [TearDown]
        public void TearDown()
        {
            reviewRepo.Invocations.Clear();
        }

        #region Add

        [Test]
        public void Add_RepoResponseIsTrue_ResultIsTrue()
        {
            //Arrange
            var request = fixture.Create<Review>();

            reviewRepo.Setup(x => x.AddReview(It.Is<Review>(r => 
                r.ShowID == request.ShowID && 
                r.Rating == request.Rating && 
                r.Description == request.Description
            ))).Returns(true);

            //Act
            var result = reviewService.Add(request);

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<bool>(result, "IsInstanceOf");
                Assert.IsTrue(result);
            });
            reviewRepo.Verify(x => x.AddReview(It.IsAny<Review>()), Times.Once);
        }

        [Test]
        public void Add_RepoResponseIsFalse_ResultIsFalse()
        {
            //Arrange
            var request = fixture.Create<Review>();

            reviewRepo.Setup(x => x.AddReview(It.Is<Review>(r =>
                r.ShowID == request.ShowID &&
                r.Rating == request.Rating &&
                r.Description == request.Description
            ))).Returns(false);

            //Act
            var result = reviewService.Add(request);

            //Assert
            Assert.Multiple(() => {
                Assert.IsNotNull(result, "IsNotNull");
                Assert.IsInstanceOf<bool>(result, "IsInstanceOf");
                Assert.IsFalse(result);
            });
            reviewRepo.Verify(x => x.AddReview(It.IsAny<Review>()), Times.Once);
        }

        #endregion

        #region GetReviews

        [Test]
        public void GetReviews_ReviewsResponseIsNull_NoShowHasReview()
        {
            //Arrange
            var request = fixture.Build<Show>().Without(x => x.Reviews).CreateMany();

            reviewRepo.Setup(x => x.GetReviews(It.IsAny<IEnumerable<string>>())).Returns(() => null);

            //Act
            var result = reviewService.GetReviews(request);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IEnumerable<Show>>(result);
                Assert.IsNull(result.ToArray()[0].Reviews);
                Assert.IsNull(result.ToArray()[1].Reviews);
                Assert.IsNull(result.ToArray()[2].Reviews);
            });
        }

        [Test]
        public void GetReviews_ReviewsResponseIsEmpty_NoShowHasReview()
        {
            //Arrange
            var request = fixture.Build<Show>().Without(x => x.Reviews).CreateMany();

            reviewRepo.Setup(x => x.GetReviews(It.IsAny<IEnumerable<string>>())).Returns(
                fixture.CreateMany<ReviewsResult>(0)
            );

            //Act
            var result = reviewService.GetReviews(request);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IEnumerable<Show>>(result);
                Assert.IsNull(result.ToArray()[0].Reviews);
                Assert.IsNull(result.ToArray()[1].Reviews);
                Assert.IsNull(result.ToArray()[2].Reviews);
            });
        }

        [Test]
        public void GetReviews_ReviewReturned_ShowHasReview()
        {
            //Arrange
            var request = fixture.Build<Show>().Without(x => x.Reviews).CreateMany();
            var reviews = fixture.Build<Review>().With(x => x.ShowID, request.First().ID).CreateMany(1).ToList();
            var reviewResult = fixture.Build<ReviewsResult>().With(x => x.ShowID, request.First().ID).With(x => x.Reviews, reviews).CreateMany(1);

            reviewRepo.Setup(x => x.GetReviews(It.IsAny<IEnumerable<string>>())).Returns(reviewResult);

            //Act
            var result = reviewService.GetReviews(request);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IEnumerable<Show>>(result);
                Assert.IsTrue(result.ToArray()[0].Reviews.Count() > 0);
                Assert.IsNull(result.ToArray()[1].Reviews);
                Assert.IsNull(result.ToArray()[2].Reviews);
            });
        }

        #endregion
    }
}
