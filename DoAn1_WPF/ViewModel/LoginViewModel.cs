using DoAn1_WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangeCommand { get; set; }
        private string userName;
        public string UserName { get=>userName; set { userName = value;OnPropertyChanged(); } }
        private string passWord;
        public string PassWord { get => passWord; set { passWord = value; OnPropertyChanged(); } }
        public LoginViewModel()
        {
            IsLogin = false;
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Login(p);
            });
            PasswordChangeCommand = new RelayCommand<PasswordBox>((P) => { return true; }, (p) =>
            {
                PassWord = p.Password;
            });
        }

        void Login(Window p)
        {
            //admin
            //DataProvider.Isn.DB.TAIKHOANs.Where(a=>a.TaiKhoan)
            if (p == null) return;
            IsLogin = true;
            p.Close();
        }
    }
}
