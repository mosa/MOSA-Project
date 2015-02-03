All icons are licensed under the MIT or BSD license.

Sources:
http://raphaeljs.com/icons
https://github.com/driftyco/ionicons

To create SVG to ICO, simple use
- convert -background none source.svg output.png
- http://www.icoconverter.com

or directly:
convert -background none source.svg +antialias -define icon:auto-resize=64,48,32,16 output.ico
(has some antialias glitches)