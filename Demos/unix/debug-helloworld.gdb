#######################
# general configuration

set history remove-duplicates unlimited
set disassembly-flavor intel

define hook-quit
    # kill
    set confirm off
end

define hook-kill
    set confirm off
end

########################
# specific configuration

file bin/Mosa.HelloWorld.x86.bin
target remote | Demos/unix/debug-helloworld-internal.sh
b *0x0
# b *0xc010609f

#hbreak System.Void Mosa.HelloWorld.x86.Boot::Main()
#hbreak System.Void Mosa.Kernel.x86.ConsoleSession::GotoTop()
b *0x0531019
b Source/Mosa.Kernel.x86/ConsoleSession.cs:170
b *0x000000000050042c

####################
# continue debugging

continue
