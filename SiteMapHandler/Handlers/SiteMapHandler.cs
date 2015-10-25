using System;
using System.Web;
using System.Xml;

namespace SiteMapHandler.Handlers
{
    public class SiteMapHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.ContentType = "text/xml";

            var writer = new XmlTextWriter(context.Response.Output);
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            writer = SetItem(writer);

            writer.WriteEndDocument();
            writer.Flush();
        }

        private XmlTextWriter SetItem(XmlTextWriter writer)
        {
            writer.WriteStartElement("url");
            writer.WriteElementString("loc", "http://www.example.at");
            writer.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
            writer.WriteElementString("changefreq", "daily");
            writer.WriteElementString("priority", "1.0");
            writer.WriteEndElement();

            return writer;
        }
    }
}