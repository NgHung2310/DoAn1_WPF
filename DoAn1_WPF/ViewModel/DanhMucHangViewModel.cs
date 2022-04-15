using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DoAn1_WPF.Model;

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
        public DanhMucHangViewModel()
        {
            List = new ObservableCollection<DANHMUCHANG>(DataProvider.Isn.DB.DANHMUCHANGs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(TenLoaiHang) || string.IsNullOrEmpty(MaLoaiHang))
                    return false;
                var displayList = DataProvider.Isn.DB.DANHMUCHANGs.Where(x => x.TenLoaiHang == TenLoaiHang);
                if (displayList.Count() != 0 || displayList == null)
                    return false;
                return true;
            }, (p) =>
            {
                DANHMUCHANG danhmuchang = new DANHMUCHANG()
                {
                    TenLoaiHang = TenLoaiHang,
                    MaLoaiHang = MaLoaiHang
                };
                DataProvider.Isn.DB.DANHMUCHANGs.Add(danhmuchang);
                DataProvider.Isn.DB.SaveChanges();
                List.Add(danhmuchang);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                if (string.IsNullOrEmpty(TenLoaiHang))
                    return false;
                if (DataProvider.Isn.DB.DANHMUCHANGs.Where(x => x.TenLoaiHang == TenLoaiHang && x.MaLoaiHang != MaLoaiHang).Count() > 0)
                    return false;
                return true;
            }, (p) =>
            {
                DANHMUCHANG danhmuchang = DataProvider.Isn.DB.DANHMUCHANGs.Where(x => x.MaLoaiHang == SelectedItem.MaLoaiHang).SingleOrDefault();
                danhmuchang.TenLoaiHang = TenLoaiHang;
                danhmuchang.MaLoaiHang = MaLoaiHang;
                DataProvider.Isn.DB.SaveChanges();
                SelectedItem.TenLoaiHang = TenLoaiHang;
                SelectedItem.MaLoaiHang = MaLoaiHang;
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                DANHMUCHANG danhmuchang = DataProvider.Isn.DB.DANHMUCHANGs.Where(x => x.MaLoaiHang == SelectedItem.MaLoaiHang).SingleOrDefault();
                DataProvider.Isn.DB.DANHMUCHANGs.Remove(danhmuchang);
                DataProvider.Isn.DB.SaveChanges();
                List.Remove(danhmuchang);
            });
        }
    }
}
