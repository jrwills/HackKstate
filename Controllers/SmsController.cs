using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//--------------------------Important
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using System.IO;
using System.Text;
//--------------------------------------
namespace WebApplication1.Controllers
{
    public class SmsController : TwilioController
    {
        public TwiMLResult Index(SmsRequest incomingMessage)
        {
            var messagingResponse = new MessagingResponse();
            string fileName = @"C:\Users\John Wills\Desktop\testFile.txt";
            //string url = incomingMessage.Body;
            
            /*if (incomingMessage.Body.Length > 5)
            {
                url = incomingMessage.Body;         //was for determinging what the URL was
                //Console.WriteLine(url);
            }    
            else
                url = null;*/

            string[] splitEd = incomingMessage.Body.Split(' '); 
            if(splitEd[0] == "!get")
            {
                if (GetUserPassword(fileName, splitEd[1]) != null )
                {
                    messagingResponse.Message("\nYour " + GetUserPassword(fileName, splitEd[1]));
                }

            }
            else if(incomingMessage.Body == "!help")
            {
                messagingResponse.Message("Text !get (url name) to find your information" +
                        "\nText !contains (url name) to find out if we have that information");
            }
            else if(splitEd[0] == "!contains")
            {
                if (ContainsInFile(fileName, splitEd[1]))
                {
                    messagingResponse.Message("We do have the URL, try the !get command.");
                }
                else
                {
                    messagingResponse.Message("We do NOT have the URL");
                }
            }
            else
            {
                messagingResponse.Message("Welcome to your Password Recovery!" + "\nText a !get (url name) to find your USER and PASS, or" +
                        "\nText !help for a list of commands");
            }
            
            return TwiML(messagingResponse);
        }

        /// <summary>
        /// Gets password and username from file
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns></returns>
        public string GetUserPassword(string fileName, string url)  //WORKS PERFECTLY
        {
            string user;
            string pass;
            StreamReader sr = new StreamReader(fileName);
            
            while (!sr.EndOfStream)
            {
                string test = sr.ReadLine();
                if (test.Length > 5)
                {
                    string[] arr = test.Split(' ');
                    if (arr[1] == url)
                    {
                        user = sr.ReadLine();
                        pass = sr.ReadLine();
                        return user + "" + pass;
                    }
                }
            }
            sr.Close();
                
            
            return "URL not found";
        }
        
        /// <summary>
        /// Checks to see if the URL you are looking for is in the file
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns></returns>
        public bool ContainsInFile(string fileName, string url)
        {
            StreamReader sr = new StreamReader(fileName);

            while (!sr.EndOfStream)
            {
                string test = sr.ReadLine();
                if (test.Length > 5)
                {
                    string[] arr = test.Split(' ');
                    if (arr[1] == url)
                    {
                        return true;
                    }
                }
            }
            sr.Close();
            return false;
        }
    }
}
