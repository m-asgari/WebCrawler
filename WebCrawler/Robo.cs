using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
namespace Automation
{
    /// <summary>
    /// Class for use of the Web Request.
    /// </summary>
    public class Robo
    {
        public RoboWebClient RoboWebClient { get; set; }

        /// <summary>
        /// Methods for making calls via GET.
        /// </summary>
        /// <param name="url">Url to be searched.</param>
        /// <returns>HtmlDocumento - Used to facilitate the parsing of Html.</returns>
        public HtmlDocument HttpGet(string url)
        {
            lock (this)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(RoboWebClient.DownloadString(url));

                return htmlDocument;
            }
        }

        /// <summary>
        /// Método para efetuar chamadas via POST
        /// </summary>
        /// <param name="url">Url a ser pesquisada.</param>
        /// <param name="parametros">Page Parameters.</param>
        /// <returns>HtmlDocumento - Used to facilitate the parsing of Html.</returns>
        public HtmlDocument HttpPost(string url, NameValueCollection parametros)
        {

            var htmlDocument = new HtmlDocument();
            byte[] pagina = RoboWebClient.UploadValues(url, parametros);
            htmlDocument.LoadHtml(Encoding.Default.GetString(pagina, 0, pagina.Count()));

            return htmlDocument;
        }
    }

}