using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.UI;
using System.Xml;
using SiteMapHandler.Helpers;

namespace SiteMapHandler.Handlers
{
    public class SiteMapHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ClearHeaders();
            context.Response.ClearContent();

            //using (var gzipStream = new GZipStream(context.Response.OutputStream, CompressionMode.Compress))
            //{
            //    var xml = new XmlDocument();
            //    xml.LoadXml(GetSiteMap(1).ToString());
            //    xml.Save(gzipStream);
            //}
            
            if (context.Request.CurrentExecutionFilePathExtension == ".gz")
            {
                context.Response.ContentType = "application/x-gzip";
                int mandatorNumber = 1;

                using (var gzipStream = new GZipStream(context.Response.OutputStream, CompressionMode.Compress))
                {
                    using (var writer = XmlWriter.Create(gzipStream, new XmlWriterSettings()))
                    {
                        var sitemap = GetSiteMap(mandatorNumber);
                        XmlHelper.TransformSiteMapData(writer, sitemap);
                    }
                }
            }
            else if (context.Request.CurrentExecutionFilePathExtension == ".html")
            {
                context.Response.ContentType = "text/html";
                int mandatorNumber = 1;

                var sitemap = GetSiteMap(mandatorNumber);

                using (var stringWriter = new StringWriter())
                {
                    using (var htmlWriter = new HtmlTextWriter(stringWriter))
                    {
                        htmlWriter.WriteLine("<!DOCTYPE html>");
                        htmlWriter.WriteLine("<head><title>Sitemap</title></head><body style=\"background-color:#00285c\">");

                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Src, "https://admiral.at/img4/Admiral_white.png");
                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Width, "250");
                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Height, "40");
                        htmlWriter.AddAttribute(HtmlTextWriterAttribute.Alt, "www.admiral.at");
                        htmlWriter.RenderBeginTag(HtmlTextWriterTag.Img);
                        htmlWriter.RenderEndTag();
                        htmlWriter.WriteLine(Environment.NewLine);

                        foreach (var link in sitemap)
                        {
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                            htmlWriter.AddAttribute(HtmlTextWriterAttribute.Href, link.Url);
                            htmlWriter.AddAttribute(HtmlTextWriterAttribute.Style, "color:#ffcc00");
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.A);
                            htmlWriter.Write(link.Url);
                            htmlWriter.RenderEndTag();
                            htmlWriter.RenderEndTag();
                            htmlWriter.WriteLine(Environment.NewLine);
                        }

                        htmlWriter.WriteLine("</body></html>");
                        context.Response.Write(stringWriter.ToString());
                    }
                }
            }
            else
            {
                context.Response.ContentType = "application/xml";
                int mandatorNumber = 1;

                using (var writer = XmlWriter.Create(context.Response.Output, new XmlWriterSettings()))
                {
                    var sitemap = GetSiteMap(mandatorNumber);
                    XmlHelper.TransformSiteMapData(writer, sitemap);
                }
            }
        }

        internal IList<SiteMapData> GetSiteMap(int mandatorNumber)
        {
            return new List<SiteMapData>
            {
                new SiteMapData
                {
                    Url = "http://cpp.cloudapp.net/sitemap.html",
                    LastModified = DateTime.Parse("02.11.2015"),
                    ChangeFrequenc = ChangeFrequenc.Weekly,
                    Priority = "0.5"
                },
                new SiteMapData
                {
                    Url = "http://cpp.cloudapp.net/sitemap.xml",
                    LastModified = DateTime.Parse("02.11.2015"),
                    ChangeFrequenc = ChangeFrequenc.Weekly,
                    Priority = "0.5"
                },
                new SiteMapData
                {
                    Url = "http://cpp.cloudapp.net/sitemap.xml.gz",
                    LastModified = DateTime.Parse("02.11.2015"),
                    ChangeFrequenc = ChangeFrequenc.Weekly,
                    Priority = "0.5"
                },
                new SiteMapData
                {
                    Url = "http://cpp.cloudapp.net/robots.txt",
                    LastModified = DateTime.Parse("02.11.2015"),
                    ChangeFrequenc = ChangeFrequenc.Weekly,
                    Priority = "0.5"
                }
            };
        }
    }

    public class SiteMapData
    {
        public string Url { get; set; }
        public DateTime LastModified { get; set; }
        public ChangeFrequenc ChangeFrequenc { get; set; }
        public string Priority { get; set; }
    }

    public enum ChangeFrequenc
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }
}