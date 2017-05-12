using Automation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Automation
{
    public class NetCoders : Robo
    {
        /// <summary>
        /// Builder to Instantiate Client
        /// </summary>
        public NetCoders()
        {
            RoboWebClient = new RoboWebClient();
        }

        /// <summary>
        /// Method where the crawler is done to load the posts.
        /// </summary>
        /// <returns>List of articles sorted by date.</returns>
        public List<artigo> CarregaPosts()
        {
            NameValueCollection parametros = new NameValueCollection();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            //Upload the Blog homepage.
            //I'm assigning the result to the HtmlAgilityPack to parse the HTML.
            this.RoboWebClient._allowAutoRedirect = false;
            var ret = this.HttpGet(@"http://stackoverflow.com/");

            //Capturing only the tags that are defined as article and sorting by the ID of each tag.
            var artigosOrdenados = ret.DocumentNode.Descendants().Where(n => n.Name == "article").OrderBy(d => d.Id).ToList();
            List<artigo> artigos = new List<artigo>();

            //Browsing the articles that have already been selected.
            foreach (var item in artigosOrdenados)
            {
                artigo art = new artigo();

                //Loading the Html of each article.
                doc.LoadHtml(item.InnerHtml);

                //I'm using the HtmlAgilityPack.HtmlEntity.DeEntitize to make the HtmlDecode of the captured texts of each article.
                // I also use UTF8 to clean up the rest of Encodes that are on the page.
                art.Titulo = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(doc.DocumentNode.DescendantsAndSelf().FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "post-title entry-title").InnerText));
                art.Data = Convert.ToDateTime(HtmlAgilityPack.HtmlEntity.DeEntitize(doc.DocumentNode.DescendantsAndSelf().FirstOrDefault(d => d.Name == "span" && d.Attributes["class"].Value == "post-time").InnerText));
                art.Descricao = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(doc.DocumentNode.DescendantsAndSelf().FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "entry-content").InnerText));
                art.Autor = HtmlAgilityPack.HtmlEntity.DeEntitize(ConvertUTF(doc.DocumentNode.DescendantsAndSelf().FirstOrDefault(d => d.Attributes["class"] != null && d.Attributes["class"].Value == "post-author").InnerText));
                artigos.Add(art);
            }

            return artigos.OrderBy(d => d.Data).ToList();
        }

        private string ConvertUTF(string texto)
        {
            // Converting the text to the Default Encoding and Byte Array.
            byte[] data = Encoding.Default.GetBytes(texto);

            //Converting clean text to UTF8.
            string ret = Encoding.UTF8.GetString(data);

            return ret;
        }

    }

    /// <summary>
    /// Article Mirror Class.
    /// </summary>
    public class artigo
    {
        public string Titulo { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string Autor { get; set; }
    }
}