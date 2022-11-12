using DoAn1_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace DoAn1_WPF.ViewModel
{
    public class NhapHangViewModel : BaseViewModel
    {
        private ObservableCollection<TTPHIEUNHAP> list;
        public ObservableCollection<TTPHIEUNHAP> List { get => list; set { list = value; OnPropertyChanged(); } }

        private ObservableCollection<HANGHOA> hangHoa;
        public ObservableCollection<HANGHOA> HangHoa { get => hangHoa; set { hangHoa = value; OnPropertyChanged(); } }

        private ObservableCollection<NHACUNGCAP> nhaCungCap;
        public ObservableCollection<NHACUNGCAP> NhaCungCap { get => nhaCungCap; set { nhaCungCap = value; OnPropertyChanged(); } }

        private ObservableCollection<KHOHANG> khoHang;
        public ObservableCollection<KHOHANG> KhoHang { get => khoHang; set { khoHang = value; OnPropertyChanged(); } }

        private TTPHIEUNHAP selectedItem;
        public TTPHIEUNHAP SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    NgayNhap = SelectedItem.PHIEUNHAP.NgayNhap;
                    MaPhieuNhap = SelectedItem.PHIEUNHAP.MaPN;
                    SelectedHangHoa = SelectedItem.HANGHOA;
                    SelectedNhaCungCap = SelectedItem.NHACUNGCAP;
                    SoLuong = SelectedItem.SLNhap;
                    DonGia = SelectedItem.DGNhap;
                    SelectedKhoHang = SelectedItem.KHOHANG;                  
                }
            }
        }

        private DateTime? ngayNhap;
        public DateTime? NgayNhap 
        {
            get => ngayNhap; 
            set { ngayNhap = value; OnPropertyChanged(); } 
        }

        private string maPhieuNhap;
        public string MaPhieuNhap 
        { 
            get => maPhieuNhap; 
            set { maPhieuNhap = value; OnPropertyChanged(); } 
        }

        private HANGHOA selectedHangHoa;
        public HANGHOA SelectedHangHoa
        {
            get => selectedHangHoa;
            set
            {
                selectedHangHoa = value; OnPropertyChanged();
            }
        }

        private NHACUNGCAP selectedNhaCungCap;
        public NHACUNGCAP SelectedNhaCungCap
        {
            get => selectedNhaCungCap;
            set
            {
                selectedNhaCungCap = value; OnPropertyChanged();
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

        private KHOHANG selectedKhoHang;
        public KHOHANG SelectedKhoHang
        {
            get => selectedKhoHang;
            set
            {
                selectedKhoHang = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FindCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand ReportCommand { get; set; }

        public NhapHangViewModel()
        {
            List = new ObservableCollection<TTPHIEUNHAP>(DataProvider.Isn.DB.TTPHIEUNHAPs.OrderByDescending(x => x.PHIEUNHAP.NgayNhap));
            HangHoa = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
            NhaCungCap = new ObservableCollection<NHACUNGCAP>(DataProvider.Isn.DB.NHACUNGCAPs);
            KhoHang = new ObservableCollection<KHOHANG>(DataProvider.Isn.DB.KHOHANGs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (NgayNhap == null || MaPhieuNhap == null || SelectedHangHoa == null || SelectedNhaCungCap == null || SelectedKhoHang == null || SoLuong == null || DonGia == null)
                    return false;
                if (MaPhieuNhap.Length > 5)
                    return false;
                return true;
            }, (p) =>
            {
                if (DataProvider.Isn.DB.PHIEUNHAPs.Where(x => x.MaPN == MaPhieuNhap).Count() == 0 || DataProvider.Isn.DB.PHIEUNHAPs.Where(x => x.MaPN == MaPhieuNhap) == null)
                {
                    var phieuNhap = new PHIEUNHAP()
                    {
                        MaPN = MaPhieuNhap,
                        NgayNhap = NgayNhap.Value
                    };
                    var ttPhieuNhap = new TTPHIEUNHAP()
                    {
                        PHIEUNHAP = phieuNhap,
                        HANGHOA = SelectedHangHoa,
                        NHACUNGCAP = SelectedNhaCungCap,
                        KHOHANG = SelectedKhoHang,
                        SLNhap = SoLuong.Value,
                        DGNhap = DonGia.Value
                    };
                    DataProvider.Isn.DB.PHIEUNHAPs.Add(phieuNhap);
                    DataProvider.Isn.DB.TTPHIEUNHAPs.Add(ttPhieuNhap);
                }
                else
                {
                    var ttPhieuNhap = new TTPHIEUNHAP()
                    {
                        PHIEUNHAP = DataProvider.Isn.DB.PHIEUNHAPs.Where(x => x.MaPN == MaPhieuNhap).SingleOrDefault(),
                        HANGHOA = SelectedHangHoa,
                        NHACUNGCAP = SelectedNhaCungCap,
                        KHOHANG = SelectedKhoHang,
                        SLNhap = SoLuong.Value,
                        DGNhap = DonGia.Value
                    };
                    DataProvider.Isn.DB.TTPHIEUNHAPs.Add(ttPhieuNhap);
                }                                                    
                DataProvider.Isn.DB.SaveChanges();
                List = new ObservableCollection<TTPHIEUNHAP>(DataProvider.Isn.DB.TTPHIEUNHAPs.OrderByDescending(x => x.PHIEUNHAP.NgayNhap));
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (NgayNhap == null || MaPhieuNhap == null || SelectedHangHoa == null || SelectedNhaCungCap == null || SelectedKhoHang == null || SoLuong == null || DonGia == null)
                    return false;
                if (MaPhieuNhap.Length > 5)
                    return false;
                return true;
            }, (p) =>
            {
                var phieuNhap = DataProvider.Isn.DB.PHIEUNHAPs.Where(x => x.MaPN == SelectedItem.MaPN).SingleOrDefault();
                var ttPhieuNhap = DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaPN == SelectedItem.MaPN && x.MaNCC == SelectedItem.MaNCC && x.MaHang == SelectedItem.MaHang && x.MaKho == SelectedItem.MaKho).SingleOrDefault();
                string idOld = phieuNhap.MaPN;
                try
                {
                    phieuNhap.MaPN = MaPhieuNhap;
                    phieuNhap.NgayNhap = NgayNhap.Value;
                    ttPhieuNhap.PHIEUNHAP = phieuNhap;
                    ttPhieuNhap.HANGHOA = SelectedHangHoa;
                    ttPhieuNhap.NHACUNGCAP = SelectedNhaCungCap;
                    ttPhieuNhap.KHOHANG = SelectedKhoHang;
                    ttPhieuNhap.SLNhap = SoLuong.Value;
                    ttPhieuNhap.DGNhap = DonGia.Value;
                    DataProvider.Isn.DB.SaveChanges();
                    List = new ObservableCollection<TTPHIEUNHAP>(DataProvider.Isn.DB.TTPHIEUNHAPs.OrderByDescending(x => x.PHIEUNHAP.NgayNhap));
                }
                catch
                {
                    MessageBox.Show("Mã phiếu nhập là thuộc tính quan trọng!!\nNếu đã nhập sai, hãy xóa đi và nhập lại", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                    phieuNhap.MaPN = idOld;
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
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Chắc không?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        var ttPhieuNhap = DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaPN == SelectedItem.MaPN && x.MaNCC == SelectedItem.MaNCC && x.MaHang == SelectedItem.MaHang && x.MaKho == SelectedItem.MaKho && x.SLNhap == SelectedItem.SLNhap && x.DGNhap == SelectedItem.DGNhap && x.PHIEUNHAP.NgayNhap == SelectedItem.PHIEUNHAP.NgayNhap).FirstOrDefault();
                        //DataProvider.Isn.DB.PHIEUNHAPs.Remove(phieuNhap);
                        DataProvider.Isn.DB.TTPHIEUNHAPs.Remove(ttPhieuNhap);
                        DataProvider.Isn.DB.SaveChanges();
                        List = new ObservableCollection<TTPHIEUNHAP>(DataProvider.Isn.DB.TTPHIEUNHAPs.OrderByDescending(x => x.PHIEUNHAP.NgayNhap));
                    }
                }
                catch { }
            });

            FindCommand = new RelayCommand<object>((p) =>
            {
                if (NgayNhap == null && MaPhieuNhap == null && SelectedHangHoa == null && SelectedNhaCungCap == null && SelectedKhoHang == null && SoLuong == null && DonGia == null)
                    return false;
                return true;
            }, (p) =>
            {
                try 
                {
                    if (SelectedHangHoa != null)
                    {
                        List = new ObservableCollection<TTPHIEUNHAP>(DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaPN.Contains(MaPhieuNhap) || x.PHIEUNHAP.NgayNhap.ToString().Contains(NgayNhap.Value.ToString()) || x.HANGHOA.TenHang.ToString().Contains(SelectedHangHoa.TenHang.ToString())));
                    }
                    else if (SelectedNhaCungCap != null)
                    {
                        List = new ObservableCollection<TTPHIEUNHAP>(DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaPN.Contains(MaPhieuNhap) || x.PHIEUNHAP.NgayNhap.ToString().Contains(NgayNhap.Value.ToString()) || x.NHACUNGCAP.TenNCC.ToString().Contains(SelectedNhaCungCap.TenNCC.ToString())));
                    }
                    else if (SelectedKhoHang != null)
                    {
                        List = new ObservableCollection<TTPHIEUNHAP>(DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaPN.Contains(MaPhieuNhap) || x.PHIEUNHAP.NgayNhap.ToString().Contains(NgayNhap.Value.ToString()) || x.KHOHANG.TenKho.ToString().Contains(SelectedKhoHang.TenKho.ToString())));
                    }
                    else
                    {
                        List = new ObservableCollection<TTPHIEUNHAP>(DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaPN.Contains(MaPhieuNhap) || x.PHIEUNHAP.NgayNhap.ToString().Contains(NgayNhap.Value.ToString())));
                    }
                }
                catch { }
            });

            BackCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                List = new ObservableCollection<TTPHIEUNHAP>(DataProvider.Isn.DB.TTPHIEUNHAPs.OrderByDescending(x => x.PHIEUNHAP.NgayNhap));
                HangHoa = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
                NhaCungCap = new ObservableCollection<NHACUNGCAP>(DataProvider.Isn.DB.NHACUNGCAPs);
                KhoHang = new ObservableCollection<KHOHANG>(DataProvider.Isn.DB.KHOHANGs);
                NgayNhap = null;
                MaPhieuNhap = null; 
                SelectedHangHoa = null; 
                SelectedNhaCungCap = null;
                SelectedKhoHang = null;
                SoLuong = null;
                DonGia = null;
            });

            ReportCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                try
                {
                    CreateReport();
                }
                catch { }
            });            
        }

        void CreateReport()
        {
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook;
            COMExcel.Worksheet exSheet;
            COMExcel.Range exRange;
            int hang = 0, cot = 0;


            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:Z300"].Font.Name = "Times new roman";
            exRange.Range["A1:B3"].Font.Size = 10;
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 5;
            exRange.Range["A1:A1"].ColumnWidth = 7;
            exRange.Range["B1:B1"].ColumnWidth = 15;
            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "Kho hàng nhóm 1 DHTI13A1HN";
            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "UNETI";
            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại: 09999999999";
            exRange.Range["D2:G2"].Font.Size = 12;
            exRange.Range["D2:G2"].Font.Bold = true;
            exRange.Range["D2:G2"].Font.ColorIndex = 3;
            exRange.Range["D2:G2"].MergeCells = true;
            exRange.Range["D2:G2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["D2:G2"].Value = "BÁO CÁO THÔNG TIN PHIẾU NHẬP MÃ " + SelectedItem.MaPN;
            exRange.Range["B5:G5"].MergeCells = true;
            exRange.Range["B5:G5"].Value = "Nhà cung cấp: " + SelectedItem.NHACUNGCAP.TenNCC;


            exRange.Range["A7:A7"].Font.Bold = true;
            exRange.Range["A7:A7"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A7:A7"].ColumnWidth = 5;
            exRange.Range["B7:B7"].ColumnWidth = 20;
            exRange.Range["C7:C7"].ColumnWidth = 20;
            exRange.Range["D7:D7"].ColumnWidth = 12;
            exRange.Range["E7:E7"].ColumnWidth = 10;
            exRange.Range["F7:F7"].ColumnWidth = 15;
            exRange.Range["G7:G7"].ColumnWidth = 15;

            exRange.Range["A7:G7"].Font.Bold = true;
            exRange.Range["A7:G7"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;

            exRange.Range["A7:A7"].Value = "STT";
            exRange.Range["B7:B7"].Value = "Tên hàng";
            exRange.Range["C7:C7"].Value = "Kho nhập";
            exRange.Range["D7:D7"].Value = "Đơn vị tính";
            exRange.Range["E7:E7"].Value = "Số lượng";
            exRange.Range["F7:F7"].Value = "Đơn giá";
            exRange.Range["G7:G7"].Value = "Thành tiền";


            hang = 7;
            int STT = 1;
            double? TongTien = 0;
            var danhsach = DataProvider.Isn.DB.TTPHIEUNHAPs.Where(x => x.MaPN == SelectedItem.MaPN);
            danhsach.ToList().ForEach(x =>
            {
                hang++;
                cot = 0;
                exSheet.Cells[hang, cot + 1].Value = STT++;
                cot++;
                exSheet.Cells[hang, cot + 1].Value = x.HANGHOA.TenHang;
                cot++;
                exSheet.Cells[hang, cot + 1].Value = x.KHOHANG.TenKho;
                cot++;
                exSheet.Cells[hang, cot + 1].Value = x.HANGHOA.DonViTinh;
                cot++;
                exSheet.Cells[hang, cot + 1].Value = x.SLNhap;
                cot++;
                exSheet.Cells[hang, cot + 1].Value = x.DGNhap;
                cot++;
                exSheet.Cells[hang, cot + 1].Value = x.SLNhap * x.DGNhap;
                TongTien += x.SLNhap * x.DGNhap;
            });
            exRange = exSheet.Cells[2][hang + 2];
            exRange.Range["A1:F1"].MergeCells = true;
            exRange.Range["A1:F1"].Font.Italic = true;
            exRange.Range["A1:F1"].Font.Bold = true;
            exRange.Range["A1:F1"].Value = "Tổng: " + TongTien.ToString();

            exRange = exSheet.Cells[2][hang + 5];
            exRange.Range["E1:G1"].MergeCells = true;
            exRange.Range["E1:G1"].Font.Italic = true;
            exRange.Range["E1:G1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["E1:G1"].Value = "Hà Nội, Ngày " + DateTime.Now.ToString("dd/MM/yyyy");

            exRange.Range["E2:G2"].MergeCells = true;
            exRange.Range["E2:G2"].Font.Italic = true;
            exRange.Range["E2:G2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["E2:G2"].Value = "Người lập báo cáo";

            exRange.Range["E3:G3"].MergeCells = true;
            exRange.Range["E3:G3"].Font.Italic = true;
            exRange.Range["E3:G3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["E3:G3"].Value = "(ký, ghi rõ họ tên)";
            exSheet.Name = "Báo cáo";
            exApp.Visible = true;
        }
    }
}

