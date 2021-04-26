using HQ_Plus_Task_1.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ_Plus_Task_1.Contracts
{
    public interface IHotelExtractorSerivce 
    {
        string GetMainHotelName(HtmlDocument doc);
        string GetMainHotelAddress(HtmlDocument doc);
        double GetMainHotelStars(HtmlDocument doc);
        ReviewPoint GetMainHotelReviewPoint(HtmlDocument doc);
        string GetMainHotelDescription(HtmlDocument doc);
        List<RoomCategory> GetMainHotelRoomCategories(HtmlDocument doc);
        List<Hotel> GetMainAlternative(HtmlDocument doc);
    }
}
