using CameraDataWebApp.Controllers;
using CameraDataWebApp.WordProcessing;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace wordTest
{
    [TestClass]
    public class MonitoringVisitDocumentTests
    {
        
        [TestMethod]
        public void Test_NewDocFileCreated()
        {
            
            try
            {
                MonitoringVisitDocument document = new MonitoringVisitDocument(GetTestMonitoringVisit());
                document.GetMonitoringVisitDocument();
                Assert.IsTrue(File.Exists($"{document.newFilePath}"));
                
            }
            catch (System.Exception)
            {
                Assert.Fail();
                //throw;
            }
        }

        [TestMethod]
        public void Test_TemplateFileExists()
        {

            try
            {
                MonitoringVisitDocument document = new MonitoringVisitDocument(GetTestMonitoringVisit());
                Assert.IsTrue(document.CheckTemplateFile());


            }
            catch (System.Exception)
            {
                Assert.Fail();
                //throw;
            }
        }

        //Tekee mockin monitoringvisitistä, joka haetaan ObservationContextista lopullisessa versiossa
        private MonitoringVisit GetTestMonitoringVisit()
        {
            
            MonitoringVisit monitoringVisit = new MonitoringVisit {casename = "test", casenumber = "666", Id = 18, observations = new List<Observation>()  };
            Observation observation1 = new Observation { imageuri = @"C:\\DS asennus\\SVN20 - elävä asiakirja\\DocStarter\\bin\\Debug\\imagedata\\Image_2d9c3b24-49d3-4fec-94ec-4ecdfd9f9d34.jpg", Id = 44, observation = "Kuvassa paperiteline", observationguid = "testasfasf", visitguid = "asdasgag", visitname = "testikäynti" };
            Observation observation2 = new Observation { imageuri = @"C:\\DS asennus\\SVN20 - elävä asiakirja\\DocStarter\\bin\\Debug\\imagedata\\Image_111ca365-b098-4ce4-ba7e-b03fe7ceaa5a.jpg", Id = 43, observation = "Kuvassa tuoli" };
            monitoringVisit.observations.Add(observation1);
            monitoringVisit.observations.Add(observation2);
            return monitoringVisit;
        }
    }
}
