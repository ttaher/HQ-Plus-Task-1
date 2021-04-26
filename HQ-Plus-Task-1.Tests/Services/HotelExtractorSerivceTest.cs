using HQ_Plus_Task_1.Contracts;
using HQ_Plus_Task_1.Models;
using HQ_Plus_Task_1.Services;
using HtmlAgilityPack;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace HQ_Plus_Task_1.Tests.Services
{
    public class Tests
    {
        private IHotelExtractorSerivce target;
        [SetUp]
        public void Setup()
        {
            target = new HotelExtractorSerivce();
        }

        [Test]
        [TestCase(@"Documents\task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public async Task GetHotelNameAsync(string fileName)
        {

            HtmlDocument doc = await GetHtmlDocument(fileName);
            var actual = target.GetMainHotelName(doc);
            var expected = "Kempinski Hotel Bristol Berlin";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(@"Documents\task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public async Task GetMainHotelAddressAsync(string fileName)
        {

            HtmlDocument doc = await GetHtmlDocument(fileName);
            var actual = target.GetMainHotelAddress(doc);
            var expected = "Kurfürstendamm 27, Charlottenburg-Wilmersdorf, 10719 Berlin, Germany";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(@"Documents\task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public async Task GetMainHotelReviewPointAsync(string fileName)
        {

            HtmlDocument doc = await GetHtmlDocument(fileName);
            var actual = target.GetMainHotelReviewPoint(doc);
            var expected = new ReviewPoint()
            {
                NumberOfReviews = 1401,
                ReviewPointValue = 8.3,
                ReviewStatus = "Very good"
            };
            
            Assert.AreEqual(expected.ReviewPointValue, actual.ReviewPointValue);
            Assert.AreEqual(expected.ReviewStatus, actual.ReviewStatus);
            Assert.AreEqual(expected.NumberOfReviews, actual.NumberOfReviews);
        }

        [Test]
        [TestCase(@"Documents\task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html")]
        public async Task GetMainHotelStarsAsync(string fileName)
        {

            HtmlDocument doc = await GetHtmlDocument(fileName);
            var actual = target.GetMainHotelStars(doc);
            var expected = 5;
            Assert.AreEqual(expected, actual);
        }

        private async Task<HtmlDocument> GetHtmlDocument(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                var doc = new HtmlDocument();
                var content = await reader.ReadToEndAsync();
                doc.LoadHtml(content);
                return doc;
            }
        }
    }
}