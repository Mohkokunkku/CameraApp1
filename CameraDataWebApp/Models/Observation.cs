using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace CameraDataWebApp.Controllers
{
    public class ObservationContext : DbContext
    {
        public ObservationContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Observation> Observations { get; set; }
        public DbSet<MonitoringVisit> MonitoringVisits { get; set; }
    }
    public class Observation
    {
        
        public int Id { get; set; }

        public string observation { get; set; }

        public string imageuri { get; set; }

        public string observationguid { get; set; } 

        public string visitguid { get; set; }

        public string visitname { get; set; }
    }

    public class MonitoringVisit
    {
        public int Id { get; set; }
        public string visitname { get; set; }
        public string visitguid { get; set; }
        public string casename { get; set; }
        public string casenumber { get; set; }
        public ICollection<Observation> observations { get; set; }
    }
}