<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:g="http://www.gallio.org/">
  <xsl:param name="resourceRoot" select="''" />
  
  <xsl:output method="text" encoding="utf-8"/>

  <xsl:template match="/">
    <xsl:apply-templates select="//g:report" />
  </xsl:template>

  <xsl:template match="g:report">
		<xsl:apply-templates select="." mode="results"/>
    <xsl:apply-templates select="g:testPackageRun/g:statistics" />
  </xsl:template>
  
	<xsl:template match="g:statistics">
    <xsl:text>* Results: </xsl:text>
    <xsl:call-template name="format-statistics">
      <xsl:with-param name="statistics" select="." />
    </xsl:call-template>
		<xsl:text>&#xD;&#xA;</xsl:text>
	</xsl:template>
  
  <xsl:template match="g:report" mode="results">
    <xsl:variable name="testCases" select="g:testPackageRun/g:testStepRun/descendant-or-self::g:testStepRun[g:testStep/@isTestCase='true']" />

    <xsl:if test="$testCases">
      <xsl:apply-templates select="$testCases" />
    </xsl:if>
    
	</xsl:template>
  
  <xsl:template match="g:testStepRun">
    <xsl:variable name="kind" select="g:testStep/g:metadata/g:entry[@key='TestKind']/g:value" />

    <xsl:text>[</xsl:text>
    <xsl:value-of select="g:result/g:outcome/@status" />
    <xsl:text>] </xsl:text>
    
    <xsl:if test="$kind">
      <xsl:text></xsl:text>
      <xsl:value-of select="$kind" />
      <xsl:text>: </xsl:text>
    </xsl:if>

    <xsl:value-of select="substring-after(g:testStep/@fullName,'&#47;')" />
    <xsl:text>&#xD;&#xA;</xsl:text>
    
    <xsl:apply-templates select="g:children/g:testStepRun" />
  </xsl:template>

  <xsl:template match="*">
  </xsl:template>
  
  <!-- Formats a statistics line like 5 run, 3 passed, 2 failed (1 error), 0 inconclusive, 2 skipped -->
  <xsl:template name="format-statistics">
    <xsl:param name="statistics" />

    <xsl:value-of select="$statistics/@runCount"/>
    <xsl:text> run, </xsl:text>

    <xsl:value-of select="$statistics/@passedCount"/>
    <xsl:text> passed</xsl:text>
    <xsl:call-template name="format-statistics-category-counts">
      <xsl:with-param name="statistics" select="$statistics" />
      <xsl:with-param name="status">passed</xsl:with-param>
    </xsl:call-template>
    <xsl:text>, </xsl:text>

    <xsl:value-of select="$statistics/@failedCount"/>
    <xsl:text> failed</xsl:text>
    <xsl:call-template name="format-statistics-category-counts">
      <xsl:with-param name="statistics" select="$statistics" />
      <xsl:with-param name="status">failed</xsl:with-param>
    </xsl:call-template>
    <xsl:text>, </xsl:text>

    <xsl:value-of select="$statistics/@inconclusiveCount"/>
    <xsl:text> inconclusive</xsl:text>
    <xsl:call-template name="format-statistics-category-counts">
      <xsl:with-param name="statistics" select="$statistics" />
      <xsl:with-param name="status">inconclusive</xsl:with-param>
    </xsl:call-template>
    <xsl:text>, </xsl:text>

    <xsl:value-of select="$statistics/@skippedCount"/>
    <xsl:text> skipped</xsl:text>
    <xsl:call-template name="format-statistics-category-counts">
      <xsl:with-param name="statistics" select="$statistics" />
      <xsl:with-param name="status">skipped</xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="format-statistics-category-counts">
    <xsl:param name="statistics" />
    <xsl:param name="status" />

    <xsl:variable name="outcomeSummaries" select="$statistics/g:outcomeSummaries/g:outcomeSummary[g:outcome/@status=$status and g:outcome/@category]" />

    <xsl:if test="$outcomeSummaries">
      <xsl:text> (</xsl:text>
      <xsl:for-each select="$outcomeSummaries">
        <xsl:sort data-type="text" order="ascending" select="g:outcome/@category"/>

        <xsl:if test="position() != 1">
          <xsl:text>, </xsl:text>
        </xsl:if>
        <xsl:value-of select="@count"/>
        <xsl:text> </xsl:text>
        <xsl:value-of select="g:outcome/@category"/>
      </xsl:for-each>
      <xsl:text>)</xsl:text>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>