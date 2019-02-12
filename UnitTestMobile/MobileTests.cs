using CameraApp1.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestMobile
{
    [TestClass]
    public class MobileTests
    {
        [TestMethod]
        public void Test_JsonSerialize()
        {
            // Arrange
            List<Observation> observations = new List<Observation>();

            var obs = new Observation();
            obs.absolutepath = "test";
            obs.imageuri = "testuri";
            obs.observationguid = "asl�dkas�lf";
            obs.observation = "erikoinen juttu";
            obs.cachepath = "testcache";
            obs.visitguid = "as�kdjas�kjdaslkdjvisit";

            observations.Add(obs);

            string expected = "{\"observation\": \"erikoinen juttu\", \"imageuri\": \"testuri\", \"observationguid\": \"asl�dkas�lf\", \"visitguid\": \"as�kdjas�kjdaslkdjvisit\"}";

            //act
            var visitsender = new ObservationSender(observations);

            //Assert
            Assert.AreEqual(expected, visitsender.json);

        }
    }
}
