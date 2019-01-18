using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SQLite;

namespace CameraApp1.Fragments
{
    public class ChooseProjectFragment : ListFragment //Tämä sisältää defaulttina ListViewin joten ei ole tehty erillistä layout-tiedostoa
    {
        static HttpClient client = new HttpClient();
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            
        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        //voiko olla näin? on kyllä tutoriaalissakin?
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            Toast.MakeText(Android.App.Application.Context, "Lataa projekteja", ToastLength.Long).Show();
            string dbPath = Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
        "database.docstarter");

            try
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                db.CreateTable<Project>();
                db.CreateTable<MonitoringVisit>();
                //Näillä voi laittaa kovakoodatut projektit SqlLiteen 
                //db.InsertAll(LocalDB.AddTestProjects());
                //db.InsertAll(LocalDB.AddTestMonitorings());

                //var projectTable = db.Table<Project>();
                GetProjects(); //projectTable.ToList();


                //muuta lista javalistaksi 


                
                ((AppCompatActivity)Activity).SupportActionBar.SetTitle(Resource.String.projects_title);
            }



            catch (global::System.Exception ex)
            {

                string error = ex.Message;
            }
        }


        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            //Tämä tavoittaa listasta valitun projektin
            Project project = (Project)this.ListAdapter.GetItem(position);
            //Project project = new Project() { caseId = }
            //Tämä vaihtaa fragmentin --> on vielä ihan testinä vain kuvanotto fragmentti
            VisitFragmentOn(project);

        }

        private void VisitFragmentOn(Project project)
        {
            VisitsFragment visits = new VisitsFragment();
            Bundle args = new Bundle();
            args.PutString("case", project.caseId);
            visits.Arguments = args;
            //ois kyllä helppoa käyttää sitä Visits-classia jossa on kaikki käynnit niin ei tarvitsisi passailla listoja tai muuta 
            //vaan seuraava fragmentti voisi hakea suoraan tietokannasta tavarat ilman datan syöttelyä
            
            FragmentTransaction transaction = this.Activity.FragmentManager.BeginTransaction();
            
            transaction.Replace(Resource.Id.fragment_placeholder, visits, "CaseId");
            transaction.AddToBackStack(null);
            transaction.Commit();

        }

        public void GetProjects()
        {

            try
            {
           

                //List<Project> projects = new List<Project>();
                //Uri uri = new Uri(@"https://192.168.137.1:45455/projectsapi/GetProjects");
                //HttpClient client = new HttpClient();
                string downloadaddress = @"http://10.0.2.2:49785/projectsapi/GetProjects";

                // client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); //TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                //HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, downloadaddress);
                var response = client.GetAsync(downloadaddress).Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Tuleeko mitään konsoliin?");
                    var content = response.Content.ReadAsStringAsync().Result;
                    var projects = JsonConvert.DeserializeObject<List<Project>>(content);
                    JavaList<Project> javaprojects = new JavaList<Project>();

                    foreach (var item in projects)
                    {
                        javaprojects.Add(item);
                    }
                    this.ListAdapter = new Models.ProjectAdapter(Android.App.Application.Context, javaprojects);
                }

                // List<Project> projects = JsonConvert.DeserializeObject<List<Project>>($"{httpgetresponse.Content}");
                //return projects;
            }
            catch (HttpRequestException ex)
            {

                Console.WriteLine(ex.InnerException.Message);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

//palautaa kovakoodatun projektilistan testikäyttöä varten
//on korvattu nyt ORM-ratkaisulla
//private JavaList<Project> GetProjects()
//{

//    JavaList<Project> projects = new JavaList<Project>()
//    {
//        (new Project("Aleksiskiven Katu 50", "345"),
//        new Project("Ruosilankuja 3", "655"),
//        new Project("Kolmaslinja 7", "1008"),
//        new Project("Sipulikatu 14", "871"))}        ;

//    //Lisää kovakoodatun valvontakäyntilistan Aleksiskivenkadun projektiin 
//    List<MonitoringVisit> visits = new List<MonitoringVisit>()
//    {
//        new MonitoringVisit("Viikko 11", "345"),
//        new MonitoringVisit("Viikko 12", "345"),
//        new MonitoringVisit("Viikko 13", "345")
//    };

//    projects.First(s => s.CaseId == "345").Visits = visits;

//    return projects;


//}