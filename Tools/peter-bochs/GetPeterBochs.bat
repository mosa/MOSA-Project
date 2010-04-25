if exist peter-bochs-debugger20100411.jar goto exit

del peter-bochs-debugger*.jar

..\wget\wget http://peter-bochs.googlecode.com/files/peter-bochs-debugger20100411.jar

copy peter-bochs-debugger20100411.jar peter-bochs-debugger.jar

:exit
