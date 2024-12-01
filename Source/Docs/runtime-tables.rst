###################
MOSA Runtime Tables
###################

This diagram represents the internal runtime tables within the MOSA virtual machine:

.. graphviz:: mosa-runtime-tables.dot

Internal String Object
----------------------

.. csv-table::
   :header: "Object Header"
   :widths: 200
	
	Pointer to Method Table
	String Length
	Unicode String

Internal String Object
----------------------

.. csv-table::
   :header: "Assembly Table"
   :widths: 200
	
	Number of Assemblies
	Pointer to Assembly 1..N

Assembly
--------

.. csv-table::
   :header: "Assembly"
   :widths: 200

	Pointer to Assembly Name
	Pointer to Custom Attributes
	Flags: IsReflectionOnly
	Number of Types
	Pointer to Type Definition 1..N

Type Definition
---------------

.. csv-table::
   :header: "Type Definition"
   :widths: 200

	Pointer to Typeame
	Pointer to Custom Attributes
	Type Code & Attributes
	Type Size
	Pointer to Assembly
	Pointer to Parent Type
	Pointer to Declaring Type
	Pointer to Element Type
	Pointer to Default Constructor Method
	Pointer to Properties Table
	Pointer to Fields Table
	Pointer to Interface Slot Table
	Pointer to Interface Bitmap
	Number of Methods
	Pointer to Method 1..N
	Pointer to Method Definition 1..N

Fields Table
---------------

.. csv-table::
   :header: "Fields Table"
   :widths: 200

	Number of Fields
	Pointer to Field Definition 1..N
	
.. csv-table::
   :header: "Field Definition"
   :widths: 200

	Pointer to Fieldame
	Pointer to Custom Attributes
	Field Attributes
	Pointer to Field Type
	Address
	Offset / Size
	
.. csv-table::
   :header: "Properties Table"
   :widths: 200

	Number of Properties
	Pointer to Property Definition 1..N
