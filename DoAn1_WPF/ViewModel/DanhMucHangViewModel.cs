using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DoAn1_WPF.Model;
using DoAn1_WPF.ViewModel;

namespace DoAn1_WPF.ViewModel
{
    public class DanhMucHangViewModel:BaseViewModel
    {
        private ObservableCollection<DANHMUCHANG> list;
        public ObservableCollection<DANHMUCHANG> List { get => list; set { list = value; OnPropertyChanged(); } }

        private DANHMUCHANG selectedItem;
        public DANHMUCHANG SelectedItem 
        { 
            get => selectedItem; 
            set 
            { 
                selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    TenLoaiHang = SelectedItem.TenLoaiHang;
                    MaLoaiHang = SelectedItem.MaLoaiHang;
                }
            }
        }

        private string tenLoaiHang;
        public string TenLoaiHang
        { 
            get => tenLoaiHang; 
            set 
            {
                tenLoaiHang = value; 
                OnPropertyChanged();
            } 
        }

        private string maLoaiHang;
        public string MaLoaiHang
        {
            get => maLoaiHang;
            set
            {
                maLoaiHang = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FindCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public DanhMucHangViewModel()
        {
            List = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TenLoaiHang) || string.IsNullOrEmpty(MaLoaiHang))
                    return false;
                if (MaLoaiHang.Length > 5)
                    return false;
                var displayList = DataProvider.Isn.DB.DANHMUCHANGs.Where(x => x.MaLoaiHang == MaLoaiHang);
                if (displayList.Count() > 0 || displayList == null)
                    return false;
                return true;
            }, (p) =>
            {
                DANHMUCHANG dmh = new DANHMUCHANG()
                {
                    TenLoaiHang = TenLoaiHang,
                    MaLoaiHang = MaLoaiHang
                };
                DataProvider.Isn.DB.DANHMUCHANGs.Add(dmh);
                DataProvider.Isn.DB.SaveChanges();
                //List.Add(danhmuchang);
                List = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs);
                //HangHoaViewModel.DanhMucHang = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                if (string.IsNullOrEmpty(TenLoaiHang)|| string.IsNullOrEmpty(MaLoaiHang))
                    return false;
                if (MaLoaiHang.Length > 5)
                    return false;
                return true;
            }, (p) =>
            {
                string idOld = SelectedItem.MaLoaiHang;
                DANHMUCHANG dmh = DataProvider.Isn.DB.DANHMUCHANGs.Where(x => x.MaLoaiHang == SelectedItem.MaLoaiHang).SingleOrDefault();
                try 
                {                    
                    dmh.MaLoaiHang = MaLoaiHang;
                    dmh.TenLoaiHang = TenLoaiHang;
                    DataProvider.Isn.DB.SaveChanges();
                    SelectedItem.TenLoaiHang = TenLoaiHang;
                    SelectedItem.MaLoaiHang = MaLoaiHang;
                } catch 
                {
                    MessageBox.Show("Mã loại hàng là thuộc tính quan trọng!!\nNếu đã nhập sai, hãy xóa đi và nhập lại", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                    dmh.MaLoaiHang = idOld;
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
                    if (DataProvider.Isn.DB.HANGHOAs.Where(x => x.MaLoaiHang == SelectedItem.MaLoaiHang).Count() > 0)
                    {
                        MessageBox.Show("Không thể xóa loại hàng này vì có hàng hóa thuộc loại này", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                        DANHMUCHANG dmh = DataProvider.Isn.DB.DANHMUCHANGs.Single(x => x.MaLoaiHang == SelectedItem.MaLoaiHang);
                        DataProvider.Isn.DB.DANHMUCHANGs.Remove(dmh);
                        DataProvider.Isn.DB.SaveChanges();
                        return;
                    }
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Chắc không?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        DANHMUCHANG dmh = DataProvider.Isn.DB.DANHMUCHANGs.Single(x => x.MaLoaiHang == SelectedItem.MaLoaiHang);
                        DataProvider.Isn.DB.DANHMUCHANGs.Remove(dmh);
                        DataProvider.Isn.DB.SaveChanges();
                        //List.Remove(danhmuchang);
                        List = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs);
                    }
                }
                catch 
                {
                    List = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs);
                }
            });

            FindCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TenLoaiHang) && string.IsNullOrEmpty(MaLoaiHang))
                    return false;
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs.Where(x => x.TenLoaiHang.Contains(TenLoaiHang) || x.MaLoaiHang.Contains(MaLoaiHang)));
            });

            BackCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs);
                TenLoaiHang = null;
                MaLoaiHang =null;
            });
        }
    }
}
