Prebuild Instructions

Prebuild is an XML-driven pre-build tool allowing developers to easily generate project or make files for major IDE's and .NET development tools including: Visual Studio 2005, Visual Studio 2003, Visual Studio 2002, SharpDevelop, SharpDevelop2, MonoDevelop, and NAnt.

_______________________________________________________________________________
Overview

Prebuild can be either be run from the command line to generate the
project and make files or you can execute the included batch (*.bat)
and Unix Shell script (*.sh) files.

_______________________________________________________________________________
The currently supported developement tools and their associated batch
and shell script files.

Visual Studio .NET 2005 (VS2005.bat)
Visual Studio .NET 2003 (VS2003.bat)
Visual Studio .NET 2002 (VS2002.bat)
SharpDevelop (SharpDevelop.bat) - http://www.icsharpcode.net/OpenSource/SD/
SharpDevelop2 (SharpDevelop.bat) - http://www.icsharpcode.net/OpenSource/SD/
MonoDevelop (MonoDevelop.sh) - http://www.monodevelop.com/
NAnt (nant.sh and nant.bat) - http://nant.sourceforge.net/
Autotools (autotools.bat and autotools.sh) - http://en.wikipedia.org/wiki/GNU_build_system

Notes:

A Unix Shell script is provided for MonoDevelop, as it does not run on
Windows at this time.

Visual Studio .NET 2005 and the Visual Express IDE's can import
solutions from older versions of Visual Studio .NET.

Makefiles are not currently supported.

_______________________________________________________________________________
Command Line Syntax:
 
Example:
> Prebuild /target vs2003

This will generate the project files for Visual Studio.NET 2003 and
place the redirect the log to a file named PrebuildLog.txt in the
parent directory


The syntax structure is as below, where commandParameter is optional
depending on the command and you can provide several option-value
pairs.

Note: The '> ' signifies the command prompt, do not enter this literally

> Prebuild /<option> <commandParameter>

> Prebuild /target vs2003 /pause

> Prebuild /target vs2003 /log ../Log.txt /pause /ppo /file ProjectConfig.xml

> Prebuild /target sharpdev /log

> Prebuild /removedir obj|bin

> Prebuild /target vs2003 /allowedgroups Group1|Group2

> Prebuild /clean

> Prebuild /clean /yes

> Prebuild /clean vs2003

_______________________________________________________________________________
Command Line Options:

/usage - Shows the help information on how to use Prebuild and what
the different options are and what they do

/clean - The project files generated for the target type specified as
a parameter for this option will be deleted.  If no value is specified
or if 'all' is specified, then project files for all the target types
will be deleted.

/target - Specified the name of the development tool for which project
or make files will be generated.  Possible parameter values include:
vs2003, vs2002, sharpdev

/file - Specifies the name of the XML which defines what files are to
be referenced by the generated project files as well as configures the
options for them.  If not specified, prebuild.xml in the current
directory will be used as the default.

/log - Specified the log file that should be written to for build
errors.  If this option is not specified, no log file is generated,
but if just no value is specified, then the defaul filename will be
used for the log (Prebuild.log).

/ppo - Preprocesses the xml file to test for syntax errors or problems
but doesn't generate the files

/pause - Shows the console until you press a key so that you can view
the messages written while performing the specified actions.

This allows you to check if an errors occurred and - if so - what it
was.

/showtargets - Shows a list of all the targets that can be specified
as values for the /clean and /target commands.

/allowedgroups - This is followed by a pipe-delimited list of project
group filter flags (eg. Group1|Group2) allow optional filtering of all
projects that dont have at least one of these flags

/removedir - This is followed by a pipe-delimited list of directory
names that will be deleted while recursivly searching the directory of
the prebuild application and its child directories (eg. use obj|bin to
delete all output and temporary directories before file releases)

/yes - Answer yes to any warnings (e.g. when cleaning all projects).

_______________________________________________________________________________
Example Batch Files and Shell Scripts

NOTE: Common batch and shell script files are included with Prebuild source and file releases.
______________________________
MonoDevelop

#!/bin/sh
# Generates a solution (.mds) and a set of project files (.mdp) 

# for MonoDevelop, a Mono port of SharpDevelop
#  (http://icsharpcode.net/OpenSource/SD/Default.aspx)

./Prebuild /target monodev /pause

______________________________
Visual Studio .NET 2003

@rem Generates a solution (.sln) and a set of project files (.csproj) 
@rem for Microsoft Visual Studio .NET 2002
Prebuild /target vs2003 /pause

Notes:
Text after lines that start with @rem are comments and are not evaluated
You can also place pause on the last line instead of specifing the /pause command.

_______________________________________________________________________________
Example XML Configuration File

Note:

XML Comments (<!-- Comment -->) are used to markup the prebuild.xml
file with notes

The below file may be out-of-date, however the RealmForge Prebuild
file serves as an up-to-date and extensive example.

It can be viewed using Tigris.org's WebSVN
(http://realmforge.tigris.org/source/browse/realmforge/trunk/src/prebuild.xml)
by just clicking on the "view file" link for the latest revision.

_________________________________

<?xml version="1.0" encoding="utf-8"?>
    <!--The version of the XML schema specified in the version and xmlns attributes should match the one for which the version of Prebuild.exe used was compiled for.  In this example it is the version 1.3 schema, you can find the XSD schema file at the url specified in the xmlns attribute. -->
<Prebuild version="1.6" xmlns="http://dnpb.sourceforge.net/schemas/prebuild-1.6.xsd">
	<Solution name="RealmForge"> <!--The title and file name for the solution, combine, workspace, or project group (depending on what development tool you are using)-->
                       <!--Configurations found as children of Solution are used as templates for the configurations found in the project, this allows you to avoid writing the same options in each project (and maintaining each of these).  You can provide defaults and then override them in the configurations defined for each project. All options are optional.-->
		<Configuration name="Debug">
			<Options>
				<!-- simple logically expressions can be evaluated, if, else, elseif, and endif are valid statements.  Note that it is not neccisary to define POSIX or WIN32 -->
				<?if OS = "Win32" ?>
					<CompilerDefines>DEBUG;TRACE;WIN32</CompilerDefines>
				<?else ?>
					<CompilerDefines>DEBUG;TRACE;POSIX</CompilerDefines>
				<?endif ?>
				<OptimizeCode>false</OptimizeCode>
				<CheckUnderflowOverflow>false</CheckUnderflowOverflow>
				<AllowUnsafe>false</AllowUnsafe>
				<WarningLevel>4</WarningLevel>   
				<!-The filter for the number of warnings or errors shown and the tolerance level as to what is an error. This is value from 0 to 4 where 4 is the most strict (least tolerent).-->

				<WarningsAsErrors>false</WarningsAsErrors>
				<SuppressWarnings>1591;219;1573;1572;168</SuppressWarnings> 
 				<!-- A semicolon ';'  delimited list of the warnings that are filtered and not shown in the output window during compiling a project.  Only include the number portion of the warning codes that are shown in output during compilation (eg CS1591, should be entered as 1591)-->

				<OutputPath>..\bin</OutputPath>
				<DebugInformation>true</DebugInformation>
				<RegisterComInterop>false</RegisterComInterop>
				<IncrementalBuild>true</IncrementalBuild>
				<BaseAddress>285212672</BaseAddress>
				<FileAlignment>4096</FileAlignment>
				<NoStdLib>false</NoStdLib>
				<XmlDocFile>Docs.xml</XmlDocFile>
			</Options>
		</Configuration>
		<Configuration name="Release"> <!-- You can define multple configurations that projects can have, but there is no way to define which one is selected by default as this is a part of the user preferences for a project, not the solution or project files -->
			<Options>
				<CompilerDefines>TRACE</CompilerDefines>
				<OptimizeCode>true</OptimizeCode>
				<CheckUnderflowOverflow>false</CheckUnderflowOverflow>
				<AllowUnsafe>false</AllowUnsafe>
				<WarningLevel>4</WarningLevel>
				<WarningsAsErrors>false</WarningsAsErrors>
				<SuppressWarnings>1591;219;1573;1572;168</SuppressWarnings>
				<OutputPath>..\bin</OutputPath>
				<DebugInformation>false</DebugInformation>
				<RegisterComInterop>false</RegisterComInterop>
				<IncrementalBuild>true</IncrementalBuild>
				<BaseAddress>285212672</BaseAddress>
				<FileAlignment>4096</FileAlignment>
				<NoStdLib>false</NoStdLib>
				<GenerateXmlDocFile>true</GenerateXmlDocFile>
				<XmlDocFile>Docs.xml</XmlDocFile>				
			</Options>
		</Configuration>

		<!-- One of the projects that is included in the Solution -->
		<Project name="RealmForge.Utility" Language="VisualBasic" path="Utility" type="Library" assemblyName="RealmForge.Utility" rootNamespace="RealmForge">
			<Configuration name="Debug">
				<Options>
					<OutputPath>..\bin\lib\Utility</OutputPath>
					<XmlDocFile>RealmForge.Utility.xml</XmlDocFile>
				</Options>
			</Configuration>
			<Configuration name="Release">
				<Options>
					<OutputPath>..\bin\lib\Utility</OutputPath>
					<XmlDocFile>RealmForge.Utility.xml</XmlDocFile>
				</Options>
			</Configuration>
			<ReferencePath>../bin</ReferencePath>
			<Reference name="System"/>
			<Reference name="System.Data"/> 
			<Reference name="System.Drawing"/>
			<Reference name="System.Xml"/>
			<Reference name="System.Runtime.Serialization.Formatters.Soap"/>
			<Reference name="ICSharpCode.SharpZipLib"/>
			<Files>
				<Match path="." pattern="*.vb" recurse="true"/>
			</Files>
		</Project>

		<!-- Another projects that is included in the Solution -->
		<Project name="DemoGame" Language="C#" path="DemoGame" type="WinExe" icon="..\bin\RealmForge.ico" assemblyName="DemoGame" rootNamespace="RealmForge">
				<!-- icon is used to define the location of the .ico file that is embeeded in the assembly when the project is compiled.  This is relative to the project path -->
				<!--type defines the type of project, valid types are Library (.dll), WinExe (.exe), and Exe (.exe).  WinExe is not windows specific, it just defines that it is a GUI application and that no Console or Command window will show when it is started-->

			<Configuration name="Debug">
				<Options>
					<OutputPath>..\bin</OutputPath>
					<XmlDocFile>DemoGame.xml</XmlDocFile>
				</Options>
			</Configuration>
			<Configuration name="Release">
				<Options>
					<OutputPath>..\bin</OutputPath>
					<XmlDocFile>DemoGame.xml</XmlDocFile>		
				</Options>
			</Configuration>
			<ReferencePath>../bin</ReferencePath>
			<Reference name="System"/> <!-- Assemblies that are located in the GAC (installed, global) can be referenced-->
			<Reference name="ode"/>  <!-- Assemblies that are located in the output directory to which the file is built can be referenced -->
			<Reference name="RealmForge.Utility"/> <!-- When you reference the name of another project, then that project (and it's output) will be referenced instead of looking for a pre-built assembly-->
			<Files>
				<Match path="." pattern="*.cs" recurse="true"/>
				<Match path="." pattern="*.bmp" recurse="true" buildAction="EmbeddedResource"/>
				<Match path="." pattern="[^a]*\.(png|jpg)" useRegex="true" buildAction="EmbeddedResource"/>
				
				<!-- Uses a regex or regular expression to find all files that end with .png or .jpg but dont have the letter 'a' in their name and add them to the project as EmbeddedResource's.  Because recurse enabled (default is false), only the values in the files in that are directly in the project directory (not child directories) are checked.-->
				<!--EmbeddedResource, Content, and Compile are valid buildAction's-->
			</Files>
		</Project>
		
	</Solution>
</Prebuild>

