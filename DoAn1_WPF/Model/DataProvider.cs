using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1_WPF.Model
{
    public class DataProvider
    {
        private static DataProvider _isn;
        public static DataProvider Isn
        {
            get
            {
                if (_isn == null)
                    _isn = new DataProvider();
                return _isn;
            }
            set
            {
                _isn = value;
            }
        }
        public DoAn1_WPFEntities DB { get; set; }
        private DataProvider()
        {
            DB = new DoAn1_WPFEntities();
        }

    }
}
