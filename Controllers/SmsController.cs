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
//--------------------------------------
namespace WebApplication1.Controllers
{
    public class SmsController : TwilioController
    {
        public TwiMLResult Index(SmsRequest incomingMessage)
        {
            var messagingResponse = new MessagingResponse();
            string fileName = @"C:\Users\John Wills\Desktop\fileUserInfo.txt";
            string url;
            
            if (incomingMessage.Body.Length > 5)
                url = incomingMessage.Body.Substring(5);
            else
                url = null;

            string[] splitEd = incomingMessage.Body.Split(' ');
            if(splitEd[0] == "!get")
            {
                /*messagingResponse.Message("The URL you texted was: " +
                                          incomingMessage.Body.Substring(5) + " Is that correct?(Y+++++/N-----)");*/
                if (GetUsername(fileName, url) != null || GetPassword(fileName, url) != null)
                {
                    messagingResponse.Message("\nYour " + GetUsername(fileName, url) + "\nYour " + GetPassword(fileName, url));
                }
                else
                {
                    messagingResponse.Message("URL Not Found..." + "\nTry again with the !get command to find your information");
                }

            }
            else if(splitEd[0] == "!help")
            {
                messagingResponse.Message("Text !get (url name) to find your information" +
                        "\nText !contains (url name) to find out if we have that information");
            }
            else if(splitEd[0] == "!contains" + url)
            {
                if (ContainsInFile(fileName, url))
                {
                    messagingResponse.Message("We do have the URL");
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
        /// Gets the username
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns></returns>
        public string GetUsername(string fileName, string url)
        {
            string newUser;
            
            StreamReader temp = new StreamReader(fileName);
            int count = 0;
            while (!temp.EndOfStream)
            {
                count++;
                temp.ReadLine();
            }
            temp.Close();
            //-----Temp is used to find length of the file
            string[] fileCollect = new string[count];
            int i = 0;
            StreamReader sr = new StreamReader(fileName); //using sr to read thru the file
            while (!sr.EndOfStream) //while file is not fully read
            {
                fileCollect[i] = sr.ReadLine();
                i++;
            }

            int index = 0;

            for (int j = 0; j < fileCollect.Length; j++)
            {
                if (fileCollect[j] != "\n")
                {
                    if (fileCollect[j].Substring(5) == url)  //URL: 5   DID NOT LIKE GMAIL
                    {
                        index = j;
                        break;
                    }
                }
            }
            newUser = fileCollect[index + 1];
            return newUser;
        }

        /// <summary>
        /// Gets password from file
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns></returns>
        public string GetPassword(string fileName, string url)
        {
            string newPass;
            
            StreamReader temp = new StreamReader(fileName);
            int count = 0;
            while (!temp.EndOfStream)
            {
                count++;
                temp.ReadLine();
            }
            temp.Close();
            //-----Temp is used to find length of the file
            
            string[] fileCollect = new string[count];
            int i = 0;
            StreamReader sr = new StreamReader(fileName); //using sr to read thru the file
            while (!sr.EndOfStream) //while file is not fully read
            {
                fileCollect[i] = sr.ReadLine();
                i++;
            }

            int index = 0;

            for (int j = 0; j < fileCollect.Length; j++)
            {
                if (fileCollect[j] != "\n")
                {
                    if (fileCollect[j].Substring(5) == url)  //URL: 5
                    {
                        index = j;
                        break;
                    }
                }
            }
            newPass = fileCollect[index + 2];
            return newPass;
        }

        /// <summary>
        /// Checks to see if the URL you are looking for is in the file
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <returns></returns>
        public bool ContainsInFile(string fileName, string url)
        {
            StreamReader temp = new StreamReader(fileName);
            int count = 0;
            while (!temp.EndOfStream)
            {
                count++;
                temp.ReadLine();
            }
            temp.Close();
            //-----Temp is used to find length of the file

            string[] fileCollect = new string[count];

            StreamReader urlLookup = new StreamReader(fileName);
            int line = 0;
            while(!urlLookup.EndOfStream)
            {
                if(fileCollect[line].Contains(url))
                {
                    return true;
                }
                line++;
                urlLookup.ReadLine();
            }
            return false;
        }
    }
}


/*switch (incomingMessage.Body)
            {
                default:
                    messagingResponse.Message("Text a !get (url name) to find your USER and PASS" +
                        "\nText !help for a list of commands");
                    break;
                case "Y":
                    messagingResponse.Message("\nYour " + GetUsername(fileName, url) + "\nYour " + GetPassword(fileName, url));
                    break;
                case "N":
                    messagingResponse.Message("Text the URL containing the USER and PASSWORD you're looking for");
                    break;
                case "!help":
                    messagingResponse.Message("Text !get (url name) to find your information" +
                        "\nText !set (url name) to set a new username and password for a site");
                    break;
                case "!get " + url:
                    messagingResponse.Message("The URL you texted was: " +
                                          incomingMessage.Body.Substring(5) + " Is that correct?(Y/N)");
                    break;

            }*/
