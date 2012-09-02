using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NetKeeper.Bll.Model;

namespace NetKeeper.Bll.Infrastructure
{
    public interface INetKeeperService
    {
        /// <summary>
        ///  添加分类
        /// </summary>
        /// <param name="catalogName"></param>
        Boolean AddCatalog(String catalogName);

        /// <summary>
        ///  添加网页
        /// </summary>
        /// <param name="catalogName"></param>
        /// <param name="webUrl"></param>
        void AddWeb(String catalogID, String webName, String webUrl, String webNote);

        /// <summary>
        ///  获得所有分类
        /// </summary>
        /// <returns></returns>
        List<NetKeeperNode> GetCatalog();

        /// <summary>
        ///  获取网页详情
        /// </summary>
        /// <param name="catalogID"></param>
        /// <param name="webName"></param>
        /// <param name="webUrl"></param>
        /// <returns></returns>
        WebModel GetWebInfo(String catalogID, String webID);

        /// <summary>
        ///  添加网页笔记
        /// </summary>
        /// <param name="catalogID"></param>
        /// <param name="webID"></param>
        /// <param name="webNote"></param>
        /// <returns></returns>
        WebModel ChangeNote(String catalogID, String webID, String webNote);
    }
}
