using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieManagementDataAccess.Models
{
    public class Movie
    {
        public int id { set; get; }
        public string title { set; get; }
        public int yearOfRelease { set; get; }

        //running time in hours
        public double runningTime { set; get; }
        public string genres { set; get; }

        public double UsereRatings { set; get; }
    }
     
    public class Users
    {
        public int id { get; set; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string Adddress { set; get; }
    }
    public class UserRatings
    {
        public int id { set; get; }
        public int UserID { set; get; }
        public int MovieID { set; get; }
        public double Rating { set; get; }
    }
    public class FilterCriteria
    {
        
        public string title { set; get; }
        public int yearOfRelease { set; get; }
        public string genres { set; get; }

    }
}
