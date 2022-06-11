using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClipboardHistory
{
    [Serializable]
    public class WaitProcessItem
    {
        public string priority;
        public DateTime beginTime;
        public DateTime endTime;
        public string content;

    }
}
