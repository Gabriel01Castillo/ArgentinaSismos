using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using GlobalAppData;
using LogUtility;

namespace XMLManagment
{
    public class XMLManager : IXMLManager
    {
        public List<string> GetTweeterForbiddenWords()
        {
            List<string> defaults = new List<string>();
           
            try
            {
                var file = GlobalWebData.GetRootPath() + "tweeterForbiddenWords.xml";
                if (File.Exists(file))
                {

                    ReadWords(defaults, file);
                }
                else
                {
                    defaults = CreateTweeterForbiddenWords();
                }

                return defaults;

            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return GetDefaultTweeterForbiddenWords();
            }
        }


        private List<string> CreateTweeterForbiddenWords()
        {
            try
            {
                var file = GlobalWebData.GetRootPath() + "tweeterForbiddenWords.xml";
                return CreateXmlFile(file, "TweeterForbiddenWords", GetDefaultTweeterForbiddenWords());
            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return GetDefaultTweeterForbiddenWords();
            }
        }

        private static List<string> GetDefaultTweeterForbiddenWords()
        {

            List<string> defaults = new List<string>();
            defaults.Add("GOL");
            defaults.Add("FUTBOL");
            defaults.Add("JUGADOR");
            defaults.Add("SELECCION");
            defaults.Add("SELECCIÓN");

            return defaults;
        }

        public List<string> GetKeyWords()
        {
            List<string> defaults = new List<string>();          

            try
            {
                var file = GlobalWebData.GetRootPath() + "tweeterKeyWords.xml";
                if (File.Exists(file))
                {


                    ReadWords(defaults, file);
                }
                else
                {
                    defaults = CreateTweeterKeyWords();
                }

                return defaults;

            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return GetDefaultTweeterKeyWords();
            }
        }

        private List<string> CreateTweeterKeyWords()
        {
            try
            {
                var file = GlobalWebData.GetRootPath() + "tweeterKeyWords.xml";
                return CreateXmlFile(file, "TweeterKeyWords", GetDefaultTweeterKeyWords());

            }
            catch (Exception ex)
            {
                ExceptionUtility.Error(ex, this.GetType());
                return GetDefaultTweeterKeyWords();
            }
        }

        private List<string> GetDefaultTweeterKeyWords()
        {
            List<string> defaults = new List<string>();
            defaults.Add("ARGENTINA");
            defaults.Add("SISMO");

            return defaults;
        }


        private static List<string> CreateXmlFile(string fileName, string headFile, List<string> defaultList)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(fileName, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement(headFile);

            foreach (var word in defaultList)
            {
                writer.WriteElementString("word", word);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
            return defaultList;
        }


        private void ReadWords(List<string> defaults, string fileName)
        {
            XElement ele = XElement.Load(fileName);
            var forbidden = from w in XElement.Load(fileName).Elements("word") select w;

            // Execute the query 
            foreach (var word in forbidden)
            {
                defaults.Add(word.Value);
            }
        }
    }
}
