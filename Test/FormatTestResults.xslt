<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="/report/testPackageRun/testStepRun/result">
    <root>
      <xsl:apply-templates select="outcome"/>
    </root>
  </xsl:template>

  <xsl:template match="outcome">
    <xsl:value-of select="@status" />
  </xsl:template>

</xsl:stylesheet>