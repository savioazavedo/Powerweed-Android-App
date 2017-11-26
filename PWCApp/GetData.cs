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
using System.Net.Sockets;
using Java.Net;
using System.IO;
using Android.Net;

namespace PWCApp
{
    class GetData
    {
        public static List<Job> myList;
        public static void GetJobData()
        {
            //bool isConnected = CheckInternet.CheckForInternetConnection();

            //if(isConnected == true)
            //{
            try
            {
                JOBSHandler objRest = new JOBSHandler();

                myList = objRest.ExecuteGetRequest();
                myList = myList.OrderBy(o => o.JobNUMBER).ToList();
            }
            catch (Exception )
            {
                throw; 
            }
                

           
            //}
            //else
            //{
            //    AlertDialog.Builder alertDialog = new AlertDialog.Builder();

            //    alertDialog.SetMessage("Do you want to remove selected worker from this job?");
            //    alertDialog.SetPositiveButton("Yes", delegate
            //    {

            //        alertDialog.Dispose();
            //    });
            //    alertDialog.SetNegativeButton("Cancel", delegate
            //    {
            //        alertDialog.Dispose();
            //    });
            //    alertDialog.Show();
            //}

        }

        

    }


}