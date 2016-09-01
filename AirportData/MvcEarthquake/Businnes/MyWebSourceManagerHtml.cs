using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using LogUtility;
using MvcEarthquake.Utils;

namespace MvcEarthquake.Businnes
{
    public class MyWebSourceManagerHtml : IWebSourceManagerHtml
    {
        public MyWebRequest WebRequest { get; set; }

        
        public HtmlDocument GetDocument(string webSiteUrl)
        {
            try

            {
                //-------------------------------------------------
               // var urlInpres1 = "D:\\pendrive\\02\\Instituto Nacional de Prevención Sísmica.htm";
                //webSiteUrl = urlInpres1;
                //ExceptionUtility.Warn(" codigo solo para test! " + this.GetType());

                //catch a web exception
                WebRequest = new MyWebRequest(webSiteUrl);
                var asd = WebRequest.GetResponse();
                //-----------------------------------------------------------------

               
                
                // Load the html document
                HtmlWeb htmlWeb = new HtmlWeb();
               return htmlWeb.Load(webSiteUrl);
               
            }

            catch (WebException webExcp)
            {
                ExceptionUtility.Error(string.Concat(webExcp.InnerException.Message, " ", this.GetType(), webSiteUrl));
                return new HtmlDocument();
            }

            catch(Exception ex){

                ExceptionUtility.Error(ex, this.GetType());
                return new HtmlDocument();
            }
        }
    }
}