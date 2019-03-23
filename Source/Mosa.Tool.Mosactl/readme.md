Try to avoid references to other Dependencies, because loaded assemblies are locked on windows.
If dependencies are required, try to access them as late as possible, to make rebuilds possible.
Another solution would be to have an separate assembly and load them via reflection.
