<?xml version='1.0' encoding='UTF-8'?>

<!--

Copyright (c) 1999 - 2008 Managed Operating Systems Alliance

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

-->

<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0' xmlns:rfc='http://www.mosa.org/schemas/mosa-rfc.xsd' xmlns:xs='http://www.w3.org/2001/XMLSchema'>

  <xsl:output doctype-public='-//W3C//DTD XHTML 1.0 Transitional//EN' doctype-system='http://www.w3.org/TR/xhtml1/DTD/xhtml1-loose.dtd' method='html' indent='yes'/>
  
  <xsl:template match='/'>
    <html>
      <head>
        <title>
          Mosa-RFC-<xsl:value-of select='/rfc:rfc/rfc:header/rfc:number'/>:<xsl:text> </xsl:text><xsl:value-of select='/rfc:rfc/rfc:header/rfc:title' />
        </title>
        <link rel='stylesheet' type='text/css' href='../tools/mosa-rfc.css' />
        
        <!-- BEGIN META TAGS FOR DUBLIN CORE -->
        <meta>
          <xsl:attribute name='name'>
            <xsl:text>DC.Title</xsl:text>
          </xsl:attribute>
          <xsl:attribute name='content'>
            <xsl:value-of select='/rfc:rfc/rfc:header/rfc:title'/>
          </xsl:attribute>
        </meta>
        <xsl:apply-templates select='/rfc:rfc/rfc:header/rfc:author' mode='meta'/>
        <meta>
          <xsl:attribute name='name'>
            <xsl:text>DC.Description</xsl:text>
          </xsl:attribute>
          <xsl:attribute name='content'>
            <xsl:value-of select='/rfc:rfc/rfc:header/rfc:abstract'/>
          </xsl:attribute>
        </meta>
        <meta>
          <xsl:attribute name='name'>
            <xsl:text>DC.Publisher</xsl:text>
          </xsl:attribute>
          <xsl:attribute name='content'>Managed Operating Systems Alliance (MOSA)</xsl:attribute>
        </meta>
        <meta>
          <xsl:attribute name='name'>
            <xsl:text>DC.Date</xsl:text>
          </xsl:attribute>
          <xsl:attribute name='content'>
            <xsl:value-of select='/rfc:rfc/rfc:header/rfc:revision/rfc:date'/>
          </xsl:attribute>
        </meta>
        <meta>
          <xsl:attribute name='name'>
            <xsl:text>DC.Type</xsl:text>
          </xsl:attribute>
          <xsl:attribute name='content'>MOSA Standard</xsl:attribute>
        </meta>
        <meta>
          <xsl:attribute name='name'>
            <xsl:text>DC.Format</xsl:text>
          </xsl:attribute>
          <xsl:attribute name='content'>XHTML</xsl:attribute>
        </meta>
        <meta>
          <xsl:attribute name='name'>
            <xsl:text>DC.Identifier</xsl:text>
          </xsl:attribute>
          <xsl:attribute name='content'>
            Mosa-RFC-<xsl:value-of select='/rfc:rfc/rfc:header/rfc:number'/>
          </xsl:attribute>
        </meta>
        <meta>
          <xsl:attribute name='name'>
            <xsl:text>DC.Language</xsl:text>
          </xsl:attribute>
          <xsl:attribute name='content'>en</xsl:attribute>
        </meta>
        <meta>
          <xsl:attribute name='name'>
            <xsl:text>DC.Rights</xsl:text>
          </xsl:attribute>
          <xsl:attribute name='content'>
            This MOSA standard is copyright (c) 1999 - 2008 by the Managed Operating Systems Alliance (MOSA).
          </xsl:attribute>
        </meta>
        <!-- END META TAGS FOR DUBLIN CORE -->
      </head>
      
      <body>
        <!-- TITLE -->
        <h1>
          Mosa-RFC-<xsl:value-of select='/rfc:rfc/rfc:header/rfc:number' />:<xsl:text> </xsl:text><xsl:value-of select='/rfc:rfc/rfc:header/rfc:title' />
        </h1>
        <!-- ABSTRACT -->
        <p>
          <xsl:value-of select='/rfc:rfc/rfc:header/rfc:abstract'/>
        </p>
        <!-- NOTICE -->
        <hr />
        <xsl:variable name='thestatus' select='/rfc:rfc/rfc:header/rfc:status'/>
        <xsl:variable name='thetype' select='/rfc:rfc/rfc:header/rfc:type'/>
        
        <xsl:if test='$thestatus = "Standard"'>
          <p style='color:green'>This document represents a standard, that is a information about an implementation.</p>
        </xsl:if>
        <xsl:if test='$thestatus = "Deprecated"'>
          <p style='color:orange'>This standard has been deprecated in favour of a new one. It may have implementations, but new software and standards MUST NOT depend on it. Current implementations and dependencies should try to use it superceedor instead.</p>
        </xsl:if>
        <xsl:if test='$thestatus = "Draft"'>
          <p style='color:orange'>This is a draft, that is, it has been accepted by the editor and is currently under review.</p>
        </xsl:if>
        <xsl:if test='$thestatus = "Experimental"'>
          <p style='color:orange'>This is an experimental standard, that is, it is currently being implemented for experimental purposed. This standard may be deprecated or standardised depending on how well it works.</p>
        </xsl:if>
        <xsl:if test='$thestatus = "Proposal"'>
          <p style='color:orange'>This is a proposal for a new standard that is currently under review by the editor. It may or may not become a draft.</p>
        </xsl:if>
        <xsl:if test='$thestatus = "Rejected"'>
          <p style='color:red'>This standard has been rejected by the editor and MUST NOT be used.</p>
        </xsl:if>
        <xsl:if test='$thestatus = "Retracted"'>
          <p style='color:red'>The original author of this standard has retracted it. It MUST NOT be used.</p>
        </xsl:if>
        <xsl:if test='$thestatus = "Obsolete"'>
          <p style='color:red'>This standard has become obsolete. New implementations MUST NOT be created, and it MUST be removed from all implementations as quickly as possible.</p>
        </xsl:if>

        <xsl:if test='$thetype = "Historical"'>
          <p style='color:orange'>This standard is provided as a reference-point for implementations that existed before MOSA was initiated.</p>
        </xsl:if>
        <xsl:if test='$thetype = "Humorous"'>
          <p style='color:red'>This standard is humorous. It is RECOMMENDED that the reader find it funny. Real-world implementations MUST NOT be created.</p>
        </xsl:if>
        <xsl:if test='$thetype = "Informational"'>
          <p style='color:orange'>This standard has no implementation. It provides information on other standards and how to use them appropriately.</p>
        </xsl:if>
        <xsl:if test='$thetype = "Organizational"'>
          <p style='color:orange'>This standard has no implementation. It provides information on how MOSA operates.</p>
        </xsl:if>
        <xsl:if test='$thetype = "Standards Track"'>
          <p style='color:orange'>This standard is part of a standards-track.</p>
        </xsl:if>
        
        <hr />
        
        <!-- RFC INFO -->
        <h2>Document Information</h2>
        <p class='indent'>
          Series: <a href='http://www.mosa.org/standards/'>Mosa-RFC</a><br />
          Number: <xsl:value-of select='/rfc:rfc/rfc:header/rfc:number'/><br />
          Status:
          <a>
            <xsl:attribute name='href'>
              <xsl:text>http://www.mosa.org/standards/rfc-0001.html#states-</xsl:text>
              <xsl:value-of select='/rfc:rfc/rfc:header/rfc:status'/>
            </xsl:attribute>
            <xsl:value-of select='/rfc:rfc/rfc:header/rfc:status'/>
          </a>
          <br />
          Type:
          <a>
            <xsl:attribute name='href'>
              <xsl:text>http://www.mosa.org/standards/rfc-0001.html#types-</xsl:text>
              <xsl:value-of select='/rfc:rfc/rfc:header/rfc:type'/>
            </xsl:attribute>
            <xsl:value-of select='/rfc:rfc/rfc:header/rfc:type'/>
          </a>
          <br />
          Version: <xsl:value-of select='/rfc:rfc/rfc:header/rfc:revision[position()=1]/rfc:version'/><br />
          Last Updated: <xsl:value-of select='/rfc:rfc/rfc:header/rfc:revision[position()=1]/rfc:date'/><br />
          <xsl:variable name='dependencies.count' select='count(/rfc:rfc/rfc:header/rfc:dependencies/rfc:spec)'/>
          <xsl:choose>
            <xsl:when test='$dependencies.count &gt; 0'>
              <xsl:text>Dependencies: </xsl:text>
              <xsl:apply-templates select='/rfc:rfc/rfc:header/rfc:dependencies/rfc:spec' mode='list'>
                <xsl:with-param name='speccount' select='$dependencies.count'/>
              </xsl:apply-templates>
              <br />
            </xsl:when>
            <xsl:otherwise>
              Dependencies: None<br />
            </xsl:otherwise>
          </xsl:choose>
          <xsl:variable name='supersedes.count' select='count(/rfc:rfc/rfc:header/rfc:supersedes/rfc:spec)'/>
          <xsl:choose>
            <xsl:when test='$supersedes.count &gt; 0'>
              <xsl:text>Supersedes: </xsl:text>
              <xsl:apply-templates select='/rfc:rfc/rfc:header/rfc:supersedes/rfc:spec' mode='list'>
                <xsl:with-param name='speccount' select='$supersedes.count'/>
              </xsl:apply-templates>
              <br />
            </xsl:when>
            <xsl:otherwise>
              Supersedes: None<br />
            </xsl:otherwise>
          </xsl:choose>
          <xsl:variable name='supersededby.count' select='count(/rfc:rfc/rfc:header/rfc:supersededby/rfc:spec)'/>
          <xsl:choose>
            <xsl:when test='$supersededby.count &gt; 0'>
              <xsl:text>Superseded By: </xsl:text>
              <xsl:apply-templates select='/rfc:rfc/rfc:header/rfc:supersededby/rfc:spec' mode='list'>
                <xsl:with-param name='speccount' select='$supersededby.count'/>
              </xsl:apply-templates>
              <br />
            </xsl:when>
            <xsl:otherwise>
              Superseded By: None<br />
            </xsl:otherwise>
          </xsl:choose>
        </p>
        
        <hr />
        
        <!-- AUTHOR INFO -->
        <h2>Author Information</h2>
        <div class='indent'>
          <xsl:apply-templates select='/rfc:rfc/rfc:header/rfc:author'/>
        </div>
        <hr />
        <!-- LEGAL NOTICES -->
        <h2>Legal Notices</h2>
        <div class='indent'>
          <h3>Copyright</h3>
          <p>
            This MOSA standard is copyright (c) 1999 - 2008 by the Managed Operating Systems Alliance (MOSA).
          </p>
          <h3>Permissions</h3>
          <p>
            Permission is hereby granted, free of charge, to any person obtaining a copy of this specification (the &quot;Specification&quot;), to make use of the Specification without restriction, including without limitation the rights to implement the Specification in a software program, deploy the Specification in a network service, and copy, modify, merge, publish, translate, distribute, sublicense, or sell copies of the Specification, and to permit persons to whom the Specification is furnished to do so, subject to the condition that the foregoing copyright notice and this permission notice shall be included in all copies or substantial portions of the Specification. Unless separate permission is granted, modified works that are redistributed shall not contain misleading information regarding the authors, title, number, or publisher of the Specification, and shall not claim endorsement of the modified works by the authors, any organization or project to which the authors belong, or the Managed Operating Systems Alliance (MOSA).
          </p>
          <h3>Disclaimer of Warranty</h3>
          <span style='font-weight: bold'>
            ## NOTE WELL: This Specification is provided on an &quot;AS IS&quot; BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, express or implied, including, without limitation, any warranties or conditions of TITLE, NON-INFRINGEMENT, MERCHANTABILITY, or FITNESS FOR A PARTICULAR PURPOSE. In no event shall the Managed Operating Systems Alliance (MOSA) or the authors of this Specification be liable for any claim, damages, or other liability, whether in an action of contract, tort, or otherwise, arising from, out of, or in connection with the Specification or the implementation, deployment, or other use of the Specification. ##
          </span>
          <h3>Limitation of Liability</h3>
          <p>
            In no event and under no legal theory, whether in tort (including negligence), contract, or otherwise, unless required by applicable law (such as deliberate and grossly negligent acts) or agreed to in writing, shall the Managed Operating Systems Alliance (MOSA) or any author of this Specification be liable for damages, including any direct, indirect, special, incidental, or consequential damages of any character arising out of the use or inability to use the Specification (including but not limited to damages for loss of goodwill, work stoppage, computer failure or malfunction, or any and all other commercial damages or losses), even if the Managed Operating Systems Alliance (MOSA) or such author has been advised of the possibility of such damages.
          </p>
        </div>
        <!-- Conformance Terms -->
        <xsl:if test='boolean(/rfc:rfc/rfc:header/rfc:conformanceterms/@used)'>
          <hr/>
          <h2>Conformance Terms</h2>
          <p>
            The following keywords as used in this document are to be interpreted as described in <a href='http://www.ietf.org/rfc/rfc2119.txt'>IETF RFC 2119</a>: "MUST", "SHALL", "REQUIRED"; "MUST NOT", "SHALL NOT"; "SHOULD", "RECOMMENDED"; "SHOULD NOT", "NOT RECOMMENDED"; "MAY", "OPTIONAL".
          </p>
        </xsl:if>
        
        <!-- TABLE OF CONTENTS -->
        <hr />
        <xsl:call-template name='processTOC' />
        <!-- RFC CONTENTS -->
        <hr />
        <xsl:apply-templates select='/rfc:rfc/rfc:section1'/>
        <!-- NOTES -->
        <hr />
        <h2>
          <a name="notes"></a>Notes
        </h2>
        <div class='indent'>
          <xsl:apply-templates select='//rfc:note' mode='endlist'/>
        </div>
        <!-- REVISION HISTORY -->
        <hr />
        <h2>
          <a name="rfc:revs"></a>Revision History
        </h2>
        <div class='indent'>
          <xsl:apply-templates select='/rfc:rfc/rfc:header/rfc:revision'/>
        </div>
        <hr />
        <p>END</p>
      </body>
    </html>
  </xsl:template>

  <!-- From the docbook XSL -->
  <xsl:template name="object.id">
    <xsl:param name="object" select="."/>
    <xsl:choose>
      <xsl:when test="$object/@id">
        <xsl:value-of select="$object/@id"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="generate-id($object)"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name='processTOC'>
    <h2>Table of Contents</h2>
    <div class='indent'>
      <p>
        <xsl:apply-templates select='//rfc:section1' mode='toc'/>
        <br />
        <a href="#notes">Notes</a>
        <br />
        <a href="#revs">Revision History</a>
      </p>
    </div>
  </xsl:template>

  <xsl:template match='rfc:author' mode='meta'>
    <meta>
      <xsl:attribute name='name'>
        <xsl:text>DC.Creator</xsl:text>
      </xsl:attribute>
      <xsl:attribute name='content'>
        <xsl:value-of select='rfc:firstname'/>
        <xsl:text> </xsl:text>
        <xsl:value-of select='rfc:surname'/>
      </xsl:attribute>
    </meta>
  </xsl:template>

  <xsl:template match='rfc:author'>
    <h3>
      <xsl:value-of select='rfc:firstname'/>
      <xsl:text> </xsl:text>
      <xsl:value-of select='rfc:surname'/>
    </h3>
    <p class='indent'>
      <xsl:variable name='org.count' select='count(rfc:org)'/>
      <xsl:variable name='uri.count' select='count(rfc:uri)'/>
      <xsl:variable name='authornote.count' select='count(rfc:authornote)'/>
      <xsl:if test='$authornote.count &gt; 0'>
        See <a href='#authornote'>Author Note</a><br />
      </xsl:if>
      <xsl:if test='$org.count=1'>
        Organization: <xsl:value-of select='rfc:org'/><br />
      </xsl:if>
      <xsl:if test='$uri.count=1'>
        URI:
        <a>
          <xsl:attribute name='href'>
            <xsl:value-of select='uri' />
          </xsl:attribute>
          <xsl:value-of select='uri' />
        </a>
        <br />
      </xsl:if>
    </p>
  </xsl:template>

  <xsl:template match='rfc:spec' mode='list'>
    <xsl:param name='speccount' select='""'/>
    <xsl:variable name='specpos' select='position()'/>
    <xsl:choose>
      <xsl:when test='$specpos &lt; $speccount'>
        <a>
          <xsl:attribute name='href'>
            <xsl:text>http://svn.mosa.ensemble-os.org/rfc/standards/rfc-</xsl:text>
            <xsl:value-of select='.' />
            <xsl:text>.html</xsl:text>
          </xsl:attribute>
          Mosa-RFC-<xsl:value-of select='.'/>
        </a>
        <xsl:text>, </xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <a>
          <xsl:attribute name='href'>
            <xsl:text>http://svn.mosa.ensemble-os.org/rfc/standards/rfc-</xsl:text>
            <xsl:value-of select='.' />
            <xsl:text>.html</xsl:text>
          </xsl:attribute>
          Mosa-RFC-<xsl:value-of select='.'/>
        </a>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match='revision'>
    <h4>
      Version <xsl:value-of select='rfc:version'/><xsl:text> </xsl:text>(<xsl:value-of select='rfc:date'/>)
    </h4>
    <div class='indent'>
      <xsl:apply-templates select='rfc:remark'/>
      <xsl:text> </xsl:text>(<xsl:value-of select='rfc:initials'/>)
    </div>
  </xsl:template>

  <xsl:template match='rfc:section1' mode='toc'>
    <xsl:variable name='oid'>
      <xsl:call-template name='object.id'/>
    </xsl:variable>
    <xsl:variable name='anch'>
      <xsl:value-of select='@anchor'/>
    </xsl:variable>
    <xsl:variable name='num'>
      <xsl:number level='multiple' count='rfc:section1'/>
      <xsl:text>.</xsl:text>
    </xsl:variable>
    <xsl:variable name='sect2.count' select='count(rfc:section2)'/>
    <br />
    <xsl:value-of select='$num'/>
    <xsl:text>  </xsl:text>
    <a>
      <xsl:attribute name='href'>
        <xsl:text>#</xsl:text>
        <xsl:choose>
          <xsl:when test='$anch != ""'>
            <xsl:value-of select='@anchor'/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>sect-</xsl:text>
            <xsl:value-of select='$oid'/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:value-of select='@topic' />
    </a>
    <xsl:if test='$sect2.count &gt; 0'>
      <xsl:apply-templates select='rfc:section2' mode='toc'>
        <xsl:with-param name='prevnum' select='$num'/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>

  <xsl:template match='rfc:section1'>
    <xsl:variable name='oid'>
      <xsl:call-template name='object.id'/>
    </xsl:variable>
    <xsl:variable name='anch'>
      <xsl:value-of select='@anchor'/>
    </xsl:variable>
    <h2>
      <xsl:number level='single' count='rfc:section1'/>.
      <xsl:text> </xsl:text>
      <a>
        <xsl:attribute name='name'>
          <xsl:choose>
            <xsl:when test='$anch != ""'>
              <xsl:value-of select='@anchor'/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>sect-</xsl:text>
              <xsl:value-of select='$oid'/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:value-of select='@topic' />
      </a>
    </h2>
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match='rfc:section2' mode='toc'>
    <xsl:param name='prevnum' select='""'/>
    <xsl:variable name='oid'>
      <xsl:call-template name='object.id'/>
    </xsl:variable>
    <xsl:variable name='anch'>
      <xsl:value-of select='@anchor'/>
    </xsl:variable>
    <xsl:variable name='num'>
      <xsl:value-of select='$prevnum'/>
      <xsl:number level='multiple' count='rfc:section2'/>
      <xsl:text>.</xsl:text>
    </xsl:variable>
    <xsl:variable name='sect3.count' select='count(rfc:section3)'/>
    <br />&#160;&#160;&#160;
    <xsl:value-of select='$num'/> <xsl:text>  </xsl:text>
    <a>
      <xsl:attribute name='href'>
        <xsl:text>#</xsl:text>
        <xsl:choose>
          <xsl:when test='$anch != ""'>
            <xsl:value-of select='@anchor'/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>sect-</xsl:text>
            <xsl:value-of select='$oid'/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:value-of select='@topic' />
    </a>
    <xsl:if test='$sect3.count &gt; 0'>
      <xsl:apply-templates select='rfc:section3' mode='toc'>
        <xsl:with-param name='prevnum' select='$num'/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>

  <xsl:template match='rfc:section2'>
    <xsl:variable name='oid'>
      <xsl:call-template name='object.id'/>
    </xsl:variable>
    <xsl:variable name='anch'>
      <xsl:value-of select='@anchor'/>
    </xsl:variable>
    <div class='indent'>
      <h3>
        <xsl:number level='single' count='rfc:section1'/>.<xsl:number level='single' count='rfc:section2'/>
        <xsl:text> </xsl:text>
        <a>
          <xsl:attribute name='name'>
            <xsl:choose>
              <xsl:when test='$anch != ""'>
                <xsl:value-of select='@anchor'/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>sect-</xsl:text>
                <xsl:value-of select='$oid'/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:value-of select='@topic' />
        </a>
      </h3>
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match='rfc:section3' mode='toc'>
    <xsl:param name='prevnum' select='""'/>
    <xsl:variable name='oid'>
      <xsl:call-template name='object.id'/>
    </xsl:variable>
    <xsl:variable name='anch'>
      <xsl:value-of select='@anchor'/>
    </xsl:variable>
    <xsl:variable name='num'>
      <xsl:value-of select='$prevnum'/>
      <xsl:number level='multiple' count='rfc:section3'/>
      <xsl:text>.</xsl:text>
    </xsl:variable>
    <xsl:variable name='sect4.count' select='count(rfc:section4)'/>
    <br />&#160;&#160;&#160;&#160;&#160;&#160;
    <xsl:value-of select='$num'/> <xsl:text>  </xsl:text>
    <a>
      <xsl:attribute name='rfc:href'>
        <xsl:text>#</xsl:text>
        <xsl:choose>
          <xsl:when test='$anch != ""'>
            <xsl:value-of select='@anchor'/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>sect-</xsl:text>
            <xsl:value-of select='$oid'/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:value-of select='@topic' />
    </a>
    <xsl:if test='$sect4.count &gt; 0'>
      <xsl:apply-templates select='rfc:section4' mode='toc'>
        <xsl:with-param name='prevnum' select='$num'/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>

  <xsl:template match='rfc:section3'>
    <xsl:variable name='oid'>
      <xsl:call-template name='object.id'/>
    </xsl:variable>
    <xsl:variable name='anch'>
      <xsl:value-of select='@anchor'/>
    </xsl:variable>
    <div class='indent'>
      <h3>
        <xsl:number level='single' count='rfc:section1'/>.<xsl:number level='single' count='rfc:section2'/>.<xsl:number level='single' count='rfc:section3'/>
        <xsl:text> </xsl:text>
        <a>
          <xsl:attribute name='name'>
            <xsl:choose>
              <xsl:when test='$anch != ""'>
                <xsl:value-of select='@anchor'/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>sect-</xsl:text>
                <xsl:value-of select='$oid'/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:value-of select='@topic' />
        </a>
      </h3>
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match='rfc:section4' mode='toc'>
    <xsl:param name='prevnum' select='""'/>
    <xsl:variable name='oid'>
      <xsl:call-template name='object.id'/>
    </xsl:variable>
    <xsl:variable name='anch'>
      <xsl:value-of select='@anchor'/>
    </xsl:variable>
    <xsl:variable name='num'>
      <xsl:value-of select='$prevnum'/>
      <xsl:number level='multiple' count='rfc:section4'/>
      <xsl:text>.</xsl:text>
    </xsl:variable>
    <br />&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;
    <xsl:value-of select='$num'/> <xsl:text>  </xsl:text>
    <a>
      <xsl:attribute name='href'>
        <xsl:text>#</xsl:text>
        <xsl:choose>
          <xsl:when test='$anch != ""'>
            <xsl:value-of select='@anchor'/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>sect-</xsl:text>
            <xsl:value-of select='$oid'/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:value-of select='@topic' />
    </a>
  </xsl:template>

  <xsl:template match='rfc:section4'>
    <xsl:variable name='oid'>
      <xsl:call-template name='object.id'/>
    </xsl:variable>
    <xsl:variable name='anch'>
      <xsl:value-of select='@anchor'/>
    </xsl:variable>
    <div class='indent'>
      <h3>
        <xsl:number level='single' count='rfc:section1'/>.<xsl:number level='single' count='rfc:section2'/>.<xsl:number level='single' count='rfc:section3'/>.<xsl:number level='single' count='rfc:section4'/>
        <xsl:text> </xsl:text>
        <a>
          <xsl:attribute name='name'>
            <xsl:choose>
              <xsl:when test='$anch != ""'>
                <xsl:value-of select='@anchor'/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>sect-</xsl:text>
                <xsl:value-of select='$oid'/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:value-of select='@topic' />
        </a>
      </h3>
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match='rfc:remark'>
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match='rfc:p'>
    <p>
      <xsl:variable name='class.count' select='count(@class)'/>
      <xsl:if test='$class.count=1'>
        <xsl:attribute name='class'>
          <xsl:value-of select='@class'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:variable name='style.count' select='count(@style)'/>
      <xsl:if test='$style.count=1'>
        <xsl:attribute name='style'>
          <xsl:value-of select='@style'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates/>
    </p>
  </xsl:template>

  <xsl:template match='rfc:ul'>
    <ul>
      <xsl:variable name='class.count' select='count(@class)'/>
      <xsl:if test='$class.count=1'>
        <xsl:attribute name='class'>
          <xsl:value-of select='@class'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:variable name='style.count' select='count(@style)'/>
      <xsl:if test='$style.count=1'>
        <xsl:attribute name='style'>
          <xsl:value-of select='@style'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates/>
    </ul>
  </xsl:template>

  <xsl:template match='rfc:ol'>
    <ol>
      <xsl:variable name='start.count' select='count(@start)'/>
      <xsl:if test='$start.count=1'>
        <xsl:attribute name='start'>
          <xsl:value-of select='@start'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:variable name='class.count' select='count(@class)'/>
      <xsl:if test='$class.count=1'>
        <xsl:attribute name='class'>
          <xsl:value-of select='@class'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:variable name='style.count' select='count(@style)'/>
      <xsl:if test='$style.count=1'>
        <xsl:attribute name='style'>
          <xsl:value-of select='@style'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates/>
    </ol>
  </xsl:template>

  <xsl:template match='rfc:li'>
    <li>
      <xsl:variable name='class.count' select='count(@class)'/>
      <xsl:if test='$class.count=1'>
        <xsl:attribute name='class'>
          <xsl:value-of select='@class'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:variable name='style.count' select='count(@style)'/>
      <xsl:if test='$style.count=1'>
        <xsl:attribute name='style'>
          <xsl:value-of select='@style'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates/>
    </li>
  </xsl:template>

  <xsl:template match='rfc:link'>
    <a>
      <xsl:attribute name='href'>
        <xsl:value-of select='@url'/>
      </xsl:attribute>
      <xsl:apply-templates/>
    </a>
  </xsl:template>

  <xsl:template match='rfc:example'>
    <p class='caption'>
      <a>
        <xsl:attribute name='name'>
          <xsl:text>example-</xsl:text>
          <xsl:number level='any' count='example'/>
        </xsl:attribute>
      </a>Example <xsl:number level='any' count='example'/>.<xsl:text> </xsl:text><xsl:value-of select='@caption'/>
    </p>
    <div class='indent'>
      <pre>
        <xsl:apply-templates/>
      </pre>
    </div>
  </xsl:template>

  <xsl:template match='rfc:code'>
    <p class='caption'>
      Code Example: <xsl:value-of select='@caption'/>
    </p>
    <div class='code'>
      <pre>
        <xsl:apply-templates/>
      </pre>
    </div>
  </xsl:template>

  <xsl:template match='rfc:diagram'>
    <p class='caption'>
      Diagram: <xsl:value-of select='@caption'/>
    </p>
    <div class='diagram'>
      <pre>
        <xsl:apply-templates/>
      </pre>
    </div>
  </xsl:template>

  <xsl:template match='rfc:img'>
    <img>
      <xsl:attribute name='alt'>
        <xsl:value-of select='@alt'/>
      </xsl:attribute>
      <xsl:attribute name='height'>
        <xsl:value-of select='@height'/>
      </xsl:attribute>
      <xsl:attribute name='src'>
        <xsl:value-of select='@src'/>
      </xsl:attribute>
      <xsl:attribute name='width'>
        <xsl:value-of select='@width'/>
      </xsl:attribute>
    </img>
  </xsl:template>

  <xsl:template match='rfc:table'>
    <p class='caption'>
      <a>
        <xsl:attribute name='name'>
          <xsl:text>table-</xsl:text>
          <xsl:number level='any' count='table'/>
        </xsl:attribute>
      </a>Table <xsl:number level='any' count='table'/>:<xsl:text> </xsl:text><xsl:value-of select='@caption'/>
    </p>
    <table border='1' cellpadding='3' cellspacing='0'>
      <xsl:apply-templates/>
    </table>
  </xsl:template>

  <xsl:template match='rfc:tr'>
    <tr class='body'>
      <xsl:apply-templates/>
    </tr>
  </xsl:template>

  <xsl:template match='rfc:th'>
    <th>
      <xsl:variable name='colspan.count' select='count(@colspan)'/>
      <xsl:variable name='rowspan.count' select='count(@rowspan)'/>
      <xsl:if test='$colspan.count=1'>
        <xsl:attribute name='colspan'>
          <xsl:value-of select='@colspan'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test='$rowspan.count=1'>
        <xsl:attribute name='rowspan'>
          <xsl:value-of select='@rowspan'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates/>
    </th>
  </xsl:template>

  <xsl:template match='rfc:td'>
    <td>
      <xsl:variable name='align.count' select='count(@align)'/>
      <xsl:variable name='colspan.count' select='count(@colspan)'/>
      <xsl:variable name='rowspan.count' select='count(@rowspan)'/>
      <xsl:if test='$align.count=1'>
        <xsl:attribute name='align'>
          <xsl:value-of select='@align'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test='$colspan.count=1'>
        <xsl:attribute name='colspan'>
          <xsl:value-of select='@colspan'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test='$rowspan.count=1'>
        <xsl:attribute name='rowspan'>
          <xsl:value-of select='@rowspan'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates/>
    </td>
  </xsl:template>

  <xsl:template match='rfc:note'>
    <xsl:variable name='notenum'>
      <xsl:number level='any' count='note'/>
    </xsl:variable>
    <xsl:variable name='oid'>
      <xsl:call-template name='object.id'/>
    </xsl:variable>
    <xsl:text> [</xsl:text>
    <a href='#nt-{$oid}'>
      <xsl:value-of select='$notenum'/>
    </a>
    <xsl:text>]</xsl:text>
  </xsl:template>

  <xsl:template match='rfc:note' mode='endlist'>
    <p>
      <xsl:variable name='oid'>
        <xsl:call-template name='object.id'/>
      </xsl:variable>
      <a name='nt-{$oid}'>
        <xsl:value-of select='position()'/>
      </a>
      <xsl:text>. </xsl:text>
      <xsl:apply-templates/>
    </p>
  </xsl:template>

  <!-- PRESENTATIONAL ELEMENTS -->

  <xsl:template match='rfc:cite'>
    <span class='ref'>
      <xsl:apply-templates/>
    </span>
  </xsl:template>

  <xsl:template match='rfc:dfn'>
    <span class='dfn'>
      <xsl:apply-templates/>
    </span>
  </xsl:template>

  <xsl:template match='rfc:div'>
    <div>
      <xsl:variable name='class.count' select='count(@class)'/>
      <xsl:if test='$class.count=1'>
        <xsl:attribute name='class'>
          <xsl:value-of select='@class'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:variable name='style.count' select='count(@style)'/>
      <xsl:if test='$style.count=1'>
        <xsl:attribute name='style'>
          <xsl:value-of select='@style'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match='rfc:em'>
    <span class='em'>
      <xsl:apply-templates/>
    </span>
  </xsl:template>

  <xsl:template match='rfc:pre'>
    <pre>
      <xsl:apply-templates/>
    </pre>
  </xsl:template>

  <xsl:template match='rfc:span'>
    <span>
      <xsl:variable name='class.count' select='count(@class)'/>
      <xsl:if test='$class.count=1'>
        <xsl:attribute name='class'>
          <xsl:value-of select='@class'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:variable name='style.count' select='count(@style)'/>
      <xsl:if test='$style.count=1'>
        <xsl:attribute name='style'>
          <xsl:value-of select='@style'/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates/>
    </span>
  </xsl:template>

  <xsl:template match='rfc:strong'>
    <span class='strong'>
      <xsl:apply-templates/>
    </span>
  </xsl:template>

  <xsl:template match='rfc:tt'>
    <tt>
      <xsl:apply-templates/>
    </tt>
  </xsl:template>

  <xsl:template match='rfc:spec'>
    <a>
      <xsl:attribute name='href'>
        <xsl:text>http://svn.mosa.ensemble-os.org/rfc/standards/rfc-</xsl:text>
        <xsl:value-of select='.' />
        <xsl:text>.html</xsl:text>
      </xsl:attribute>
      Mosa-RFC-<xsl:value-of select='.'/>
    </a>
  </xsl:template>

  <!-- END OF PRESENTATIONAL ELEMENTS -->

</xsl:stylesheet>
