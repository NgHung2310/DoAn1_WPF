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
    public class KhachHangViewModel : BaseViewModel
    {
        private ObservableCollection<KHACHHANG> list;
        public ObservableCollection<KHACHHANG> List { get => list; set { list = value; OnPropertyChanged(); } }

        private KHACHHANG selectedItem;
        public KHACHHANG SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    MaKH = SelectedItem.MaKH;
                    HoTenKH = SelectedItem.HoTenKH;
                    DiaChiKH = SelectedItem.DiaChiKH;
                    SdtKH = SelectedItem.SdtKH;
                }
            }
        }
        private string maKH;
        public string MaKH
        {
            get => maKH;
            set
            {
                maKH = value;
                OnPropertyChanged();
            }
        }

        private string hoTenKH;
        public string HoTenKH
        {
            get => hoTenKH;
            set
            {
                hoTenKH = value;
                OnPropertyChanged();
            }
        }

        private string diaChiKH;
        public string DiaChiKH
        {
            get => diaChiKH;
            set
            {
                diaChiKH = value;
                OnPropertyChanged();
            }
        }
        private string sdtKH;
        public string SdtKH
        {
            get => sdtKH;
            set
            {
                sdtKH = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FindCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public KhachHangViewModel()
        {
            List = new ObservableCollection<KHACHHANG>(DataProvider.Isn.DB.KHACHHANGs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MaKH) || string.IsNullOrEmpty(HoTenKH) || string.IsNullOrEmpty(SdtKH) || string.IsNullOrEmpty(DiaChiKH))
                    return false;
                if (MaKH.Length > 5)
                    return false;
                var displayList = DataProvider.Isn.DB.KHACHHANGs.Where(x => x.MaKH == MaKH);
                if (displayList.Count() > 0 || displayList == null)
                    return false;
                return true;
            }, (p) =>
            {
                KHACHHANG kh = new KHACHHANG()
                {
                    MaKH = MaKH,
                    HoTenKH = HoTenKH,
                    DiaChiKH = DiaChiKH,
                    SdtKH = SdtKH
                };
                DataProvider.Isn.DB.KHACHHANGs.Add(kh);
                DataProvider.Isn.DB.SaveChanges();
                List = new ObservableCollection<KHACHHANG>(DataProvider.Isn.DB.KHACHHANGs);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                if (string.IsNullOrEmpty(MaKH) || string.IsNullOrEmpty(HoTenKH) || string.IsNullOrEmpty(SdtKH) || string.IsNullOrEmpty(DiaChiKH))
                    return false;
                if (MaKH.Length > 5)
                    return false;               
                return true;
            }, (p) =>
            {
                string idOld = SelectedItem.MaKH;
                KHACHHANG kh = DataProvider.Isn.DB.KHACHHANGs.Where(x => x.MaKH == SelectedItem.MaKH).SingleOrDefault();
                try
                {
                    kh.MaKH = MaKH;
                    kh.HoTenKH = HoTenKH;
                    kh.DiaChiKH = DiaChiKH;
                    kh.SdtKH = SdtKH;
                    DataProvider.Isn.DB.SaveChanges();
                    SelectedItem.MaKH = MaKH;
                    SelectedItem.HoTenKH = HoTenKH;
                    SelectedItem.DiaChiKH = DiaChiKH;
                    SelectedItem.SdtKH = SdtKH;
                }
                catch
                {
                    MessageBox.Show("Mã khách hàng là thuộc tính quan trọng!!\nNếu đã nhập sai, hãy xóa đi và nhập lại", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                    kh.MaKH = idOld;
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
                    if (DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaKH == SelectedItem.MaKH).Count() > 0)
                    {
                        MessageBox.Show("Khách hàng này đang trong danh sách xuất hàng, không thể xóa", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                        var kh = DataProvider.Isn.DB.KHACHHANGs.Where(x => x.MaKH == SelectedItem.MaKH).SingleOrDefault();
                        DataProvider.Isn.DB.KHACHHANGs.Remove(kh);
                        DataProvider.Isn.DB.SaveChanges(); 
                        return;
                    }
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Chắc không?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var kh = DataProvider.Isn.DB.KHACHHANGs.Where(x => x.MaKH == SelectedItem.MaKH).SingleOrDefault();
                        DataProvider.Isn.DB.KHACHHANGs.Remove(kh);
                        DataProvider.Isn.DB.SaveChanges();
                        //List.Remove(KHACHHANG);
                        List = new ObservableCollection<KHACHHANG>(DataProvider.Isn.DB.KHACHHANGs);
                    }
                }
                catch 
                {
                    List = new ObservableCollection<KHACHHANG>(DataProvider.Isn.DB.KHACHHANGs);
                }
            });

            FindCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MaKH) && string.IsNullOrEmpty(HoTenKH) && string.IsNullOrEmpty(SdtKH) && string.IsNullOrEmpty(DiaChiKH))
                    return false;
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<KHACHHANG>(DataProvider.Isn.DB.KHACHHANGs.Where(x => x.MaKH.Contains(MaKH) || x.HoTenKH.Contains(DiaChiKH) || x.DiaChiKH.Contains(DiaChiKH) || x.SdtKH.Contains(SdtKH)));
            });

            BackCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<KHACHHANG>(DataProvider.Isn.DB.KHACHHANGs);
                MaKH = null;
                HoTenKH = null;
                DiaChiKH = null;
                SdtKH = null;
            });
        }
    }
}
