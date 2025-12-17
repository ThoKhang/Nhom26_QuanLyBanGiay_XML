<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<html>
			<head>
				<title>Trang chủ</title>
				<style>
					body { font-family: Arial; }
					table { border-collapse: collapse; }
					th, td { border: 1px solid black; padding: 8px; }
					th { background-color: #e8e8e8; }
				</style>
			</head>
			<body>

				<h2>THÔNG TIN NGƯỜI DÙNG ĐĂNG NHẬP</h2>

				<table>
					<tr>
						<th>Nội dung</th>
						<th>Giá trị</th>
					</tr>
					<tr>
						<td>Tên đăng nhập</td>
						<td>
							<xsl:value-of select="TrangChu/TenDangNhap"/>
						</td>
					</tr>
					<tr>
						<td>Họ tên</td>
						<td>
							<xsl:value-of select="TrangChu/HoTen"/>
						</td>
					</tr>
					<tr>
						<td>Quyền</td>
						<td>
							<xsl:value-of select="TrangChu/Quyen"/>
						</td>
					</tr>
				</table>

			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
