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
    public class NhaCungCapViewModel : BaseViewModel
    {
        private ObservableCollection<NHACUNGCAP> list;
        public ObservableCollection<NHACUNGCAP> List { get => list; set { list = value; OnPropertyChanged(); } }

        private NHACUNGCAP selectedItem;
        public NHACUNGCAP SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    MaNCC = SelectedItem.MaNCC;
                    TenNCC = SelectedItem.TenNCC;
                    DiaChiNCC = SelectedItem.DiaChiNCC;
                    SdtNCC = SelectedItem.SdtNCC;
                    MailNCC = SelectedItem.MailNCC;
                }
            }
        }
        private string maNCC;
        public string MaNCC
        {
            get => maNCC;
            set
            {
                maNCC = value;
                OnPropertyChanged();
            }
        }
        
        private string tenNCC;
        public string TenNCC
        {
            get => tenNCC;
            set
            {
                tenNCC = value;
                OnPropertyChanged();
            }
        }

        private string diaChiNCC;
        public string DiaChiNCC
        {
            get => diaChiNCC;
            set
            {
                diaChiNCC = value;
                OnPropertyChanged();
            }
        }
        private string sdtNCC;
        public string SdtNCC
        {
            get => sdtNCC;
            set
            {
                sdtNCC = value;
                OnPropertyChanged();
            }
        }
        private string mailNCC;
        public string MailNCC
        {
            get => mailNCC;
            set
            {
                mailNCC = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FindCommand { get; set; }
        public ICommand BackCommand { get; set; }
        
        public NhaCungCapViewModel()
        {
            List = new ObservableCollection<NHACUNGCAP>(DataProvider.Isn.DB.NHACUNGCAPs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MaNCC) || string.IsNullOrEmpty(TenNCC) || string.IsNullOrEmpty(DiaChiNCC) || string.IsNullOrEmpty(SdtNCC) || string.IsNullOrEmpty(MailNCC))
                    return false;
                if (MaNCC.Length > 5)
                    return false;
                var displayList = DataProvider.Isn.DB.NHACUNGCAPs.Where(x => x.MaNCC == MaNCC);
                if (displayList.Count() > 0 || displayList == null)
                    return false;
                return true;
            }, (p) =>
            {
                NHACUNGCAP NCC = new NHACUNGCAP()
                {
                    MaNCC = MaNCC,
                    TenNCC = TenNCC,
                    DiaChiNCC = DiaChiNCC,
                    SdtNCC = SdtNCC,
                    MailNCC = MailNCC
                };
                DataProvider.Isn.DB.NHACUNGCAPs.Add(NCC);
                DataProvider.Isn.DB.SaveChanges();
                List = new ObservableCollection<NHACUNGCAP>(DataProvider.Isn.DB.NHACUNGCAPs);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                if (string.IsNullOrEmpty(MaNCC) || string.IsNullOrEmpty(TenNCC) || string.IsNullOrEmpty(DiaChiNCC) || string.IsNullOrEmpty(SdtNCC) || string.IsNullOrEmpty(MailNCC))
                    return false;
                if (MaNCC.Length > 5)
                    return false;
                return true;
            }, (p) =>
            {
                string idOld = SelectedItem.MaNCC;
                NHACUNGCAP NCC = DataProvider.Isn.DB.NHACUNGCAPs.Where(x => x.MaNCC == SelectedItem.MaNCC).SingleOrDefault();
                try
                {
                    NCC.MaNCC = MaNCC;
                    NCC.TenNCC = TenNCC;
                    NCC.DiaChiNCC = DiaChiNCC;
                    NCC.SdtNCC = SdtNCC;
                    NCC.MailNCC = MailNCC;
                    DataProvider.Isn.DB.SaveChanges();
                    SelectedItem.TenNCC = TenNCC;
                    SelectedItem.MaNCC = MaNCC;
                    SelectedItem.DiaChiNCC = DiaChiNCC;
                    SelectedItem.SdtNCC = SdtNCC;
                    SelectedItem.MailNCC = MailNCC;
                }
                catch
                {
                    MessageBox.Show("Mã nhà cung cấp là thuộc tính quan trọng!!\nNếu đã nhập sai, hãy xóa đi và nhập lại", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                    NCC.MaNCC = idOld;
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
                    if (DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaNCC == SelectedItem.MaNCC).Count() > 0)
                    {
                        MessageBox.Show("Nhà cung cấp này đang được sử dụng trong phiếu nhập hàng, không thể xóa", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                        NHACUNGCAP NCC = DataProvider.Isn.DB.NHACUNGCAPs.Where(x => x.MaNCC == SelectedItem.MaNCC).SingleOrDefault();
                        DataProvider.Isn.DB.NHACUNGCAPs.Remove(NCC);
                        DataProvider.Isn.DB.SaveChanges();
                        return;
                    }
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Chắc không?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        NHACUNGCAP NCC = DataProvider.Isn.DB.NHACUNGCAPs.Where(x => x.MaNCC == SelectedItem.MaNCC).SingleOrDefault();
                        DataProvider.Isn.DB.NHACUNGCAPs.Remove(NCC);
                        DataProvider.Isn.DB.SaveChanges();
                        List = new ObservableCollection<NHACUNGCAP>(DataProvider.Isn.DB.NHACUNGCAPs);
                    }
                }
                catch 
                {
                        List = new ObservableCollection<NHACUNGCAP>(DataProvider.Isn.DB.NHACUNGCAPs);
                }
            });

            FindCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(MaNCC) && string.IsNullOrEmpty(TenNCC) && string.IsNullOrEmpty(DiaChiNCC) && string.IsNullOrEmpty(SdtNCC) && string.IsNullOrEmpty(MailNCC))
                    return false;
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<NHACUNGCAP>(DataProvider.Isn.DB.NHACUNGCAPs.Where(x => x.TenNCC.Contains(TenNCC) || x.MaNCC.Contains(MaNCC) || x.DiaChiNCC.Contains(DiaChiNCC) || x.SdtNCC.Contains(SdtNCC) || x.MailNCC.Contains(MailNCC)));
            });

            BackCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<NHACUNGCAP>(DataProvider.Isn.DB.NHACUNGCAPs);
                MaNCC = null;
                TenNCC = null;
                DiaChiNCC = null;
                SdtNCC = null;
                MailNCC = null;
            });
        }
    }
}
