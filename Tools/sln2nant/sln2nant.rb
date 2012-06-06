#!/usr/bin/ruby

require 'rexml/document'
require 'tsort'

slnFile = ARGV[0]
@baseDir = ARGV[1]
@project2Assembly = {}
@projectReferences = {}
@buildfiles = []

def sln2nant(solutionFile)
  projectFiles = []
  File.open(solutionFile, "r") do |infile|
    while (line = infile.gets)
      if line.start_with?("Project(")
        values = line.split(',')
        if values[1].end_with?(".csproj\"")
          cleaned = values[1].strip.tr('\"', '').tr('\\', '/').tr('//', '/')
          projectFile = @baseDir + cleaned
          if (not projectFile.include? 'Test') or (projectFile.include? 'CodeDom') or (projectFile.include? 'Mosa.Test.Collection')
            projectFiles << projectFile
            @buildfiles << cleaned[0..cleaned.rindex('/')] + 'mosa.build'
          end
        end
      end
    end
  end
  
  projectFiles.each do |projectFile|
    puts "Preprocessing " + projectFile
    proj2nant_preprocessing projectFile
  end
  
  projectFiles.each do |projectFile|
    puts "Processing " + projectFile
    proj2nant projectFile, projectFile
  end
  create_main_buildfile
end

def create_main_buildfile 
  fileHandle = File.new(@baseDir + 'mosa.build', "w")
  preset = File.open('main.preset', 'rb') { |file| file.read }
  fileHandle.puts preset
  targetList = []
  @buildfiles.each do |buildfile|
    fileHandle.puts create_target_string buildfile
    targetList << get_target_name(buildfile)
  end
  fileHandle.puts
  fileHandle.puts '        <target name="all" depends ="'
  allTargets = ""
  targetList.each do |target|
    allTargets = allTargets + target + ','
  end
  allTargets = allTargets[0..-1]
  fileHandle.puts allTargets + '"/>'
  fileHandle.puts '</project>'
end

def create_target_string(buildfile)
  targetName = get_target_name buildfile
  depends = ""
  @projectReferences[buildfile].each do |reference|
    depends = depends + reference + ','
  end
  '        <target name="' +  targetName + '" depends="init,' + depends + '"><nant buildfile="' + buildfile + '"/></target>'
end

def get_target_name(buildfile)
  buildfile[0..-11].tr('/', '')
end

def proj2nant_preprocessing(projectFile)
  xmlData = File.open(projectFile, 'rb') { |file| file.read }
  doc = REXML::Document.new(xmlData)
  
  assemblyName = projectFile
  doc.elements.each('Project/PropertyGroup/AssemblyName') do |assembly|
    if assembly.text.include? 'mosacl'
	assemblyName = assembly.text + ".exe"
    else
	assemblyName = assembly.text + ".dll"
    end
  end
  
  index = projectFile.rindex('/') + 1
  length = projectFile.length
  @project2Assembly[projectFile[index..length]] = assemblyName
end

def proj2nant(projectFile, nantFile)
  buildFile = projectFile[@baseDir.length..-1]
  buildFile = buildFile[0..buildFile.rindex('/')] + 'mosa.build'
  @projectReferences[buildFile] = []
  xmlData = File.open(projectFile, 'rb') { |file| file.read }
  doc = REXML::Document.new(xmlData)
  
  includeFiles = []
  doc.elements.each('Project/ItemGroup/Compile') do |file|
    includeFiles << file.attributes["Include"].tr('\\', '/')
  end
  
  references = []
  mosaReferences = []
  startupObject = nil
  
  doc.elements.each('Project/PropertyGroup/StartupObject') do |startup|
    startupObject = startup.text
  end
  
  isLibrary = false
  doc.elements.each('Project/PropertyGroup/OutputType') do |outputType|
    if outputType.text == 'Library'
      isLibrary = true
    end
  end
  
  if not projectFile.include? ("Korlib")
    doc.elements.each('Project/ItemGroup/Reference') do |reference|
      unless reference.attributes["Include"].include?(",")
        if not reference.has_elements?
          references << reference.attributes["Include"] + ".dll"
        end
      end
    end
  
    doc.elements.each('Project/ItemGroup/Reference/HintPath') do |reference|
      references << reference.text.tr('\\', '/')
    end
  
    doc.elements.each('Project/ItemGroup/ProjectReference') do |reference|
      mosaReferences << reference.attributes["Include"].tr('\\', '/')
      dependency = reference.attributes["Include"].tr('\\', '/')
      dependency = dependency[0..dependency.rindex('/') - 1]
      dependency = dependency[dependency.rindex('/') + 1..-1]
      @projectReferences[buildFile] << dependency
    end
  end
  
  nostdlib = false
  doc.elements.each('Project/PropertyGroup/NoStdLib') do |reference|
    if reference.text == 'True'
      nostdlib = true
    end
  end
  
  assemblyName = projectFile
  doc.elements.each('Project/PropertyGroup/AssemblyName') do |assembly|
    if isLibrary
      assemblyName = assembly.text + ".dll"
    else
      assemblyName = assembly.text + ".exe"
    end
  end
  
  create_project_buildfile buildFile, includeFiles, assemblyName, nostdlib, references, mosaReferences, isLibrary, startupObject
end

def create_project_buildfile (buildFile, sourceFiles, assemblyName, nostdlib, references, projectReferences, isLibrary, startupObject)
  fileHandle = File.new(@baseDir + buildFile, "w")
  part1 = nil
  if buildFile.include? 'Tools/'
    part1 = File.open('project.1.preset_sub', 'rb') { |file| file.read }
  else
    part1 = File.open('project.1.preset', 'rb') { |file| file.read }
  end
  part1 = part1.chop
  part2 = File.open('project.2.preset', 'rb') { |file| file.read }
  part3 = File.open('project.3.preset', 'rb') { |file| file.read }
  part4 = File.open('project.4.preset', 'rb') { |file| file.read }
  part5 = File.open('project.5.preset', 'rb') { |file| file.read }
  
  if not isLibrary
    part1 = part1.sub('library', 'exe')
    if not (startupObject == nil)
      part1 = part1.sub('exe"', 'exe" main="' + startupObject + '"')
    end
  end
  
  nostdlibString = 'nostdlib="' + nostdlib.to_s + '"'
  
  fileHandle.puts part1 + assemblyName + part2 + nostdlibString + part3
  
  sourceFiles.each do |file|
    fileHandle.puts '                    <include name="' + file + '"/>'
  end
  
  fileHandle.puts part4
  
  references.each do |reference|
    fileHandle.puts '                    <include name="' + reference + '"/>'
  end
  
  projectReferences.each do |reference|
    puts '  > ' + reference
    fileHandle.puts '                    <include name="${outputDirectory}/' + @project2Assembly[reference[reference.rindex('/') + 1..-1]] + '"/>'
  end
  
  fileHandle.puts part5
end

sln2nant(slnFile)
