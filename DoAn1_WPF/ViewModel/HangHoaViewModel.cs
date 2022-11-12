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
    public class HangHoaViewModel : BaseViewModel
    {
        private ObservableCollection<HANGHOA> list;
        public ObservableCollection<HANGHOA> List { get => list; set { list = value; OnPropertyChanged(); } }
        
        private ObservableCollection<DANHMUCHANG> danhMucHang;
        public ObservableCollection<DANHMUCHANG> DanhMucHang { get => danhMucHang; set { danhMucHang = value;OnPropertyChanged(); } }

        private HANGHOA selectedItem;
        public HANGHOA SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    MaHang = SelectedItem.MaHang;
                    TenHang = SelectedItem.TenHang;
                    DonViTinh = SelectedItem.DonViTinh;
                    DonGia = SelectedItem.DonGia;
                    SoLuong = SelectedItem.SoLuong;
                    SelectedDanhMucHang = SelectedItem.DANHMUCHANG;
                }
            }
        }
     
        private string maHang;
        public string MaHang
        {
            get => maHang;
            set
            {
                maHang = value;
                OnPropertyChanged();
            }
        }
        
        private string tenHang;
        public string TenHang
        {
            get => tenHang;
            set
            {
                tenHang = value;
                OnPropertyChanged();
            }
        }

        private string donViTinh;
        public string DonViTinh
        {get => donViTinh;
            set
            {
                donViTinh = value;
                OnPropertyChanged();
            }
        }

        private int? soLuong;
        public int? SoLuong
        {
            get => soLuong;
            set
            {
                soLuong = value;
                OnPropertyChanged();
            }
        }

        private double? donGia;
        public double? DonGia
        {
            get => donGia;
            set
            {
                donGia = value;
                OnPropertyChanged();
            }
        }

        

        private DANHMUCHANG selectedDanhMucHang;
        public DANHMUCHANG SelectedDanhMucHang
        {
            get => selectedDanhMucHang;
            set
            {
                selectedDanhMucHang = value; OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FindCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public HangHoaViewModel()
        {
            List = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
            DanhMucHang = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MaHang) || string.IsNullOrEmpty(TenHang) || string.IsNullOrEmpty(DonViTinh) || SoLuong == null || DonGia == null || SelectedDanhMucHang == null)
                    return false;
                if (MaHang.Length > 5)
                    return false;
                var displayList = DataProvider.Isn.DB.HANGHOAs.Where(x => x.MaHang == MaHang);
                if (displayList.Count() > 0 || displayList == null)
                    return false;
                return true;
            }, (p) =>
            {
                try 
                {
                    var hhoa = new HANGHOA();
                    hhoa.MaHang = MaHang;
                    hhoa.TenHang = TenHang;
                    hhoa.DonViTinh = DonViTinh;
                    hhoa.SoLuong = SoLuong;
                    hhoa.DonGia = DonGia;
                    hhoa.MaLoaiHang = SelectedDanhMucHang.MaLoaiHang;
                    DataProvider.Isn.DB.HANGHOAs.Add(hhoa);
                    DataProvider.Isn.DB.SaveChanges();
                    List = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
                }
                catch { }
            });
            
            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                if (string.IsNullOrEmpty(TenHang) || string.IsNullOrEmpty(MaHang))
                    return false;
                if (MaHang.Length > 5)
                    return false;               
                return true;
            }, (p) =>
            {
                string idOld = SelectedItem.MaHang;
                var hhoa = DataProvider.Isn.DB.HANGHOAs.Where(x => x.MaHang == SelectedItem.MaHang).SingleOrDefault();
                try
                {  
                    hhoa.MaHang = MaHang;
                    hhoa.TenHang = TenHang;
                    hhoa.DonViTinh = DonViTinh;
                    hhoa.SoLuong = SoLuong;
                    hhoa.DonGia = DonGia;
                    hhoa.MaLoaiHang = SelectedDanhMucHang.MaLoaiHang;
                    DataProvider.Isn.DB.SaveChanges();
                    List = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
                }
                catch
                {
                    MessageBox.Show("Mã hàng là thuộc tính quan trọng!!\nNếu đã nhập sai, hãy xóa đi và nhập lại", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                    hhoa.MaHang = idOld;
                }
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                try
                {
                    if (DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaHang == SelectedItem.MaHang).Count() > 0)
                    {
                        MessageBox.Show("Không thể xóa hàng hóa này vì đã có phiếu nhập", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                        var hhoa = DataProvider.Isn.DB.HANGHOAs.Where(x => x.MaHang == SelectedItem.MaHang).SingleOrDefault();
                        DataProvider.Isn.DB.HANGHOAs.Remove(hhoa);
                        DataProvider.Isn.DB.SaveChanges(); 
                        return;
                    }
                    if (DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaHang == SelectedItem.MaHang).Count() > 0)
                    {
                        MessageBox.Show("Không thể xóa hàng hóa này vì đã có phiếu xuất", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                        var hhoa = DataProvider.Isn.DB.HANGHOAs.Where(x => x.MaHang == SelectedItem.MaHang).SingleOrDefault();
                        DataProvider.Isn.DB.HANGHOAs.Remove(hhoa);
                        DataProvider.Isn.DB.SaveChanges(); 
                        return;
                    }
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Chắc không?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var hhoa = DataProvider.Isn.DB.HANGHOAs.Where(x => x.MaHang == SelectedItem.MaHang).SingleOrDefault();
                        DataProvider.Isn.DB.HANGHOAs.Remove(hhoa);
                        DataProvider.Isn.DB.SaveChanges();
                        List = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
                    }
                }
                catch 
                {
                    List = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
                }
            });

            FindCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TenHang) && string.IsNullOrEmpty(MaHang))
                    return false;
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs.Where(x => x.TenHang.Contains(TenHang) || x.MaHang.Contains(MaHang)));
            });

            BackCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                DanhMucHang = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs);
                List = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
                MaHang = null;
                TenHang = null;
                DonViTinh = null;
                SoLuong = null;
                DonGia = null;
            });
        }
    }
}
