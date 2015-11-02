using System.Xml;
using SiteMapHandler.Handlers;
using System.Collections.Generic;

namespace SiteMapHandler.Helpers
{
    internal static class XmlHelper
    {
        internal static void TransformSiteMapData(XmlWriter writer, IList<SiteMapData> data)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            foreach (var link in data)
            {
                // http://www.sitemaps.org/protocol.html

                writer.WriteStartElement("url");
                writer.WriteElementString("loc", link.Url);
                writer.WriteElementString("lastmod", link.LastModified.ToString("yyyy-MM-dd"));
                writer.WriteElementString("changefreq", link.ChangeFrequenc.ToString());
                writer.WriteElementString("priority", link.Priority);
                writer.WriteEndElement();
            }

            writer.WriteEndDocument();
            writer.Flush();
        }
    }
}