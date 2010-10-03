if exist peter-bochs-debugger20100901.jar goto exit

del peter-bochs-debugger*.jar

..\wget\wget http://peter-bochs.googlecode.com/files/peter-bochs-debugger20100901.jar

copy peter-bochs-debugger20100901.jar peter-bochs-debugger.jar

:exit
