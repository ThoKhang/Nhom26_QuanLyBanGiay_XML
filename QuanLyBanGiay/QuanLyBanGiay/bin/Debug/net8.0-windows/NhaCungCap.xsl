<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template match="/">
<html>
<head>
    <meta charset="utf-8"/>
    <title>Danh sách nhà cung cấp</title>
    <style>
        body { font-family: Arial; }
        h2 { text-align: center; color: #0b5394; }
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #333; padding: 6px; text-align: center; }
        th { background-color: #fce5cd; }
        tr:nth-child(even) { background-color: #f9f9f9; }
    </style>
</head>
<body>

<h2>DANH SÁCH NHÀ CUNG CẤP</h2>

<table>
    <tr>
        <th>Mã NCC</th>
        <th>Tên nhà cung cấp</th>
        <th>Địa chỉ</th>
        <th>Điện thoại</th>
    </tr>

    <xsl:for-each select="NewDataSet/NhaCungCap">
        <tr>
            <td><xsl:value-of select="MaNhaCungCap"/></td>
            <td><xsl:value-of select="TenNhaCungCap"/></td>
            <td><xsl:value-of select="DiaChi"/></td>
            <td><xsl:value-of select="DienThoai"/></td>
        </tr>
    </xsl:for-each>

</table>

</body>
</html>
</xsl:template>

</xsl:stylesheet>
