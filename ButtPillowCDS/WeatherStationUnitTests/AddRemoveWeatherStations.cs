using ButtPillowCDS.Analysis;
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

        [TestCleanup]
        public void TearDown()
        {
            WeatherStationManager.ClearWeatherStations();
        }

    }
}
