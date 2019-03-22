FROM debian:stretch

RUN apt-get update \
    && apt-get upgrade -y

RUN apt-get install -y apt-transport-https dirmngr \
    && apt-key adv --no-tty --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF \
    && echo "deb https://download.mono-project.com/repo/debian stable-stretch main" | tee /etc/apt/sources.list.d/mono-official-stable.list \
    && apt-get update

RUN apt-get install -y qemu-system-x86 git wget mono-devel
