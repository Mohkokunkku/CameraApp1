using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CameraApp1.Fragments;
using FFImageLoading;
using Newtonsoft.Json;

namespace CameraApp1.Models
{

    public class ObservationSender
    {
        public string visitconnstring = @"http://192.168.100.210:47850/api/MonitoringVisits";
        public string connstring = @"http://192.168.100.210:47850/api/ObservationsApi";
        public List<SendObservation> _observations = new List<SendObservation>();
        public SendMonitoringVisit _monitoringVisit;
        public List<Observation> _observs = new List<Observation>();
        public static HttpClient client = new HttpClient();
        public string json;

        public ObservationSender(List<Observation> observations, MonitoringVisit monitoringVisit)
        {
            _monitoringVisit = new SendMonitoringVisit
            {
                casename = monitoringVisit.casename,
                casenumber = monitoringVisit.casenumber,
                visitguid = monitoringVisit.visitguid,
                visitname = monitoringVisit.visitname
               
            };

            _observs = observations;
            foreach (var observation in observations)
            {
                SendObservation sendObservation = new SendObservation();
                sendObservation.absolutepath = observation.absolutepath;
                sendObservation.imageuri = observation.imageuri;
                sendObservation.visitguid = _monitoringVisit.visitguid;
                sendObservation.visitname = _monitoringVisit.visitname; //observation.visitname;
                sendObservation.observation = observation.observation;
                sendObservation.observationguid = observation.observationguid;
                _observations.Add(sendObservation); 
            }
            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_observations = observations.ToList<IObservation>();
            //SendObservation();
        }

        public void SendVisit()
        {
            var jsonobservations = JsonConvert.SerializeObject(_monitoringVisit, Formatting.Indented); //serialisointi menee vituiksi
            //json = jsonobservations;
            HttpContent httpContent = new StringContent(jsonobservations, Encoding.UTF8, "application/json");
            var respons = ObservationSender.client.PostAsync(visitconnstring, httpContent);

            if (respons.Result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                //JOS TULEE CREATED NIIN ALOITTAISI OBSERVATIONIEN JA KUVADATAN LÄHETYKSEN
                SendObservation();
            }
            else
            {
                Toast.MakeText(Android.App.Application.Context, $"Error while sending visitdata, message {respons.Result.StatusCode}", ToastLength.Long).Show();
                //HEITÄ JOKU ILMOITUS ETTÄ VIRHE LÄHETYKSESSÄ
            }
        }
                
            

        public void SendObservation()
        {
            try
            {
                List<SendObservation> sendobservations = _observations.ToList<SendObservation>();
                var jsonobservations = JsonConvert.SerializeObject(_observations, Formatting.Indented); //serialisointi menee vituiksi
                json = jsonobservations;
                HttpContent httpContent = new StringContent(jsonobservations, Encoding.UTF8, "application/json");
                var respons = client.PostAsync(connstring, httpContent);
                Console.WriteLine(respons.Result.ToString());

                if (respons.Result.StatusCode == System.Net.HttpStatusCode.OK || respons.Result.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    var imagesender = new ImageDataSender(_observations, client, _monitoringVisit.visitname);
                    var uiContext = TaskScheduler.FromCurrentSynchronizationContext();
                    Task.Factory.StartNew(() => imagesender.SendImageData(), CancellationToken.None, TaskCreationOptions.None, uiContext);
                    //Task.Run(() => imagesender.SendImageData());
                    //task.Wait();
                    
                }
                else
                {
                    Toast.MakeText(Android.App.Application.Context, $"Error while sending observationdata, message: {respons.Result.StatusCode}", ToastLength.Long).Show();
                    //Jokin ilmoitus että virhe lähetyksessä
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, $"Error while sending observationdata, message: {ex.Message}", ToastLength.Long).Show();
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }

    public class ImageDataSender
    {
        private string connstring = @"http://192.168.100.210:47850/photoapi/AddObservations"; //CONNECTIONSTRINGIT OLISI HYVÄ OLLA JOSSAIN TIEDOSTOSSA NIIN NIIHIN VIITTAMINEN OLISI KÄTEVÄMPÄÄ
        //private object client;
        private List<SendObservation> _observations;
        private string _visitname;
        private HttpClient _client;
        public ImageDataSender(List<SendObservation> observations, HttpClient client, string visitname)
        {
            
            _visitname = visitname; //Antaa nullin tässä niin tekee virheen, pitäisi saada vielä joku callback UI-threadiin 

            _observations = observations;
            _client = client;
        }

        public async Task SendImageData()
        {
            try
            {
                var httpContent = new MultipartFormDataContent();

                using (var content = new MultipartFormDataContent())
                {

                    List<StreamContent> tasks = await GetStreamsAsync(_observations);

                    foreach (var task in tasks)
                    {
                        content.Add(task);
                    }

                    var response = _client.PostAsync(connstring, content);
                    if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Toast.MakeText(Android.App.Application.Context, $"Valtontakäynti \"{_visitname}\" tallennettu serverille!", ToastLength.Long).Show();
                        Console.WriteLine("Toimii!" + response.Result.ToString());
                    }
                    else
                    {
                        Toast.MakeText(Android.App.Application.Context, $"Error while sending imagedata \"{_visitname}\"", ToastLength.Long).Show();
                        Console.WriteLine("Kuvien lähetys ei toimi");
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, $"Error while sending imagedata \"{ex.Message}\"", ToastLength.Long).Show();
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        private async Task<List<StreamContent>> GetStreamsAsync(List<SendObservation> observations)
        {
            List<StreamContent> streamcontents = new List<StreamContent>();

            foreach (var observ in observations)
            {

               var streamcontent = new StreamContent(await ImageService.Instance.LoadFile(observ.absolutepath)
                            .DownSample(700)
                            .AsJPGStreamAsync());
                streamcontent.Headers.Add("Content-Disposition", $"form-data; name=\"items\"; filename=\"{observ.imageuri}\"");

                streamcontents.Add(streamcontent); 
            }
            //Task.WaitAll();
            return streamcontents;

        }

        private StreamContent CreateFileContent(Stream stream, string fileName)
        {
            
            var fileContent = new StreamContent(stream);
            fileContent.Headers.Add("Content-Disposition", $"form-data; name=\"items\"; filename=\"{fileName}\"");
            return fileContent;
        }
    }
}