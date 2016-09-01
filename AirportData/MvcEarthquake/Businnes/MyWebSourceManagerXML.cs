using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using LogUtility;
using MvcEarthquake.Utils;

namespace MvcEarthquake.Businnes
{
    public class MyWebSourceManagerXML : IWebSourceManagerXML
    {
        public XDocument GetDocument(string webSiteUrl)
        {
            try
            {
                return XDocument.Load(webSiteUrl);
            }
            catch (WebException webExcp)
            {
                ExceptionUtility.Error(webExcp, this.GetType());
                return new XDocument();
            }

            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return new XDocument();
            }
        }
    }
}