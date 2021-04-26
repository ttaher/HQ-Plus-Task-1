using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQ_Plus_Task_1.Models
{
    public class Hotel
    {
        public string Name;
        
        public string Address { get; set; }

        public double Stars { get; set; }

        public ReviewPoint ReviewPoints { get; set; }

        public int NumberOfReview { get; set; }

        public string Description { get; set; }
        public ICollection<RoomCategory> RoomCategories { get; set; }
        public List<Hotel> Alternativehotels { get; set; }

    }
}
