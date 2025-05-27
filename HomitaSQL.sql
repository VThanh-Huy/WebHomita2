CREATE DATABASE TRA_SUA;
GO
USE TRA_SUA;
GO

-- 1. Bảng LoaiSanPham
CREATE TABLE LoaiSanPham (
    MaLoaiSP VARCHAR(50) PRIMARY KEY,
    TenLoaiSP NVARCHAR(100)
);
INSERT INTO LoaiSanPham (MaLoaiSP, TenLoaiSP) VALUES
('LSP01', N'Trà sữa'),
('LSP02', N'Trà trái cây'),				
('LSP03', N'Nước ép'),
('LSP04', N'Topping'),
('LSP05', N'Bánh ngọt');

-- 2. Bảng SanPham
CREATE TABLE SanPham (
    MaSP VARCHAR(50) PRIMARY KEY,
    TenSP NVARCHAR(100),
    Gia DECIMAL(10,2),
    Size VARCHAR(10),
    HinhAnh NVARCHAR(255),
    MaLoaiSP VARCHAR(50),
    FOREIGN KEY (MaLoaiSP) REFERENCES LoaiSanPham(MaLoaiSP)
);
INSERT INTO SanPham (MaSP, TenSP, Gia, Size, HinhAnh, MaLoaiSP) VALUES
('SP01', N'Trà sữa truyền thống', 25000, 'M', N'trasua1.jpg', 'LSP01'),
('SP02', N'Cà phê sữa đá', 30000, 'L', N'cafe1.jpg', 'LSP02'),
('SP03', N'Nước ép cam', 28000, 'M', N'cam.jpg', 'LSP03'),
('SP04', N'Thạch dừa', 5000, 'S', N'thachdua.jpg', 'LSP04'),
('SP05', N'Bánh su kem', 15000, 'M', N'sukem.jpg', 'LSP05');


CREATE TABLE TaiKhoan (
    MaTK VARCHAR(20) PRIMARY KEY,
	Email VARCHAR(30),
    TenDangNhap NVARCHAR(50),
    MatKhau NVARCHAR(50),
    VaiTro NVARCHAR(50) -- "Admin", "NhanVien", "KhachHang",
);

INSERT INTO TaiKhoan (MaTK,Email, TenDangNhap, MatKhau, VaiTro)
VALUES
('nd01','admin@gmail.com', 'admin1', 'admin123', 'Admin'),
-- Tài khoản nhân viên
('nd02','lan@gmail.com', 'nhanvien1', 'nv012345', 'NhanVien'),
('nd03','huuhoa@gmail.com', 'nhanvien2', 'nv123456', 'NhanVien'),
('nd04','quocbao@gmail.com', 'nhanvien3', 'nvquocbao', 'NhanVien'),
('nd10','khanhduy@gmail.com', 'nhanvien5', 'nv234567', 'NhanVien'),
('nd05','giahan@gmail.com', 'nhanvien4', 'nvGiaHan', 'NhanVien'),
-- Tài khoản khách hàng
('nd06','ngocmai@gmail.com', 'khachhang2', 'khNgocMai', 'KhanhHang'),
('nd07','vanan@gmail.com', 'khachhang1', 'kh012345', 'KhachHang'),
('nd08','honghanh@gmail.com', 'khachhang1', 'kh012345', 'KhachHang'),
('nd09','nhatquang@gmail.com', 'khachhang1', 'kh012345', 'KhachHang'),
('nd011','minhtuan@gmail.com', 'khachhang1', 'kh012345', 'KhachHang');

-- 3. Bảng KhachHang 
CREATE TABLE KhachHang (
    MaKH VARCHAR(50) PRIMARY KEY,
    HoTen NVARCHAR(100),
    SDT VARCHAR(20),
    DiaChi NVARCHAR(255),
	MaTK VARCHAR (20),
	FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK),
);
INSERT INTO KhachHang (MaKH, HoTen, SDT, DiaChi, MaTK) VALUES
('KH01', N'Trần Minh Tuấn', '0901234567', N'Nha Trang', 'nd011'),
('KH02', N'Lê Ngọc Mai','0907654321', N'Ninh Hòa', 'nd06'),
('KH03', N'Nguyễn Văn An','0911122334', N'Cam Ranh', 'nd07'),
('KH04', N'Trịnh Hồng Hạnh','0923456789', N'Vạn Ninh', 'nd08'),
('KH05', N'Hồ Nhật Quang','0938765432', N'Diên Khánh', 'nd09');

GO
-- 4. Bảng NhanVien 
CREATE TABLE NhanVien (
    MaNV VARCHAR(50) PRIMARY KEY,
    HoTen NVARCHAR(100),
    Email NVARCHAR(100),
    MatKhau VARCHAR(100),
	MaTK VARCHAR(20),
	FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK),
);
INSERT INTO NhanVien (MaNV, HoTen, Email, MatKhau, MaTK) VALUES
('NV01', N'Nguyễn Thị Lan', 'lan@gmail.com', 'nv012345', 'nd02'),
('NV02', N'Phạm Hữu Hòa', 'huuhoa@gmail.com', 'nv123456', 'nd03'),
('NV03', N'Trần Quốc Bảo', 'quocbao@gmail.com', 'nvquocbao', 'nd04'),
('NV04', N'Đặng Gia Hân', 'giahan@gmail.com', 'nvGiaHan', 'nd05'),
('NV05', N'Lý Khánh Duy', 'khanhduy@gmail.com', 'nv234567', 'nd10');
GO
-- 5. Bảng GioHang
CREATE TABLE GioHang (
    MaGioHang VARCHAR(50) PRIMARY KEY,
    NgayTao DATE,
    MaKH VARCHAR(50),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);
INSERT INTO GioHang (MaGioHang, NgayTao, MaKH) VALUES
('GH01', '2025-05-01', 'KH01'),
('GH02', '2025-05-02', 'KH02'),
('GH03', '2025-05-03', 'KH03'),
('GH04', '2025-05-04', 'KH04'),
('GH05', '2025-05-05', 'KH05');
GO
-- 6. Bảng ChiTietGioHang
CREATE TABLE ChiTietGioHang (
    MaCTGH VARCHAR(50) PRIMARY KEY,
    SoLuong INT,
    MaGioHang VARCHAR(50),
    MaSP VARCHAR(50),
    FOREIGN KEY (MaGioHang) REFERENCES GioHang(MaGioHang),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
GO
INSERT INTO ChiTietGioHang (MaCTGH, SoLuong, MaGioHang, MaSP) VALUES
('CTGH01', 2, 'GH01', 'SP01'),
('CTGH02', 1, 'GH01', 'SP04'),
('CTGH03', 1, 'GH02', 'SP02'),
('CTGH04', 3, 'GH03', 'SP03'),
('CTGH05', 2, 'GH04', 'SP05');

-- 7. Bảng DonHang
CREATE TABLE DonHang (
    MaDH VARCHAR(50) PRIMARY KEY,
    NgayDat DATE,
    GioVao TIME,
    Ca VARCHAR(20),
    TongTien DECIMAL(10,2),
    TienKhachDua DECIMAL(10,2),
    TienThoiLai DECIMAL(10,2),
    TrangThai NVARCHAR(50),
    PhuongThucThanhToan NVARCHAR(100),
    MaKH VARCHAR(50),
    MaNV VARCHAR(50),
	MaGioHang VARCHAR(50),
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
	FOREIGN KEY (MaGioHang) REFERENCES GioHang(MaGioHang),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
INSERT INTO DonHang (MaDH, NgayDat, GioVao, Ca, TongTien, TienKhachDua, TienThoiLai, TrangThai, PhuongThucThanhToan, MaKH, MaNV, MaGioHang) 
VALUES
('DH01', '2025-05-01', '10:00', 'Sáng', 55000, 60000, 5000, N'Đã thanh toán', N'Tiền mặt', 'KH01', 'NV01', 'GH01'),
('DH02', '2025-05-02', '13:30', 'Chiều', 30000, 30000, 0, N'Đã thanh toán', N'Momo', 'KH02', 'NV02', 'GH02'),
('DH03', '2025-05-03', '19:00', 'Tối', 42000, 50000, 8000, N'Đã thanh toán', N'Chuyển khoản', 'KH03', 'NV01', 'GH03'),
('DH04', '2025-05-04', '11:00', 'Sáng', 15000, 20000, 5000, N'Đã thanh toán', N'Tiền mặt', 'KH04', 'NV04', 'GH04'),
('DH05', '2025-05-05', '14:45', 'Chiều', 75000, 100000, 25000, N'Đã thanh toán', N'Momo', 'KH05', 'NV05', 'GH05');

-- 8. Bảng ChiTietDonHang
CREATE TABLE ChiTietDonHang (
    MaCTDH VARCHAR(50) PRIMARY KEY,
    SoLuong INT,
    DonGia DECIMAL(10,2),
    MaDH VARCHAR(50),
    MaSP VARCHAR(50),
    FOREIGN KEY (MaDH) REFERENCES DonHang(MaDH),
    FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);
INSERT INTO ChiTietDonHang (MaCTDH, SoLuong, DonGia, MaDH, MaSP) VALUES
('CTDH01', 2, 25000, 'DH01', 'SP01'),
('CTDH02', 1, 5000, 'DH01', 'SP04'),
('CTDH03', 1, 30000, 'DH02', 'SP02'),
('CTDH04', 2, 21000, 'DH03', 'SP03'),
('CTDH05', 1, 15000, 'DH04', 'SP05');

-- 9. Bảng ThamSo
CREATE TABLE ThamSo (
    MaTS VARCHAR(50) PRIMARY KEY,
    TenTS NVARCHAR(100),
    GiaTri VARCHAR(255)
);
INSERT INTO ThamSo (MaTS, TenTS, GiaTri) VALUES
('TS01', N'Thời gian mở cửa', '08:00'),
('TS02', N'Thời gian đóng cửa', '22:00'),
('TS03', N'Số lượng tồn tối đa', '500'),
('TS04', N'Phụ phí topping mặc định', '5000'),
('TS05', N'Tỷ lệ thuế VAT', '10%');
------------------------
ALTER TABLE DonHang DROP CONSTRAINT FK__DonHang__MaKH__239E4DCF;
ALTER TABLE DonHang DROP CONSTRAINT FK__DonHang__MaNV__24927208;
ALTER TABLE ChiTietDonHang DROP CONSTRAINT FK__ChiTietDon__MaDH__276EDEB3;
ALTER TABLE DonHang DROP CONSTRAINT PK__DonHang__27258661FFBFDA26;
DROP TABLE DonHang;

ALTER TABLE ChiTietDonHang DROP CONSTRAINT FK__ChiTietDon__MaSP__286302EC;
ALTER TABLE ChiTietDonHang DROP CONSTRAINT PK__ChiTietD__1E4E40F0A57CE9FF;
DROP TABLE ChiTietDonHang;