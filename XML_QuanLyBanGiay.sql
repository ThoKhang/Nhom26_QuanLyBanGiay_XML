-- ================================
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'QuanLyBanGiay')
BEGIN
    USE master;
    ALTER DATABASE QuanLyBanGiay SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyBanGiay;
END
GO

/* =====================================================
      TẠO DATABASE QUẢN LÝ BÁN GIÀY
===================================================== */
IF DB_ID('QuanLyBanGiay') IS NOT NULL
    DROP DATABASE QuanLyBanGiay;
GO

CREATE DATABASE QuanLyBanGiay;
GO

USE QuanLyBanGiay;
GO


/* =====================================================
      1. BẢNG NHÂN VIÊN (NhanVien.xml)
===================================================== */
CREATE TABLE NhanVien (
    MaNhanVien   NVARCHAR(10)  NOT NULL PRIMARY KEY,
    HoTen        NVARCHAR(100) NOT NULL,
    NgaySinh     DATE          NULL,
    GioiTinh     NVARCHAR(10)  NULL,
    DiaChi       NVARCHAR(200) NULL,
    DienThoai    NVARCHAR(20)  NULL,
    Email        NVARCHAR(100) NULL,
    ChucVu       NVARCHAR(50)  NULL,
    NgayVaoLam   DATE          NULL,
    TrangThai    NVARCHAR(20)  NULL  -- DangLam, Nghi, ...
);
GO


/* =====================================================
      2. BẢNG TÀI KHOẢN (TaiKhoan.xml)
===================================================== */
CREATE TABLE TaiKhoan (
    TenDangNhap NVARCHAR(50)  NOT NULL PRIMARY KEY,
    MatKhau     NVARCHAR(50)  NOT NULL,
    Quyen       NVARCHAR(20)  NOT NULL,
    MaNhanVien  NVARCHAR(10)  NOT NULL
);
GO

ALTER TABLE TaiKhoan
ADD CONSTRAINT FK_TK_NV FOREIGN KEY (MaNhanVien)
REFERENCES NhanVien(MaNhanVien);
GO


/* =====================================================
      3. BẢNG NHÀ CUNG CẤP (NhaCungCap.xml)
===================================================== */
CREATE TABLE NhaCungCap (
    MaNhaCungCap  NVARCHAR(10)   NOT NULL PRIMARY KEY,
    TenNhaCungCap NVARCHAR(200)  NOT NULL,
    DiaChi        NVARCHAR(200)  NULL,
    DienThoai     NVARCHAR(20)   NULL
);
GO


/* =====================================================
      4. BẢNG GIÀY / SẢN PHẨM (Giay.xml)
===================================================== */
CREATE TABLE SanPham (
    MaGiay        NVARCHAR(10)   NOT NULL PRIMARY KEY,
    TenGiay       NVARCHAR(200)  NOT NULL,
    Loai          NVARCHAR(100)  NULL,
    Size          INT            NULL,
    Mau           NVARCHAR(50)   NULL,
    SoLuongTon    INT            NULL,
    DonGiaNhap    INT            NULL,
    DonGiaBan     INT            NULL,
    MaNhaCungCap  NVARCHAR(10)   NOT NULL
);
GO

ALTER TABLE SanPham
ADD CONSTRAINT FK_SP_NCC FOREIGN KEY (MaNhaCungCap)
REFERENCES NhaCungCap(MaNhaCungCap);
GO


/* =====================================================
      5. BẢNG PHIẾU NHẬP (PhieuNhap.xml)
===================================================== */
CREATE TABLE PhieuNhap (
    MaPhieuNhap  NVARCHAR(10) NOT NULL PRIMARY KEY,
    NgayNhap     DATE         NULL,
    MaNhanVien   NVARCHAR(10) NULL,
    MaNhaCungCap NVARCHAR(10) NULL,
    TongTien     INT          NULL
);
GO

ALTER TABLE PhieuNhap
ADD CONSTRAINT FK_PN_NV FOREIGN KEY (MaNhanVien)
REFERENCES NhanVien(MaNhanVien);

ALTER TABLE PhieuNhap
ADD CONSTRAINT FK_PN_NCC FOREIGN KEY (MaNhaCungCap)
REFERENCES NhaCungCap(MaNhaCungCap);
GO


/* =====================================================
      6. BẢNG CHI TIẾT PHIẾU NHẬP (ChiTietPhieuNhap.xml)
===================================================== */
CREATE TABLE ChiTietPhieuNhap (
    MaPhieuNhap NVARCHAR(10) NOT NULL,
    MaGiay      NVARCHAR(10) NOT NULL,
    SoLuong     INT          NULL,
    DonGiaNhap  INT          NULL,
    ThanhTien   INT          NULL,
    CONSTRAINT PK_CTPN PRIMARY KEY (MaPhieuNhap, MaGiay)
);
GO

ALTER TABLE ChiTietPhieuNhap
ADD CONSTRAINT FK_CTPN_PN FOREIGN KEY (MaPhieuNhap)
REFERENCES PhieuNhap(MaPhieuNhap);

ALTER TABLE ChiTietPhieuNhap
ADD CONSTRAINT FK_CTPN_SP FOREIGN KEY (MaGiay)
REFERENCES SanPham(MaGiay);
GO


/* =====================================================
      7. BẢNG PHIẾU MUA (PhieuMua.xml)
===================================================== */
CREATE TABLE PhieuMua (
    MaPhieuMua   NVARCHAR(20)   NOT NULL PRIMARY KEY,
    MaGiay       NVARCHAR(10)   NOT NULL,
    SoLuongMua   INT            NULL,
    DonGiaBan    INT            NULL,
    ThanhTien    INT            NULL,
    NgayMua      DATETIME       NULL,
    TenDangNhap  NVARCHAR(50)   NULL,
    TenNguoiDung NVARCHAR(100)  NULL
);
GO

ALTER TABLE PhieuMua
ADD CONSTRAINT FK_PM_SP FOREIGN KEY (MaGiay)
REFERENCES SanPham(MaGiay);

ALTER TABLE PhieuMua
ADD CONSTRAINT FK_PM_TK FOREIGN KEY (TenDangNhap)
REFERENCES TaiKhoan(TenDangNhap);
GO


/* =====================================================
      8. BẢNG CHẤM CÔNG (ChamCong.xml)
      → SỬA THEO CÁCH 1 (GioVao NOT NULL)
===================================================== */
CREATE TABLE ChamCong (
    MaNhanVien NVARCHAR(10) NOT NULL,
    Ngay       DATE         NOT NULL,
    TrangThai  NVARCHAR(20) NULL, 
    GioVao     NVARCHAR(10) NOT NULL,
    GioRa      NVARCHAR(10) NULL,
    CONSTRAINT PK_ChamCong PRIMARY KEY (MaNhanVien, Ngay, GioVao)
);
GO

ALTER TABLE ChamCong
ADD CONSTRAINT FK_CC_NV FOREIGN KEY (MaNhanVien)
REFERENCES NhanVien(MaNhanVien);
GO
DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql = @sql + 'SELECT * FROM ' + name + '; '
FROM sys.tables;

EXEC(@sql);

select * from taikhoan
