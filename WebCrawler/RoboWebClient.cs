using System;
using System.IO;
using System.Net;
using System.Text;
namespace Automation
{
    public class RoboWebClient : WebClient
    {
        // I have created these properties for control only.
        // If I need to expose these properties to change, just take the readonly and change to public.
        // Remember the sites can vary and a lot, so exposing a variable can make your code more flexible.
        public CookieContainer _cookie = new CookieContainer();
        public bool _allowAutoRedirect;

        // <summary>
        // I made an override of the method to contemplate the exposed variables.
        // In this case it is private, but in case of exposing, it should follow the form below.
        // </ summary>
        // <param name = "address"> Search address. </ Param>
        // <returns> Return of the request to be handled. </ Returns>
        protected override WebRequest GetWebRequest(Uri address)
        {

            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).ServicePoint.Expect100Continue = false;
                (request as HttpWebRequest).CookieContainer = _cookie;
                (request as HttpWebRequest).KeepAlive = false;
                (request as HttpWebRequest).AllowAutoRedirect = _allowAutoRedirect;
            }

            return request;
        }
    }
}