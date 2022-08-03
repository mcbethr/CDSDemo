using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ButtPillowCDS.Analysis
{
    public class WeatherStation
    {
        public enum WeatherStationErrorsEnum
        {
            Ok,
            SensorIDTooLarge,
            SensorIDTooSmall,
            TemperatureTooCold,
            TemperatureTooHot,
            EastingGridRangeTooLow,
            EastingGridRangeTooHigh,
            NorthingGridRangeTooLow,
            NorthingGridRangeTooHigh,
            MaxSensorsReached,
            CannotRemoveSensor,
            SensorDoesNotExist,
            SensorDateTimeInFuture
        }
    

        readonly int _sensorID;
        readonly string _MeterSquareIdentifier;
        readonly int _Easting;
        readonly int _Northing;

        List<WeatherUpdate> _WeatherUpdates = new List<WeatherUpdate>();
        

        public int SensorID { get { return _sensorID; } }
        public string GridLocation { get { return (AssembleGrid()); } }

        public List<WeatherUpdate> WeatherUpdates { get {return _WeatherUpdates; } }

        public WeatherStation(int SensorID, string MeterSquareIdentifier, int Easting, int Northing)
        {
            _sensorID = SensorID;
            _MeterSquareIdentifier = MeterSquareIdentifier;
            _Easting = Easting;
            _Northing = Northing;
        }

        public void UpdateWeather(WeatherUpdate WU)
        {
            _WeatherUpdates.Add(WU);
        }

        private string AssembleGrid()
        {
            return (_MeterSquareIdentifier + " " + _Easting.ToString() + " " + _Northing.ToString());
        }

    }
}
