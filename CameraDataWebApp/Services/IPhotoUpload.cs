using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraDataWebApp.Controllers;

namespace CameraDataWebApp.Services
{
    public interface IPhotoUpload
    {
        Observation AddPhotos(Observation items);
        Dictionary<string, Observation> GetObservations();
    }
}
