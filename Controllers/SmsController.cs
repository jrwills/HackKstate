/*
 * SmsController.cs
 * Uses the Twilio SMS function to recover your password from a text file(a.k.a a Database)
 * Name: John Wills
 * Hack K-State
 */
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
            
            string fileName = @"C:\Users\John Wills\Desktop\testFile.txt";  //A hardcoded file, hypothetically would be a database of passwords
            
            string[] splitEd = incomingMessage.Body.Split(' '); 

            if(splitEd[0] == "!get")
            {
                if (GetUserPassword(fileName, splitEd[1]) != null )
                {
                    messagingResponse.Message(GetUserPassword(fileName, splitEd[1]));
                }

            }
            else if(splitEd[0] == "!add")   //sadly have to use this first for some reason?
            {
                string newURL = splitEd[1];
                string newUser = splitEd[2];
                string newPass = splitEd[3];
                Add(fileName, newURL, newUser, newPass);
                messagingResponse.Message("Info successfully added to the database!");
            }
            else if(incomingMessage.Body == "!help")
            {
                messagingResponse.Message("#Text !get (url name) to find your information" +
                        "\n#Text !contains (url name) to find out if we have that information" + 
                        "\n#Text !add (url) (username) (password) to add a set of information to our database!");
            }
            else if(splitEd[0] == "!contains")
            {
                if (ContainsInFile(fileName, splitEd[1]))
                {
                    messagingResponse.Message("We have the url, try the !get command.");
                }
                else
                {
                    messagingResponse.Message("We do NOT have the URL, try using the !add command");
                }
            }
            else
            {
                messagingResponse.Message("Hi! Welcome to your Password Recovery!" + "\n#Text !help for a list of commands");
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
                        return "\n" + user + "\n" + pass;
                    }
                }
            }
            sr.Close();
                
            
            return "URL not found, did you spell it right?";
        }
        
        /// <summary>
        /// Checks to see if the URL you are looking for is in the file
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns></returns>
        public bool ContainsInFile(string fileName, string url) //Suddenly not working for some reason?
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

        /// <summary>
        /// Add to a file
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <param name="url">url</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        public void Add(string fileName, string url, string user, string pass)
        {
            string condensed = "\nURL: " + url + "\nUSERNAME: " + user + "\nPASSWORD: " + pass;
            StreamWriter sw = new StreamWriter(fileName,true);
            sw.WriteLine(condensed);
            sw.Close();
        }
    }
}
