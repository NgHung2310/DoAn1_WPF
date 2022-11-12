using DoAn1_WPF.Model;
using DoAn1_WPF.UserControlBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DoAn1_WPF.ViewModel
{
    public class NguoiDungViewModel : BaseViewModel
    {
        private ObservableCollection<TAIKHOAN> list;
        public ObservableCollection<TAIKHOAN> List { get => list; set { list = value; OnPropertyChanged(); } }
        
        private ObservableCollection<PHANQUYEN> chucVu;
        public ObservableCollection<PHANQUYEN> ChucVu { get => chucVu; set { chucVu = value; OnPropertyChanged(); } }

        private TAIKHOAN selectedItem;
        public TAIKHOAN SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    ID = SelectedItem.ID;
                    TenDangNhap = SelectedItem.TaiKhoan1;
                    SelectedChucVu = SelectedItem.PHANQUYEN;
                    Ten = SelectedItem.Ten;                    
                }
            }
        }
        private int id;
        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private string tenDangNhap;
        public string TenDangNhap
        {
            get => tenDangNhap;
            set
            {
                tenDangNhap = value;
                OnPropertyChanged();
            }
        }

        private string ten;
        public string Ten
        {
            get => ten;
            set
            {
                ten = value;
                OnPropertyChanged();
            }
        }

        private PHANQUYEN selectedChucVu;
        public PHANQUYEN SelectedChucVu
        {
            get => selectedChucVu;
            set
            {
                selectedChucVu = value;
                OnPropertyChanged();
            }
        }

        private string changePassword;
        public string ChangePassword
        {
            get => changePassword;
            set
            {
                changePassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public NguoiDungViewModel()
        {
            List = new ObservableCollection<TAIKHOAN>(DataProvider.Isn.DB.TAIKHOANs);
            ChucVu = new ObservableCollection<PHANQUYEN>(DataProvider.Isn.DB.PHANQUYENs);
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (AccountStaticClass.User.UserRole.ID != 1)
                    return false;
                if (string.IsNullOrEmpty(TenDangNhap) || string.IsNullOrEmpty(Ten) || SelectedChucVu == null)
                    return false;
                var displayList = DataProvider.Isn.DB.TAIKHOANs.Where(x => x.TaiKhoan1 == TenDangNhap);
                if (displayList.Count() > 0 || displayList == null)
                    return false;
                return true;
            }, (p) =>
            {
                var taikhoan = new TAIKHOAN()
                {
                    TaiKhoan1 = TenDangNhap,
                    MatKhau = TenDangNhap,
                    Ten = Ten,
                    PHANQUYEN = SelectedChucVu
                };
                DataProvider.Isn.DB.TAIKHOANs.Add(taikhoan);
                DataProvider.Isn.DB.SaveChanges();
                List = new ObservableCollection<TAIKHOAN>(DataProvider.Isn.DB.TAIKHOANs);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (AccountStaticClass.User.UserRole.ID != 1)
                    return false;
                if (string.IsNullOrEmpty(TenDangNhap) || string.IsNullOrEmpty(Ten) || SelectedChucVu == null)
                    return false;
                var displayList = DataProvider.Isn.DB.TAIKHOANs.Where(x => x.TaiKhoan1 == TenDangNhap);
                if (displayList.Count() > 0 || displayList == null)
                    return false;
                return true;
            }, (p) =>
            {
                var taikhoan = DataProvider.Isn.DB.TAIKHOANs.Where(x => x.ID == ID).SingleOrDefault();
                if (taikhoan != null)
                {
                    taikhoan.TaiKhoan1 = TenDangNhap;
                    taikhoan.MatKhau = TenDangNhap;
                    taikhoan.Ten = Ten;
                    taikhoan.PHANQUYEN = SelectedChucVu;
                    DataProvider.Isn.DB.SaveChanges();
                    List = new ObservableCollection<TAIKHOAN>(DataProvider.Isn.DB.TAIKHOANs);
                }
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (AccountStaticClass.User.UserRole.ID != 1)
                    return false;
                if (string.IsNullOrEmpty(ID.ToString()))
                    return false;
                return true;
            }, (p) =>
            {
                try
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Chắc không?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var taikhoan = DataProvider.Isn.DB.TAIKHOANs.Where(x => x.ID == ID).SingleOrDefault();
                        if (taikhoan != null)
                        {
                            DataProvider.Isn.DB.TAIKHOANs.Remove(taikhoan);
                            DataProvider.Isn.DB.SaveChanges();
                            List = new ObservableCollection<TAIKHOAN>(DataProvider.Isn.DB.TAIKHOANs);
                        }
                    }
                }
                catch { }
            });
            
            ChangePasswordCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                AccountDialog dl = new AccountDialog();
                dl.ShowDialog();
            });

            SubmitCommand = new RelayCommand<Window>((p) =>
            {
                if (String.IsNullOrEmpty(ChangePassword))
                    return false;
                return true;
            }, (p) =>
            {
                var idCurrentUser = AccountStaticClass.User.UserAccount.ID;
                var currentUser = DataProvider.Isn.DB.TAIKHOANs.Single(x => x.ID == idCurrentUser);
                currentUser.MatKhau = ChangePassword;
                MessageBox.Show("Đổi mật khẩu thành công");
                DataProvider.Isn.DB.SaveChanges();
                p.Close();
            });

            CancelCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.Close();
            });
        }

    }
}
