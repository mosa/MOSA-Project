/************************************************************************ 
  MKDOSFS  Make DOS FileSystem image    v00.12.65
  Forever Young Software      Benjamin David Lunt

  This utility was desinged for use with Bochs to make a DOS
   FAT 12, 16, or 32 disk image.

  Bochs is located at:
    http://bochs.sourceforge.net

  I designed this program to be used for testing my own OS,
   though you are welcome to use it any way you wish.

  Please note that I release it and it's code for others to
   use and do with as they want.  You may copy it, modify it,
   do what ever you want with it as long as you release the
   source code and display this entire comment block in your
   source or documentation file.
   (you may add to this comment block if you so desire)

  Please use at your own risk.  I do not specify that this
   code is correct and unharmful.  No warranty of any kind
   is given for its release.

  I take no blame for what may or may not happen by using
   this code with your purposes.

  'nuff of that!  You may modify this to your liking and if you
   see that it will help others with their use of Bochs, please
   send the revised code to fys@cybertrails.com.  I will then
   release it as I have this one.

  P.S.  Please don't laugh at my code :)  I didn't spend but
   a few minutes on this.  BXimage just wasn't working for my
   needs, so here it is.

  You may get the latest and greatest at:
    http://www.frontiernet.net/~fys/fysos.htm

  Thanks, and thanks to those who contributed to Bochs....

  ********************************************************

  Things to know:
  - I have not checked or put a lot of effort into any FAT size,
    other than FAT12.  I am only using FAT12 at the time.  If you
    see that the other two FAT sizes are incorrectly coded, please
    let me know.  Maybe you could fix it and send it in :)
  - 12 Aug 2007: I have worked with the FAT32 stuff a bit.  I am
    not saying that it is correct, I just put a little bit of 
    work in it to help it along.

  ********************************************************

  To compile using DJGPP:  (http://www.delorie.com/djgpp/)
     gcc -Os mkdosfs.c -o mkdosfs.exe -s  (DOS .EXE requiring DPMI)

  Compiles as is with MS VC++ 6.x         (Win32 .EXE file)

  Compiles as is with MS QC2.5            (TRUE DOS only)

  ********************************************************

  Usage:
    Simply answer the questions given.  Hit <enter> for the
     default of any question that has a default value in []'s.
    That's it....

************************************************************************/

// don't know which ones are needed or not needed.  I just copied them
//  across from another project. :)
#include <ctype.h>
#include <conio.h>
#include <stdio.h>
#include <errno.h>
#include <memory.h>
#include <stdlib.h>
#include <string.h>
#include <stddef.h>
#include <limits.h>
#include <math.h>
#include <time.h>

#include "mkdosfs.h"   // our include

#define TRUE   -1
#define FALSE   0

#define FAT12  0
#define FAT16  1
#define FAT32  2

FILE *fp;

unsigned  char buffer[512];          // a temp buffer
          char strbuff[80];          // a temp str buffer
          char drv_type[80];         // drive type 'fd' or 'hd'
          char filename[80];         // filename

unsigned  char fat_size = 12;        // 12 (default), 16, 32
unsigned  char fats = 2;             // number of FAT's
unsigned short spfat;                // sectors per fat

unsigned  char heads = 2;            // set up as floppy
unsigned  char spt   = 18;           //
unsigned short cylinders = 80;       //
unsigned  char spclust = 1;          // default sectors per cluster
unsigned  long sectors = 2880;       // total sectors


void write_sectors(FILE *fp, void *ptr, unsigned short cnt);
void putdot();

int main() {

  int i, j;
  unsigned long last_cnt;

  // print start string
  printf(strtstr);

  do {
    printf("Make Floppy or Hard Drive image? (fd/hd) [fd]: ");
    gets(drv_type);
    if (!strlen(drv_type)) { strcpy(drv_type, "fd"); break; }
  } while (strcmp(drv_type, "fd") && strcmp(drv_type, "hd"));

  printf("                          As filename [%c.img]: ", (drv_type[0] == 'h') ? 'c' : 'a');
  gets(filename);
  if (!strlen(filename)) strcpy(filename, (drv_type[0] == 'h') ? "c.img" : "a.img");

  if (drv_type[0] == 'h') {
    do {
      printf("                   Size (meg) (1 - 1024) [10]: ");
      gets(strbuff);
      if (!strlen(strbuff))
        i = 10;
      else
        i = atoi(strbuff);
    } while ((i < 1) || (i > 1024));
    cylinders = (i << 1);  // very close at 16 heads and 63 spt
    heads = 16;
    spt = 63;
    fat_size = 16;
  } else {
    do {
      printf("   Sectors per track (8, 9, 15, 18, 36)  [18]: ");
      gets(strbuff);
      if (!strlen(strbuff))
        i = 18;
      else
        i = atoi(strbuff);
    } while ((i != 8) && (i != 9) && (i != 15) && (i != 18) && (i != 36));
    if (i == 36) spclust = 2;
    spt = i;
    do {
      printf("                           Heads: (1, 2)  [2]: ");
      gets(strbuff);
      if (!strlen(strbuff))
        i = 2;
      else
        i = atoi(strbuff);
    } while ((i != 1) && (i != 2));
    heads = i;
    do {
      printf("                    Cylinders: (40, 80)  [80]: ");
      gets(strbuff);
      if (!strlen(strbuff))
        i = 80;
      else
        i = atoi(strbuff);
    } while ((i != 40) && (i != 80));
    cylinders = i;
    fat_size = 12;
  }
  sectors = (cylinders * heads * spt);

  do {
    printf("                     Sectors per Cluster [% 2i]: ", spclust);
    gets(strbuff);
    if (!strlen(strbuff))
      i = spclust;
    else
      i = atoi(strbuff);
  } while (i > 255);
  spclust = i;

  do {
    printf("                          Number of FAT's [2]: ");
    gets(strbuff);
    if (!strlen(strbuff))
      fats = 2;
    else
      fats = atoi(strbuff);
  } while (fats > 2);
  
  do {
    printf("                               FAT Size: [%02i]: ", fat_size);
    gets(strbuff);
    if (!strlen(strbuff))
      i = fat_size;
    else
      i = atoi(strbuff);
  } while ((i != 12) && (i != 16) && (i != 32));
  fat_size = i;

  switch (fat_size) {
  case 12:
    if ((sectors / (unsigned long) spclust) > 4086L) {
      printf(" *** Illegal Size disk with FAT 12 *** \n");
      exit(0x12);
    }
    spfat = (unsigned short) ((unsigned short)((float) sectors * 1.5) / (512 * spclust)) + 1;

    // actual count bytes on last fat sector needed as zeros (???)
    last_cnt = ((sectors - ((fats * spfat) + 17)) / spclust);
    last_cnt = ((unsigned long) ((float) last_cnt * 1.5) % 512);

    break;
  case 16:
    if ((sectors / (unsigned long) spclust) > 65526L) {
      printf(" *** Illegal Size disk with FAT 16 *** \n");
      exit(0x12);
    }
    spfat = (unsigned short) ((sectors << 1) / (512 * spclust)) + 1;

    // actual count bytes on last fat sector needed as zeros (???)
    last_cnt = ((sectors - ((fats * spfat) + 17)) / spclust);
    last_cnt = ((unsigned long) (last_cnt << 1) % 512);
    
    break;
  default:
    spfat = (unsigned short) ((sectors << 2) / (512 * spclust)) + 1;

    // actual count bytes on last fat sector needed as zeros (???)
    last_cnt = ((sectors - ((fats * spfat) + 17)) / spclust);
    last_cnt = ((unsigned long) (last_cnt << 2) % 512);
  }
  
  printf("\n       Creating file:   [%s]"
       "\n           Cylinders:    %i"
       "\n               Sides:    %i"
       "\n       Sectors/Track:    %i"
       "\n       Total Sectors:    %lu"
       "\n                Size:    %3.2f (megs)"
       "\n     Sectors/Cluster:    %i"
       "\n               FAT's:    %i"
       "\n         Sectors/FAT:    %i"
       "\n            FAT size:    %i",
       filename, cylinders, heads, spt, sectors,
       (float) ((float) sectors / 2000.0), spclust, fats, spfat, fat_size);

  // create BPB/boot block
  unsigned char boot_sector[512];
  memset(boot_sector, 0, 512);  // first, clear it out
  switch (fat_size) {
    case 12:
    case 16: {
      struct S_FAT1216_BPB *bpb = (struct S_FAT1216_BPB *) boot_sector;
      bpb->jmps[0] = 0xEB; bpb->jmps[1] = 0x3C;
      bpb->nop = 0x90;
      memcpy(bpb->oemname, "MKDOSFS ", 8);  //    char oemname[8];    // OEM name
      bpb->nBytesPerSec = 512;    //  unsigned short nBytesPerSec;  // Bytes per sector
      bpb->nSecPerClust = spclust;  //  unsigned  char nSecPerClust;  // Sectors per cluster
      bpb->nSecRes = 1;     //  unsigned short nSecRes;       // Sectors reserved for Boot Record
      bpb->nFATs = fats;      //  unsigned  char nFATs;         // Number of FATs
      bpb->nRootEnts = 224;   //  unsigned short nRootEnts;     // Max Root Directory Entries allowed
      if (sectors < 65536) {    //  unsigned short nSecs;     // Number of Logical Sectors
        bpb->nSecs = (unsigned short) sectors;
        bpb->nSecsExt = 0;    //  unsigned  long nSecsExt;      // This value used when there are more
      } else {
        bpb->nSecs = 0;
        bpb->nSecsExt = sectors;  //  unsigned  long nSecsExt;      // This value used when there are more
      }
      bpb->mDesc = ((drv_type[0] == 'h') ? 0xF8 : 0xF9);      //  unsigned  char mDesc;         // Medium Descriptor Byte (we have it set as floppy 1.44)
      bpb->nSecPerFat = spfat;  //  unsigned short nSecPerFat;    // Sectors per FAT
      bpb->nSecPerTrack = spt;    //  unsigned short nSecPerTrack;  // Sectors per Track
      bpb->nHeads = heads;      //  unsigned short nHeads;        // Number of Heads
      bpb->nSecHidden = 0;      //  unsigned  long nSecHidden;    // Number of Hidden Sectors
      bpb->DriveNum = 0;      //  unsigned  char DriveNum;      // Physical drive number
      bpb->nResByte = 0;      //  unsigned  char nResByte;      // Reserved (we use for FAT type (12- 16-bit)
      bpb->sig = 0x29;        //  unsigned  char sig;           // Signature for Extended Boot Record
      bpb->SerNum = 0;        //  unsigned  long SerNum;        // Volume Serial Number
      memcpy(bpb->VolName, "NO LABEL    ", 11); // char VolName[11]; // Volume Label
      sprintf(strbuff, "FAT%2i   ", fat_size);
      memcpy(bpb->FSType, strbuff, 8);     // char FSType[8];   // File system type
      memcpy(bpb->filler, boot_code, sizeof(boot_code));                   // then place code
      memcpy(bpb->filler+sizeof(boot_code), boot_data, sizeof(boot_data)); // then place data
      memset(bpb->part_tble, 0, 4 * sizeof(struct PART_TBLE));
      if (drv_type[0] == 'h') {
        bpb->part_tble[0].bi = 0x80;
        bpb->part_tble[0].s_sector = 1;  // sectors are 1 based
        bpb->part_tble[0].si = (sectors < 65536) ? 0x04 : 0x06;   // System ID
        // TODO: lba -> chs for ending CHS entries.
        bpb->part_tble[0].startlba = 0;
        bpb->part_tble[0].size = sectors;
      }
      bpb->boot_sig = 0xAA55;
      break;
    }
      
    case 32: {
      struct S_FAT32_BPB *bpb = (struct S_FAT32_BPB *) boot_sector;
      bpb->jmps[0] = 0xEB; bpb->jmps[1] = 0x3C;
      bpb->nop = 0x90;
      memcpy(bpb->oemname, "MKDOSFS ", 8);  //    char oemname[8];    // OEM name
      bpb->nBytesPerSec = 512;        // Bytes per sector
      bpb->nSecPerClust = spclust;    // Sectors per cluster
      bpb->nSecRes = SECT_RES32;      // Sectors reserved for Boot Record
      bpb->nFATs = 0;       
      bpb->nRootEnts = 0;
      if (sectors < 65536) {          // Number of Logical Sectors
        bpb->nSecs = (unsigned short) sectors;
        bpb->nSecsExt = 0;            // This value used when there are more
      } else {
        bpb->nSecs = 0;
        bpb->nSecsExt = sectors;      // This value used when there are more
      }
      bpb->mDesc = ((drv_type[0] == 'h') ? 0xF8 : 0xF9);              // Medium Descriptor Byte
      bpb->nSecPerFat = 0;
      bpb->nSecPerTrack = spt;        // Sectors per Track
      bpb->nHeads = heads;            // Number of Heads
      bpb->nSecHidden = 0;            // Number of Hidden Sectors
      bpb->sect_per_fat32 = spfat; 
      bpb->DriveNum = 0;              // Physical drive number
      bpb->ext_flags = 0x00;
      bpb->fs_version = 0;
      bpb->root_base_cluster = 0x02;
      bpb->fs_info_sec = 1;
      bpb->backup_boot_sec = 0;  // 6
      bpb->nResByte = 0;              // Reserved (we use for FAT type (12- 16-bit)
      bpb->sig = 0x29;                // Signature for Extended Boot Record
      bpb->SerNum = rand();
      memcpy(bpb->VolName, "NO LABEL    ", 11);    // Volume Label
      memcpy(bpb->FSType, "FAT32   ", 8);          // File system type
      memcpy(bpb->filler, boot_code, sizeof(boot_code));                   // then place code
      memcpy(bpb->filler+sizeof(boot_code), boot_data, sizeof(boot_data)); // then place data
      memset(bpb->part_tble, 0, 4 * sizeof(struct PART_TBLE));
      if (drv_type[0] == 'h') {
        bpb->part_tble[0].bi = 0x80;
        bpb->part_tble[0].s_sector = 1;  // sectors are 1 based
        bpb->part_tble[0].si = (sectors < 65536) ? 0x04 : 0x06;   // System ID
        // TODO: lba -> chs for ending CHS entries.
        bpb->part_tble[0].startlba = 0;
        bpb->part_tble[0].size = sectors;
      }
      bpb->boot_sig = 0xAA55;
      break;
    }
  }

  if ((fp = fopen(filename, "wb")) == NULL) {
    printf("\nError creating file [%s]", filename);
    return 0x01;
  }
  
  printf("\n\nWorking[");

  // write the BPB
  putdot();
  write_sectors(fp, boot_sector, 1);
  sectors--;

  // if fat32, write the info sector
  if (fat_size == 32) {
    struct S_FAT32_FSINFO fsInfo;
    memset(&fsInfo, 0, 512);
    fsInfo.sig0 = 0x41615252;
    fsInfo.sig1 = 0x61417272;
    fsInfo.free_clust_fnt = 0xFFFFFFFF;
    fsInfo.next_free_clust = 3;
    fsInfo.trail_sig = 0xAA550000;
    putdot();
    write_sectors(fp, &fsInfo, 1);
    
    memset(buffer, 0, 512);
    buffer[510] = 0x55;
    buffer[511] = 0xAA;
    write_sectors(fp, buffer, 1);
    
    memset(buffer, 0, 512);
    write_sectors(fp, buffer, 1);
    write_sectors(fp, buffer, 1);
    write_sectors(fp, buffer, 1);
    
    write_sectors(fp, boot_sector, 1);
    write_sectors(fp, &fsInfo, 1);
    buffer[510] = 0x55;
    buffer[511] = 0xAA;
    write_sectors(fp, buffer, 1);
    
    memset(buffer, 0, 512);
    int r;
    for (r=9; r<SECT_RES32; r++)
      write_sectors(fp, buffer, 1);
    
    sectors -= (SECT_RES32 - 1);  // - 1 = original boot sector
  }
  
  // write the FAT(s)
  for (i=0; i<fats; i++) {
    memset(buffer, 0, 512);
    switch (fat_size) {
      case 32:
        buffer[3] = 0x0F;
      case 16:
        buffer[2] = 0xFF;
      case 12:
        buffer[1] = 0xFF;
        buffer[0] = ((drv_type[0] == 'h') ? 0xF8 : 0xF9);
    }
    switch (fat_size) {
      case 32:
        buffer[7] = 0xFF;
      case 16:
        buffer[6] = 0xFF;
      case 12:
        buffer[5] = 0xFF;
        buffer[4] = 0xFF;
    }
    putdot();
    write_sectors(fp, &buffer, 1);
    sectors--;
    memset(buffer, 0, 512);
    for (j=0; j<spfat-2; j++) {
      putdot();
      write_sectors(fp, &buffer, 1);
      sectors--;
    }
    // write last sector of FAT filling in unused entries
    // (last_cnt isn't exact, but it is pretty close.  I just
    //  didn't want to spend the time to get it exact :)
    memset(buffer, 0, (unsigned short) last_cnt);
    memset(buffer + last_cnt, 0xFF, (unsigned short) (512-last_cnt));
    putdot();
    write_sectors(fp, &buffer, 1);
    sectors--;
  }

  // write the root
  memset(buffer, 0, 512);
  for (i=0; i<14; i++) {
    putdot();
    write_sectors(fp, &buffer, 1);
    sectors--;
  }

  // write data area
  memset(buffer, 0, 512);
  while (sectors--) {
    putdot();
    write_sectors(fp, &buffer, 1);
  }

  printf("]Done");

  // close the file
  fclose(fp);

  return 0x00;

}

// Write sector(s)
void write_sectors(FILE *fp, void *ptr, unsigned short cnt) {
  if (fwrite(ptr, 512, cnt, fp) < cnt) {
    printf("\n **** Error writing to file ****");
    exit(0xFF);
  }
}

// put a dot
void putdot() {
  if (!(sectors & 0x000003FFL)) putch('.');
}
