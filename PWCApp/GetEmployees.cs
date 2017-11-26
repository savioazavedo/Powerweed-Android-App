using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PWBackend;

namespace PWCApp
{
    public static class GetEmployees
    {
        static List<Employee> myList;
        static EmployeesHandler obj = new EmployeesHandler();
        
        public static List<Employee> getEmployees()
        {
            myList = obj.ExecuteGetRequest();
            return myList;
           
        }

        public static List<string> getEmpNames()
        {
            List<string> workers = new List<string>();
            foreach (var w in myList)
            {
                if(w.empType == "Available")
                {
                    workers.Add(w.empNAME);
                }
                
            }
            return workers;
        }

        public static string getEmpNumber(string empName)
        {
            myList = obj.ExecuteGetRequest();
            

            string mobileNumber = (from m in myList
                                  where m.empNAME == empName
                                  select m.empMobile).First();
            return mobileNumber;
        }
    }
}