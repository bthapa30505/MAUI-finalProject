using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MAUI_finalProject.Models
{
    public class LocationInfo
    {

            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }

            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public DateTime Timestamp { get; set; }
        }
    }


