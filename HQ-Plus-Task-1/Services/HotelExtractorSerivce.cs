using HQ_Plus_Task_1.Contracts;
using HQ_Plus_Task_1.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ_Plus_Task_1.Services
{
    public class HotelExtractorSerivce : IHotelExtractorSerivce
    {
        public List<Hotel> GetMainAlternative(HtmlDocument doc)
        {
            List<Hotel> alternativeHotels = new List<Hotel>();
            var altHotelsRow = doc.GetElementbyId("althotelsRow").ChildNodes.Where(x => x.Name == "td").Select(x => x.ChildNodes).ToList();

            foreach (var item in altHotelsRow)
            {
                alternativeHotels.Add(new Hotel()
                {
                    Name = item.Descendants("a").Where(x => x.HasClass("althotel_link")).Select(x => x.InnerHtml.Trim()).FirstOrDefault(),
                    Stars = GetAltHotelStars(item),
                    Description = item.Where(x => x.HasClass("hp_compset_description")).Select(x => x.InnerHtml.Trim()).FirstOrDefault(),
                    NumberOfReview = item.Descendants("strong").Where(x => x.HasClass("count")).Select(x => int.Parse(x.InnerHtml.Trim())).FirstOrDefault(),
                    ReviewPoints = new ReviewPoint
                    {
                        NumberOfReviews = double.Parse(item.Descendants("strong").Where(x => x.HasClass("count")).Select(x => x.InnerHtml.Trim()).FirstOrDefault()),
                        ReviewPointValue = double.Parse(item.Descendants("span").Where(x => x.HasClass("js--hp-scorecard-scoreval")).Select(x => x.InnerHtml).FirstOrDefault()),
                        ReviewStatus = item.Descendants("span").Where(x => x.HasClass("js--hp-scorecard-scoreword")).Select(x => x.InnerHtml.Trim()).FirstOrDefault()
                    }
                });
            }
            return alternativeHotels;
        }
        private int GetAltHotelStars(HtmlNodeCollection item)
        {
            var rating = item.Descendants("span").Where(x => x.HasClass("b-sprite")).Select(x => x.InnerHtml.Split('-')[0]).FirstOrDefault();
            if (int.TryParse(rating, out int starRating))
            {
                return starRating;
            }
            return 5;
        }
        public string GetMainHotelAddress(HtmlDocument doc)
        {
            return doc.GetElementbyId("hp_address_subtitle").InnerText.Trim();
        }

        public string GetMainHotelDescription(HtmlDocument doc)
        {
            var html = doc.GetElementbyId("hotel_main_content");
            var divDescription = html.Descendants("div").Where(x => x.HasClass("hotel_description_wrapper_exp")).FirstOrDefault();
            var pSummary = divDescription.Descendants("p").Select(x => x.InnerHtml);
            StringBuilder st = new StringBuilder();
            foreach (var item in pSummary)
            {
                st.Append(item);
            }
            return st.ToString();
        }

        public string GetMainHotelName(HtmlDocument doc)
        {
            return doc.GetElementbyId("hp_hotel_name").InnerText.Trim();
        }

        public ReviewPoint GetMainHotelReviewPoint(HtmlDocument doc)
        {
            var reviewPoint = new ReviewPoint();
            var html = doc.GetElementbyId("js--hp-gallery-scorecard");
            reviewPoint.ReviewStatus = html.Descendants("span").Where(x => x.HasClass("js--hp-scorecard-scoreword")).Select(x => x.InnerHtml.Trim()).FirstOrDefault();
            reviewPoint.ReviewPointValue = double.Parse(html.Descendants("span").Where(x => x.HasClass("js--hp-scorecard-scoreval")).Select(x => x.InnerHtml).FirstOrDefault());
            reviewPoint.NumberOfReviews = double.Parse(html.Descendants("strong").Select(x => x.InnerHtml.Trim()).FirstOrDefault());

            return reviewPoint;
        }

        public List<RoomCategory> GetMainHotelRoomCategories(HtmlDocument doc)
        {
            List<RoomCategory> roomCategories = new List<RoomCategory>();
            var maxotel_rooms = doc.GetElementbyId("maxotel_rooms");
            var maxotel_roomstbody = maxotel_rooms.Descendants("tbody").FirstOrDefault();
            var roomTypes = maxotel_roomstbody?.Descendants("td").Where(x => x.HasClass("ftd")).Select(x => x.InnerHtml.Trim()).ToList();
            var roomCapacity = maxotel_rooms?.Descendants("td").Where(x => x.HasClass("occ_no_dates")).Select(x => x.Descendants("i")).ToList();
            for (int i = 0; i < roomTypes.Count(); i++)
            {
                roomCategories.Add(new RoomCategory()
                {
                    CategoryName = roomTypes[i],
                    Capacity = roomCapacity[i].ToList()[0].Attributes.SingleOrDefault(x => x.Name == "title").Value
                });
            }

            return roomCategories;
        }

        public double GetMainHotelStars(HtmlDocument doc)
        {
            var starsClassTage = doc.DocumentNode.SelectSingleNode("/html/body/div[1]/div[1]/div[4]/div[1]/div[1]/div[1]/h1/span[2]/span/i").Attributes.Where(x => x.Name == "class").FirstOrDefault().Value;

            if (double.TryParse(starsClassTage.Replace("b-sprite", "").Replace("star_track", "").Replace("ratings_stars_", "").Replace("stars", "").Trim(), out double result))
            {
                return result;
            }
            return 0;
        }
    }
}
