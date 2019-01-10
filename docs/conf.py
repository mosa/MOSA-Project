# -*- coding: utf-8 -*-

import sys, os
extensions = []
templates_path = ['/www/readthedocs.org/readthedocs/templates/sphinx', 'templates', '_templates', '.templates']
source_suffix = '.rst'
master_doc = 'index'
project = u'Technische IT Dokumentation Wechselpilot GmbH'
copyright = u''
version = 'latest'
release = 'latest'
exclude_patterns = ['_build']
pygments_style = 'sphinx'
htmlhelp_basename = 'mosadocs'
file_insertion_enabled = False
latex_documents = [
  ('index', 'techdocs.tex', u'Mosa Documentation',
   u'', 'manual'),
]

latex_elements = {
  'extraclassoptions': ',openany,oneside'
}
