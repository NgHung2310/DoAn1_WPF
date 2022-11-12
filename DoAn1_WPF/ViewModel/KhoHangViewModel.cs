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
    public class KhoHangViewModel : BaseViewModel
    {
        private ObservableCollection<KHOHANG> list;
        public ObservableCollection<KHOHANG> List { get => list; set { list = value; OnPropertyChanged(); } }

        private KHOHANG selectedItem;
        public KHOHANG SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    MaKho = SelectedItem.MaKho;
                    TenKho = SelectedItem.TenKho;
                    DiaChiKho = SelectedItem.DiaChiKho;
                }
            }
        }

        private string maKho;
        public string MaKho
        {
            get => maKho;
            set
            {
                maKho = value;
                OnPropertyChanged();
            }
        }

        private string tenKho;
        public string TenKho
        {
            get => tenKho;
            set
            {
                tenKho = value;
                OnPropertyChanged();
            }
        }

        private string diaChiKho;
        public string DiaChiKho
        {
            get => diaChiKho;
            set
            {
                diaChiKho = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FindCommand { get; set; }
        public ICommand BackCommand { get; set; }        
        public KhoHangViewModel()
        {
            List = new ObservableCollection<KHOHANG>(DataProvider.Isn.DB.KHOHANGs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MaKho) || string.IsNullOrEmpty(TenKho) || string.IsNullOrEmpty(DiaChiKho))
                    return false;
                if (MaKho.Length > 5)
                    return false;
                var displayList = DataProvider.Isn.DB.KHOHANGs.Where(x => x.MaKho == MaKho);
                if (displayList.Count() > 0 || displayList == null)
                    return false;
                return true;
            }, (p) =>
            {
                KHOHANG kho = new KHOHANG()
                {
                    MaKho = MaKho,
                    TenKho = TenKho,
                    DiaChiKho = DiaChiKho
                };
                DataProvider.Isn.DB.KHOHANGs.Add(kho);
                DataProvider.Isn.DB.SaveChanges();
                List = new ObservableCollection<KHOHANG>(DataProvider.Isn.DB.KHOHANGs);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                if (string.IsNullOrEmpty(MaKho) || string.IsNullOrEmpty(TenKho) || string.IsNullOrEmpty(DiaChiKho))
                    return false;
                if (MaKho.Length > 5)
                    return false;              
                return true;
            }, (p) =>
            {
                string idOld = SelectedItem.MaKho;
                KHOHANG kho = DataProvider.Isn.DB.KHOHANGs.Where(x => x.MaKho == SelectedItem.MaKho).SingleOrDefault();
                try
                {
                    kho.MaKho = MaKho;
                    kho.TenKho = TenKho;
                    kho.DiaChiKho = DiaChiKho;
                    DataProvider.Isn.DB.SaveChanges();
                    SelectedItem.TenKho = TenKho;
                    SelectedItem.MaKho = MaKho;
                    SelectedItem.DiaChiKho = DiaChiKho;
                }
                catch
                {
                    MessageBox.Show("Mã kho hàng là thuộc tính quan trọng!!\nNếu đã nhập sai, hãy xóa đi và nhập lại", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                    kho.MaKho = idOld;
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
                    if (DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaKho == SelectedItem.MaKho).Count() > 0)
                    {
                        MessageBox.Show("Không thể xóa kho hàng này vì đã có phiếu nhập", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                        KHOHANG kho = DataProvider.Isn.DB.KHOHANGs.Where(x => x.MaKho == SelectedItem.MaKho).SingleOrDefault();
                        DataProvider.Isn.DB.KHOHANGs.Remove(kho);
                        DataProvider.Isn.DB.SaveChanges();
                        return;
                    }
                    if (DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaKho == SelectedItem.MaKho).Count() > 0)
                    {
                        MessageBox.Show("Không thể xóa kho hàng này vì đã có phiếu xuất", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                        KHOHANG kho = DataProvider.Isn.DB.KHOHANGs.Where(x => x.MaKho == SelectedItem.MaKho).SingleOrDefault();
                        DataProvider.Isn.DB.KHOHANGs.Remove(kho);
                        DataProvider.Isn.DB.SaveChanges();
                        return;
                    }
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Chắc không?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        KHOHANG kho = DataProvider.Isn.DB.KHOHANGs.Where(x => x.MaKho == SelectedItem.MaKho).SingleOrDefault();
                        DataProvider.Isn.DB.KHOHANGs.Remove(kho);
                        DataProvider.Isn.DB.SaveChanges();
                        List = new ObservableCollection<KHOHANG>(DataProvider.Isn.DB.KHOHANGs);
                    }
                }
                catch { }
            });

            FindCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MaKho) && string.IsNullOrEmpty(TenKho) && string.IsNullOrEmpty(DiaChiKho))
                    return false;
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<KHOHANG>(DataProvider.Isn.DB.KHOHANGs.Where(x => x.MaKho.Contains(MaKho) || x.TenKho.Contains(TenKho) || x.DiaChiKho.Contains(DiaChiKho)));
            });

            BackCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<KHOHANG>(DataProvider.Isn.DB.KHOHANGs);
                MaKho = null;
                TenKho = null;
                DiaChiKho = null; 
            });
        }
    }
}
