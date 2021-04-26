using HQ_Plus_Task_1.Contracts;
using HQ_Plus_Task_1.Controllers;
using HQ_Plus_Task_1.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ_Plus_Task_1.Tests.Controllers
{
    public class HotelExtractorControllerTests
    {
        private Mock<IHotelExtractorSerivce> mockHotelExtractorSerivce;
        private HotelExtractorController controller;

        [SetUp]
        public void Setup()
        {
            mockHotelExtractorSerivce = new Mock<IHotelExtractorSerivce>();
            mockHotelExtractorSerivce.Setup(x => x.GetMainHotelName(It.IsAny<HtmlDocument>())).Returns("Kempinski Hotel Bristol Berlin");
            controller = new HotelExtractorController(mockHotelExtractorSerivce.Object);
        }

        [Test]
        public async Task GetHotemNameBySendingFile()
        {
            this.controller.ControllerContext = GetTargetFile();
            string content = "";
            var hotel = new Hotel() { Name = "Kempinski Hotel Bristol Berlin" };
            var actionResult = await controller.PostContent(content);

            var response = actionResult as OkObjectResult;
            var actual = response.Value as Hotel;
            Assert.IsNotNull(response);

            Assert.AreEqual(hotel.Name, actual.Name);
        }

        [Test]
        public async Task GetHotemNameBySendingContent()
        {
            this.controller.ControllerContext = GetTargetFile();
            string content = GetHtmlDocument();
            var hotel = new Hotel() { Name = "Kempinski Hotel Bristol Berlin" };
            var actionResult = await controller.PostContent(content);

            var response = actionResult as OkObjectResult;
            var actual = response.Value as Hotel;
            Assert.IsNotNull(response);

            Assert.AreEqual(hotel.Name, actual.Name);
        }

        public ControllerContext GetTargetFile()
        {
            var routeData = new Microsoft.AspNetCore.Routing.RouteData();
            var action = new ControllerActionDescriptor();
            action.ActionName = "PostContent";
            var fileBytes = Encoding.UTF8.GetBytes(GetHtmlDocument());
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var file = new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, "", @"Documents\task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
            var actx = new ActionContext(httpContext, routeData, action);
            return new ControllerContext(actx);
        }
        public ControllerContext GetTargetContent()
        {
            var routeData = new Microsoft.AspNetCore.Routing.RouteData();
            var action = new ControllerActionDescriptor();
            action.ActionName = "PostContent";
            var fileBytes = Encoding.UTF8.GetBytes(GetHtmlDocument());
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            var actx = new ActionContext(httpContext, routeData, action);
            return new ControllerContext(actx);
        }
        private string GetHtmlDocument()
        {
            using (StreamReader reader = new StreamReader(@"Documents\task 1 - Kempinski Hotel Bristol Berlin, Germany - Booking.com.html"))
            {
                var content = reader.ReadToEnd();
                return content;
            }
        }
    }
}
