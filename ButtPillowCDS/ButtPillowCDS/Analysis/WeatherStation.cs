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
            SensorDoesNotExist
        }
    

        readonly int _sensorID;
        readonly string _MeterSquareIdentifier;
        readonly int _Easting;
        readonly int _Northing;

        public int SensorID { get { return _sensorID; } }
        public string GridLocation { get { return (AssembleGrid()); } }

        public WeatherStation(int SensorID, string MeterSquareIdentifier, int Easting, int Northing)
        {
            _sensorID = SensorID;
            _MeterSquareIdentifier = MeterSquareIdentifier;
            _Easting = Easting;
            _Northing = Northing;
        }

        private string AssembleGrid()
        {
            return (_MeterSquareIdentifier + " " + _Easting.ToString() + " " + _Northing.ToString());
        }

    }
}
