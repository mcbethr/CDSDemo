using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ButtPillowCDS.Analysis.WeatherStation;


namespace ButtPillowCDS.Analysis
{
    public static class WeatherStationManager
    {
        static List<WeatherStation> _WeatherStations = new List<WeatherStation>();

        public static List<WeatherStation> WeatherStations {get { return _WeatherStations;} }

        

        public static (bool,List<WeatherStationErrorsEnum>) AddWeatherStation(string MeterSquareIdentifier, int Easting, int Northing)
        {
            //First check to make sure we are creating a valid weather station
            //if everything is valid, return success.

            List<WeatherStationErrorsEnum> WeatherStationErrors = new List<WeatherStationErrorsEnum>();
            bool Success = false;

            if (_WeatherStations.Count == WeatherSensorParameters.SensorIdMax)
            {
                WeatherStationErrors.Add(WeatherStationErrorsEnum.MaxSensorsReached);
            }

            if (Easting < WeatherSensorParameters.LowestGrid)
            {
                WeatherStationErrors.Add(WeatherStationErrorsEnum.EastingGridRangeTooLow);
            }

            if (Easting > WeatherSensorParameters.HighestGrid)
            {
                WeatherStationErrors.Add(WeatherStationErrorsEnum.EastingGridRangeTooHigh);
            }

            if (Northing < WeatherSensorParameters.LowestGrid)
            {
                WeatherStationErrors.Add(WeatherStationErrorsEnum.NorthingGridRangeTooLow);
            }

            if (Northing > WeatherSensorParameters.HighestGrid)
            {
                WeatherStationErrors.Add(WeatherStationErrorsEnum.NorthingGridRangeTooHigh);
            }

            //if we reached here with no errors, we are good.  Create the Weather station
            if (WeatherStationErrors.Count == 0)
            {

                WeatherStation NewWeatherStation = ConstructWeatherStation(MeterSquareIdentifier, Easting, Northing);
                _WeatherStations.Add(NewWeatherStation);
                Success = true;

            }


            return (Success, WeatherStationErrors);
        }

        public static void ClearWeatherStations()
        {
            _WeatherStations.Clear();
        }

        public static (bool, List<WeatherStationErrorsEnum>) RemoveWeatherStation (int SensorID)
        {
            List<WeatherStationErrorsEnum> Errors = new List<WeatherStationErrorsEnum>();

            int Success = _WeatherStations.RemoveAll(x => x.SensorID == SensorID);

            if (Success >= 1)
            {
                Errors.Add(WeatherStationErrorsEnum.Ok);
                return (true, Errors);
            }
            else
            {
                Errors.Add(WeatherStationErrorsEnum.SensorDoesNotExist);
                return (false, Errors);
            }
        }

        private static WeatherStation ConstructWeatherStation(string MeterSquareIdentifier, int Easting, int Northing)
        {

            int WeatherStationID = CreateWeatherStationID();
            WeatherStation NewWeatherStation = new WeatherStation(WeatherStationID,MeterSquareIdentifier,Easting,Northing);

            return NewWeatherStation;
        }

        

        private static int CreateWeatherStationID()
        {
            int WeatherStationID = 1;

            for (int i = WeatherSensorParameters.sensorIdMin; i < WeatherSensorParameters.SensorIdMax; i++)
            {
                if(_WeatherStations.Any(x => x.SensorID == i))
                {
                    continue;                  
                }
                else
                {
                    WeatherStationID = i;
                    break;
                }
            }

            return WeatherStationID;
        }
        
    }
}
