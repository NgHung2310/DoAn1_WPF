using DoAn1_WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace DoAn1_WPF.ViewModel
{
    public class XuatHangViewModel : BaseViewModel
    {
        private ObservableCollection<TTPHIEUXUAT> list;
        public ObservableCollection<TTPHIEUXUAT> List { get => list; set { list = value; OnPropertyChanged(); } }

        private ObservableCollection<HANGHOA> hangHoa;
        public ObservableCollection<HANGHOA> HangHoa { get => hangHoa; set { hangHoa = value; OnPropertyChanged(); } }

        private ObservableCollection<KHACHHANG> khachHang;
        public ObservableCollection<KHACHHANG> KhachHang { get => khachHang; set { khachHang = value; OnPropertyChanged(); } }

        private ObservableCollection<KHOHANG> khoHang;
        public ObservableCollection<KHOHANG> KhoHang { get => khoHang; set { khoHang = value; OnPropertyChanged(); } }

        private TTPHIEUXUAT selectedItem;
        public TTPHIEUXUAT SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    NgayXuat = SelectedItem.PHIEUXUAT.NgayXuat;
                    MaPhieuXuat = SelectedItem.PHIEUXUAT.MaPX;
                    SelectedHangHoa = SelectedItem.HANGHOA;
                    SelectedKhachHang = SelectedItem.KHACHHANG;
                    SoLuong = SelectedItem.SLXuat;
                    DonGia = SelectedItem.DGXuat;
                    SelectedKhoHang = SelectedItem.KHOHANG;
                }
            }
        }

        private DateTime? ngayXuat;
        public DateTime? NgayXuat
        {
            get => ngayXuat;
            set { ngayXuat = value; OnPropertyChanged(); }
        }

        private string maPhieuXuat;
        public string MaPhieuXuat
        {
            get => maPhieuXuat;
            set { maPhieuXuat = value; OnPropertyChanged(); }
        }

        private KHACHHANG selectedKhachHang;
        public KHACHHANG SelectedKhachHang
        {
            get => selectedKhachHang;
            set
            {
                selectedKhachHang = value; OnPropertyChanged();
            }
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
        
        public XuatHangViewModel()
        {
            List = new ObservableCollection<TTPHIEUXUAT>(DataProvider.Isn.DB.TTPHIEUXUATs.OrderByDescending(x => x.PHIEUXUAT.NgayXuat));
            HangHoa = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
            KhachHang = new ObservableCollection<KHACHHANG>(DataProvider.Isn.DB.KHACHHANGs);
            KhoHang = new ObservableCollection<KHOHANG>(DataProvider.Isn.DB.KHOHANGs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (NgayXuat == null || MaPhieuXuat == null || SelectedKhachHang == null || SelectedHangHoa == null || SelectedKhoHang == null || SoLuong == null || DonGia == null)
                    return false;
                if (MaPhieuXuat.Length > 5)
                    return false;
                return true;
            }, (p) =>
            {
                if (DataProvider.Isn.DB.PHIEUXUATs.Where(x => x.MaPX == MaPhieuXuat).Count() == 0 || DataProvider.Isn.DB.PHIEUXUATs.Where(x => x.MaPX == MaPhieuXuat) == null)
                {
                    var phieuXuat = new PHIEUXUAT()
                    {
                        MaPX = MaPhieuXuat,
                        NgayXuat = NgayXuat.Value
                    };
                    var TTPHIEUXUAT = new TTPHIEUXUAT()
                    {
                        PHIEUXUAT = phieuXuat,
                        HANGHOA = SelectedHangHoa,
                        KHACHHANG = SelectedKhachHang,
                        KHOHANG = SelectedKhoHang,
                        SLXuat = SoLuong.Value,
                        DGXuat = DonGia.Value
                    };
                    DataProvider.Isn.DB.PHIEUXUATs.Add(phieuXuat);
                    DataProvider.Isn.DB.TTPHIEUXUATs.Add(TTPHIEUXUAT);
                }
                else
                {
                    var ttphieuXuat = new TTPHIEUXUAT()
                    {
                        PHIEUXUAT = DataProvider.Isn.DB.PHIEUXUATs.Where(x => x.MaPX == MaPhieuXuat).SingleOrDefault(),
                        HANGHOA = SelectedHangHoa,
                        KHACHHANG = SelectedKhachHang,
                        KHOHANG = SelectedKhoHang,
                        SLXuat = SoLuong.Value,
                        DGXuat = DonGia.Value
                    };
                    DataProvider.Isn.DB.TTPHIEUXUATs.Add(ttphieuXuat);
                }

                DataProvider.Isn.DB.SaveChanges();
                List = new ObservableCollection<TTPHIEUXUAT>(DataProvider.Isn.DB.TTPHIEUXUATs.OrderByDescending(x => x.PHIEUXUAT.NgayXuat));
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (NgayXuat == null || MaPhieuXuat == null || SelectedKhachHang == null || SelectedHangHoa == null || SelectedKhoHang == null || SoLuong == null || DonGia == null)
                    return false;
                if (MaPhieuXuat.Length > 5)
                    return false;
                return true;
            }, (p) =>
            {
                var phieuXuat = DataProvider.Isn.DB.PHIEUXUATs.Where(x => x.MaPX == SelectedItem.MaPX).SingleOrDefault();
                var ttphieuXuat = DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaPX == SelectedItem.MaPX && x.MaKH == SelectedItem.MaKH && x.MaHang == SelectedItem.MaHang && x.MaKho == SelectedItem.MaKho).SingleOrDefault();
                string idOld = phieuXuat.MaPX;
                try
                {
                    phieuXuat.MaPX = MaPhieuXuat;
                    phieuXuat.NgayXuat = NgayXuat.Value;
                    ttphieuXuat.PHIEUXUAT = phieuXuat;
                    ttphieuXuat.HANGHOA = SelectedHangHoa;
                    ttphieuXuat.KHACHHANG = SelectedKhachHang;
                    ttphieuXuat.KHOHANG = SelectedKhoHang;
                    ttphieuXuat.SLXuat = SoLuong.Value;
                    ttphieuXuat.DGXuat = DonGia.Value;
                    DataProvider.Isn.DB.SaveChanges();
                    List = new ObservableCollection<TTPHIEUXUAT>(DataProvider.Isn.DB.TTPHIEUXUATs.OrderByDescending(x => x.PHIEUXUAT.NgayXuat));
                }
                catch
                {
                    MessageBox.Show("Mã phiếu xuất là thuộc tính quan trọng!!\nNếu đã nhập sai, hãy xóa đi và nhập lại", "Cẩn thận", MessageBoxButton.OK, MessageBoxImage.Warning);
                    phieuXuat.MaPX = idOld;
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
                        var ttphieuXuat = DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaPX == SelectedItem.MaPX && x.MaKH == SelectedItem.MaKH && x.MaHang == SelectedItem.MaHang && x.MaKho == SelectedItem.MaKho && x.SLXuat == SelectedItem.SLXuat && x.DGXuat == SelectedItem.DGXuat && x.PHIEUXUAT.NgayXuat == SelectedItem.PHIEUXUAT.NgayXuat).FirstOrDefault();
                        //DataProvider.Isn.DB.PHIEUXUATs.Remove(PHIEUXUAT);
                        DataProvider.Isn.DB.TTPHIEUXUATs.Remove(ttphieuXuat);
                        DataProvider.Isn.DB.SaveChanges();
                        List = new ObservableCollection<TTPHIEUXUAT>(DataProvider.Isn.DB.TTPHIEUXUATs.OrderByDescending(x => x.PHIEUXUAT.NgayXuat));
                    }
                }
                catch { }
            });

            FindCommand = new RelayCommand<object>((p) =>
            {
                if (NgayXuat == null && MaPhieuXuat == null && SelectedHangHoa == null && SelectedKhachHang == null && SelectedKhoHang == null && SoLuong == null && DonGia == null)
                    return false;
                return true;
            }, (p) =>
            {
                try
                {
                    if (SelectedHangHoa != null)
                    {
                        List = new ObservableCollection<TTPHIEUXUAT>(DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaPX.Contains(MaPhieuXuat) || x.PHIEUXUAT.NgayXuat.ToString().Contains(NgayXuat.Value.ToString()) || x.HANGHOA.TenHang.ToString().Contains(SelectedHangHoa.TenHang.ToString())));
                    }
                    else if (SelectedKhachHang != null)
                    {
                        List = new ObservableCollection<TTPHIEUXUAT>(DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaPX.Contains(MaPhieuXuat) || x.PHIEUXUAT.NgayXuat.ToString().Contains(NgayXuat.Value.ToString()) || x.KHACHHANG.HoTenKH.ToString().Contains(SelectedKhachHang.HoTenKH.ToString())));
                    }
                    else if (SelectedKhoHang != null)
                    {
                        List = new ObservableCollection<TTPHIEUXUAT>(DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaPX.Contains(MaPhieuXuat) || x.PHIEUXUAT.NgayXuat.ToString().Contains(NgayXuat.Value.ToString()) || x.KHOHANG.TenKho.ToString().Contains(SelectedKhoHang.TenKho.ToString())));
                    }
                    else
                    {
                        List = new ObservableCollection<TTPHIEUXUAT>(DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaPX.Contains(MaPhieuXuat) || x.PHIEUXUAT.NgayXuat.ToString().Contains(NgayXuat.Value.ToString())));
                    }
                }
                catch { }
            });

            BackCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    List = new ObservableCollection<TTPHIEUXUAT>(DataProvider.Isn.DB.TTPHIEUXUATs.OrderByDescending(x => x.PHIEUXUAT.NgayXuat));
                    HangHoa = new ObservableCollection<HANGHOA>(DataProvider.Isn.DB.HANGHOAs);
                    KhachHang = new ObservableCollection<KHACHHANG>(DataProvider.Isn.DB.KHACHHANGs);
                    KhoHang = new ObservableCollection<KHOHANG>(DataProvider.Isn.DB.KHOHANGs);
                    NgayXuat = null;
                    MaPhieuXuat = null;
                    SelectedHangHoa = null;
                    SelectedKhachHang = null;
                    SelectedKhoHang = null;
                    SoLuong = null;
                    DonGia = null;
                }
                catch { }
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
            exRange.Range["D2:G2"].Value = "BÁO CÁO THÔNG TIN PHIẾU XUẤT MÃ " + SelectedItem.MaPX;
            exRange.Range["B5:G5"].MergeCells = true;
            exRange.Range["B5:G5"].Value = "Khách hàng: " + SelectedItem.KHACHHANG.HoTenKH;


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
            exRange.Range["C7:C7"].Value = "Kho xuất";
            exRange.Range["D7:D7"].Value = "Đơn vị tính";
            exRange.Range["E7:E7"].Value = "Số lượng";
            exRange.Range["F7:F7"].Value = "Đơn giá";
            exRange.Range["G7:G7"].Value = "Thành tiền";


            hang = 7;
            int STT = 1;
            double? TongTien = 0;
            var danhsach = DataProvider.Isn.DB.TTPHIEUXUATs.Where(x => x.MaPX == SelectedItem.MaPX);
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
                exSheet.Cells[hang, cot + 1].Value = x.SLXuat;
                cot++;
                exSheet.Cells[hang, cot + 1].Value = x.DGXuat;                
                cot++;
                exSheet.Cells[hang, cot + 1].Value = x.SLXuat * x.DGXuat;
                TongTien += x.SLXuat * x.DGXuat;
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
