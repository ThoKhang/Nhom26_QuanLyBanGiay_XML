<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="/">
		<html>
			<head>
				<title>Danh sách tài khoản</title>
				<style>
					table { border-collapse: collapse; }
					th, td { border: 1px solid black; padding: 6px; }
					th { background-color: #f0f0f0; }
				</style>
			</head>
			<body>

				<h2>Danh sách tài khoản (Preview XML + XSLT)</h2>

				<table>
					<tr>
						<th>Mã nhân viên</th>
						<th>Tên đăng nhập</th>
						<th>Quyền</th>
					</tr>

					<xsl:for-each select="NewDataSet/TaiKhoan">
						<tr>
							<td>
								<xsl:value-of select="MaNhanVien"/>
							</td>
							<td>
								<xsl:value-of select="TenDangNhap"/>
							</td>
							<td>
								<xsl:value-of select="Quyen"/>
							</td>
						</tr>
					</xsl:for-each>

				</table>

				<p>
					<i>* Mật khẩu không hiển thị vì lý do bảo mật</i>
				</p>

			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
