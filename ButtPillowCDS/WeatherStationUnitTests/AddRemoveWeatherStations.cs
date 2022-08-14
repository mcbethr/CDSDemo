using ButtPillowCDS;
using ButtPillowCDS.Analysis;
using ButtPillowCDS.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace WeatherStationUnitTests
{
    [TestClass]
    public class AddRemoveWeatherStations
    {
        [TestMethod]
        public void TestCreateWeatherStation()
        {
            WeatherStationManager.AddWeatherStation("AB", 1234, 5678);
            WeatherStationManager.AddWeatherStation("EG", 1234, 5678);

            Assert.AreEqual(2, WeatherStationManager.WeatherStations.Count);
            
        }

        [TestMethod]
        public void AddMaxWeatherStations()
        {

            for (int i = WeatherSensorParameters.SensorIdMin; i <= WeatherSensorParameters.SensorIdMax; i++)
            {
                WeatherStationManager.AddWeatherStation("AB", 1234, 5678);

            }

            Assert.AreEqual(10, WeatherStationManager.WeatherStations.Count);

        }

        [TestMethod]
        public void AddMaxWeatherStationsPlusOneFail()
        {

            for (int i = 0; i < WeatherSensorParameters.SensorIdMax; i++)
            {
                WeatherStationManager.AddWeatherStation("AB", 1234, 5678);
            }

            (bool Sucess, List<WeatherStation.WeatherStationErrorsEnum> Errors) Result
                    = new ValueTuple<bool, List<WeatherStation.WeatherStationErrorsEnum>>();

            Result = WeatherStationManager.AddWeatherStation("AB", 1234, 5678);

            Assert.AreEqual(Result.Sucess,false);
            Assert.AreEqual(WeatherStation.WeatherStationErrorsEnum.MaxSensorsReached,Result.Errors[0]);

        }

        [TestMethod]
        public void RemoveWeatherStation()
        {
            WeatherStationManager.AddWeatherStation("AB", 1111, 1111);
            WeatherStationManager.AddWeatherStation("CD", 2222, 2222);
            WeatherStationManager.AddWeatherStation("EF", 3333, 3333);

            WeatherStationManager.RemoveWeatherStation(2);
            Assert.AreEqual(2, WeatherStationManager.WeatherStations.Count);
        }

        [TestMethod]
        public void RemoveWeatherStationThenAddOne()
        {
            WeatherStationManager.AddWeatherStation("AB", 1111, 1111);
            WeatherStationManager.AddWeatherStation("CD", 2222, 2222);
            WeatherStationManager.AddWeatherStation("EF", 3333, 3333);

            WeatherStationManager.RemoveWeatherStation(2);
            WeatherStationManager.AddWeatherStation("GH", 3333, 3333);
            Assert.AreEqual(2, WeatherStationManager.WeatherStations[2].SensorID);
        }

        [TestMethod]
        public void AddWeatherStationAndUpdateWeatherSuccess()
        {
            WeatherUpdate WU = new WeatherUpdate();
            WU.Date = DateTime.Now;
            WU.Easting = 1111;
            WU.Northing = 1111;
            WU.SensorID = 1;
            WU.MeterSquareIdentifier = "AB";
            WU.TemperatureF = 72;


            WeatherStationManager.AddWeatherStation("AB", 1111, 1111);
            WeatherController WC = new WeatherController();
            WC.Post(WU);
            int result = WeatherStationManager.WeatherStations[0].WeatherUpdates[0].TemperatureF;
            Assert.AreEqual(WU.TemperatureF, result);

        }

        [TestMethod]
        public void UpdateWeatherStationThatDoesNotExist()
        {
            WeatherUpdate WU = new WeatherUpdate();
            WU.Date = DateTime.Now;
            WU.Easting = 1111;
            WU.Northing = 1111;
            WU.SensorID = 2;
            WU.MeterSquareIdentifier = "AB";
            WU.TemperatureF = 72;


            WeatherStationManager.AddWeatherStation("AB", 1111, 1111);
            WeatherController WC = new WeatherController();
            WC.Post(WU);
            Assert.AreEqual(1, WeatherStationManager.WeatherStations.Count);


        }

        /// <summary>
        /// This is perfectly valid although it shouldn't be because its colder than absolute zero
        /// </summary>
        [TestMethod]
        public void AddTempretureBelowAbsoluteZeroNoCDS()
        {
            WeatherUpdate WU = new WeatherUpdate();
            WU.Date = DateTime.Now;
            WU.Easting = 1111;
            WU.Northing = 1111;
            WU.SensorID = 1;
            WU.MeterSquareIdentifier = "AB";
            WU.TemperatureF = -500;


            WeatherStationManager.AddWeatherStation("AB", 1111, 1111);
            WeatherController WC = new WeatherController();
            WC.NoCDSPost(WU);

            int result = WeatherStationManager.WeatherStations[0].WeatherUpdates[0].TemperatureF;

            Assert.AreEqual(WU.TemperatureF, result);


        }


        /// <summary>
        /// Send a Valud Temp
        /// </summary>
        [TestMethod]
        public void AddTempreture72DegreesCDS()
        {
            WeatherUpdate WU = new WeatherUpdate();
            WU.Date = DateTime.Now;
            WU.Easting = 1111;
            WU.Northing = 1111;
            WU.SensorID = 1;
            WU.MeterSquareIdentifier = "AB";
            WU.TemperatureF = 72;


            WeatherStationManager.AddWeatherStation("AB", 1111, 1111);
            WeatherController WC = new WeatherController();
            WC.NoCDSPost(WU);

            int result = WeatherStationManager.WeatherStations[0].WeatherUpdates[0].TemperatureF;

            Assert.AreEqual(WU.TemperatureF, result);


        }

        [TestCleanup]
        public void TearDown()
        {
            WeatherStationManager.ClearWeatherStations();
        }

    }
}
