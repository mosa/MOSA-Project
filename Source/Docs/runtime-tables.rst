###################
MOSA runtime tables
###################

This diagram represents the internal runtime tables within the MOSA virtual machine:

.. graphviz:: mosa-runtime-tables.dot

Table Definitions
-----------------

.. csv-table::
   :header: "Internal String Object"
   :widths: 200

	Pointer to Method Table
	String Length
	Unicode String

.. csv-table::
   :header: "Assembly Table"
   :widths: 200

	Number of Assemblies
	Pointer to Assembly 1..N

.. csv-table::
   :header: "Assembly"
   :widths: 200

	Pointer to Assembly Name
	Pointer to Custom Attributes
	Flags: IsReflectionOnly
	Number of Types
	Pointer to Type Definition 1..N

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

.. csv-table::
   :header: "Property Definition"
   :widths: 200

	Pointer to Propertyame
	Pointer to Custom Attributes
	Property Attributes
	Pointer to Property Type
	Pointer to Getter Method Definition
	Pointer to Setter Method Defiinition

.. csv-table::
   :header: "Method Lookup Table"
   :widths: 200

	Number of Methods
	Pointer to Method (Starting Address)
	Method Size
	Pointer to Method Definition

.. csv-table::
   :header: "Protected Region Table"
   :widths: 200

	Number of Regions
	Pointer to Protected Region Definition 1..N

.. csv-table::
   :header: "Protected Region Definition"
   :widths: 200

	Region Start
	Region End
	Region Handler
	Exception Handler Type
	Pointer to Exception Type

.. csv-table::
   :header: "Interface Slot Table"
   :widths: 200

	Number of Interface Method Tables
	Pointer to Interface Method Table 1..N

.. csv-table::
   :header: "Interface Method Table"
   :widths: 200

	Pointer to Interface Type
	Number of Methods
	Pointer to Method Definition 1..N

.. csv-table::
   :header: "Method Definition"
   :widths: 200

	Pointer to Methodame
	Pointer to Custom Attributes
	Method Attributes
	Local & Parameter Stack Size
	Pointer to Method
	Pointer to Return Type Definition
	Pointer to Protected Region Table
	Pointer to Method GC Data
	Number of Parameters
	Pointer to Parameter Definition 1..N

.. csv-table::
   :header: "Property Definition"
   :widths: 200

	Pointer to Parameterame
	Pointer to Custom Attributes
	Parameter Attributes
	Pointer to Parameter Type

.. csv-table::
   :header: "Custom Attributes Table"
   :widths: 200

	Number of Attributes
	Pointer to Custom Attribute 1..N

.. csv-table::
   :header: "Custom Attribute"
   :widths: 200

	Pointer to Attribute Type
	Pointer to Constructor Method
	Number of Arguments
	Pointer to Argument 1..N

.. csv-table::
   :header: "Custom Attribute Argument"
   :widths: 200

	Pointer to Argumentame
	Is Argument a Field
	Pointer to Argument Type
	Argument Size
	Argument

.. csv-table::
   :header: "Method GC Data"
   :widths: 200

	Pointer to SafePoint Table
	Pointer to Method GC Stack Data

.. csv-table::
   :header: "Method GC Stack Data"
   :widths: 200

	Number of Method GC Stack Entries
	Pointer to Method GC Stack Entry 1..N

.. csv-table::
   :header: "Method GC Stack Entry"
   :widths: 200

	Stack Offset
	Type (0=Object/1=Managed Pointer)
	Live Ranges 1..N
	. Address Offset
	. Address Range

.. csv-table::
   :header: "Method SafePoint Table"
   :widths: 200

	Number of SafePoints
	SafePoint 1..N
	. Address Offset
	. Address Range
	. CPU Registers Bitmap (32 bit)
	. Type Bitmap (32 bit)
	. Breakpoint Indicator
