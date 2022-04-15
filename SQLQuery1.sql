
insert PHIEUNHAP values(@MaPN,@NgayNhap)
go

delete PHIEUNHAP where MaPN=@MaPN
go

update PHIEUNHAP 
set MaPN=@MaPN,NgayNhap=@NgayNhap
where MaPN=@MaPN
go
//
insert TTPHIEUNHAP(MaPN,MaNCC,MaHang,MaKho,SLNhap,DGNhap) 
values(@MaPN,@MaNCC,@MaHang,@MaKho,@SLNhap,@DGNhap)
go

delete TTPHIEUNHAP where ID=@ID
go

update TTPHIEUNHAP 
set MaPN=@MaPN,MaNCC=@MaNCC,MaHang=@MaHang,MaKho=@MaKho,SLNhap=@SLNhap,DGNhap=@DGNhap
where ID=@ID
go

///////////////////////////////////////////////////////////


insert PHIEUXUAT values(@MaPX,@NgayXuat)
go

delete PHIEUXUAT where MaPX=@MaPX
go

update PHIEUXUAT 
set MaPX=@MaPX,NgayXuat=@NgayXuat
where MaPX=@MaPX
go
//
insert TTPHIEUXUAT(MaKH,MaHang,MaKho,SLXuat,DGXuat,MaPX) 
values(@MaKH,@MaHang,@MaKho,@SLXuat,@DGXuat,@MaPX)
go

delete TTPHIEUXUAT where ID=@ID
go

update TTPHIEUXUAT 
set MaKH=@MaKH,MaHang=@MaHang,MaKho=@MaKho,SLXuat=@SLXuat,DGXuat=DGXuat,MaPX=MaPX
where ID=@ID
go

////////////////////////////////////////////////////////////

insert HANGHOA
values(@MaHang,@TenHang,@DonViTinh,@SoLuong,@DonGia,@MaLoaiHang)
go

delete HANGHOA where MaHang=@MaHang
go

update HANGHOA
set MaHang=@MaHang,TenHang=@TenHang,DonViTinh=@DonViTinh,SoLuong@SoLuong,DonGia=@DonGia,MaLoaiHang=@MaLoaiHang
where MaHang=@MaHang
go

////////////////////////////////////////////////////////////

insert DANHMUCHANG values(@MaLoaiHang,@TenLoaiHang)
go

delete DANHMUCHANG where MaLoaiHang=@MaLoaiHang
go

update DANHMUCHANG 
set MaLoaiHang=@MaLoaiHang,TenLoaiHang=@TenLoaiHang
where MaLoaiHang=@MaLoaiHang
go

////////////////////////////////////////////////////////////

insert NHACUNGCAP values(@MaNCC,@TenNCC,@DiaChiNCC,@SdtNCC,@MailNCC)
go

delete NHACUNGCAP where MaNCC=@MaNCC
go

update NHACUNGCAP 
set MaNCC=@MaNCC,TenNCC=@TenNCC,TenNCC=@DiaChiNCC,TenNCC=@SdtNCC,MailNCC=@MailNCC
where MaNCC=@MaNCC
go

////////////////////////////////////////////////////////////

insert KHOHANG values(@MaKho,@TenKho,@DiaChiKho)
go

delete KHOHANG where MaKho=@MaKho
go

update KHOHANG 
set MaKho=@MaKho,TenKho=@TenKho,DiaChiKho=@DiaChiKho
where MaKho=@MaKho
go

////////////////////////////////////////////////////////////

insert KHACHHANG values(@MaKH,@HoTenKH,@DiaChiKH,@SdtKH)
go

delete KHACHHANG where MaKH=@MaKH
go

update KHACHHANG 
set MaKH=@MaKH,HoTenKH=@HoTenKH,DiaChiKH=@DiaChiKH,SdtKH=@SdtKH
where MaKH=@MaKH
go

////////////////////////////////////////////////////////////

insert TAIKHOAN(Ten,TaiKhoan,MatKhau,IdQuyen)
values(@Ten,@TaiKhoan,@MatKhau,@IdQuyen)
go

delete TAIKHOAN where ID=@ID
go

update TAIKHOAN 
set Ten=@Ten,TaiKhoan=@TaiKhoan,MatKhau=@MatKhau,IdQuyen=@IdQuyen
where ID=@ID
go