using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraDataWebApp.Controllers;

namespace CameraDataWebApp.Services
{
    public class PhotoUpload : IPhotoUpload
    {
        private readonly Dictionary<string, Observation> _observations;

        public PhotoUpload()
        {
            _observations = new Dictionary<string, Observation>();
        }

        public Observation AddPhotos(Observation items)
        {
            _observations.Add(items.imageuri, items);
            return items;
        }

        public Dictionary<string, Observation> GetObservations()
        {
           return _observations;
        }
    }
}
