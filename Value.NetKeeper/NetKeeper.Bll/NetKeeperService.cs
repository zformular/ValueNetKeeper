using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetKeeper.Bll.Infrastructure;
using NetKeeper.DAl;
using NetKeeper.Bll.Model;

namespace NetKeeper.Bll
{
    public class NetKeeperService : INetKeeperService
    {
        NetKeeperDAO netKeeperDAO = new NetKeeperDAO();

        #region INetKeeperService 成员

        public Boolean AddCatalog(string catalogName)
        {
            return netKeeperDAO.AddCatalog(catalogName);
        }

        public void AddWeb(string catalogID, string webName, string webUrl, string webNote)
        {
            netKeeperDAO.AddWeb(catalogID, webName, webUrl, webNote);
        }

        public List<NetKeeperNode> GetCatalog()
        {
            var catalogs = netKeeperDAO.GetCatalog();
            var catalogList = new List<NetKeeperNode>();
            foreach (var catalog in catalogs)
            {
                var webs = netKeeperDAO.GetWebList(catalog[0]);
                var webList = new List<NetKeeperNode>();
                foreach (var web in webs)
                {
                    webList.Add(new NetKeeperNode
                    {
                        ID = web[0],
                        Name = web[1],
                        PID = catalog[0]
                    });
                }

                catalogList.Add(new NetKeeperNode
                {
                    ID = catalog[0],
                    Name = catalog[1],
                    Child = webList
                });
            }

            return catalogList;
        }

        public WebModel GetWebInfo(string catalogID, string webID)
        {
            var webs = netKeeperDAO.GetWebInfo(catalogID, webID);
            var web = new WebModel
            {
                ID = webs[0],
                Name = webs[1],
                Url = webs[2],
                Note = webs[3]
            };

            return web;
        }

        public WebModel ChangeNote(string catalogID, string webID, string webNote)
        {
            netKeeperDAO.ChangeNote(catalogID, webID, webNote);

            return this.GetWebInfo(catalogID, webID);
        }

        #endregion
    }
}
