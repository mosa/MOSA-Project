###################
MOSA Runtime Tables
###################

This diagram represents the internal runtime tables within the MOSA virtual machine:

.. graphviz:: mosa-runtime-tables.dot

Internal String Object
----------------------

.. csv-table::
   :header: "Fields"
   :widths: 200

	Object Header
	Pointer to Method Table
	String Length
	Unicode String
