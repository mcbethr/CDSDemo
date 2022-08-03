using System;

namespace ButtPillowCDS
{
    public class WeatherUpdate
    {
        public int SensorID { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureF { get; set; }

        public string MeterSquareIdentifier { get; set; }
        public int Easting { get; set; }
        public int Northing { get; set; }
    }
}
