using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RestSharp;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using PWBackend;
using System.Collections.Generic;
using System;

namespace PWCApp
{
    public class EmployeesHandler
    {
        private string url = "https://pwbackendapi.azurewebsites.net/api/Employees";
        private IRestResponse response;
        private RestRequest request;

        public void AddParameter(string name, string value)
        {
            if (request != null)
            {
                request.AddParameter(name, value);
            }
        }

        public List<Employee> ExecuteGetRequest()
        {
            var client = new RestClient(url);

            request = new RestRequest();

            response = client.Execute(request);

            var objRoot = JsonConvert.DeserializeObject<List<Employee>>(response.Content);

            return objRoot;
        }

        public bool ExecutePostRequest(JobsAssigned item)
        {
            var client = new RestClient(url);

            request = new RestRequest(Method.POST);

            // Json to post.
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            try
            {

                IRestResponse response = client.Execute(request);

                if (response != null && response.StatusCode == HttpStatusCode.Created)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception error)
            {
                Console.WriteLine("Error:" + error.Message);
                return false;
            }
        }


        //Update
        public bool ExecutePutRequest(Employee item)
        {
            var client = new RestClient("https://pwbackendapi.azurewebsites.net/api/Employees" + "/" + item.empID);

            request = new RestRequest(Method.PUT);

            // Json to Put
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item);

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            try
            {

                IRestResponse response = client.Execute(request);

                if (response != null && response.StatusCode == HttpStatusCode.NoContent)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception error)
            {
                Console.WriteLine("Error:" + error.Message);
                return false;
            }
        }


        //Delete 
        public bool ExecuteDeleteRequest(String Id)
        {
            var client = new RestClient(url + "/" + Id);

            request = new RestRequest(Method.DELETE);

            request.AddParameter("application/json; charset=utf-8", ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            try
            {

                IRestResponse response = client.Execute(request);

                if (response != null && response.StatusCode == HttpStatusCode.NoContent)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception error)
            {
                Console.WriteLine("Error:" + error.Message);
                return false;
            }
        }
    }
}