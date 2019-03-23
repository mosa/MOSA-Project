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

b *0x0

####################
# continue debugging

continue
