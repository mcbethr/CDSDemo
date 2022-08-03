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

        public static (bool, List<WeatherStationErrorsEnum>) InspectWeatherUpdate(WeatherUpdate WU)
        {
            bool success = false;
            List<WeatherStationErrorsEnum> StationErrors = new List<WeatherStationErrorsEnum>();

            //Check the time.  If the time on the System is in the future, generate an error
            if (DateTime.Now < WU.Date)
            {
                StationErrors.Add(WeatherStationErrorsEnum.SensorDateTimeInFuture);
            }

            if (WU.TemperatureF < WeatherSensorParameters.ColdestMin)
            {
                StationErrors.Add(WeatherStationErrorsEnum.TemperatureTooCold);
            }

            if (WU.TemperatureF > WeatherSensorParameters.HottestMax)
            {
                StationErrors.Add(WeatherStationErrorsEnum.TemperatureTooHot);
            }

            if (WU.Northing > WeatherSensorParameters.HighestGrid)
            {
                StationErrors.Add(WeatherStationErrorsEnum.NorthingGridRangeTooHigh);
            }

            if (WU.Northing < WeatherSensorParameters.LowestGrid)
            {
                StationErrors.Add(WeatherStationErrorsEnum.NorthingGridRangeTooLow);
            }

            if (WU.Easting > WeatherSensorParameters.HighestGrid)
            {
                StationErrors.Add(WeatherStationErrorsEnum.EastingGridRangeTooHigh);
            }

            if (WU.Easting < WeatherSensorParameters.LowestGrid)
            {
                StationErrors.Add(WeatherStationErrorsEnum.EastingGridRangeTooLow);
            }

            if (WU.SensorID < WeatherSensorParameters.SensorIdMin)
            {
                StationErrors.Add(WeatherStationErrorsEnum.SensorDoesNotExist);
            }

            if (WU.SensorID >WeatherSensorParameters.SensorIdMax)
            {
                StationErrors.Add(WeatherStationErrorsEnum.SensorDoesNotExist);
            }

            ///Then the sensor doesn't exist
            if (CheckSensorListForSensorID(WU.SensorID) == false)
            {
                StationErrors.Add(WeatherStationErrorsEnum.SensorDoesNotExist);
            }

            //if we made it here with 0 station errors then return success true and OK
            if (StationErrors.Count ==0)
            {
                success = true;
                StationErrors.Add(WeatherStationErrorsEnum.Ok);
            }

            return (success, StationErrors);
        }

        public static void UpdateWeather(WeatherUpdate WU)
        {
            _WeatherStations.Find(x => x.SensorID == WU.SensorID).WeatherUpdates.Add(WU);

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

            for (int i = WeatherSensorParameters.SensorIdMin; i < WeatherSensorParameters.SensorIdMax; i++)
            {
                if( CheckSensorListForSensorID(i) == true)
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

        private static bool CheckSensorListForSensorID(int SensorID)
        {
            if (_WeatherStations.Any(x => x.SensorID == SensorID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
