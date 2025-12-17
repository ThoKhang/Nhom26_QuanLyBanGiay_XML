<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<html>
			<head>
				<title>Báo cáo thống kê</title>
				<style>
					body { font-family: Arial; }
					table { border-collapse: collapse; }
					th, td { border: 1px solid black; padding: 8px; }
					th { background-color: #e0e0e0; }
				</style>
			</head>
			<body>

				<h2>BÁO CÁO THỐNG KÊ (XML + XSLT)</h2>

				<table>
					<tr>
						<th>Nội dung</th>
						<th>Giá trị</th>
					</tr>
					<tr>
						<td>Tổng tài khoản</td>
						<td>
							<xsl:value-of select="ThongKe/TongTaiKhoan"/>
						</td>
					</tr>
					<tr>
						<td>Tổng nhân viên</td>
						<td>
							<xsl:value-of select="ThongKe/TongNhanVien"/>
						</td>
					</tr>
					<tr>
						<td>Nhân viên đang làm</td>
						<td>
							<xsl:value-of select="ThongKe/TongNhanVienDangLam"/>
						</td>
					</tr>
					<tr>
						<td>Mặt hàng đang bán</td>
						<td>
							<xsl:value-of select="ThongKe/TongMatHangDangBan"/>
						</td>
					</tr>
					<tr>
						<td>Phiếu đã bán</td>
						<td>
							<xsl:value-of select="ThongKe/TongPhieuHangDaBan"/>
						</td>
					</tr>
					<tr>
						<td>Tổng tiền bán</td>
						<td>
							<xsl:value-of select="ThongKe/TongTienDaBan"/>
						</td>
					</tr>
					<tr>
						<td>Nhà cung cấp</td>
						<td>
							<xsl:value-of select="ThongKe/TongNhaCungCap"/>
						</td>
					</tr>
					<tr>
						<td>Mặt hàng đã cung cấp</td>
						<td>
							<xsl:value-of select="ThongKe/TongMatHangDaCungCap"/>
						</td>
					</tr>
					<tr>
						<td>Tổng tiền mua</td>
						<td>
							<xsl:value-of select="ThongKe/TongTienDaMua"/>
						</td>
					</tr>
				</table>

			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
