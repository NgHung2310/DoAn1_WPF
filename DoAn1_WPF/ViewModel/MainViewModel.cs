using DoAn1_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DoAn1_WPF.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        private ObservableCollection<TonKho> tonKhoList;
        public ObservableCollection<TonKho> TonKhoList { get => tonKhoList; set { tonKhoList = value; OnPropertyChanged(); } }
        
        public bool IsLoaded { get; set; } = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand UnitCommand { get; set; }
        public ICommand SuplierCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ObjectCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand WarehouseCommand { get; set; }
        public ICommand InputCommand { get; set; }
        public ICommand OutputCommand { get; set; }

        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                if (p == null) return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                if (loginWindow.DataContext == null) return;
                var loginVM = loginWindow.DataContext as LoginViewModel;
                if (loginVM.IsLogin)
                {
                    p.Show();
                    LoadTonKhoData();
                }
                else p.Close();
            });

            UnitCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                DanhMucHang wd = new DanhMucHang();
                wd.ShowDialog();
            });

            SuplierCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                NhaCungCap wd = new NhaCungCap();
                wd.ShowDialog();
            });

            CustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                KhachHang wd = new KhachHang();
                wd.ShowDialog();
            });

            ObjectCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                HangHoa wd = new HangHoa();
                wd.ShowDialog();
            });

            UserCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                NguoiDung wd = new NguoiDung();
                wd.ShowDialog();
            });

            WarehouseCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                KhoHang wd = new KhoHang();
                wd.ShowDialog();
            });

            InputCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                NhapHang wd = new NhapHang();
                wd.ShowDialog();
            });

            OutputCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                IsLoaded = true;
                XuatHang wd = new XuatHang();
                wd.ShowDialog();
            });
        }
        
        void LoadTonKhoData()
        {
            TonKhoList = new ObservableCollection<TonKho>();
            var objiectList = DataProvider.Isn.DB.HANGHOAs;
            int i = 1;            
            foreach (var item in objiectList)
            {
                try 
                {
                    var inputList = DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaHang == item.MaHang);
                    var outputList = DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaHang == item.MaHang);
                    int sumInput = 0;
                    int sumOutput = 0;
                    if (inputList != null)
                    {
                        sumInput = (int)inputList.Sum(x => x.SLNhap);
                    }
                    if (outputList != null)
                    {
                        sumOutput = (int)outputList.Sum(x => x.SLXuat);
                    }
                    TonKho tonKho = new TonKho();
                    tonKho.STT = i;
                    tonKho.SoLuong = sumInput - sumOutput;
                    tonKho.HangHoa = item;
                    TonKhoList.Add(tonKho);
                    i++;
                }
                catch { }
                
            }

        }
    }
}
