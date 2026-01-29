<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" encoding="UTF-8" indent="yes"/>

	<xsl:template match="/eksamid">
		<html>
			<head>
				<title>Exam detailid</title>
				<style>

					body {
					font-family: Arial, sans-serif;
					background-color: #f9f9f9;
					color: #333;
					margin: 0;
					padding: 20px;
					}
					h1 {
					text-align: center;
					color: #4CAF50;
					}
					.export-button {
					display: block;
					margin: 20px auto;
					padding: 12px 24px;
					background-color: #4CAF50;
					color: white;
					border: none;
					border-radius: 5px;
					cursor: pointer;
					font-size: 16px;
					box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
					transition: background-color 0.3s ease;
					}
					.export-button:hover {
					background-color: #45a049;
					}
					table {
					width: 100%;
					border-collapse: collapse;
					margin-top: 20px;
					background-color: white;
					box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
					border-radius: 5px;
					overflow: hidden;
					}
					th, td {
					padding: 12px;
					text-align: left;
					}
					th {
					background-color: #4CAF50;
					color: white;
					font-weight: bold;
					text-transform: uppercase;
					}
					td {
					border-bottom: 1px solid #ddd;
					}
					td:last-child {
					text-align: center;
					}
					a {
					color: white;
					text-decoration: none;
					font-weight: bold;
					}
					a:hover {
					text-decoration: underline;
					}
					form {
					text-align: center;
					margin-bottom: 20px;
					}
					input[type="text"] {
					padding: 8px 12px;
					font-size: 14px;
					border: 1px solid #ccc;
					border-radius: 4px;
					margin-right: 10px;
					}
					button {
					padding: 8px 16px;
					background-color: #4CAF50;
					color: white;
					border: none;
					border-radius: 4px;
					cursor: pointer;
					font-size: 14px;
					}
					button:hover {
					background-color: #45a049;
					}
				</style>
			</head>
			<body>
				<form method="POST" action="ExportToJSON.php">
					<button type="submit" class="export-button">Export to JSON</button>
				</form>
				<form method="POST" action="ImportToXml.php">
					<button type="submit" class="export-button">Import to XML</button>
				</form>
				<table>
					<tr>
						<th><a href="?sort=id&amp;order=asc">ID</a></th>
						<th><a href="?sort=eksamiaeg&amp;order=asc">Exam aeg</a></th>
						<th><a href="?sort=koht&amp;order=asc">Koht</a></th>
						<th><a href="?sort=nimi/eksamineerijanimi&amp;order=asc">Eksamineerija nimi</a></th>
						<th><a href="?sort=nimi/opilanenimi&amp;order=asc">Opilane nimi</a></th>
						<th><a href="?sort=kestvus&amp;order=asc">Kestvus (tunnid)</a></th>
						<th><a href="?sort=email&amp;order=asc">Email</a></th>
					</tr>
					<xsl:apply-templates select="eksam"/>
				</table>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="eksam">
		<tr>
			<td><xsl:value-of select="id"/></td>
			<td><xsl:value-of select="eksamiaeg"/></td>
			<td><xsl:value-of select="koht"/></td>
			<td><xsl:value-of select="nimi/eksamineerijanimi"/></td>
			<td><xsl:value-of select="nimi/opilanenimi"/></td>
			<td><xsl:value-of select="kestvus"/></td>
			<td><xsl:value-of select="@email"/></td>
		</tr>
	</xsl:template>
</xsl:stylesheet>
