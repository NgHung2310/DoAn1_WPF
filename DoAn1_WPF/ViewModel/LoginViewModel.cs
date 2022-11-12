using DoAn1_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DoAn1_WPF.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        private string userName;
        public string UserName { get => userName; set { userName = value; OnPropertyChanged(); } }
        private string passWord;
        public string PassWord { get => passWord; set { passWord = value; OnPropertyChanged(); } }
        public LoginViewModel()
        {
            UserName = "";
            PassWord = "";
            IsLogin = false;
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Login(p);
            });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { PassWord = p.Password; });

        }

        void Login(Window p)
        {
            //admin
            if (p == null) return;
            string passEnCode = PassWord;
            if (DataProvider.Isn.DB.TAIKHOANs.Where(a => a.TaiKhoan1 == UserName && a.MatKhau == passEnCode).Count()>0)
            {
                AccountStaticClass.User.UserRole = DataProvider.Isn.DB.TAIKHOANs.Single(e => e.TaiKhoan1 == UserName).PHANQUYEN;
                AccountStaticClass.User.UserAccount = DataProvider.Isn.DB.TAIKHOANs.Single(e => e.TaiKhoan1 == UserName);
                IsLogin = true;
                p.Close();
            }
            else
            {
                IsLogin = false;
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu");
            }
            //IsLogin = true;
            //p.Close();
        }

        //public static string Base64Encode(string plainText)
        //{
        //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        //    return System.Convert.ToBase64String(plainTextBytes);
        //}

        //public static string MD5Hash(string input)
        //{
        //    StringBuilder hash = new StringBuilder();
        //    MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        //    byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        hash.Append(bytes[i].ToString("x2"));
        //    }
        //    return hash.ToString();
        //}
    }
}
