<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
<html>
<head>
    <meta charset="utf-8"/>
    <title>Danh sách giày</title>
    <style>
        body { font-family: Arial; }
        h2 { text-align: center; color: #0b5394; }
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #333; padding: 6px; text-align: center; }
        th { background-color: #d9ead3; }
        tr:nth-child(even) { background-color: #f3f3f3; }
    </style>
</head>
<body>
<h2>DANH SÁCH GIÀY</h2>
<table>
    <tr>
        <th>Mã giày</th>
        <th>Tên giày</th>
        <th>Loại</th>
        <th>Size</th>
        <th>Màu</th>
        <th>Số lượng tồn</th>
        <th>Đơn giá nhập</th>
        <th>Đơn giá bán</th>
        <th>Mã NCC</th>
    </tr>
    <xsl:for-each select="NewDataSet/SanPham">
        <tr>
            <td><xsl:value-of select="MaGiay"/></td>
            <td><xsl:value-of select="TenGiay"/></td>
            <td><xsl:value-of select="Loai"/></td>
            <td><xsl:value-of select="Size"/></td>
            <td><xsl:value-of select="Mau"/></td>
            <td><xsl:value-of select="SoLuongTon"/></td>
            <td><xsl:value-of select="DonGiaNhap"/></td>
            <td><xsl:value-of select="DonGiaBan"/></td>
            <td><xsl:value-of select="MaNhaCungCap"/></td>
        </tr>
    </xsl:for-each>
</table>
</body>
</html>
</xsl:template>
</xsl:stylesheet>
