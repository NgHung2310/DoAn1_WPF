using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1_WPF.Model
{
    public class AccountStaticClass
    {
        public static AccountStaticClass user;
        public static AccountStaticClass User
        {
            get 
            {
                if (user == null) user = new AccountStaticClass();                
                return user;
            }
            set { user = value; }
        }

        public PHANQUYEN UserRole { get; set; }
        public TAIKHOAN UserAccount { get; set; }
    }
}
