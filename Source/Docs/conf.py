# -*- coding: utf-8 -*-

from __future__ import division, print_function, unicode_literals

import os
import sys

import sphinx_rtd_theme
from recommonmark.parser import CommonMarkParser

sys.path.insert(0, os.path.abspath('..'))
sys.path.append(os.path.dirname(__file__))

sys.path.append(os.path.abspath('_ext'))
extensions = [
    'sphinx.ext.autosectionlabel',
    'sphinx.ext.autodoc',
    'sphinx.ext.intersphinx',
    'sphinxcontrib.httpdomain',
    'sphinx_tabs.tabs',
    'sphinx-prompt',
]

templates_path = ['_templates']

source_suffix = ['.rst', '.md']
source_parsers = {
    '.md': CommonMarkParser,
}

master_doc = 'index'
project = 'Mosa Project'
copyright = '2008-{}, Mosa Project & contributors'.format(
    2021
)

exclude_patterns = ['_build']
index_role = 'obj'
intersphinx_mapping = {
    'mosa': ('http://www.mosa-project.org/', None),
}
htmlhelp_basename = 'MosaDoc'
latex_documents = [
    ('default', 'mosa.tex', 'Mosa Project',
     '', 'manual'),
]
man_pages = [
    ('default', 'mosa', 'Mosa Project',
     [''], 1)
]

exclude_patterns = [
    # 'api' # needed for ``make gettext`` to not die.
]

html_theme = 'sphinx_rtd_theme'
html_static_path = ['_static']
html_theme_path = [sphinx_rtd_theme.get_html_theme_path()]
#html_logo = 'img/logo.svg'
#html_theme_options = {
#    'logo_only': True,
#    'display_version': False,
#}

# Activate autosectionlabel plugin
autosectionlabel_prefix_document = True


def setup(app):
    app.add_stylesheet('css/sphinx_prompt_css.css')
