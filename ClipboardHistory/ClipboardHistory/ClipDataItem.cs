using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections.Specialized;

namespace ClipboardHistory
{
    public class ClipDataItem
    {
        public DateTime date;
        public string type;
        public object data;
        public string comment;
        public bool locked = false;
        public ClipDataItem() { }
        public ClipDataItem(IDataObject obj)
        {
            this.date = DateTime.Now;
            resolveDataObject(obj);
        }
        public ClipDataItem(IDataObject obj, DateTime date)
        {
            this.date = date;
            resolveDataObject(obj);
        }

        public void resolveDataObject(IDataObject obj)
        {
            string[] types = { 
                                     DataFormats.Text,
                                     DataFormats.UnicodeText,
                                     DataFormats.Html, 
                                     DataFormats.Bitmap,
                                     DataFormats.WaveAudio,
                                     DataFormats.CommaSeparatedValue,
                                     DataFormats.Dib,
                                     DataFormats.Dif,
                                     DataFormats.EnhancedMetafile,
                                     DataFormats.FileDrop,
                                     DataFormats.Locale,
                                     DataFormats.MetafilePict,
                                     DataFormats.OemText,
                                     DataFormats.Palette,
                                     DataFormats.PenData,
                                     DataFormats.Riff,
                                     DataFormats.Rtf,
                                     DataFormats.Serializable,
                                     DataFormats.StringFormat,
                                     DataFormats.SymbolicLink,
                                     DataFormats.Tiff
                                 };
            for (int i = 0; i < types.Length; i++)
            {
                if (resolveType(obj, types[i]))
                {
                    break;
                }
            }

        }
        public bool resolveType(IDataObject obj, string fmt)
        {
            if (obj.GetDataPresent(fmt))
            {
                this.type = fmt;
                this.data = obj.GetData(this.type);
                return true;
            }
            return false;
        }
    }
}
