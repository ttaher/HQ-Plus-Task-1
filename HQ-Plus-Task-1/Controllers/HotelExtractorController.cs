using HQ_Plus_Task_1.Contracts;
using HQ_Plus_Task_1.Helpers;
using HQ_Plus_Task_1.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HQ_Plus_Task_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelExtractorController : ControllerBase
    {
        private readonly IHotelExtractorSerivce _hotelService;

        public HotelExtractorController(IHotelExtractorSerivce hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpPost]
        public async Task<IActionResult> PostContent([FromForm] string content)
        {


            if (!Request.ContentType.Contains("multipart/form-data") && string.IsNullOrEmpty(content))
            {
                return BadRequest("No Content");
            }
            if (!string.IsNullOrEmpty(content) && !string.IsNullOrWhiteSpace(content))
            {
                return GetHtmlDocument(content);
            }
            else
            if (Request.ContentType.Contains("multipart/form-data"))
            {
                var file = Request.Form.Files[0];
                var result = new StringBuilder();
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    content = reader.ReadToEnd();
                }

                return GetHtmlDocument(content);
            }
            else { return BadRequest("No Content"); }
        }

        #region private

        private IActionResult GetHtmlDocument(string content)
        {
            var hotel = new Hotel();
            HtmlDocument doc = new HtmlDocument();
            if (content is null)
            {
                return BadRequest("No Content");
            }

            if (content.Length == 0)
            {
                return BadRequest("Content is Empty");
            }

            //if (!ValidateContentExtension.ValidateContent(content))
            //{
            //    return BadRequest("Not a valid Content");
            //}
            doc.LoadHtml(content);


            hotel.Name = _hotelService.GetMainHotelName(doc);
            hotel.Address = _hotelService.GetMainHotelAddress(doc);
            hotel.Stars = _hotelService.GetMainHotelStars(doc);
            hotel.ReviewPoints = _hotelService.GetMainHotelReviewPoint(doc);
            hotel.Description = _hotelService.GetMainHotelDescription(doc);
            hotel.RoomCategories = _hotelService.GetMainHotelRoomCategories(doc);
            hotel.Alternativehotels = _hotelService.GetMainAlternative(doc);

            return Ok(hotel);
        }
        #endregion
    }
}