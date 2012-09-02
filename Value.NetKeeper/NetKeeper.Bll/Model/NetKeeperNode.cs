using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetKeeper.Bll.Model
{
    public class NetKeeperNode
    {
        public String PID { get; set; }

        public String ID { get; set; }

        public String Name { get; set; }

        public String Url { get; set; }

        public String Note { get; set; }

        public List<NetKeeperNode> Child { get; set; }
    }
}
