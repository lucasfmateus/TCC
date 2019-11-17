using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UI.Service.Base
{
    public class ServiceBase
    {
        public HttpClient client = new HttpClient();
        public HttpClient classificationClient = new HttpClient();

        public ServiceBase()
        {
            ParkingClientReset();
            client.DefaultRequestHeaders.Accept.Clear();

            ClassificationClientReset();
            classificationClient.DefaultRequestHeaders.Accept.Clear();
        }

        public void ParkingClientReset()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5002/api/");
        }

        public void ClassificationClientReset()
        {
            classificationClient = new HttpClient();
            classificationClient.BaseAddress = new Uri("http://localhost:5003/api/");
        }

    }
}
