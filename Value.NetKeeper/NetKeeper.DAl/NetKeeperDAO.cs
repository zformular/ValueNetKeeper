using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NetKeeper.Commom;
using System.IO;
using System.Xml;

namespace NetKeeper.DAl
{
    public class NetKeeperDAO
    {
        public NetKeeperDAO()
        {
            this.CreateXML();
        }

        public String[][] GetCatalog()
        {
            var dataXml = loadXML();
            var catalogs = dataXml.Elements("Catalog").ToArray();
            var result = new String[catalogs.Length][];
            for (int index = 0; index < catalogs.Length; index++)
            {
                var cataInfo = new String[2];
                cataInfo[0] = catalogs[index].Attribute("ID").Value;
                cataInfo[1] = catalogs[index].Attribute("Name").Value;
                result[index] = cataInfo;
            }
            return result;
        }

        public String[][] GetWebList(String catalogID)
        {
            var dataXml = loadXML();
            var catalogs = dataXml.Elements("Catalog");
            var catalog = (from cataInfo in catalogs
                           where cataInfo.Attribute("ID").Value == catalogID
                           select cataInfo).FirstOrDefault();

            var webList = catalog.Elements("Web").ToArray();
            var result = new String[webList.Length][];
            for (int index = 0; index < webList.Length; index++)
            {
                var web = new String[4];
                web[0] = webList[index].Attribute("ID").Value;
                web[1] = webList[index].Attribute("Name").Value;
                result[index] = web;
            }
            return result;
        }

        public String[] GetWebInfo(String catalogID, String webID)
        {
            var dataXml = loadXML();
            var catalogs = dataXml.Elements("Catalog");
            var catalog = (from cataInfo in catalogs
                           where cataInfo.Attribute("ID").Value == catalogID
                           select cataInfo).FirstOrDefault();
            var webs = catalog.Elements("Web");
            var web = (from webInfo in webs
                       where webInfo.Attribute("ID").Value == webID
                       select webInfo).FirstOrDefault();

            var result = new String[4];
            result[0] = web.Attribute("ID").Value;
            result[1] = web.Attribute("Name").Value;
            result[2] = web.Attribute("Url").Value;
            result[3] = web.Element("Note").Value;
            return result;
        }

        public Boolean AddCatalog(String catalogName)
        {
            var dataXml = loadXML();
            var catalogs = dataXml.Elements("Catalog");
            var catalog = (from cataInfo in catalogs
                           where cataInfo.Attribute("Name").Value == catalogName
                           select cataInfo).FirstOrDefault();
            if (catalog != null)
                return false;

            var catalogID = catalogs.Count() + 1;

            XElement catalogxml = new XElement("Catalog",
                new XAttribute("ID", catalogID),
                new XAttribute("Name", catalogName));
            dataXml.Add(catalogxml);

            SaveDataXML(dataXml);
            return true;
        }

        public void AddWeb(String catalogID, String webName, String webUrl, String webNote)
        {
            var dataXml = loadXML();
            var catalogs = dataXml.Elements("Catalog");
            var catalog = (from cataInfo in catalogs
                           where cataInfo.Attribute("ID").Value == catalogID
                           select cataInfo).FirstOrDefault();

            if (catalog != null)
            {
                var webID = catalog.Elements("Web").Count() + 1;
                XElement web = new XElement("Web",
                    new XAttribute("ID", webID),
                    new XAttribute("Name", webName),
                    new XAttribute("Url", webUrl),
                    new XElement("Note", webNote));
                catalog.Add(web);

                SaveDataXML(dataXml);
            }
        }

        public void ChangeNote(String catalogID, String webID, String webNote)
        {
            var dataXml = loadXML();
            var catalogs = dataXml.Elements("Catalog");
            var catalog = (from cataInfo in catalogs
                           where cataInfo.Attribute("ID").Value == catalogID
                           select cataInfo).FirstOrDefault();
            if (catalog != null)
            {
                var webs = catalog.Elements("Web");
                var web = (from webInfo in webs
                           where webInfo.Attribute("ID").Value == webID
                           select webInfo).FirstOrDefault();
                if (web != null)
                {
                    web.Element("Note").Value = webNote;

                    SaveDataXML(dataXml);
                }
            }
        }

        private void CreateXML()
        {
            if (!File.Exists(CommonVar.FileName))
            {
                var filePath = getFilePath(CommonVar.FileName);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                XDocument xmlDoc = new XDocument(new XElement("Webs"));
                xmlDoc.Save(CommonVar.FileName);
            }
        }

        private XElement loadXML()
        {
            try
            {
                XElement xml = XElement.Load(CommonVar.FileName);
                return xml;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void SaveDataXML(XElement dataXml)
        {
            dataXml.Save(CommonVar.FileName);
        }

        private String getFilePath(String fileFullName)
        {
            var fileName = Path.GetFileName(fileFullName);
            var filePath = fileFullName.Substring(0, fileFullName.Length - fileName.Length - 1);
            return filePath;
        }
    }
}
