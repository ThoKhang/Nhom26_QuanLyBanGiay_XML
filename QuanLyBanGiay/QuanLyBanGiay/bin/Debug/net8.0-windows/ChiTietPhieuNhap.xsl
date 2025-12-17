<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:output method="html" encoding="utf-8" indent="yes"/>

    <xsl:template match="/">
        <html>
        <head>
            <title>Chi tiết phiếu nhập</title>
            <style>
                body {
                    font-family: Arial, Helvetica, sans-serif;
                    background-color: #f5f6fa;
                }
                h2 {
                    text-align: center;
                    color: #2c3e50;
                }
                table {
                    border-collapse: collapse;
                    width: 90%;
                    margin: 20px auto;
                    background-color: white;
                }
                th, td {
                    border: 1px solid #ccc;
                    padding: 8px 12px;
                    text-align: center;
                }
                th {
                    background-color: #2980b9;
                    color: white;
                }
                tr:nth-child(even) {
                    background-color: #f2f2f2;
                }
            </style>
        </head>
        <body>
            <h2>DANH SÁCH CHI TIẾT PHIẾU NHẬP</h2>

            <table>
                <tr>
                    <th>Mã phiếu nhập</th>
                    <th>Mã giày</th>
                    <th>Số lượng</th>
                    <th>Đơn giá nhập</th>
                    <th>Thành tiền</th>
                </tr>

                <xsl:for-each select="ChiTietPhieuNhap/Row">
                    <tr>
                        <td><xsl:value-of select="MaPhieuNhap"/></td>
                        <td><xsl:value-of select="MaGiay"/></td>
                        <td><xsl:value-of select="SoLuong"/></td>
                        <td>
                            <xsl:value-of select="format-number(DonGiaNhap, '#,###')"/>
                        </td>
                        <td>
                            <xsl:value-of select="format-number(ThanhTien, '#,###')"/>
                        </td>
                    </tr>
                </xsl:for-each>
            </table>
        </body>
        </html>
    </xsl:template>

</xsl:stylesheet>
