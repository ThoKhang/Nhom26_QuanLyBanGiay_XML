<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
<html>
<head>
    <title>Danh sách phiếu nhập</title>
    <style>
        table { border-collapse: collapse; font-family: Arial; }
        th, td { border: 1px solid black; padding: 6px; }
        th { background: #e8f0fe; }
    </style>
</head>
<body>

<h2>DANH SÁCH PHIẾU NHẬP</h2>

<table>
<tr>
    <th>Mã phiếu nhập</th>
    <th>Ngày nhập</th>
    <th>Mã nhân viên</th>
    <th>Mã nhà cung cấp</th>
    <th>Tổng tiền</th>
</tr>

<xsl:for-each select="NewDataSet/PhieuNhap">
<tr>
    <td><xsl:value-of select="MaPhieuNhap"/></td>
    <td><xsl:value-of select="NgayNhap"/></td>
    <td><xsl:value-of select="MaNhanVien"/></td>
    <td><xsl:value-of select="MaNhaCungCap"/></td>
    <td><xsl:value-of select="TongTien"/></td>
</tr>
</xsl:for-each>

</table>

</body>
</html>
</xsl:template>
</xsl:stylesheet>
