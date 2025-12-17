<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output method="html" encoding="utf-8" indent="yes"/>

  <xsl:template match="/">
    <html>
      <head>
        <title>Danh sách nhân viên</title>
        <style>
          body { font-family: Arial; background:#f4f6f8; }
          h2 { text-align:center; color:#2c3e50; }
          table { border-collapse: collapse; width: 100%; background:white; }
          th, td { border:1px solid #ccc; padding:8px; text-align:center; }
          th { background:#3498db; color:white; }
          tr:nth-child(even) { background:#f2f2f2; }
        </style>
      </head>
      <body>
        <h2>DANH SÁCH NHÂN VIÊN</h2>
        <table>
          <tr>
            <th>Mã NV</th>
            <th>Họ tên</th>
            <th>Ngày sinh</th>
            <th>Giới tính</th>
            <th>Địa chỉ</th>
            <th>Điện thoại</th>
            <th>Trạng thái</th>
          </tr>
          <xsl:for-each select="//NhanVien">
            <tr>
              <td><xsl:value-of select="MaNhanVien"/></td>
              <td><xsl:value-of select="HoTen"/></td>
              <td><xsl:value-of select="NgaySinh"/></td>
              <td><xsl:value-of select="GioiTinh"/></td>
              <td><xsl:value-of select="DiaChi"/></td>
              <td><xsl:value-of select="DienThoai"/></td>
              <td><xsl:value-of select="TrangThai"/></td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>
