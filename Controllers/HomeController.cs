using LBS_API.Model;
using LBS_UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace LBS_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly string Baseurl = "https://localhost:44338/";
        static HttpClient client = new HttpClient();
        // GET: Home
        public async Task<ActionResult> Index()
        {

            List<load> LoadInfo = new List<load>();
            using (var client = new HttpClient())
            {
             
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  
                HttpResponseMessage Res = await client.GetAsync("api/Load");
              
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var LdResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    LoadInfo = JsonConvert.DeserializeObject<List<load>>(LdResponse);
                }
                //returning the Product list to view
                return View(LoadInfo);
            }

        }
        //Details
        public async Task<ActionResult> Details()
        {
            List <load> LoadInfo = new List<load>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Load/GetLoads");
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    LoadInfo = JsonConvert.DeserializeObject<List<load>>(EmpResponse);
                }
                return View(LoadInfo);
            }
        }

        //GET: Load/Edit
        public async Task<ActionResult> Edit(int id)
        {
            load load = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Load/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var LdResponse = Res.Content.ReadAsStringAsync().Result;

                    load = JsonConvert.DeserializeObject<load>(LdResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error.Please contact admin");
            }
            return View(load);
        }

        //POST: load/edit
        [HttpPost]
        public async Task<ActionResult> Edit(int id, load load)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpResponseMessage Res = await client.GetAsync("api/Load/" + id);
                    load load1 = null;

                    if (Res.IsSuccessStatusCode)
                    {
                        var LdResponse = Res.Content.ReadAsStringAsync().Result;

                        load1 = JsonConvert.DeserializeObject<load>(LdResponse);
                    }
                    load.LoadNumber = load1.LoadNumber;
                    var postTask = client.PutAsJsonAsync<load>("api/Load/" + load.Id, load);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
            }

        //DELETE:
        static async Task<HttpStatusCode> DeleteLoadAsync(int id)
        {
            HttpResponseMessage res = await client.DeleteAsync($"api/load/{id}");
            return res.StatusCode;
        }
        
         
        
    }
}
