using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;


namespace MvcEarthquake.Businnes
{
    public interface IWebSourceManagerHtml
    {
         MyWebRequest WebRequest { get; set; }
         HtmlDocument GetDocument(string webSiteUrl);
    }
}