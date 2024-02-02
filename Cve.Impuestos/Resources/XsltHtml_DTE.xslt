<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:sii="http://www.sii.cl/SiiDte"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
				exclude-result-prefixes="msxsl">
	<xsl:output method="xml" indent="yes" encoding="iso-8859-1"/>

	<xsl:template match="/*">
		<html>
			<head>
				<title></title>
				<style type="text/css">
					.test{
					width:20cm;
					}
				</style>
			</head>
			<body>
				<div align="center">
					<table class="test">
						<tr>
							<td>
								<!--DTE/Documento/Encabezado/Emisor/-->
								<table>
									<tr>
										<td></td>
										<td></td>
										<font>
											<b>
												a:<xsl:value-of select="sii:Documento/sii:Encabezado/sii:Emisor/sii:RznSoc"/>
												b:<xsl:value-of select="Documento/Encabezado/Emisor/RznSoc"/>
											</b>
										</font>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</div>
			</body>
		</html>
		<!--<xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>-->
	</xsl:template>
</xsl:stylesheet>
