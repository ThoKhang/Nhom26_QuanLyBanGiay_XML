/* =====================================================
   XÓA DATABASE NẾU TỒN TẠI
===================================================== */
DROP DATABASE IF EXISTS QuanLyBanGiay;

/* =====================================================
   TẠO DATABASE
===================================================== */
CREATE DATABASE QuanLyBanGiay
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE QuanLyBanGiay;

/* =====================================================
   1. BẢNG NHÂN VIÊN (NhanVien.xml)
===================================================== */
CREATE TABLE NhanVien (
    MaNhanVien   VARCHAR(10)  NOT NULL PRIMARY KEY,
    HoTen        VARCHAR(100) NOT NULL,
    NgaySinh     DATE         NULL,
    GioiTinh     VARCHAR(10)  NULL,
    DiaChi       VARCHAR(200) NULL,
    DienThoai    VARCHAR(20)  NULL,
    Email        VARCHAR(100) NULL,
    ChucVu       VARCHAR(50)  NULL,
    NgayVaoLam   DATE         NULL,
    TrangThai    VARCHAR(20)  NULL
);

/* =====================================================
   2. BẢNG TÀI KHOẢN (TaiKhoan.xml)
===================================================== */
CREATE TABLE TaiKhoan (
    TenDangNhap VARCHAR(50) NOT NULL PRIMARY KEY,
    MatKhau     VARCHAR(50) NOT NULL,
    Quyen       VARCHAR(20) NOT NULL,
    MaNhanVien  VARCHAR(10) NOT NULL,
    CONSTRAINT FK_TK_NV FOREIGN KEY (MaNhanVien)
        REFERENCES NhanVien(MaNhanVien)
);

/* =====================================================
   3. BẢNG NHÀ CUNG CẤP (NhaCungCap.xml)
===================================================== */
CREATE TABLE NhaCungCap (
    MaNhaCungCap  VARCHAR(10)  NOT NULL PRIMARY KEY,
    TenNhaCungCap VARCHAR(200) NOT NULL,
    DiaChi        VARCHAR(200) NULL,
    DienThoai     VARCHAR(20)  NULL
);

/* =====================================================
   4. BẢNG GIÀY / SẢN PHẨM (Giay.xml)
===================================================== */
CREATE TABLE SanPham (
    MaGiay       VARCHAR(10)  NOT NULL PRIMARY KEY,
    TenGiay      VARCHAR(200) NOT NULL,
    Loai         VARCHAR(100) NULL,
    Size         INT          NULL,
    Mau          VARCHAR(50)  NULL,
    SoLuongTon   INT          NULL,
    DonGiaNhap   INT          NULL,
    DonGiaBan    INT          NULL,
    MaNhaCungCap VARCHAR(10)  NOT NULL,
    CONSTRAINT FK_SP_NCC FOREIGN KEY (MaNhaCungCap)
        REFERENCES NhaCungCap(MaNhaCungCap)
);

/* =====================================================
   5. BẢNG PHIẾU NHẬP (PhieuNhap.xml)
===================================================== */
CREATE TABLE PhieuNhap (
    MaPhieuNhap  VARCHAR(10) NOT NULL PRIMARY KEY,
    NgayNhap     DATE        NULL,
    MaNhanVien   VARCHAR(10) NULL,
    MaNhaCungCap VARCHAR(10) NULL,
    TongTien     INT         NULL,
    CONSTRAINT FK_PN_NV FOREIGN KEY (MaNhanVien)
        REFERENCES NhanVien(MaNhanVien),
    CONSTRAINT FK_PN_NCC FOREIGN KEY (MaNhaCungCap)
        REFERENCES NhaCungCap(MaNhaCungCap)
);

/* =====================================================
   6. BẢNG CHI TIẾT PHIẾU NHẬP (ChiTietPhieuNhap.xml)
===================================================== */
CREATE TABLE ChiTietPhieuNhap (
    MaPhieuNhap VARCHAR(10) NOT NULL,
    MaGiay      VARCHAR(10) NOT NULL,
    SoLuong     INT         NULL,
    DonGiaNhap  INT         NULL,
    ThanhTien   INT         NULL,
    PRIMARY KEY (MaPhieuNhap, MaGiay),
    CONSTRAINT FK_CTPN_PN FOREIGN KEY (MaPhieuNhap)
        REFERENCES PhieuNhap(MaPhieuNhap),
    CONSTRAINT FK_CTPN_SP FOREIGN KEY (MaGiay)
        REFERENCES SanPham(MaGiay)
);

/* =====================================================
   7. BẢNG PHIẾU MUA (PhieuMua.xml)
===================================================== */
CREATE TABLE PhieuMua (
    MaPhieuMua   VARCHAR(20)  NOT NULL PRIMARY KEY,
    MaGiay       VARCHAR(10)  NOT NULL,
    SoLuongMua   INT          NULL,
    DonGiaBan    INT          NULL,
    ThanhTien    INT          NULL,
    NgayMua      DATETIME     NULL,
    TenDangNhap  VARCHAR(50)  NULL,
    TenNguoiDung VARCHAR(100) NULL,
    CONSTRAINT FK_PM_SP FOREIGN KEY (MaGiay)
        REFERENCES SanPham(MaGiay),
    CONSTRAINT FK_PM_TK FOREIGN KEY (TenDangNhap)
        REFERENCES TaiKhoan(TenDangNhap)
);

/* =====================================================
   8. BẢNG CHẤM CÔNG (ChamCong.xml)
===================================================== */
CREATE TABLE ChamCong (
    MaNhanVien VARCHAR(10) NOT NULL,
    Ngay       DATE        NOT NULL,
    TrangThai  VARCHAR(20) NULL,
    GioVao     VARCHAR(10) NOT NULL,
    GioRa      VARCHAR(10) NULL,
    PRIMARY KEY (MaNhanVien, Ngay, GioVao),
    CONSTRAINT FK_CC_NV FOREIGN KEY (MaNhanVien)
        REFERENCES NhanVien(MaNhanVien)
);

/* =====================================================
   KIỂM TRA DỮ LIỆU
===================================================== */
SHOW TABLES;

