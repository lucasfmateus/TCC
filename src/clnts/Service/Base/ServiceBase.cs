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

        public ServiceBase()
        {
            client.BaseAddress = new Uri("http://localhost:5000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
        }

    }
}
