using UI.Service.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Models;

namespace UI.Service.Controller
{
    public class SlotController : ServiceBase
    {
        public async Task<List<Slot>> GetFreeSlotsAsync()
        {
            try
            {
                var request = await client.GetAsync("slot/GetFreeSlots");

                var x = await request.Content.ReadAsAsync<List<Slot>>();

                return x;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Slot>> GetAllSlotsAsync()
        {
            try
            {
                var request = await client.GetAsync("slot/GetSlots");

                var x = await request.Content.ReadAsAsync<List<Slot>>();

                return x;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Slot> NewSlotAsync(Slot slot)
        {
            var myContent = JsonConvert.SerializeObject(slot);

            using (var stringContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json"))

            using (client)
            {
                try
                {
                    var request = await client.PostAsync("slot/NewSlot/", stringContent);
                    var result = await request.Content.ReadAsStringAsync();
                    return await request.Content.ReadAsAsync<Slot>();

                }
                catch (Exception)
                {
                    return null;
                }
            }

        }

    }
}
