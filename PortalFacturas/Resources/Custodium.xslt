<xsl:stylesheet version="2.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:sii="http://www.sii.cl/SiiDte"
xmlns:str="http://exslt.org/strings"
extension-element-prefixes="str sii"
                xmlns:ms="urn:schemas-microsoft-com:xslt">
  <xsl:output method="xml" indent="yes" />

  <xsl:param name="TedTimbre"> </xsl:param>
  <xsl:variable name="FechaHora" select="/sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:FchEmis|
									   /DTE/Documento/Encabezado/IdDoc/FchEmis|
									   /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:FchEmis|
									   /DTE/Liquidacion/Encabezado/IdDoc/FchEmis|
									   /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FchEmis|
									   /DTE/Exportaciones/Encabezado/IdDoc/FchEmis"/>


  <xsl:variable name="smallcase" select="'abcdefghijklmnopqrstuvwxyzàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿžšœ'" />
  <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞŸŽŠŒ'" />


  <!--<xsl:variable name="TedTimbre" select="ScriptNS:MostrarMensaje(string($nodeTED))" />-->
  <xsl:decimal-format name="cl" decimal-separator="," grouping-separator="."/>
  <xsl:decimal-format name="us" decimal-separator="." grouping-separator=","/>

  <!-- TEMPLATE RAIZ ............................................................................................................ -->
  <xsl:template match="/">
    <html>
      <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

        <title>Visualizacion DTEs Recibidos</title>
        <style type="text/css">
          .facturaDocumento { 
        
          width: 20cm;
          }


          .letraEncabezadoGesys {
          font-style: normal;
          font-size: 15pt;
          font-family: arial;
          color: black;
          }

          .letraValores {
          font-style: normal;
          font-size: 9pt;
          font-family: arial;
          line-height: 12px;
          }

          .tablaFactura {
          border-style:solid;
          border-color:red;
          width		    :100%;
          }

          .tablaDatos {
          border-style:solid;
          border-width:1;
          border-color:black;
          }
          .cabeceraClienteInterno {
          font-family :arial;
          font-size   :8pt;
          text-align : left;

          }

          .cabeceraClientes {
          font-family :times;
          font-size   :8pt;
          font-weight : bold;
          }
          .cabeceraClientes2 {
          font-family :arial;
          font-size   :8pt;
          text-align : center;
          font-weight : bold;
          }

          .TextGiro{
          font-family :arial;
          font-size   :9pt;
          font-weight : bold;
          }

          .textoReferencia {
          font-family :arial;
          font-size   :12px;
          text-align  :left;
          }

          .productoTextoConMargenDF {
          font-family :arial;
          font-size   :10px;

          border-bottom:1px solid black;
          }

          .productoTextoConMargenSIN {
          font-family :arial;
          font-size   :8pt;
          }

          .productoTextoConMargenSin2 {
          font-family :arial;
          font-size   :7pt;
          }

          .ParaDescripciones {
          font-family :arial;
          font-size   :6pt;
          }

          .cabeceraClientes {
          font-family :arial;
          font-size   :8pt;
          text-align  :left;
          font-weight :bold;
          }

          .cabeceraClientesMenor {
          font-family :arial;
          font-size   :6.5pt;
          text-align  :left;
          font-weight :bold;
          }

          .espacio {
          margin-top :2px;
          }

          .espacio2 {

          line-height: 10px;
          }

          .saltoLinea {
          line-height: 2pt;
          }



          <!-- __________________ TIMBRE ________________ -->

          #Timbre {padding: 0px 0px 0px 0px; font-family: sans-serif;	font-size:8pt; float: left;margin-left; width: 350px;	border:1px none #414040;	z-index:3;}
        </style>
      </head>
      <body>
        <div align="center">
          <table class="facturaDocumento" border="0">
            <tr>
              <td valign="top">
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                  <tr>
                    <td valign="top" align="left" width="0%"/>
                    <td valign="top" align="left" width="70%">
                      <div class="espacio" />
                      <font class="letraEncabezadoGesys">
                        <b>
                          <xsl:call-template name="UPPER">
                            <xsl:with-param name="text" select="sii:DTE/sii:Documento/sii:Encabezado/sii:Emisor/sii:RznSoc|
																	      DTE/Documento/Encabezado/Emisor/RznSoc|
																	      sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Emisor/sii:RznSoc|
																	      DTE/Liquidacion/Encabezado/Emisor/RznSoc|
																	      sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Emisor/sii:RznSoc|
																	      DTE/Exportaciones/Encabezado/Emisor/RznSoc|
																		  sii:DTE/sii:Documento/sii:Encabezado/sii:Emisor/sii:RznSocEmisor|
																		  DTE/Documento/Encabezado/Emisor/RznSocEmisor"/>
                          </xsl:call-template>
                        </b>
                      </font>
                      <br/>
                      <font class="TextGiro">
                        <b>
                          <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Emisor/sii:GiroEmis|
																	      DTE/Documento/Encabezado/Emisor/GiroEmis|
																	      sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Emisor/sii:GiroEmis|
																	      DTE/Liquidacion/Encabezado/Emisor/GiroEmis|
																	      sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Emisor/sii:GiroEmis|
																	      DTE/Exportaciones/Encabezado/Emisor/GiroEmis|
																		  sii:DTE/sii:Documento/sii:Encabezado/sii:Emisor/sii:GiroEmisor|
																		  DTE/Documento/Encabezado/Emisor/GiroEmisor"/>
                        </b>
                        <br/>

                      </font>
                      <font class="TextGiro">
                        <b>
                          <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Emisor/sii:DirOrigen|
																	  DTE/Documento/Encabezado/Emisor/DirOrigen|
																	  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Emisor/sii:DirOrigen|
																	  DTE/Liquidacion/Encabezado/Emisor/DirOrigen|
																	  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Emisor/sii:DirOrigen|
																	  DTE/Exportaciones/Encabezado/Emisor/DirOrigen"/> -
                          <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Emisor/sii:CmnaOrigen|
																	  DTE/Documento/Encabezado/Emisor/CmnaOrigen|
																	  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Emisor/sii:CmnaOrigen|
																	  DTE/Liquidacion/Encabezado/Emisor/CmnaOrigen|
																	  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Emisor/sii:CmnaOrigen|
																	  DTE/Exportaciones/Encabezado/Emisor/CmnaOrigen"/>
                        </b>
                        <br/>
                      </font>
                      <font class="TextGiro">
                        <b>
                          <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Emisor/sii:CiudadOrigen|
																	      DTE/Documento/Encabezado/Emisor/CiudadOrigen|
																	      sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Emisor/sii:CiudadOrigen|
																	      DTE/Liquidacion/Encabezado/Emisor/CiudadOrigen|
																	      sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Emisor/sii:CiudadOrigen|
																	      DTE/Exportaciones/Encabezado/Emisor/CiudadOrigen"/>
                        </b>
                        <br/>
                      </font>
                    </td>
                    <td rowspan="2" valign="top" width="30%">
                      <table border="0" class="tablaFactura" valign="top" cellpadding="0" cellspacing="0" style="border:3px solid red">
                        <tr>
                          <td align="center">
                            <font face="arial" size="2" color="red">
                              <b>
                                R.U.T.: <xsl:call-template name="formatearRut">
                                  <xsl:with-param name="input" select="sii:DTE/sii:Documento/sii:Encabezado/sii:Emisor/sii:RUTEmisor|
																								 DTE/Documento/Encabezado/Emisor/RUTEmisor|
																								 sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Emisor/sii:RUTEmisor|
																								 DTE/Liquidacion/Encabezado/Emisor/RUTEmisor|
																								 sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Emisor/sii:RUTEmisor|
																								 DTE/Exportaciones/Encabezado/Emisor/RUTEmisor"/>
                                </xsl:call-template>
                                <br/>
                                <div class="espacio2">&#160;</div>
                                <xsl:call-template name="DTEName">
                                  <xsl:with-param name="codDTE" select="/sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:TipoDTE|
																									          /DTE/Documento/Encabezado/IdDoc/TipoDTE| 																	
																											  /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:TipoDTE|
																											  /DTE/Liquidacion/Encabezado/IdDoc/TipoDTE|
																											  /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:TipoDTE|
																											  /DTE/Exportaciones/Encabezado/IdDoc/TipoDTE"/>
                                </xsl:call-template>
                                <div class="espacio2">&#160;</div>
                                N&#186; <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:Folio|
																						  DTE/Documento/Encabezado/IdDoc/Folio|
																						  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:Folio|
																						  DTE/Liquidacion/Encabezado/IdDoc/Folio|
																						  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:Folio|
																						  DTE/Exportaciones/Encabezado/IdDoc/Folio"/>
                                <br/>
                              </b>
                            </font>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                  <tr>
                    <td colspan="2" rowspan="2" align="left" valign="top">&#160;</td>
                  </tr>
                  <tr>
                    <td valign="top" align="center" width="25%" height="30">
                      <font face="arial" size="2" color="red">
                        <b>	S.I.I - SANTIAGO CENTRO</b>
                      </font>
                    </td>
                  </tr>
                </table>
                <!-- <div class="espacio"/> -->
                <!-- COMIENZO tabla datos receptor -->
                <table width="100%" border="0" class="tablaDatos" cellpadding="0" cellspacing="0">
                  <tr height="65">
                    <td width="43%" valign="middle">
                      <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                          <td width="21%" class="cabeceraClientes">&#160;Señor (es)</td>
                          <td style="font-family :arial;font-size:10px;text-align :left;">
                            :   <xsl:call-template name="UPPER">
                              <xsl:with-param name="text" select="translate(sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:RznSocRecep|
																																				  DTE/Documento/Encabezado/Receptor/RznSocRecep|
																																				  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:RznSocRecep|
																																				  DTE/Liquidacion/Encabezado/Receptor/RznSocRecep|
																																				  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:RznSocRecep|
																																				  DTE/Exportaciones/Encabezado/Receptor/RznSocRecep,'&#x7f;&#x80;&#x81;&#x82;&#x83;&#x84;&#x85;&#x86;&#x87;&#x88;&#x89;&#x8a;&#x8b;&#x8c;&#x8d;&#x8e;&#x8f;&#x90;&#x91;&#x92;&#x93;&#x94;&#x95;&#x96;&#x97;&#x98;&#x99;&#x9a;&#x9b;&#x9c;&#x9d;&#x9e;&#x9f;','')"/>
                            </xsl:call-template>
                          </td>
                        </tr>
                        <tr>
                          <td class="cabeceraClientes">&#160;Direccion</td>
                          <td style="font-family :arial;font-size:10px;text-align :left;">
                            : <xsl:call-template name="UPPER">
                              <xsl:with-param name="text" select="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:DirRecep|
																																	    DTE/Documento/Encabezado/Receptor/DirRecep|
																																		sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:DirRecep|
																																		DTE/Liquidacion/Encabezado/Receptor/DirRecep|
																																		sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:DirRecep|
																																		DTE/Exportaciones/Encabezado/Receptor/DirRecep"/>
                            </xsl:call-template>
                          </td>
                        </tr>
                        <tr>
                          <td class="cabeceraClientes">&#160;R.U.T.</td>
                          <td style="font-family :arial;font-size:10px;text-align :left;">
                            :
                            <xsl:call-template name="formatearRut">
                              <xsl:with-param name="input" select="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:RUTRecep|
																							 DTE/Documento/Encabezado/Receptor/RUTRecep|
																							 sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:RUTRecep|
																							 DTE/Liquidacion/Encabezado/Receptor/RUTRecep|
																							 sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:RUTRecep|
																							 DTE/Exportaciones/Encabezado/Receptor/RUTRecep"/>
                            </xsl:call-template>
                          </td>
                        </tr>
                        <tr>
                          <td class="cabeceraClientes">&#160;Giro</td>
                          <td style="font-family :arial;font-size:9px;text-align :left;">
                            : <xsl:call-template name="UPPER">
                              <xsl:with-param name="text" select="translate(sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:GiroRecep|
																																		DTE/Documento/Encabezado/Receptor/GiroRecep|
																																		sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:GiroRecep|
																																		DTE/Liquidacion/Encabezado/Receptor/GiroRecep|
																																		sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:GiroRecep|
																																		DTE/Exportaciones/Encabezado/Receptor/GiroRecep,'Ã','Ó')"/>
                            </xsl:call-template>
                          </td>
                        </tr>
                      </table>
                    </td>
                    <td valign="middle">
                      <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                          <td width="20%" class="cabeceraClientes">Comuna</td>
                          <td colspan="1" style="font-family :arial;font-size:10px;text-align :left;">
                            :
                            <xsl:call-template name="UPPER">
                              <xsl:with-param name="text" select="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:CmnaRecep|
																	      DTE/Documento/Encabezado/Receptor/CmnaRecep|
																		  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:CmnaRecep|
																		  DTE/Liquidacion/Encabezado/Receptor/CmnaRecep|
																		  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:CmnaRecep|
																		  DTE/Exportaciones/Encabezado/Receptor/CmnaRecep"/>
                            </xsl:call-template>
                          </td>
                          <xsl:if test="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:CdgIntRecep|
															  DTE/Documento/Encabezado/Receptor/CdgIntRecep|
															  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:CdgIntRecep|
															  DTE/Liquidacion/Encabezado/Receptor/CdgIntRecep|
															  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:CdgIntRecep|
															  DTE/Exportaciones/Encabezado/Receptor/CdgIntRecep">
                            <td width="10%" class="cabeceraClientes">Cod. Cliente</td>
                            <td style="font-family :arial;font-size:10px;text-align :left;">
                              : <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:CdgIntRecep|
																																			DTE/Documento/Encabezado/Receptor/CdgIntRecep|
																																			sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:CdgIntRecep|
																																			DTE/Liquidacion/Encabezado/Receptor/CdgIntRecep|
																																			sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:CdgIntRecep|
																																			DTE/Exportaciones/Encabezado/Receptor/CdgIntRecep"/>
                            </td>
                          </xsl:if>
                        </tr>
                        <tr>
                          <td width="20%" class="cabeceraClientes">Ciudad</td>
                          <td colspan="1" style="font-family :arial;font-size:10px;text-align :left;">
                            :      <xsl:call-template name="UPPER">
                              <xsl:with-param name="text" select="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:CiudadRecep|
																																					DTE/Documento/Encabezado/Receptor/CiudadRecep|
																																					sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:CiudadRecep|
																																					DTE/Liquidacion/Encabezado/Receptor/CiudadRecep|
																																					sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:CiudadRecep|
																																					DTE/Exportaciones/Encabezado/Receptor/CiudadRecep"/>
                            </xsl:call-template>
                          </td>
                          <!--<xsl:if test="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:CorreoRecep|
															  DTE/Documento/Encabezado/Receptor/CorreoRecep|
															  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:CorreoRecep|
															  DTE/Liquidacion/Encabezado/Receptor/CorreoRecep|
															  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:CorreoRecep|
															  DTE/Exportaciones/Encabezado/Receptor/CorreoRecep">
                            <td width="10%" class="cabeceraClientes">Email</td>
                            <td style="font-family :arial;font-size:9px;text-align :left;">
                              : <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:CorreoRecep|
																																		    DTE/Documento/Encabezado/Receptor/CorreoRecep|
																																			sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:CorreoRecep|
																																			DTE/Liquidacion/Encabezado/Receptor/CorreoRecep|
																																			sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:CorreoRecep|
																																			DTE/Exportaciones/Encabezado/Receptor/CorreoRecep"/>
                            </td>
                          </xsl:if>-->
                        </tr>
                        <tr>
                          <td width="20%" class="cabeceraClientes">F. de Pago</td>
                          <td colspan="1" style="font-family :arial;font-size:10px;text-align :left;">
                            :
                            <xsl:choose>
                              <xsl:when test="number(sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:FmaPago|
																			   DTE/Documento/Encabezado/IdDoc/FmaPago)=1"> Contado</xsl:when>
                              <xsl:when test="number(sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:FmaPago|
																			   DTE/Documento/Encabezado/IdDoc/FmaPago)=2"> CRÉDITO</xsl:when>
                              <xsl:when test="number(sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:FmaPago|
																			   DTE/Documento/Encabezado/IdDoc/FmaPago)=3"> Sin Costo(entrega gratuita)</xsl:when>
                              <xsl:when test="number(sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FmaPagExp|
																			   DTE/Exportaciones/Encabezado/IdDoc/FmaPagExp)=1"> Cobranza hasta un a&#241;o</xsl:when>
                              <xsl:when test="number(sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FmaPagExp|
																			   DTE/Exportaciones/Encabezado/IdDoc/FmaPagExp)=2"> Cobranza</xsl:when>
                              <xsl:when test="number(sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FmaPagExp|
																			   DTE/Exportaciones/Encabezado/IdDoc/FmaPagExp)=11"> Acreditivo</xsl:when>
                              <xsl:when test="number(sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FmaPagExp|
																			   DTE/Exportaciones/Encabezado/IdDoc/FmaPagExp)=21"> S / Pago</xsl:when>
                              <xsl:when test="number(sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FmaPagExp|
																			   DTE/Exportaciones/Encabezado/IdDoc/FmaPagExp)=32"> Anticipo</xsl:when>
                              <xsl:when test="number(sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FmaPagExp|
																			   DTE/Exportaciones/Encabezado/IdDoc/FmaPagExp)=35"> Contado</xsl:when>
                            </xsl:choose>
                          </td>
                          <xsl:if test="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:Contacto|
															  DTE/Documento/Encabezado/Receptor/Contacto|
															  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:Contacto|
															  DTE/Liquidacion/Encabezado/Receptor/Contacto|
															  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:Contacto|
															  DTE/Exportaciones/Encabezado/Receptor/Contacto">
                            <td width="10%" class="cabeceraClientes">Contacto</td>
                            <td style="font-family :arial;font-size:10px;text-align :left;">
                              : <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Receptor/sii:Contacto|
																																		    DTE/Documento/Encabezado/Receptor/Contacto|
																																			sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Receptor/sii:Contacto|
																																			DTE/Liquidacion/Encabezado/Receptor/Contacto|
																																			sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Receptor/sii:Contacto|
																																			DTE/Exportaciones/Encabezado/Receptor/Contacto"/>
                            </td>
                          </xsl:if>
                        </tr>
                        <tr>
                          <td width="20%" class="cabeceraClientes">Fecha Emis.</td>
                          <td width="30%" style="font-family :arial;font-size:10px;text-align :left;">
                            :
                            <xsl:call-template name="format-fecha-all">
                              <xsl:with-param name="input" select="sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:FchEmis|
																							 DTE/Documento/Encabezado/IdDoc/FchEmis|
																							 sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:FchEmis|
																							 DTE/Liquidacion/Encabezado/IdDoc/FchEmis|
																							 sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FchEmis|
																							 DTE/Exportaciones/Encabezado/IdDoc/FchEmis"/>
                              <xsl:with-param name="nombre" select="'mayuscorto'"/>
                              <xsl:with-param name="separador" select="'  '"/>
                            </xsl:call-template>
                          </td>
                          <td width="20%" class="cabeceraClientes">Fecha Venc.</td>
                          <td width="26%" style="font-family :arial;font-size:10px;text-align :left;">
                            :
                            <xsl:if test="sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:FchVenc|
																  DTE/Documento/Encabezado/IdDoc/FchVenc|
																  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:FchVenc|
																  DTE/Liquidacion/Encabezado/IdDoc/FchVenc|
																  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FchVenc|
																  DTE/Exportaciones/Encabezado/IdDoc/FchVenc!=''">
                              <xsl:call-template name="format-fecha-all">
                                <xsl:with-param name="input" select="sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:FchVenc|
																								 DTE/Documento/Encabezado/IdDoc/FchVenc|
																								 sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:FchVenc|
																								 DTE/Liquidacion/Encabezado/IdDoc/FchVenc|
																								 sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:FchVenc|
																								 DTE/Exportaciones/Encabezado/IdDoc/FchVenc"/>
                                <xsl:with-param name="nombre" select="'mayuscorto'"/>
                                <xsl:with-param name="separador" select="' '"/>
                              </xsl:call-template>
                            </xsl:if>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
                <!-- FIN tabla datos receptor -->
                <div class="espacio"/>
                <div class="espacio"/>
                <!-- Tabla Monto Pago - marcela orellana -->
                <!--<xsl:if test="sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:FchPago|
										  DTE/Documento/Encabezado/IdDoc/MntPagos/FchPago|
										  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:FchPago|
										  DTE/Liquidacion/Encabezado/IdDoc/MntPagos/FchPago|
										  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:FchPago|
										  DTE/Exportaciones/Encabezado/IdDoc/MntPagos/FchPago|
										  sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:MntPago|
										  DTE/Documento/Encabezado/IdDoc/MntPagos/MntPago|
										  sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:MntPago|
										  DTE/Liquidacion/Encabezado/IdDoc/MntPagos/MntPago|
										  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:MntPago|
										  DTE/Exportaciones/Encabezado/IdDoc/MntPagos/MntPago !=''">
                  <table width="100%" border="0" class="tablaDatos" cellpadding="0" cellspacing="0">

                    <tr>
                      <td colspan="2" style="border-bottom 	:1px solid rgb(0,0,0)" class="cabeceraClientesMenor" >&#160;Fecha Pago </td>
                      <td colspan="2" style="border-bottom 	:1px solid rgb(0,0,0)" class="cabeceraClientesMenor" >Monto Pago </td>
                      <td colspan="2" style="border-bottom 	:1px solid rgb(0,0,0)" class="cabeceraClientesMenor" >Glosa Pagos </td>
                    </tr>



                    <xsl:choose>
                      <xsl:when test="count(/sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:MntPagos|
															  /DTE/Documento/Encabezado/IdDoc/MntPagos|
															  /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:MntPagos|
															  /DTE/Liquidacion/Encabezado/IdDoc/MntPagos|
													          /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:MntPagos|
													          /DTE/Exportaciones/Encabezado/IdDoc/MntPagos)&gt; 3">


                        <xsl:for-each select="/sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:MntPagos|
														  /DTE/Documento/Encabezado/IdDoc/MntPagos|
													      /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:MntPagos|
													      /DTE/Liquidacion/Encabezado/IdDoc/MntPagos|
													      /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:MntPagos|
													      /DTE/Exportaciones/Encabezado/IdDoc/MntPagos">

                          <xsl:variable name="cuenta" select="position()"/>
                          <xsl:if test="$cuenta &lt;  '4'">
                            <tr>

                              -->
                <!-- <td width="10%" class="cabeceraClientesMenor">&#160;Fecha Pago</td> -->
                <!--
                              <td colspan="2" style="font-family :arial;font-size:9px;text-align :left;">
                                &#160;
                                <xsl:call-template name="format-fecha-all">
                                  <xsl:with-param name="input" select="/sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:MntPagos|sii:FchPago|
																							 /DTE/Documento/Encabezado/IdDoc/MntPagos/FchPago|
																							 /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:FchPago|
																							 /DTE/Liquidacion/Encabezado/IdDoc/MntPagos/FchPago|
																							 /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:FchPago|
																							 /DTE/Exportaciones/Encabezado/IdDoc/MntPagos/FchPago"/>
                                  <xsl:with-param name="nombre" select="'nombremayus'"/>
                                  <xsl:with-param name="separador" select="' '"/>
                                </xsl:call-template>



                              </td>
                              -->
                <!-- <td width="10%" class="cabeceraClientesMenor" style="border-left: 1px solid rgb(0,0,0)">&#160;Monto Pago</td> -->
                <!--
                              <td colspan="2" style="font-family :arial;font-size:9px;text-align :left;">

                                <xsl:call-template name="formatea-number">
                                  <xsl:with-param name="val" select="/sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:MntPagos|sii:MntPago|
																							 /DTE/Documento/Encabezado/IdDoc/MntPagos/MntPago|
																							 /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:MntPago|
																							 /DTE/Liquidacion/Encabezado/IdDoc/MntPagos/MntPago|
																							 /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:MntPago|
																							 /DTE/Exportaciones/Encabezado/IdDoc/MntPagos/MntPago"/>
                                  <xsl:with-param name="format-string" select="'.##0'"/>
                                  <xsl:with-param name="locale" select="'cl'"/>
                                </xsl:call-template>


                              </td>
                              -->
                <!-- <td width="10%" class="cabeceraClientesMenor" style="border-left:1px solid rgb(0,0,0)">&#160;Glosa Pagos</td> -->
                <!--
                              <td colspan="2" style="font-family :arial;font-size:9px;text-align :left;">
                                <xsl:value-of select="/sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:MntPagos|sii:GlosaPagos|
																							 /DTE/Documento/Encabezado/IdDoc/MntPagos/GlosaPagos|
																							 /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:GlosaPagos|
																							 /DTE/Liquidacion/Encabezado/IdDoc/MntPagos/GlosaPagos|
																							 /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:MntPagos/sii:GlosaPagos|
																							 /DTE/Exportaciones/Encabezado/IdDoc/MntPagos/GlosaPagos"/>


                                <br/>

                              </td>

                            </tr>



                          </xsl:if>


                        </xsl:for-each>

                        <tr>
                          <td colspan="6" class="cabeceraClientes" >&#160;...</td>
                        </tr>



                      </xsl:when>
                      <xsl:otherwise>

                        <xsl:for-each select="/sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc/sii:MntPagos|
														  /DTE/Documento/Encabezado/IdDoc/MntPagos|
													      /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc/sii:MntPagos|
													      /DTE/Liquidacion/Encabezado/IdDoc/MntPagos|
													      /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:IdDoc/sii:MntPagos|
													      /DTE/Exportaciones/Encabezado/IdDoc/MntPagos">

                          <tr>

                            -->
                <!-- <td width="10%" class="cabeceraClientesMenor">&#160;Fecha Pago</td> -->
                <!--
                            <td colspan="2" style="font-family :arial;font-size:9px;text-align :left;">
                              &#160;
                              <xsl:call-template name="format-fecha-all">
                                <xsl:with-param name="input" select="sii:FchPago|FchPago"/>
                                <xsl:with-param name="nombre" select="'nombremayus'"/>
                                <xsl:with-param name="separador" select="' '"/>
                              </xsl:call-template>



                            </td>
                            -->
                <!-- <td width="10%" class="cabeceraClientesMenor" style="border-left: 1px solid rgb(0,0,0)">&#160;Monto Pago</td> -->
                <!--
                            <td colspan="2" style="font-family :arial;font-size:9px;text-align :left;">

                              <xsl:call-template name="formatea-number">
                                <xsl:with-param name="val" select="sii:MntPago|MntPago"/>
                                <xsl:with-param name="format-string" select="'.##0'"/>
                                <xsl:with-param name="locale" select="'cl'"/>
                              </xsl:call-template>


                            </td>
                            -->
                <!-- <td width="10%" class="cabeceraClientesMenor" style="border-left:1px solid rgb(0,0,0)">&#160;Glosa Pagos</td> -->
                <!--
                            <td colspan="2" style="font-family :arial;font-size:9px;text-align :left;">
                              <xsl:value-of select="sii:GlosaPagos|GlosaPagos"/>
                            </td>
                          </tr>

                        </xsl:for-each>





                      </xsl:otherwise>



                    </xsl:choose>





                  </table>


                </xsl:if>-->
                <!-- Tabla direccion instalacion -->
                <!--<xsl:if test="sii:DTE/sii:Documento/sii:Encabezado/sii:Transporte|
										  DTE/Documento/Encabezado/Transporte|
										  sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Transporte|
										  DTE/Exportaciones/Encabezado/Transporte">
                  <table width="100%" border="0" class="tablaDatos" cellpadding="0" cellspacing="0">
                    <tr>
                      <td colspan="6" style="border-bottom 	:1px solid rgb(0,0,0)" class="cabeceraClientes" >&#160;DATOS DESPACHO O INSTALACION :</td>
                    </tr>
                    <tr>
                      <td width="10%" class="cabeceraClientes">&#160;Direcci&#243;n</td>
                      <td width="30%" style="font-family :arial;font-size:10px;text-align :left;">
                        : <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Transporte/sii:DirDest|
																																			DTE/Documento/Encabezado/Transporte/DirDest|
																																			sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Transporte/sii:DirDest|
																																			DTE/Exportaciones/Encabezado/Transporte/DirDest"/>
                      </td>
                      <td class="cabeceraClientes" style="border-left: 1px solid rgb(0,0,0)">&#160;Comuna</td>
                      <td style="font-family :arial;font-size:10px;text-align :left;">
                        : <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Transporte/sii:CmnaDest|
																																DTE/Documento/Encabezado/Transporte/CmnaDest|
																																sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Transporte/sii:CmnaDest|
																																DTE/Exportaciones/Encabezado/Transporte/CmnaDest"/>
                      </td>
                      <td class="cabeceraClientes" style="border-left:1px solid rgb(0,0,0)">&#160;Ciudad</td>
                      <td style="font-family :arial;font-size:10px;text-align :left;">
                        : <xsl:value-of select="sii:DTE/sii:Documento/sii:Encabezado/sii:Transporte/sii:CiudadDest|
																																DTE/Documento/Encabezado/Transporte/CiudadDest|
																																sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Transporte/sii:CiudadDest|
																																DTE/Exportaciones/Encabezado/Transporte/CiudadDest"/>
                      </td>
                    </tr>
                  </table>
                  <div class="espacio"/>
                  <div class="espacio"/>
                </xsl:if>-->
                <!-- FIN tabla direccion instalacion -->
                <div class="espacio"/>
                <div class="espacio"/>
                <!-- Tabla aduana -->


                <!-- fin de tabla con datos aduana, empieza tabla referencia -->
                <xsl:if test="/sii:DTE/sii:Documento/sii:Referencia|
										  /DTE/Documento/Referencia|
										  /sii:DTE/sii:Exportaciones/sii:Referencia|
										  /DTE/Exportaciones/Referencia">
                  <table width="100%" border="0" class="tablaDatos" cellpadding="0" cellspacing="0">
                    <tr>
                      <td width="20%" valign="top" class="cabeceraClientes2" style="border-bottom:1px solid black;">Doc.Referencia</td>
                      <td width="13%" valign="top" class="cabeceraClientes2" style="border-left:1px solid black;border-bottom:1px solid black;">Folio</td>
                      <td width="8%" valign="top" class="cabeceraClientes2" style="border-left:1px solid black;border-bottom:1px solid black;">Fecha</td>
                      <td width="20%" valign="top" class="cabeceraClientes2" style="border-left:1px solid black;border-bottom:1px solid black;">Razon Ref.</td>
                    </tr>
                    <tr>
                      <!-- Tipo Documento -->
                      <td rowspan="3" width="13%" valign="top" height="20" style="font-family:arial;font-size:7.5pt;" align="left">
                        <xsl:choose>
                          <xsl:when test="/sii:DTE/sii:Documento/sii:Referencia|
																/DTE/Documento/Referencia|
																/sii:DTE/sii:Exportaciones/sii:Referencia|
																/DTE/Exportaciones/Referencia">
                            <xsl:choose>
                              <xsl:when test="/sii:DTE/sii:Documento/sii:Referencia/sii:TpoDocRef[ .!='']|
																	    /DTE/Documento/Referencia/TpoDocRef[ .!='']|
																		/sii:DTE/sii:Exportaciones/sii:Referencia/sii:TpoDocRef[ .!='']|
																	    /DTE/Exportaciones/Referencia/TpoDocRef[ .!='']">
                                <xsl:for-each select="/sii:DTE/sii:Documento/sii:Referencia|
																				  /DTE/Documento/Referencia|
																				  /sii:DTE/sii:Exportaciones/sii:Referencia|
																				  /DTE/Exportaciones/Referencia">
                                  <xsl:choose>
                                    <xsl:when test="sii:TpoDocRef[.='30']|TpoDocRef[.='30']|sii:TpoDocRef[.='030']|TpoDocRef[.='030']">
                                      FACTURA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='32']|TpoDocRef[.='32']|sii:TpoDocRef[.='032']|TpoDocRef[.='032']">
                                      FACTURA NO AFECTA O EXENTA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='33']|TpoDocRef[.='33']|sii:TpoDocRef[.='033']|TpoDocRef[.='033']">
                                      FACTURA ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='34']|TpoDocRef[.='34']|sii:TpoDocRef[.='034']|TpoDocRef[.='034']">
                                      FACTURA NO AFECTA O EXENTA ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='35']|TpoDocRef[.='35']|sii:TpoDocRef[.='035']|TpoDocRef[.='035']">
                                      BOLETA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='38']|TpoDocRef[.='38']|sii:TpoDocRef[.='038']|TpoDocRef[.='038']">
                                      BOLETA EXENTA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='39']|TpoDocRef[.='39']|sii:TpoDocRef[.='039']|TpoDocRef[.='039']">
                                      BOLETA ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='40']|TpoDocRef[.='40']|sii:TpoDocRef[.='040']|TpoDocRef[.='040']">
                                      LIQUIDACI&#211;N FACTURA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='41']|TpoDocRef[.='41']|sii:TpoDocRef[.='041']|TpoDocRef[.='041']">
                                      BOLETA EXENTA ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='43']|TpoDocRef[.='43']|sii:TpoDocRef[.='043']|TpoDocRef[.='043']">
                                      LIQUIDACI&#211;N FACTURA ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='45']|TpoDocRef[.='45']|sii:TpoDocRef[.='045']|TpoDocRef[.='045']">
                                      FACTURA DE COMPRA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='46']|TpoDocRef[.='46']|sii:TpoDocRef[.='046']|TpoDocRef[.='046']">
                                      FACTURA DE COMPRA ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='50']|TpoDocRef[.='50']|sii:TpoDocRef[.='050']|TpoDocRef[.='050']">
                                      GU&#205;A DE DESPACHO<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='52']|TpoDocRef[.='52']|sii:TpoDocRef[.='052']|TpoDocRef[.='052']">
                                      GU&#205;A DE DESPACHO ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='55']|TpoDocRef[.='55']|sii:TpoDocRef[.='055']|TpoDocRef[.='055']">
                                      NOTA DE D&#201;BITO<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='56']|TpoDocRef[.='56']|sii:TpoDocRef[.='056']|TpoDocRef[.='056']">
                                      NOTA DE D&#201;BITO ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='60']|TpoDocRef[.='60']|sii:TpoDocRef[.='060']|TpoDocRef[.='060']">
                                      NOTA DE CR&#201;DITO<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='61']|TpoDocRef[.='61']|sii:TpoDocRef[.='061']|TpoDocRef[.='061']">
                                      NOTA DE CR&#201;DITO ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='103']|TpoDocRef[.='103']">
                                      LIQUIDACI&#211;N DE COMISIONISTA DISTRIBUIDOR<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='110']|TpoDocRef[.='110']">
                                      FACTURA DE EXPORTACI&#211;N ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='111']|TpoDocRef[.='111']">
                                      NOTA DE D&#201;BITO DE EXPORTACI&#211;N ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='112']|TpoDocRef[.='112']">
                                      NOTA DE CR&#201;DITO DE EXPORTACI&#211;N ELECTR&#211;NICA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='801']|TpoDocRef[.='801']">
                                      ORDEN DE COMPRA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='802']|TpoDocRef[.='802']">
                                      NOTA DE PEDIDO<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='803']|TpoDocRef[.='803']">
                                      CONTRATO<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='804']|TpoDocRef[.='804']">
                                      RESOLUCI&#211;N<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='805']|TpoDocRef[.='805']">
                                      PROCESO CHILECOMPRA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='806']|TpoDocRef[.='806']">
                                      FICHA CHILECOMPRA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='807']|TpoDocRef[.='807']">
                                      DUS<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='808']|TpoDocRef[.='808']">
                                      B/L (CONOCIMIENTO DE EMBARQUE)<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='809']|TpoDocRef[.='809']">
                                      AIR WILL BILL<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='810']|TpoDocRef[.='810']">
                                      MIC/DTA<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='811']|TpoDocRef[.='811']">
                                      CARTA DE PORTE<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='812']|TpoDocRef[.='812']">
                                      DUS<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='813']|TpoDocRef[.='813']">
                                      RESOLUCI&#211;N DEL SNA SERVICIO DE EXPORTA&#211;N<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='814']|TpoDocRef[.='814']">
                                      CERTIFICADO DE DEP&#211;SITO BOLSA PROD. CHILE<br/>
                                    </xsl:when>
                                    <xsl:when test="sii:TpoDocRef[.='815']|TpoDocRef[.='815']">
                                      VALE DE PRENDA BOLSA PROD. CHILE<br/>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <xsl:value-of select="sii:TpoDocRef|TpoDocRef" />
                                      <br/>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </xsl:for-each>
                              </xsl:when>
                              <xsl:otherwise>
                                &#160;
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:when>
                          <xsl:otherwise>
                            &#160;
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                      <!-- Folio -->
                      <td rowspan="3" width="12%" valign="top" height="20" style="font-family:arial;font-size:7.5pt;border-left:1px solid black;" align="center">
                        <xsl:choose>
                          <xsl:when test="/sii:DTE/sii:Documento/sii:Referencia|
															    /DTE/Documento/Referencia|
																/sii:DTE/sii:Exportaciones/sii:Referencia|
															    /DTE/Exportaciones/Referencia">
                            <xsl:choose>
                              <xsl:when test="/sii:DTE/sii:Documento/sii:Referencia/sii:FolioRef[ .!='']|
																	    /DTE/Documento/Referencia/FolioRef[ .!='']|
																		/sii:DTE/sii:Documento/sii:Referencia/sii:FolioRef[ .!='']|
																	    /DTE/Exportaciones/Referencia/FolioRef[ .!='']">
                                <xsl:for-each select="sii:DTE/sii:Documento/sii:Referencia|
																				  DTE/Documento/Referencia|
																				  sii:DTE/sii:Exportaciones/sii:Referencia|
																				  DTE/Exportaciones/Referencia">
                                  <xsl:value-of select="sii:FolioRef|FolioRef"/>
                                  <br/>
                                </xsl:for-each>
                              </xsl:when>
                              <xsl:otherwise>
                                &#160;
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:when>
                          <xsl:otherwise>
                            &#160;
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                      <!-- Fecha -->
                      <td rowspan="3" width="12%" valign="top" height="20" style="font-family:arial;font-size:7.5pt;border-left:1px solid black;" align="center">
                        <xsl:choose>
                          <xsl:when test="/sii:DTE/sii:Documento/sii:Referencia|
																/DTE/Documento/Referencia|
																/sii:DTE/sii:Exportaciones/sii:Referencia|
																/DTE/Exportaciones/Referencia">
                            <xsl:choose>
                              <xsl:when test="/sii:DTE/sii:Documento/sii:Referencia/sii:FchRef[ .!='']|
																		/DTE/Documento/Referencia/FchRef[ .!='']|
																		/sii:DTE/sii:Exportaciones/sii:Referencia/sii:FchRef[ .!='']|
																		/DTE/Exportaciones/Referencia/FchRef[ .!='']">
                                <xsl:for-each select="sii:DTE/sii:Documento/sii:Referencia|
																				  DTE/Documento/Referencia|
																				  sii:DTE/sii:Exportaciones/sii:Referencia|
																				  DTE/Exportaciones/Referencia">
                                  <xsl:value-of select="ms:format-date(sii:FchRef|FchRef, 'dd-MM-yyyy')"/>
                                  <br/>
                                </xsl:for-each>
                              </xsl:when>
                              <xsl:otherwise>
                                &#160;
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:when>
                          <xsl:otherwise>
                            &#160;
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                      <!-- Referencias -->
                      <td rowspan="3" width="20%" valign="top" height="20" align="left" style="font-family:arial;font-size:7.5pt;border-left:1px solid black;">
                        <xsl:choose>
                          <xsl:when test="/sii:DTE/sii:Documento/sii:Referencia|
																/DTE/Documento/Referencia|
																/sii:DTE/sii:Exportaciones/sii:Referencia|
																/DTE/Exportaciones/Referencia">
                            <xsl:choose>
                              <xsl:when test="/sii:DTE/sii:Documento/sii:Referencia/sii:FchRef[ .!='']|
																		/DTE/Documento/Referencia/FchRef[ .!='']|
																		/sii:DTE/sii:Exportaciones/sii:Referencia/sii:FchRef[ .!='']|
																		/DTE/Exportaciones/Referencia/FchRef[ .!='']">
                                <xsl:for-each select="sii:DTE/sii:Documento/sii:Referencia|
																				  DTE/Documento/Referencia|
																				  sii:DTE/sii:Exportaciones/sii:Referencia|
																				  DTE/Exportaciones/Referencia">
                                  <xsl:value-of select="sii:RazonRef|RazonRef"/>
                                  <br/>
                                </xsl:for-each>
                              </xsl:when>
                              <xsl:otherwise>
                                &#160;
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:when>
                          <xsl:otherwise>
                            &#160;
                          </xsl:otherwise>
                        </xsl:choose>
                      </td>
                    </tr>
                  </table>
                </xsl:if>
                <div class="espacio"/>
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                  <tr>
                    <td>
                      <!--  INICIO de tabla contenido - DETALLE -->
                      <table border="0" width="100%" height="400" cellspacing="0" cellpadding="2">
                        <tr>
                          <td class="cabeceraClientes" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;" width="20" align="center">
                            <b>N&#186;</b>
                          </td>
                          <td class="cabeceraClientes" width="50" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                            <b>Cantidad</b>
                          </td>
                          <td class="cabeceraClientes" width="40" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                            <b>Unidad</b>
                          </td>
                          <td class="cabeceraClientes" width="60" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                            <b>C&#243;digo</b>
                          </td>
                          <td class="cabeceraClientes" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                            <b>Descripci&#243;n</b>
                          </td>
                          <td class="cabeceraClientes" width="80" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                            <b>Precio</b>
                          </td>
                          <td class="cabeceraClientes" width="90" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                            <b>Desc/Recargo</b>
                          </td>
                          <td class="cabeceraClientes" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;border-right:1px solid black;" width="80" align="center">
                            <b>Total</b>
                          </td>
                        </tr>
                        <xsl:for-each select="sii:DTE/sii:Documento/sii:Detalle|
																  DTE/Documento/Detalle|
																  sii:DTE/sii:Liquidacion/sii:Detalle|
																  DTE/Liquidacion/Detalle|
																  sii:DTE/sii:Exportaciones/sii:Detalle|
																  DTE/Exportaciones/Detalle">
                          <tr>
                            <xsl:choose>
                              <xsl:when test="sii:NroLinDet|NroLinDet[.!='']">
                                <td class="productoTextoConMargenSIN" style="border-left:1px solid black;" align="center" valign="top">
                                  <xsl:value-of select="sii:NroLinDet|NroLinDet"/>
                                </td>
                              </xsl:when>
                              <xsl:otherwise>
                                <td class="productoTextoConMargenSIN" style="border-left:1px solid black;" valign="top">&#160;</td>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="sii:QtyItem|QtyItem[.!='']">
                                <td class="productoTextoConMargenSIN" style="border-left:1px solid black;" align="center" valign="top">
                                  <xsl:value-of select="sii:QtyItem|QtyItem"/>
                                </td>
                              </xsl:when>
                              <xsl:otherwise>
                                <td class="productoTextoConMargenSIN" style="border-left:1px solid black;"  valign="top">&#160;</td>
                              </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                              <xsl:when test="sii:UnmdItem|UnmdItem[.!='']">
                                <td class="productoTextoConMargenSIN" style="border-left:1px solid black;" align="center" valign="top" >
                                  <xsl:value-of select="sii:UnmdItem|UnmdItem"/>&#160;
                                </td>
                              </xsl:when>
                              <xsl:otherwise>
                                <td class="productoTextoConMargenSIN" style="border-left:1px solid black;" valign="top">&#160;</td>
                              </xsl:otherwise>
                            </xsl:choose>





                            <td class="productoTextoConMargenSIN" align="left" valign="top" style="border-left:1px solid black;">
                              &#160;
                              <xsl:choose>
                                <xsl:when test="sii:CdgItem/sii:VlrCodigo[.!=''] | CdgItem/VlrCodigo[.!='']">
                                  <xsl:value-of select="sii:CdgItem/sii:VlrCodigo | CdgItem/VlrCodigo"/>
                                </xsl:when>
                                <xsl:otherwise>&#160;</xsl:otherwise>
                              </xsl:choose>



                            </td>
                            <!--  DESCRIPCION PRODUCTO  -->
                            <td class="productoTextoConMargenSIN" align="left" valign="top" style="border-left:1px solid black;">
                              &#160;<xsl:value-of select="sii:NmbItem|NmbItem"/>
                              <table>
                                <tr>
                                  <td class="productoTextoConMargenSin2" valign="top" style="border-left:3px solid white;">
                                    <xsl:choose>
                                      <xsl:when test="sii:DscItem|DscItem[.!= '']">
                                        <div class="saltoLinea">&#160;</div>
                                        <xsl:call-template name="divide_en_lineas">
                                          <xsl:with-param name="val">
                                            <xsl:value-of select="sii:DscItem|DscItem"/>
                                          </xsl:with-param>
                                          <xsl:with-param name="c1">
                                            <xsl:value-of select="'^'" />
                                          </xsl:with-param>
                                        </xsl:call-template>
                                      </xsl:when>
                                      <xsl:otherwise>&#160;</xsl:otherwise>
                                    </xsl:choose>
                                  </td>
                                </tr>
                              </table>
                            </td>

                            <!--  FIN DESC  -->
                            <td class="productoTextoConMargenSIN" align="right" valign="top" style="border-left:1px solid black;">
                              <xsl:if test="sii:PrcItem|PrcItem">
                                <xsl:call-template name="formatea-number">
                                  <xsl:with-param name="val" select="sii:PrcItem|PrcItem"/>
                                  <xsl:with-param name="format-string" select="'.##0,####'"/>
                                  <xsl:with-param name="locale" select="'cl'"/>
                                </xsl:call-template>
                              </xsl:if>&#160;
                            </td>
                            <td class="productoTextoConMargenSIN" style="border-left:1px solid black;" align="right">
                              <xsl:choose>
                                <xsl:when test="sii:DescuentoMonto|DescuentoMonto[.!='']">
                                  -<xsl:call-template name="formatea-number">
                                    <xsl:with-param name="val" select="sii:DescuentoMonto|DescuentoMonto"/>
                                    <xsl:with-param name="format-string" select="'.##0'"/>
                                    <xsl:with-param name="locale" select="'cl'"/>
                                  </xsl:call-template>
                                  (-<xsl:value-of select="sii:DescuentoPct|DescuentoPct"/>%)
                                </xsl:when>
                                <xsl:when test="sii:RecargoMonto|RecargoMonto[.!='']">
                                  <xsl:call-template name="formatea-number">
                                    <xsl:with-param name="val" select="sii:RecargoMonto|RecargoMonto"/>
                                    <xsl:with-param name="format-string" select="'.##0'"/>
                                    <xsl:with-param name="locale" select="'cl'"/>
                                  </xsl:call-template>
                                  (<xsl:value-of select="sii:RecargoPct|RecargoPct"/>%)
                                </xsl:when>
                              </xsl:choose>&#160;&#160;
                            </td>
                            <td class="productoTextoConMargenSIN" align="right" style="border-right:1px solid black;border-left:1px solid black;" valign="top">
                              <xsl:call-template name="formatea-number">
                                <xsl:with-param name="val" select="sii:MontoItem|MontoItem"/>
                                <xsl:with-param name="format-string" select="'.##0'"/>
                                <xsl:with-param name="locale" select="'cl'"/>
                              </xsl:call-template>&#160;&#160;
                            </td>
                          </tr>
                        </xsl:for-each>
                        <tr>
                          <td class="productoTextoConMargenSIN" style="border-left:1px solid black;border-bottom:1px solid black;" height="100%" valign="top">
                            <br/>
                          </td>
                          <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                            <br/>
                          </td>
                          <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                            <br/>
                          </td>
                          <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                            <br/>
                          </td>
                          <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                            <br/>
                          </td>
                          <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                            <br/>
                          </td>
                          <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                            <br/>
                          </td>
                          <td class="productoTextoConMargenSIN" style="border-right:1px solid black;border-left:1px solid black;border-bottom:1px solid black;" height="100%" valign="top">
                            <br/>
                          </td>
                        </tr>
                      </table>
                      <div class="espacio"/>
                      <!--  COMISIONES  -->
                      <xsl:choose>
                        <xsl:when test="sii:DTE/sii:Liquidacion/sii:Comisiones|DTE/Liquidacion/Comisiones">
                          <table border="0" width="100%" cellspacing="0" cellpadding="2">
                            <tr>
                              <td class="cabeceraClientes" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;" width="20" align="center">
                                <b>N&#186;</b>
                              </td>
                              <td class="cabeceraClientes" width="90" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                                <b>Tipo Movimiento</b>
                              </td>
                              <td class="cabeceraClientes" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                                <b>Descripci&#243;n</b>
                              </td>
                              <td class="cabeceraClientes" width="90" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                                <b>Tasa Comisi&#243;n</b>
                              </td>
                              <td class="cabeceraClientes" width="80" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                                <b>Neto</b>
                              </td>
                              <td class="cabeceraClientes" width="90" align="center" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;">
                                <b>Valor Exento</b>
                              </td>
                              <td class="cabeceraClientes" style="border-top:1px solid black;border-left:1px solid black;border-bottom:1px solid black;border-right:1px solid black;" width="80" align="center">
                                <b>IVA Comisi&#243;n</b>
                              </td>
                            </tr>
                            <xsl:for-each select="sii:DTE/sii:Liquidacion/sii:Comisiones|DTE/Liquidacion/Comisiones|sii:DTE/sii:Liquidacion/sii:Comisiones|DTE/Liquidacion/Comisiones">
                              <tr>
                                <xsl:choose>
                                  <xsl:when test="sii:NroLinCom|NroLinCom[.!='']">
                                    <td class="productoTextoConMargenSin" style="border-left:1px solid black;" align="center" valign="top">
                                      <xsl:value-of select="sii:NroLinCom|NroLinCom"/>
                                    </td>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td class="productoTextoConMargenSin" style="border-left:1px solid black;" valign="top">&#160;</td>
                                  </xsl:otherwise>
                                </xsl:choose>
                                <xsl:choose>
                                  <xsl:when test="sii:TipoMovim|TipoMovim[.='C']">
                                    <td class="productoTextoConMargenSin" style="border-left:1px solid black;" align="left" valign="top">&#160;Comisi&#243;n</td>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td class="productoTextoConMargenSin" style="border-left:1px solid black;"  valign="top" align="left">&#160;Otros Cargos</td>
                                  </xsl:otherwise>
                                </xsl:choose>
                                <xsl:choose>
                                  <xsl:when test="sii:Glosa|Glosa[.!='']">
                                    <td class="productoTextoConMargenSin" style="border-left:1px solid black;" align="left" valign="top" >
                                      <xsl:value-of select="sii:Glosa|Glosa"/>&#160;
                                    </td>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <td class="productoTextoConMargenSin" style="border-left:1px solid black;" valign="top">&#160;</td>
                                  </xsl:otherwise>
                                </xsl:choose>
                                <td class="productoTextoConMargenSin" align="right" valign="top" style="border-left:1px solid black;">
                                  <xsl:if test="sii:TasaComision|TasaComision">
                                    <xsl:call-template name="formatea-number">
                                      <xsl:with-param name="val" select="sii:TasaComision|TasaComision"/>
                                      <xsl:with-param name="format-string" select="'.##0,####'"/>
                                      <xsl:with-param name="locale" select="'cl'"/>
                                    </xsl:call-template>
                                  </xsl:if>&#160;
                                </td>
                                <td class="productoTextoConMargenSin" align="right" valign="top" style="border-left:1px solid black;">
                                  <xsl:if test="sii:ValComNeto|ValComNeto">
                                    <xsl:call-template name="formatea-number">
                                      <xsl:with-param name="val" select="sii:ValComNeto|ValComNeto"/>
                                      <xsl:with-param name="format-string" select="'.##0,####'"/>
                                      <xsl:with-param name="locale" select="'cl'"/>
                                    </xsl:call-template>
                                  </xsl:if>&#160;
                                </td>
                                <td class="productoTextoConMargenSin" align="right" style="border-left:1px solid black;" valign="top">
                                  <xsl:if test="sii:ValComExe|ValComExe">
                                    <xsl:call-template name="formatea-number">
                                      <xsl:with-param name="val" select="sii:ValComExe|ValComExe"/>
                                      <xsl:with-param name="format-string" select="'.##0,####'"/>
                                      <xsl:with-param name="locale" select="'cl'"/>
                                    </xsl:call-template>
                                  </xsl:if>&#160;
                                </td>
                                <td class="productoTextoConMargenSin" align="right" valign="top" style="border-left:1px solid black;border-right:1px solid black;">
                                  <xsl:if test="sii:ValComIVA|ValComIVA">
                                    <xsl:call-template name="formatea-number">
                                      <xsl:with-param name="val" select="sii:ValComIVA|ValComIVA"/>
                                      <xsl:with-param name="format-string" select="'.##0,####'"/>
                                      <xsl:with-param name="locale" select="'cl'"/>
                                    </xsl:call-template>
                                  </xsl:if>&#160;
                                </td>
                              </tr>
                            </xsl:for-each>
                            <tr>
                              <td class="productoTextoConMargenSIN" style="border-left:1px solid black;border-bottom:1px solid black;" height="100%" valign="top">
                                <br/>
                              </td>
                              <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                                <br/>
                              </td>
                              <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                                <br/>
                              </td>
                              <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                                <br/>
                              </td>
                              <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                                <br/>
                              </td>
                              <td class="productoTextoConMargenSIN" height="100%" valign="top" style="border-left:1px solid black;border-bottom:1px solid black;">
                                <br/>
                              </td>
                              <td class="productoTextoConMargenSIN" style="border-right:1px solid black;border-left:1px solid black;border-bottom:1px solid black;" height="100%" valign="top">
                                <br/>
                              </td>
                            </tr>
                          </table>
                          <!-- FIN COMISIONES  -->
                          <div class="espacio"/>
                        </xsl:when>
                      </xsl:choose>
                      <!--<xsl:if test="DatoAdjunto[@nombre='MontoEscrito']">
                        <table border="0" width="100%" cellspacing="0" cellpadding="2">
                          <tr>
                            -->
                      <!-- <td class="productoTextoConMargenDF" style="border-left:1px solid black;border-bottom:1px solid black;border-right:1px solid black;" valign="top">  -->
                      <!--
                            <td class="productoTextoConMargenDF" style="border:1px solid black;" valign="top">
                              &#160;SON:
                              <xsl:value-of select="DatoAdjunto[@nombre='MontoEscrito']"/>
                              <br/>
                            </td>
                          </tr>
                        </table>
                      </xsl:if>-->
                      <div class="espacio"/>
                    </td>
                  </tr>
                </table>
                <!-- fin tabla contenido -->
                <!-- observaciones y transporte -->
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                  <tr>
                    <td valign="top" width="58%">
                      <table align="left" border="0" width="100%" style="border:1px solid black;" cellspacing="0" cellpadding="0">
                        <td valign="top">
                          <table cellpadding="0" cellspacing="0">
                            <tr>
                              <td valign="top" width="100%" class="cabeceraClientes">&#160;Observaciones</td>
                              <div class="espacio"/>
                            </tr>

                            <xsl:for-each select="/sii:DTE/sii:Documento/sii:Encabezado/sii:IdDoc|
																		  /DTE/Documento/Encabezado/IdDoc|
																		  /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:IdDoc|
																		  /DTE/Liquidacion/Encabezado/IdDoc">

                              <xsl:if test="sii:SaldoInsol|SaldoInsol">
                                <tr>
                                  <td style="font-family:arial;font-size:8pt;">
                                    &#160;Saldo Insoluto:&#160;
                                    <xsl:value-of select="sii:SaldoInsol|SaldoInsol"/>
                                  </td>
                                </tr>
                              </xsl:if>

                              <xsl:if test="sii:NumCtaPago|NumCtaPago">
                                <tr>
                                  <td style="font-family:arial;font-size:8pt;">
                                    &#160;Cuenta de Pago:&#160;
                                    <xsl:value-of select="sii:NumCtaPago|NumCtaPago"/>
                                  </td>
                                </tr>
                              </xsl:if>

                              <xsl:if test="sii:BcoPago|BcoPago">
                                <tr>
                                  <td style="font-family:arial;font-size:8pt;">
                                    &#160;Banco de Pago:&#160;
                                    <xsl:value-of select="sii:BcoPago|BcoPago"/>
                                  </td>
                                </tr>
                              </xsl:if>

                              <xsl:if test="sii:TermPagoGlosa|TermPagoGlosa">
                                <tr>
                                  <td style="font-family:arial;font-size:8pt;">
                                    &#160;Terminos del Pago:&#160;
                                    <xsl:value-of select="sii:TermPagoGlosa|TermPagoGlosa"/>
                                  </td>
                                </tr>
                              </xsl:if>

                            </xsl:for-each>
                          </table>
                        </td>
                        <!-- Pendiente agregar TRANSPORTE .....-->
                      </table>
                      <div class="espacio"/>
                    </td>
                    <td valign="top" width="1%" border="0"> &#160;</td>
                    <!-- INICIO MONTOS TOTALES -->
                    <td valign="top" border="0">
                      <table border="0" width="100%" cellspacing="0" cellpadding="0">
                        <tr>
                          <td valign="top" class="letravalores">
                            <table width="100%" border="0" class="tablaDatos" cellpadding="0" cellspacing="0">
                              <tr>
                                <td colspan="3" width="100%" align="top" class="cabeceraClientes2" style="border-bottom:1px solid black;">Montos Totales</td>
                              </tr>
                              <tr>
                                <!--<xsl:if test="/DatoAdjunto[@nombre='Subtotal']">
                                  <td valign="top" width="12%" class="cabeceraClientes2" style="text-align:left;">&#160;Subtotal</td>
                                  <td width="2%" valign="top" class="cabeceraClientes2">$</td>
                                  <td width="19%" valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                    <xsl:call-template name="formatea-number">
                                      <xsl:with-param name="val" select="/DatoAdjunto[@nombre='Subtotal']"/>
                                      <xsl:with-param name="format-string" select="'.##0'"/>
                                      <xsl:with-param name="locale" select="'cl'"/>
                                    </xsl:call-template>
                                    &#160;&#160;
                                  </td>
                                </xsl:if>-->
                              </tr>
                              <xsl:for-each select="/sii:DTE/sii:Documento/sii:DscRcgGlobal|
																  /DTE/Documento/DscRcgGlobal|
																  /sii:DTE/sii:Exportaciones/sii:DscRcgGlobal|
																  /DTE/Exportaciones/DscRcgGlobal">
                                <tr>
                                  <xsl:choose>
                                    <xsl:when test="sii:TpoMov[.='D']|TpoMov[.='D']">
                                      <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Descuento</td>
                                      <td valign="top" class="cabeceraClientes2">$</td>
                                      <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                        - <xsl:call-template name="formatea-number">
                                          <xsl:with-param name="val" select="sii:ValorDR | ValorDR"/>
                                          <xsl:with-param name="format-string" select="'.##0'"/>
                                          <xsl:with-param name="locale" select="'cl'"/>
                                        </xsl:call-template>&#160;&#160;
                                      </td>
                                    </xsl:when>
                                    <xsl:otherwise>
                                      <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Recargo</td>
                                      <td valign="top" class="cabeceraClientes2">$</td>
                                      <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                        <xsl:call-template name="formatea-number">
                                          <xsl:with-param name="val" select="sii:ValorDR | ValorDR"/>
                                          <xsl:with-param name="format-string" select="'.##0'"/>
                                          <xsl:with-param name="locale" select="'cl'"/>
                                        </xsl:call-template>
                                        &#160;&#160;
                                      </td>
                                    </xsl:otherwise>
                                  </xsl:choose>
                                </tr>
                              </xsl:for-each>
                              <xsl:for-each select="/sii:DTE/sii:Documento/sii:Encabezado/sii:Totales|
																									  /DTE/Documento/Encabezado/Totales|
																									  /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Totales|
																									  /DTE/Liquidacion/Encabezado/Totales|
																									  /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:Totales|
																									  /DTE/Exportaciones/Encabezado/Totales">
                                <!-- impresion CredEc - Credito empresas constructoras -->
                                <xsl:if test="sii:CredEC[.!='']|CredEC[.!='']|sii:CredEC[.!='0']|CredEC[.!='0']">
                                  <tr>
                                    <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Cred. Emp. Constructoras</td>
                                    <td valign="top" class="cabeceraClientes2">$</td>
                                    <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                      <xsl:call-template name="formatea-number">
                                        <xsl:with-param name="val" select="sii:CredEC|CredEC"/>
                                        <xsl:with-param name="format-string" select="'.##0'"/>
                                        <xsl:with-param name="locale" select="'cl'"/>
                                      </xsl:call-template>
                                      &#160;
                                    </td>
                                  </tr>
                                </xsl:if>
                                <xsl:if test="sii:MntExe[.!='0']|MntExe[.!='0'] or ../sii:IdDoc/sii:TipoDTE[.='34'] or ../IdDoc/TipoDTE[.='34']">
                                  <tr>
                                    <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Exento</td>
                                    <td valign="top" class="cabeceraClientes2">$</td>
                                    <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                      <xsl:call-template name="formatea-number">
                                        <xsl:with-param name="val" select="sii:MntExe|MntExe"/>
                                        <xsl:with-param name="format-string" select="'.##0'"/>
                                        <xsl:with-param name="locale" select="'cl'"/>
                                      </xsl:call-template>
                                      &#160;&#160;
                                    </td>
                                  </tr>
                                </xsl:if>
                                <tr>
                                  <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Monto Neto</td>
                                  <td valign="top" class="cabeceraClientes2">$</td>
                                  <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                    <xsl:if test="sii:MntNeto|MntNeto">
                                      <xsl:call-template name="formatea-number">
                                        <xsl:with-param name="val" select="sii:MntNeto|MntNeto"/>
                                        <xsl:with-param name="format-string" select="'#.##0'"/>
                                        <xsl:with-param name="locale" select="'cl'"/>
                                      </xsl:call-template>
                                    </xsl:if>&#160;&#160;
                                  </td>
                                </tr>
                                <tr>
                                  <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Monto I.V.A.</td>
                                  <td valign="top" class="cabeceraClientes2">$</td>
                                  <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                    <xsl:if test="sii:IVA|IVA">
                                      <xsl:call-template name="formatea-number">
                                        <xsl:with-param name="val" select="sii:IVA|IVA"/>
                                        <xsl:with-param name="format-string" select="'#.##0'"/>
                                        <xsl:with-param name="locale" select="'cl'"/>
                                      </xsl:call-template>
                                    </xsl:if>&#160;&#160;
                                  </td>
                                </tr>
                                <xsl:for-each select="sii:ImptoReten|ImptoReten">
                                  <tr>
                                    <td valign="top" class="cabeceraClientes2" style="text-align:left;">
                                      &#160;
                                      <xsl:call-template name="TipoImp">
                                        <xsl:with-param name="tipo" select="sii:TipoImp|TipoImp"/>
                                      </xsl:call-template>
                                      <xsl:if test="sii:TasaImp|TasaImp">
                                        &#160;(<xsl:value-of select="format-number(sii:TasaImp|TasaImp,'.##0','cl')"/>%)
                                      </xsl:if>
                                    </td>
                                    <td valign="top" class="cabeceraClientes2">$</td>
                                    <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                      <xsl:choose>
                                        <xsl:when test="sii:MontoImp|MontoImp">
                                          <xsl:call-template name="formatea-number">
                                            <xsl:with-param name="val" select="sii:MontoImp|MontoImp"/>
                                            <xsl:with-param name="format-string" select="'#.##0'"/>
                                            <xsl:with-param name="locale" select="'cl'"/>
                                          </xsl:call-template>&#160;&#160;
                                        </xsl:when>
                                        <xsl:otherwise>0&#160;&#160;</xsl:otherwise>
                                      </xsl:choose>
                                    </td>
                                  </tr>
                                </xsl:for-each>
                                <!-- Comisiones  -->
                                <xsl:for-each select="/sii:DTE/sii:Documento/sii:Encabezado/sii:Totales/sii:Comisiones|/DTE/Documento/Encabezado/Totales/Comisiones|/sii:DTE/sii:Liquidacion/sii:Encabezado/sii:Totales/sii:Comisiones|/DTE/Liquidacion/Encabezado/Totales/Comisiones|/DTE/Liquidacion/Encabezado/Totales/Comisiones">
                                  <xsl:choose>
                                    <xsl:when test="sii:ValComExe|ValComExe[.!='']">
                                      <tr>
                                        <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Comision Exento</td>
                                        <td valign="top" class="cabeceraClientes2">$</td>
                                        <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                          <xsl:if test="sii:ValComExe|ValComExe">
                                            <xsl:call-template name="formatea-number">
                                              <xsl:with-param name="val" select="sii:ValComExe|ValComExe"/>
                                              <xsl:with-param name="format-string" select="'#.##0'"/>
                                              <xsl:with-param name="locale" select="'cl'"/>
                                            </xsl:call-template>
                                          </xsl:if>&#160;&#160;
                                        </td>
                                      </tr>
                                    </xsl:when>
                                  </xsl:choose>
                                  <xsl:choose>
                                    <xsl:when test="sii:ValComNeto|ValComNeto[.!='']">
                                      <tr>
                                        <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Comision Neto</td>
                                        <td valign="top" class="cabeceraClientes2">$</td>
                                        <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                          <xsl:if test="sii:ValComNeto|ValComNeto">
                                            <xsl:call-template name="formatea-number">
                                              <xsl:with-param name="val" select="sii:ValComNeto|ValComNeto"/>
                                              <xsl:with-param name="format-string" select="'#.##0'"/>
                                              <xsl:with-param name="locale" select="'cl'"/>
                                            </xsl:call-template>
                                          </xsl:if>&#160;&#160;
                                        </td>
                                      </tr>
                                    </xsl:when>
                                  </xsl:choose>
                                  <xsl:choose>
                                    <xsl:when test="sii:ValComIVA|ValComIVA[.!='']">
                                      <tr>
                                        <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;IVA Comision 19%</td>
                                        <td valign="top" class="cabeceraClientes2">$</td>
                                        <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                          <xsl:if test="sii:ValComIVA|ValComIVA">
                                            <xsl:call-template name="formatea-number">
                                              <xsl:with-param name="val" select="sii:ValComIVA|ValComIVA"/>
                                              <xsl:with-param name="format-string" select="'#.##0'"/>
                                              <xsl:with-param name="locale" select="'cl'"/>
                                            </xsl:call-template>
                                          </xsl:if>&#160;&#160;
                                        </td>
                                      </tr>
                                    </xsl:when>
                                  </xsl:choose>
                                </xsl:for-each>
                                <!-- fin comisiones  -->
                                <tr>
                                  <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Monto Total</td>
                                  <td valign="top" class="cabeceraClientes2">$</td>
                                  <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                    <xsl:if test="sii:MntTotal|MntTotal">
                                      <xsl:call-template name="formatea-number">
                                        <xsl:with-param name="val" select="sii:MntTotal|MntTotal"/>
                                        <xsl:with-param name="format-string" select="'#.##0'"/>
                                        <xsl:with-param name="locale" select="'cl'"/>
                                      </xsl:call-template>
                                    </xsl:if>&#160;&#160;
                                  </td>
                                </tr>
                                <xsl:if test="sii:MontoNF|MontoNF">
                                  <tr>
                                    <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;No Facturable</td>
                                    <td valign="top" class="cabeceraClientes2">$</td>
                                    <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                      <xsl:call-template name="formatea-number">
                                        <xsl:with-param name="val" select="sii:MontoNF|MontoNF"/>
                                        <xsl:with-param name="format-string" select="'#.##0'"/>
                                        <xsl:with-param name="locale" select="'cl'"/>
                                      </xsl:call-template>&#160;&#160;
                                    </td>
                                  </tr>
                                </xsl:if>
                                <xsl:if test="sii:MontoPeriodo|MontoPeriodo">
                                  <tr>
                                    <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Monto Per&#237;odo</td>
                                    <td valign="top" class="cabeceraClientes2">$</td>
                                    <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                      <xsl:call-template name="formatea-number">
                                        <xsl:with-param name="val" select="sii:MontoPeriodo|MontoPeriodo"/>
                                        <xsl:with-param name="format-string" select="'#.##0'"/>
                                        <xsl:with-param name="locale" select="'cl'"/>
                                      </xsl:call-template>&#160;&#160;
                                    </td>
                                  </tr>
                                </xsl:if>
                                <xsl:if test="sii:SaldoAnterior|SaldoAnterior">
                                  <tr>
                                    <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Saldo Anterior</td>
                                    <td valign="top" class="cabeceraClientes2">$</td>
                                    <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                      <xsl:call-template name="formatea-number">
                                        <xsl:with-param name="val" select="sii:SaldoAnterior|SaldoAnterior"/>
                                        <xsl:with-param name="format-string" select="'#.##0'"/>
                                        <xsl:with-param name="locale" select="'cl'"/>
                                      </xsl:call-template>&#160;&#160;
                                    </td>
                                  </tr>
                                </xsl:if>
                                <xsl:if test="sii:VlrPagar|VlrPagar">
                                  <tr>
                                    <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Valor a Pagar</td>
                                    <td valign="top" class="cabeceraClientes2">$</td>
                                    <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                      <xsl:if test="sii:VlrPagar|VlrPagar">
                                        <xsl:call-template name="formatea-number">
                                          <xsl:with-param name="val" select="sii:VlrPagar|VlrPagar"/>
                                          <xsl:with-param name="format-string" select="'#.##0'"/>
                                          <xsl:with-param name="locale" select="'cl'"/>
                                        </xsl:call-template>&#160;&#160;
                                      </xsl:if>
                                    </td>
                                  </tr>
                                </xsl:if>
                              </xsl:for-each>
                            </table>
                          </td>
                        </tr>
                      </table>

                      <!-- TOTALES EN Otra Moneda Inicio -->

                      <xsl:choose>
                        <xsl:when test="/sii:DTE/sii:Documento/sii:Encabezado/sii:OtraMoneda|
																					    /DTE/Documento/Encabezado/OtraMoneda|
																						/sii:DTE/sii:Liquidacion/sii:Encabezado/sii:OtraMoneda|
																						/DTE/Liquidacion/Encabezado/OtraMoneda|
																						/sii:DTE/sii:Exportaciones/sii:Encabezado/sii:OtraMoneda|
																						/DTE/Exportaciones/Encabezado/OtraMoneda">
                          <div class="espacio"/>
                          <table align="right" border="0" width="100%m" cellspacing="0" cellpadding="0">
                            <tr>

                              <td valign="top" border="0">
                                <table width="100%" border="0" class="tablaDatos" cellpadding="0" cellspacing="0">
                                  <tr>
                                    <td colspan="3" width="100%" align="top" class="cabeceraClientes2" style="border-bottom:1px solid black;">Totales en Otra Moneda</td>
                                  </tr>
                                  <xsl:for-each select="/sii:DTE/sii:Documento/sii:Encabezado/sii:OtraMoneda|
																													  /DTE/Documento/Encabezado/OtraMoneda|
																													  /sii:DTE/sii:Liquidacion/sii:Encabezado/sii:OtraMoneda|
																													  /DTE/Liquidacion/Encabezado/OtraMoneda|
																													  /sii:DTE/sii:Exportaciones/sii:Encabezado/sii:OtraMoneda|
																													  /DTE/Exportaciones/Encabezado/OtraMoneda">


                                    <xsl:if test="sii:TpoMoneda|TpoMoneda">
                                      <tr>
                                        <td valign="top" class="cabeceraClientes2" style="text-align:left;">
                                          &#160;<xsl:value-of select="sii:TpoMoneda|TpoMoneda"/>&#160;
                                        </td>
                                        <td valign="top" class="cabeceraClientes2">$</td>
                                        <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                          <xsl:value-of select="sii:TpoCambio|TpoCambio"/>&#160;
                                        </td>
                                      </tr>
                                    </xsl:if>

                                    <xsl:if test="sii:MntExeOtrMnda|MntExeOtrMnda">
                                      <tr>
                                        <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Monto Exento</td>
                                        <td valign="top" class="cabeceraClientes2">$</td>
                                        <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                          <xsl:call-template name="formatea-number">
                                            <xsl:with-param name="val" select="sii:MntExeOtrMnda|MntExeOtrMnda"/>
                                            <xsl:with-param name="format-string" select="'###,##'"/>
                                            <xsl:with-param name="locale" select="'cl'"/>
                                          </xsl:call-template>
                                          &#160;
                                        </td>
                                      </tr>
                                    </xsl:if>
                                    <xsl:if test="sii:MntNetoOtrMnda|MntNetoOtrMnda">
                                      <tr>
                                        <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Monto Neto</td>
                                        <td valign="top" class="cabeceraClientes2">$</td>
                                        <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                          <xsl:call-template name="formatea-number">
                                            <xsl:with-param name="val" select="sii:MntNetoOtrMnda|MntNetoOtrMnda"/>
                                            <xsl:with-param name="format-string" select="'.##0'"/>
                                            <xsl:with-param name="locale" select="'cl'"/>
                                          </xsl:call-template>
                                          &#160;
                                        </td>
                                      </tr>
                                    </xsl:if>
                                    <xsl:if test="sii:IVAOtrMnda|IVAOtrMnda">
                                      <tr>
                                        <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Monto I.V.A.</td>
                                        <td valign="top" class="cabeceraClientes2">$</td>
                                        <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                          <xsl:call-template name="formatea-number">
                                            <xsl:with-param name="val" select="sii:IVAOtrMnda|IVAOtrMnda"/>

                                            <xsl:with-param name="format-string" select="'.##0,####'"/>
                                            <xsl:with-param name="locale" select="'cl'"/>
                                          </xsl:call-template>
                                          &#160;
                                        </td>
                                      </tr>
                                    </xsl:if>
                                    <xsl:if test="sii:MntTotOtrMnda|MntTotOtrMnda">
                                      <tr>
                                        <td valign="top" class="cabeceraClientes2" style="text-align:left;">&#160;Monto Total</td>
                                        <td valign="top" class="cabeceraClientes2">$</td>
                                        <td valign="top" style="font-family:arial;font-size:9pt;" align="right">
                                          <xsl:call-template name="formatea-number">
                                            <xsl:with-param name="val" select="sii:MntTotOtrMnda|MntTotOtrMnda"/>
                                            <xsl:with-param name="format-string" select="'#.###,##'"/>
                                            <xsl:with-param name="locale" select="'cl'"/>
                                          </xsl:call-template>
                                          &#160;
                                        </td>
                                      </tr>
                                    </xsl:if>

                                  </xsl:for-each>
                                </table>
                              </td>
                            </tr>

                          </table>
                        </xsl:when>
                        <xsl:otherwise> </xsl:otherwise>
                      </xsl:choose>
                      <!-- Fin totales -->
                    </td>
                  </tr>
                </table>
                <!-- inicio TIMBRE -->
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                  <tr>
                    <td valign="top" align="center" width="42%">
                      <xsl:choose>
                        <xsl:when test="sii:DTE/sii:Documento/sii:TED[.='']|DTE/Documento/TED[.='']|sii:DTE/sii:Liquidacion/sii:TED[.='']|DTE/Liquidacion/TED[.='']">

                          <br/>
                        </xsl:when>
                        <xsl:otherwise>

                          <img border="0" id="Timbre">
                            <xsl:attribute name="src">
                              <xsl:value-of select="$TedTimbre" />
                            </xsl:attribute>
                          </img>

                          <font face="arial" size="2" color="red">
                            <b>Timbre Electr&#243;nico SII</b>
                          </font>
                          <br/>
                          <font face="arial" size="1" color="red">
                            <br/>
                            Verifique documento:
                            <a href="http://www.sii.cl" target="_blank">www.sii.cl</a>
                          </font>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td align="center">&#160;</td>
                  </tr>
                </table>
                <!-- fin timbre -->
              </td>
            </tr>
          </table>
        </div>
      </body>
    </html>
  </xsl:template>

  <!-- TEMPLATES FUNCIONALES ..................................................................................  -->

  <xsl:template name="UPPER">
    <xsl:param name="text" />
    <xsl:variable name="val" select="translate($text, $smallcase, $uppercase)" />
    <xsl:value-of select="$val"/>
  </xsl:template>


  <xsl:template name="TipoImp">
    <!-- Permite dar titulo al tipo de impuesto adicional de los totales
      2010-03-10 EPS modifica actualizando tabla de impuestos y titulos  -->
    <xsl:param name="tipo"/>
    <xsl:choose>
      <xsl:when test="$tipo[.='14']">Imp. Adic. Marg. Comer.</xsl:when>
      <xsl:when test="$tipo[.='15']">
        Imp. Reten. Total Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='17']">
        Imp. Adic. Fae. Carne Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='18']">
        Imp. Adic. Ant. Carne Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='19']">
        Imp. Adic. Ant. Harina Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='23']">
        Imp. Adic. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='24']">
        I.L.A. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='25']">
        I.L.A. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='26']">
        I.L.A. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='27']">
        Imp. Adic. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='28']">
        Imp. Especifico Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='30']">
        Reten. Legumbres Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='31']">
        Reten. Silvestres Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='32']">
        Reten. Ganado Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='33']">
        Reten. Madera Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='34']">
        Reten. Trigo Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='35']">
        Imp. Esp. Gasolina Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='36']">
        Reten. Arroz Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='37']">
        Reten. Hidrob. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='38']">
        Reten. Chatarra Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='39']">
        Retenenido PPA Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='41']">
        Reten. Construccion Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='44']">
        Imp. Adic. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='45']">
        Imp. Adic. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='46']">
        Reten. Oro Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='47']">
        Reten. Cartones Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='48']">
        Reten. Franb. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='49']">
        Imp. Adic. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='50']">
        Imp. Adic. PrepagoTipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='301']">
        Reten. Legumbres Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='321']">
        Reten. Ganado Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='331']">
        Reten. Madera Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='341']">
        Reten. Trigo Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='361']">
        Reten. Arroz Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='371']">
        Reten. Hidrob. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:when test="$tipo[.='481']">
        Reten. Framb. Tipo: <xsl:value-of select="$tipo"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tipo"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template name="formatearRut">
    <!--Funcion para Formatear Rut  -->
    <xsl:param name="input"/>
    <xsl:variable name="rut" select="substring-before($input, '-')"/>
    <xsl:variable name="last" select="substring($rut,string-length($rut)-2,3)"/>
    <xsl:variable name="middle" select="substring($rut,string-length($rut)-5,3)"/>
    <xsl:variable name="first">
      <xsl:choose>
        <xsl:when test="string-length($rut)=7">
          <xsl:value-of select="substring($rut,1,1)"/>
        </xsl:when>
        <xsl:when test="string-length($rut)=8">
          <xsl:value-of select="substring($rut,1,2)"/>
        </xsl:when>
        <xsl:when test="string-length($rut)=9">
          <xsl:value-of select="substring($rut,1,3)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="dv" select="substring-after($input, '-')"/>
    <xsl:value-of select="concat($first,'.',$middle,'.',$last, '-', $dv)"/>
  </xsl:template>

  <!-- ...............................................................................................................  -->
  <xsl:template name="formatea-number">
    <!-- template para formatear numeros  -->
    <xsl:param name="val" />
    <xsl:param name="format-string" select="'#.##0'" />
    <xsl:param name="locale" select="'cl'" />
    <xsl:variable name="result" select="format-number($val, $format-string, $locale)" />
    <xsl:choose>
      <xsl:when test="$result = 'NaN'">
        <xsl:value-of select="$val" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$result" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <!--  ............................................................................................................ -->

  <xsl:template name="divide_en_lineas">
    <!--  template para separar en l&#237;neas  -->
    <xsl:param name="val" />
    <xsl:param name="c1" />

    <xsl:choose>
      <xsl:when test="not(contains($val, $c1))">
        <xsl:value-of select="$val"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="string-length(substring-before($val, $c1))=0">
            <xsl:call-template name="divide_en_lineas">
              <xsl:with-param name="val">
                <xsl:value-of select="substring-after($val, $c1)" />
              </xsl:with-param>
              <xsl:with-param name="c1">
                <xsl:value-of select="$c1" />
              </xsl:with-param>
            </xsl:call-template>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="substring-before($val, $c1)"/>
            <br/>
            <xsl:call-template name="divide_en_lineas">
              <xsl:with-param name="val">
                <xsl:value-of select="substring-after($val, $c1)" />
              </xsl:with-param>
              <xsl:with-param name="c1">
                <xsl:value-of select="$c1" />
              </xsl:with-param>
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--  DTE para titulo del documento  -->

  <xsl:template name="DTEName">
    <xsl:param name="codDTE" />
    <xsl:variable name="codDTEnum" select="number($codDTE)" />
    <xsl:choose>
      <xsl:when test="$codDTEnum = 33">FACTURA ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTEnum = 34">FACTURA NO AFECTA O EXENTA ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTEnum = 43">LIQUIDACI&#211;N DE FACTURA</xsl:when>
      <xsl:when test="$codDTEnum = 46">FACTURA DE COMPRA ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTEnum = 52">GUIA DE DESPACHO ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTEnum = 56">NOTA DE DEBITO ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTEnum = 61">NOTA DE CREDITO ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTEnum = 39">BOLETA ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTEnum = 110">FACTURA DE EXPORTACI&#211;N ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTEnum = 111">NOTA DE DEBITO DE EXPORTACI&#211;N ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTEnum = 112">NOTA DE CREDITO DE EXPORTACI&#211;N ELECTR&#211;NICA</xsl:when>
      <xsl:when test="$codDTE = 'SET'">SET</xsl:when>
      <xsl:otherwise>
        DESCONOCIDO(<xsl:value-of select="concat($codDTE, '-', $codDTEnum)" />)
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--  ............................................................................................................ -->

  <xsl:template name="format-fecha-all">
    <!--  Recibe fecha formato 2005-02-12 devuelve Fecha formateada dd-mm-aaaa u otros formatos segun parametros
	      nombre = nombre
	      nombre = corto
	      nombre = nombremayus
	      nombre = mayuscorto
	      separador = - / de -->
    <xsl:param name="input" select="''"/>
    <xsl:param name="nombre" select="''"/>
    <xsl:param name="separador" select="'-'"/>
    <xsl:variable name="year" select="substring($input, 1, 4)"/>
    <xsl:variable name="mes">
      <xsl:call-template name="dos-digitos">
        <xsl:with-param name="mes-id" select="substring($input, 6, 2)"/>
        <xsl:with-param name="id" select="'mes'"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="day">
      <xsl:call-template name="dos-digitos">
        <xsl:with-param name="mes-id" select="substring($input, 9, 2)"/>
        <xsl:with-param name="id" select="'dia'"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$nombre = 'nombre'">
        <xsl:variable name="nom">
          <xsl:call-template name="nombre-mes">
            <xsl:with-param name="tip" select="$mes"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:value-of select="concat($day, $separador, $nom, $separador, $year)"/>
      </xsl:when>
      <xsl:when test="$nombre = 'corto'">
        <xsl:variable name="nomb">
          <xsl:call-template name="nom-mes">
            <xsl:with-param name="tip" select="$mes"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:value-of select="concat($day, $separador, $nomb, $separador, $year)"/>
      </xsl:when>
      <xsl:when test="$nombre = 'nombremayus'">
        <xsl:variable name="nomM">
          <xsl:call-template name="nombre-mes-mayus">
            <xsl:with-param name="tip" select="$mes"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:value-of select="concat($day, $separador, $nomM, $separador, $year)"/>
      </xsl:when>
      <xsl:when test="$nombre = 'mayuscorto'">
        <xsl:variable name="nomMC">
          <xsl:call-template name="nom-mes-mayus">
            <xsl:with-param name="tip" select="$mes"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:value-of select="concat($day, $separador, $nomMC, $separador, $year)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat($day, $separador, $mes, $separador, $year)"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ............................................................................................................................................................ -->

  <xsl:template name="nombre-mes">
    <!-- Esta funcion recibe un n&#250;mero y devuelve el nombre del mes que
	     	  le corresponde Se utiliza para pcl principalmente.
	     -->
    <xsl:param name="tip" />
    <xsl:variable name="resultado">
      <xsl:choose>
        <xsl:when test="$tip='01'">Enero</xsl:when>
        <xsl:when test="$tip='02'">Febrero</xsl:when>
        <xsl:when test="$tip='03'">Marzo</xsl:when>
        <xsl:when test="$tip='04'">Abril</xsl:when>
        <xsl:when test="$tip='05'">Mayo</xsl:when>
        <xsl:when test="$tip='06'">Junio</xsl:when>
        <xsl:when test="$tip='07'">Julio</xsl:when>
        <xsl:when test="$tip='08'">Agosto</xsl:when>
        <xsl:when test="$tip='09'">Septiembre</xsl:when>
        <xsl:when test="$tip='10'">Octubre</xsl:when>
        <xsl:when test="$tip='11'">Noviembre</xsl:when>
        <xsl:when test="$tip='12'">Diciembre</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$resultado">
        <xsl:value-of select="$resultado" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tip" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ............................................................................................................................................................ -->

  <xsl:template name="nombre-mes-mayus">
    <!-- Esta funcion recibe un n&#250;mero y devuelve el nombre del mes que
	     	  le corresponde Se utiliza para pcl principalmente.     -->
    <xsl:param name="tip" />
    <xsl:variable name="resultado">
      <xsl:choose>
        <xsl:when test="$tip='01'">ENERO</xsl:when>
        <xsl:when test="$tip='02'">FEBRERO</xsl:when>
        <xsl:when test="$tip='03'">MARZO</xsl:when>
        <xsl:when test="$tip='04'">ABRIL</xsl:when>
        <xsl:when test="$tip='05'">MAYO</xsl:when>
        <xsl:when test="$tip='06'">JUNIO</xsl:when>
        <xsl:when test="$tip='07'">JULIO</xsl:when>
        <xsl:when test="$tip='08'">AGOSTO</xsl:when>
        <xsl:when test="$tip='09'">SEPTIEMBRE</xsl:when>
        <xsl:when test="$tip='10'">OCTUBRE</xsl:when>
        <xsl:when test="$tip='11'">NOVIEMBRE</xsl:when>
        <xsl:when test="$tip='12'">DICIEMBRE</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$resultado">
        <xsl:value-of select="$resultado" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tip" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ............................................................................................................................................................ -->

  <xsl:template name="nom-mes">
    <!-- Esta funcion recibe 02 y devuelve Feb -->
    <xsl:param name="tip" />
    <xsl:variable name="resultado">
      <xsl:choose>
        <xsl:when test="$tip='01'">Ene</xsl:when>
        <xsl:when test="$tip='02'">Feb</xsl:when>
        <xsl:when test="$tip='03'">Mar</xsl:when>
        <xsl:when test="$tip='04'">Abr</xsl:when>
        <xsl:when test="$tip='05'">May</xsl:when>
        <xsl:when test="$tip='06'">Jun</xsl:when>
        <xsl:when test="$tip='07'">Jul</xsl:when>
        <xsl:when test="$tip='08'">Ago</xsl:when>
        <xsl:when test="$tip='09'">Sep</xsl:when>
        <xsl:when test="$tip='10'">Oct</xsl:when>
        <xsl:when test="$tip='11'">Nov</xsl:when>
        <xsl:when test="$tip='12'">Dic</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$resultado">
        <xsl:value-of select="$resultado" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tip" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ............................................................................................................................................................ -->

  <xsl:template name="nom-mes-mayus">
    <!-- Esta funcion recibe 02 y devuelve FEB -->
    <xsl:param name="tip" />
    <xsl:variable name="resultado">
      <xsl:choose>
        <xsl:when test="$tip='01'">ENE</xsl:when>
        <xsl:when test="$tip='02'">FEB</xsl:when>
        <xsl:when test="$tip='03'">MAR</xsl:when>
        <xsl:when test="$tip='04'">ABR</xsl:when>
        <xsl:when test="$tip='05'">MAY</xsl:when>
        <xsl:when test="$tip='06'">JUN</xsl:when>
        <xsl:when test="$tip='07'">JUL</xsl:when>
        <xsl:when test="$tip='08'">AGO</xsl:when>
        <xsl:when test="$tip='09'">SEP</xsl:when>
        <xsl:when test="$tip='10'">OCT</xsl:when>
        <xsl:when test="$tip='11'">NOV</xsl:when>
        <xsl:when test="$tip='12'">DIC</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$resultado">
        <xsl:value-of select="$resultado" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tip" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ............................................................................................................................................................ -->
  <xsl:template name="dos-digitos">
    <!-- Recibe digitos y lo devuelve con dos digitos 
	      si por ej: viene 1 devuelve 01  mes-id recibe el valor, id recibe si se formatea mes o dia -->
    <xsl:param name="mes-id" select="0"/>
    <xsl:param name="id" select="'mes'"/>
    <xsl:choose>
      <xsl:when test="$id ='mes'">
        <!-- Para mes -->
        <xsl:choose>
          <xsl:when test="number($mes-id) &gt;= 1 and number($mes-id) &lt;= 12">
            <xsl:value-of select="format-number(number($mes-id), '00')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="ERR"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <!-- Para dia -->
        <xsl:choose>
          <xsl:when test="$id = 'dia'">
            <xsl:value-of select="format-number(number($mes-id), '00')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="ERR"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
