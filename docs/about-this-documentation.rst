########################
About this documentation
########################

This document focuses on style-guide and a short reference.
It is a kind of coding standards applied to documentation files.
It is not about documentation content.

.. list-table:: Links to Documentation

  * - Html, Online
    - http://docs.mosa-project.org/en/latest
  * - PDF
    - http://readthedocs.org/projects/mosa/downloads/pdf/latest
  * - Html, as Zip
    - http://readthedocs.org/projects/mosa/downloads/htmlzip/latest

***************************************
RestructuredText with Sphinx directives
***************************************

This documentation uses `Python-sphinx`_, which itself uses `reStructuredText`_
syntax.


*********
Filenames
*********

Use only lowercase alphanumeric characters and ``-`` (minus) symbol.

Suffix filenames with the ``.rst`` extension, so GitHub can render them.


***********
Whitespaces
***********

Indentation
===========

Indent with 2 spaces.

Except:

* ``toctree`` directive requires a 3 spaces indentation.

Blank lines
===========

Two blank lines before overlined sections, i.e. before H1 and H2.
One blank line before other sections.
See `Headings`_ for an example.

One blank line to separate directives.

.. code-block:: rst

  Some text before.

  .. note::

    Some note.

Exception: directives can be written without blank lines if they are only one
line long.

.. code-block:: rst

  .. note:: A short note.


***********
Line length
***********

Technically, there's no limitation. But if possible, limit all lines to a maximum of 120 characters.


********
Headings
********

Use the following symbols to create headings:

#. ``#`` with overline
#. ``*`` with overline
#. ``=``
#. ``-``
#. ``^``
#. ``"``

As an example:

.. code-block:: rst

  ##################
  H1: document title
  ##################

  Introduction text.


  *********
  Sample H2
  *********

  Sample content.


  **********
  Another H2
  **********

  Sample H3
  =========

  Sample H4
  ---------

  Sample H5
  ^^^^^^^^^

  Sample H6
  """""""""

  And some text.

If you need more than heading level 4 (i.e. H5 or H6), then you should consider
creating a new document.

There should be only one H1 in a document.

.. note::

  See also `Sphinx's documentation about sections`_.


**************************
Code blocks and text boxes
**************************

Use the ``code-block`` directive **and** specify the programming language. As
an example:

.. code-block:: rst

  .. code-block:: python

    import this

Text boxes:

.. code-block:: rst

  .. note::

     Note (blue box). possible values: attention, caution, danger, error, hint, important, note, tip, warning, admonition.
     Every type has its own color.

will loook like:

.. note::

   Note (blue box). possible values: attention, caution, danger, error, hint, important, note, tip, warning, admonition.
   Every type has its own color.

********************
Links and references
********************

Use links and references footnotes with the ``target-notes`` directive.
As an example:

.. code-block:: rst

  #############
  Some document
  #############

  Link without Reference: `Example <http://www.example.com>`__

  Some text which includes links to `Example website`_ and many other links.

  `Example website`_ can be referenced multiple times.

  (... document content...)

  And at the end of the document...

  **********
  References
  **********

  .. target-notes::

  .. _`Example website`: http://www.example.com/


******
Tables
******

Table as CSV

.. code-block:: rst

  .. csv-table:: Title of CSV table
    :header: "Column 1", "Column 2", "Column 3"

    "Sampel Row 1", Cell, Cell
    "Sampel Row 2", Cell, "Cell with multiple Words"

You can skip quotes, of cell content conains only a single word

Table as flat list

.. code-block:: rst

  .. list-table:: Title of table as flat list
    :header-rows: 1

    * - Column 1
      - Column 2
      - Column 3
    * - Row 1
      - Cell
      - Cell
    * - Row 2
      - Cell
      - Cell

``:header-rows:`` defines the number of header rows. Skip this line, if you do not need a header.

**********
References
**********

- https://sphinx-rtd-theme.readthedocs.io/en/latest/demo/demo.html
- http://www.ericholscher.com/blog/2016/jul/1/sphinx-and-rtd-for-writers/

.. target-notes::

.. _`Python-sphinx`: http://sphinx.pocoo.org/
.. _`reStructuredText`: http://docutils.sourceforge.net/rst.html
.. _`rst2html`:
   http://docutils.sourceforge.net/docs/user/tools.html#rst2html-py
.. _`Github`: https://github.com
.. _`Read the docs`: http://readthedocs.org
.. _`Sphinx's documentation about sections`:
   http://sphinx.pocoo.org/rest.html#sections
