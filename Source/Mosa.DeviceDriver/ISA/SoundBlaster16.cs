﻿using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA
{
	// TODO: Fix
	public class SoundBlaster16 : BaseDeviceDriver, IAudioDevice
	{
		private enum Options { TurnOn = 0xD1, SetTimeConstant = 0x40, MasterVolume = 0x22 }

		private BaseIOPortWrite Reset, Write, MixerPort, MixerDataPort, ChannelControl, FlipFlop, TransferMode, PageNumber, Position, DataLength;
		private BaseIOPortRead Read;

		private bool Play16BitSongs = false;

		public byte[] TestSound { get; } = { 0x52, 0x49, 0x46, 0x46, 0x68, 0x07, 0x00, 0x00, 0x57, 0x41, 0x56, 0x45, 0x66, 0x6d, 0x74, 0x20, 0x10, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0xab, 0x20, 0x00, 0x00, 0xab, 0x20, 0x00, 0x00, 0x01, 0x00, 0x08, 0x00, 0x64, 0x61, 0x74, 0x61, 0x44, 0x07, 0x00, 0x00, 0x80, 0x80, 0x80, 0x80, 0x76, 0x8a, 0x97, 0x96, 0x8f, 0x87, 0x80, 0x7a, 0x77, 0x71, 0x71, 0x71, 0x76, 0x7b, 0x82, 0x8b, 0x96, 0x9f, 0xa7, 0xb2, 0xb9, 0xbd, 0xbc, 0xba, 0xb9, 0xbf, 0xb1, 0x71, 0x38, 0x41, 0x96, 0xcc, 0x9f, 0x46, 0x2f, 0x5e, 0x9e, 0xba, 0xb4, 0xa3, 0x75, 0x43, 0x52, 0xa2, 0xc5, 0x74, 0x11, 0x18, 0x47, 0x63, 0x66, 0x79, 0xb3, 0xe2, 0xba, 0x6d, 0x72, 0xbb, 0xe3, 0xea, 0xda, 0xb8, 0xc5, 0xdb, 0xdb, 0xcc, 0x61, 0x1c, 0x5d, 0x4d, 0x52, 0x8b, 0xa1, 0x9f, 0x97, 0x85, 0x83, 0x81, 0x92, 0x3d, 0x02, 0x76, 0x75, 0x3f, 0x55, 0x58, 0x91, 0xa7, 0xa7, 0xbc, 0xaf, 0xc1, 0xd7, 0xc8, 0xb1, 0x71, 0x7c, 0x96, 0x7a, 0x93, 0x9f, 0x9a, 0xb4, 0xb8, 0xb2, 0x92, 0x56, 0x24, 0x12, 0x35, 0x6d, 0x66, 0x69, 0x6c, 0x80, 0x93, 0x85, 0x82, 0x79, 0x51, 0x48, 0x6c, 0x80, 0x80, 0x79, 0x79, 0x55, 0x34, 0x28, 0x29, 0x47, 0x71, 0x78, 0x67, 0x78, 0x89, 0x80, 0x6f, 0x90, 0xb0, 0xa7, 0x75, 0x72, 0x84, 0x9a, 0x6c, 0x50, 0x32, 0x61, 0x69, 0x6f, 0x86, 0x9a, 0xa6, 0xac, 0xbb, 0xb3, 0xa4, 0x81, 0x87, 0x89, 0x97, 0x8a, 0x87, 0x97, 0xa1, 0xad, 0xb5, 0xce, 0xa7, 0x87, 0x75, 0x85, 0x66, 0x68, 0x5b, 0x61, 0x71, 0x72, 0x76, 0x86, 0x80, 0x8a, 0x92, 0x99, 0x84, 0x70, 0x53, 0x41, 0x41, 0x09, 0x10, 0x2a, 0x32, 0x39, 0x5c, 0x59, 0x61, 0x45, 0x3f, 0x46, 0x6d, 0x7d, 0x71, 0x82, 0x9f, 0xaf, 0xb7, 0xd0, 0xbf, 0xbf, 0xc7, 0xd8, 0xd2, 0xcf, 0xde, 0xd8, 0xd7, 0xd8, 0xc3, 0xa7, 0x9e, 0x98, 0x89, 0x98, 0x7b, 0x63, 0x5f, 0x57, 0x74, 0x79, 0x71, 0x80, 0x61, 0x4b, 0x48, 0x40, 0x21, 0x2b, 0x32, 0x15, 0x36, 0x4b, 0x39, 0x59, 0x76, 0x7c, 0x9b, 0xae, 0xac, 0xb3, 0xae, 0xaa, 0xa2, 0x8e, 0x8d, 0x97, 0x8d, 0x9a, 0xae, 0x9f, 0x8f, 0x9f, 0xa1, 0xa1, 0x8f, 0x7e, 0x76, 0x42, 0x52, 0x62, 0x4d, 0x5c, 0x81, 0x7b, 0x81, 0x99, 0x7d, 0x70, 0x87, 0x80, 0x71, 0x64, 0x69, 0x6f, 0x68, 0x79, 0x9c, 0x98, 0x87, 0x9b, 0xb0, 0xb3, 0xb7, 0xbe, 0xba, 0x9f, 0x95, 0x94, 0x81, 0x8b, 0x99, 0x9d, 0xa2, 0x99, 0x6f, 0x6e, 0x8d, 0x65, 0x4a, 0x71, 0x97, 0x97, 0x80, 0x81, 0x77, 0x55, 0x36, 0x17, 0x0b, 0x19, 0x37, 0x41, 0x41, 0x4d, 0x5b, 0x6e, 0x88, 0x91, 0x7c, 0x7e, 0x95, 0x8b, 0x80, 0x8c, 0xa6, 0xa4, 0x95, 0xa2, 0xbc, 0xc0, 0xb8, 0xbd, 0xcc, 0xc9, 0xa1, 0x84, 0xa2, 0xa2, 0x7c, 0x79, 0x9d, 0xaf, 0xa3, 0x9f, 0xaf, 0xaf, 0x93, 0x86, 0x76, 0x5a, 0x57, 0x43, 0x31, 0x43, 0x4e, 0x42, 0x4b, 0x63, 0x72, 0x78, 0x7a, 0x80, 0x8a, 0x9c, 0xad, 0x97, 0x7a, 0x74, 0x71, 0x6b, 0x5b, 0x51, 0x5d, 0x75, 0x6c, 0x50, 0x4c, 0x5a, 0x5b, 0x57, 0x62, 0x7c, 0x95, 0xa6, 0xa4, 0x88, 0x7e, 0x95, 0xa3, 0x8a, 0x72, 0x7f, 0xa6, 0xbc, 0xaf, 0xa4, 0xaf, 0xc1, 0xb8, 0x9f, 0x97, 0xa2, 0x98, 0x87, 0x88, 0x98, 0xa6, 0xad, 0xac, 0xa4, 0xa4, 0x9a, 0x81, 0x6e, 0x61, 0x48, 0x27, 0x16, 0x1b, 0x31, 0x47, 0x54, 0x5a, 0x69, 0x7c, 0x7e, 0x76, 0x77, 0x79, 0x6f, 0x5d, 0x58, 0x69, 0x80, 0x80, 0x79, 0x77, 0x85, 0x95, 0x9a, 0x96, 0x93, 0x94, 0xa1, 0xb2, 0xb6, 0xa7, 0x97, 0x96, 0x97, 0x90, 0x88, 0x86, 0x88, 0x8a, 0x8b, 0x8b, 0x83, 0x71, 0x61, 0x59, 0x63, 0x79, 0x85, 0x82, 0x70, 0x5e, 0x62, 0x72, 0x7a, 0x79, 0x77, 0x80, 0x96, 0x9f, 0x95, 0x8b, 0x8b, 0x8e, 0x81, 0x74, 0x79, 0x8b, 0xa0, 0xaa, 0xa4, 0x9b, 0x99, 0x9e, 0xa7, 0xb2, 0xb7, 0xae, 0x8c, 0x65, 0x51, 0x57, 0x54, 0x35, 0x13, 0x0a, 0x1e, 0x45, 0x6d, 0x80, 0x7c, 0x64, 0x4d, 0x44, 0x4c, 0x55, 0x59, 0x5d, 0x69, 0x7a, 0x91, 0xa3, 0xab, 0xaa, 0xa4, 0x9f, 0xa2, 0xb2, 0xc6, 0xcd, 0xc9, 0xbf, 0xb7, 0xba, 0xc2, 0xc5, 0xc6, 0xc2, 0xbc, 0xb2, 0xa8, 0xa1, 0x96, 0x7f, 0x64, 0x54, 0x53, 0x54, 0x51, 0x48, 0x3d, 0x37, 0x39, 0x3f, 0x41, 0x45, 0x4b, 0x5a, 0x6f, 0x80, 0x84, 0x7f, 0x6d, 0x5f, 0x56, 0x50, 0x4e, 0x58, 0x6d, 0x82, 0x93, 0x9a, 0x9d, 0x9a, 0x94, 0x8f, 0x93, 0xa0, 0xac, 0xae, 0xa9, 0xa1, 0x99, 0x8e, 0x82, 0x7e, 0x81, 0x8e, 0x97, 0x9e, 0x9f, 0xa3, 0x9f, 0x8f, 0x79, 0x67, 0x61, 0x6c, 0x7b, 0x85, 0x89, 0x8a, 0x8e, 0x93, 0x97, 0x97, 0x8c, 0x7f, 0x7a, 0x7d, 0x80, 0x78, 0x62, 0x51, 0x4c, 0x57, 0x70, 0x89, 0x99, 0x9c, 0x92, 0x85, 0x7b, 0x6e, 0x5d, 0x51, 0x51, 0x61, 0x75, 0x7e, 0x7b, 0x73, 0x69, 0x5e, 0x58, 0x59, 0x66, 0x7b, 0x91, 0x9f, 0xa1, 0x9b, 0x94, 0x8b, 0x87, 0x86, 0x82, 0x84, 0x87, 0x8e, 0x96, 0x9d, 0xa0, 0xa2, 0xa3, 0xa3, 0xa0, 0x9a, 0x8e, 0x7c, 0x70, 0x6e, 0x77, 0x80, 0x85, 0x86, 0x86, 0x87, 0x87, 0x82, 0x7d, 0x79, 0x73, 0x72, 0x69, 0x5e, 0x5a, 0x60, 0x6e, 0x7d, 0x8a, 0x95, 0x98, 0x97, 0x93, 0x93, 0x92, 0x92, 0x8f, 0x87, 0x81, 0x7f, 0x79, 0x6f, 0x61, 0x55, 0x51, 0x57, 0x61, 0x6b, 0x75, 0x7a, 0x7e, 0x7a, 0x70, 0x5f, 0x4e, 0x47, 0x49, 0x59, 0x73, 0x8b, 0x9d, 0x9d, 0x8f, 0x80, 0x79, 0x7e, 0x8e, 0x9f, 0xae, 0xb7, 0xba, 0xb6, 0xb0, 0xae, 0xae, 0xb5, 0xbc, 0xc0, 0xbf, 0xbb, 0xb3, 0xa7, 0x9b, 0x8f, 0x88, 0x81, 0x7b, 0x6f, 0x5f, 0x4d, 0x42, 0x3c, 0x39, 0x3a, 0x41, 0x4b, 0x59, 0x67, 0x71, 0x78, 0x6d, 0x59, 0x3b, 0x27, 0x22, 0x32, 0x50, 0x70, 0x87, 0x95, 0x97, 0x93, 0x8d, 0x87, 0x8b, 0x94, 0xa1, 0xae, 0xb7, 0xbc, 0xb9, 0xb4, 0xaf, 0xad, 0xab, 0xa9, 0xa4, 0xa0, 0x9f, 0xa2, 0x9f, 0x9a, 0x8c, 0x7e, 0x71, 0x68, 0x63, 0x65, 0x6e, 0x79, 0x7f, 0x7c, 0x70, 0x64, 0x61, 0x67, 0x74, 0x80, 0x80, 0x77, 0x68, 0x58, 0x50, 0x52, 0x62, 0x77, 0x8a, 0x97, 0x9f, 0x9e, 0x97, 0x8f, 0x89, 0x87, 0x8b, 0x8f, 0x8e, 0x87, 0x80, 0x77, 0x70, 0x6a, 0x64, 0x5f, 0x5e, 0x62, 0x6e, 0x7f, 0x8c, 0x94, 0x95, 0x87, 0x74, 0x62, 0x59, 0x5a, 0x66, 0x76, 0x82, 0x8b, 0x8f, 0x90, 0x93, 0x9a, 0x9f, 0x9f, 0x9a, 0x92, 0x8b, 0x88, 0x8d, 0x94, 0xa0, 0xac, 0xb2, 0xb3, 0xa8, 0x99, 0x89, 0x80, 0x7c, 0x80, 0x7e, 0x78, 0x6c, 0x60, 0x5a, 0x5f, 0x65, 0x71, 0x76, 0x73, 0x6e, 0x68, 0x68, 0x6f, 0x79, 0x82, 0x87, 0x87, 0x7d, 0x6e, 0x5b, 0x4f, 0x4a, 0x4f, 0x58, 0x67, 0x79, 0x8b, 0x97, 0x9f, 0x9e, 0x97, 0x8c, 0x80, 0x75, 0x73, 0x79, 0x82, 0x8f, 0x97, 0x97, 0x8a, 0x7e, 0x73, 0x75, 0x81, 0x97, 0xa9, 0xb2, 0xab, 0x98, 0x8a, 0x80, 0x84, 0x90, 0x9f, 0xa8, 0xad, 0xaa, 0x9f, 0x93, 0x89, 0x86, 0x83, 0x85, 0x84, 0x80, 0x78, 0x68, 0x5b, 0x51, 0x51, 0x55, 0x61, 0x6b, 0x76, 0x7e, 0x7f, 0x7a, 0x70, 0x60, 0x4b, 0x3b, 0x38, 0x3d, 0x51, 0x65, 0x77, 0x7e, 0x79, 0x71, 0x6b, 0x70, 0x80, 0x97, 0xaa, 0xb3, 0xb5, 0xad, 0xa0, 0x94, 0x8f, 0x90, 0x96, 0x9f, 0xa8, 0xb1, 0xb2, 0xb1, 0xab, 0xa6, 0xa2, 0x9e, 0x97, 0x8d, 0x81, 0x79, 0x72, 0x71, 0x75, 0x77, 0x72, 0x6b, 0x69, 0x69, 0x69, 0x6b, 0x6b, 0x66, 0x5a, 0x51, 0x4d, 0x51, 0x5f, 0x70, 0x7c, 0x80, 0x80, 0x7e, 0x7c, 0x7f, 0x84, 0x8e, 0x97, 0x9c, 0x9a, 0x94, 0x8a, 0x80, 0x77, 0x74, 0x78, 0x7f, 0x84, 0x89, 0x8f, 0x92, 0x94, 0x97, 0x97, 0x8f, 0x7e, 0x66, 0x53, 0x49, 0x4b, 0x59, 0x6d, 0x7f, 0x86, 0x8b, 0x8a, 0x87, 0x87, 0x85, 0x83, 0x83, 0x86, 0x89, 0x8f, 0x97, 0x9e, 0xa4, 0xa4, 0xa1, 0x9d, 0x98, 0x96, 0x93, 0x92, 0x94, 0x95, 0x91, 0x8c, 0x81, 0x7b, 0x74, 0x71, 0x72, 0x74, 0x71, 0x68, 0x5d, 0x56, 0x5b, 0x66, 0x76, 0x81, 0x84, 0x76, 0x5c, 0x41, 0x31, 0x31, 0x3f, 0x57, 0x6f, 0x83, 0x93, 0x9a, 0x97, 0x90, 0x87, 0x81, 0x80, 0x85, 0x8d, 0x91, 0x97, 0x98, 0x9a, 0x9a, 0x9b, 0x9a, 0x9c, 0x9f, 0xa4, 0xa8, 0xaa, 0xa7, 0x9e, 0x8f, 0x80, 0x77, 0x78, 0x80, 0x8d, 0x98, 0x97, 0x8d, 0x7e, 0x71, 0x6b, 0x70, 0x79, 0x81, 0x83, 0x7c, 0x6c, 0x5c, 0x51, 0x4c, 0x51, 0x5d, 0x6b, 0x79, 0x81, 0x84, 0x80, 0x7b, 0x78, 0x73, 0x72, 0x71, 0x72, 0x71, 0x71, 0x72, 0x75, 0x79, 0x7a, 0x78, 0x79, 0x7d, 0x83, 0x8e, 0x97, 0x9b, 0x98, 0x8d, 0x80, 0x77, 0x76, 0x7f, 0x8a, 0x97, 0x9f, 0xa2, 0xa2, 0xa0, 0x9f, 0x9c, 0x9a, 0x97, 0x91, 0x89, 0x80, 0x7a, 0x7a, 0x7d, 0x85, 0x8a, 0x8f, 0x8d, 0x87, 0x7f, 0x75, 0x6f, 0x6d, 0x6e, 0x70, 0x6b, 0x65, 0x5f, 0x5c, 0x5d, 0x62, 0x68, 0x6a, 0x6c, 0x6c, 0x72, 0x78, 0x82, 0x8a, 0x8f, 0x8d, 0x86, 0x7d, 0x76, 0x71, 0x73, 0x78, 0x7f, 0x86, 0x8f, 0x99, 0x9f, 0xa0, 0xa1, 0x9c, 0x92, 0x84, 0x75, 0x69, 0x66, 0x6a, 0x77, 0x85, 0x91, 0x98, 0x94, 0x87, 0x7b, 0x71, 0x6e, 0x74, 0x7e, 0x84, 0x87, 0x86, 0x85, 0x86, 0x89, 0x8c, 0x8e, 0x8f, 0x8f, 0x8e, 0x8c, 0x8b, 0x8b, 0x8d, 0x8d, 0x8a, 0x87, 0x83, 0x80, 0x7b, 0x75, 0x71, 0x6c, 0x6e, 0x70, 0x77, 0x80, 0x88, 0x8d, 0x8b, 0x80, 0x6e, 0x56, 0x42, 0x37, 0x3a, 0x49, 0x60, 0x78, 0x85, 0x87, 0x83, 0x7b, 0x75, 0x75, 0x7a, 0x80, 0x83, 0x88, 0x8d, 0x8f, 0x91, 0x94, 0x97, 0x9b, 0x9f, 0xa1, 0xa5, 0xa6, 0xa5, 0xa2, 0x9f, 0x9a, 0x97, 0x95, 0x94, 0x95, 0x97, 0x94, 0x8f, 0x89, 0x83, 0x7e, 0x76, 0x71, 0x70, 0x72, 0x77, 0x75, 0x6b, 0x5e, 0x50, 0x48, 0x48, 0x50, 0x5a, 0x68, 0x71, 0x75, 0x77, 0x79, 0x7b, 0x7d, 0x7d, 0x7c, 0x7a, 0x79, 0x77, 0x79, 0x79, 0x7f, 0x83, 0x88, 0x8f, 0x95, 0x98, 0x9b, 0x9b, 0x98, 0x95, 0x8f, 0x8a, 0x85, 0x80, 0x7c, 0x7c, 0x80, 0x86, 0x8e, 0x91, 0x92, 0x8e, 0x8c, 0x88, 0x87, 0x83, 0x80, 0x7c, 0x79, 0x79, 0x7f, 0x81, 0x85, 0x87, 0x86, 0x83, 0x80, 0x7d, 0x7a, 0x7c, 0x80, 0x82, 0x82, 0x80, 0x7d, 0x78, 0x71, 0x6b, 0x6a, 0x6b, 0x71, 0x77, 0x7b, 0x7d, 0x7f, 0x7e, 0x7e, 0x7c, 0x79, 0x75, 0x6d, 0x66, 0x64, 0x67, 0x6f, 0x79, 0x84, 0x8d, 0x93, 0x97, 0x95, 0x90, 0x88, 0x80, 0x79, 0x75, 0x74, 0x79, 0x80, 0x8b, 0x92, 0x97, 0x97, 0x92, 0x8e, 0x88, 0x87, 0x85, 0x87, 0x8b, 0x8d, 0x90, 0x8f, 0x8a, 0x83, 0x7f, 0x7d, 0x7d, 0x7e, 0x80, 0x80, 0x80, 0x80, 0x82, 0x86, 0x87, 0x85, 0x80, 0x79, 0x71, 0x68, 0x65, 0x65, 0x69, 0x71, 0x7c, 0x84, 0x8c, 0x90, 0x8d, 0x85, 0x78, 0x6a, 0x61, 0x5a, 0x5d, 0x65, 0x71, 0x7e, 0x84, 0x88, 0x87, 0x84, 0x7f, 0x78, 0x75, 0x75, 0x79, 0x7f, 0x82, 0x85, 0x86, 0x89, 0x89, 0x8e, 0x90, 0x92, 0x92, 0x93, 0x94, 0x96, 0x95, 0x94, 0x93, 0x92, 0x92, 0x92, 0x8f, 0x8e, 0x8b, 0x88, 0x87, 0x86, 0x86, 0x86, 0x86, 0x87, 0x86, 0x82, 0x7a, 0x70, 0x64, 0x5b, 0x55, 0x53, 0x54, 0x59, 0x61, 0x69, 0x6f, 0x75, 0x79, 0x7a, 0x7b, 0x79, 0x76, 0x72, 0x70, 0x70, 0x71, 0x77, 0x7f, 0x83, 0x88, 0x8f, 0x92, 0x95, 0x95, 0x95, 0x93, 0x90, 0x8e, 0x8d, 0x8c, 0x8d, 0x8c, 0x8e, 0x8f, 0x91, 0x93, 0x93, 0x93, 0x90, 0x8c, 0x86, 0x80, 0x7a, 0x79, 0x78, 0x77, 0x79, 0x79, 0x7d, 0x7e, 0x7f, 0x7e, 0x7b, 0x79, 0x79, 0x7a, 0x80, 0x80, 0x83, 0x81, 0x7d, 0x77, 0x70, 0x6b, 0x6a, 0x6c, 0x70, 0x77, 0x80, 0x85, 0x8c, 0x8f, 0x8e, 0x88, 0x82, 0x7b, 0x76, 0x71, 0x70, 0x70, 0x74, 0x79, 0x7e, 0x82, 0x87, 0x8a, 0x88, 0x86, 0x84, 0x80, 0x7d, 0x78, 0x77, 0x78, 0x7c, 0x80, 0x84, 0x88, 0x8a, 0x8a, 0x87, 0x87, 0x86, 0x87, 0x89, 0x8b, 0x8c, 0x8e, 0x8e, 0x8e, 0x8c, 0x88, 0x87, 0x85, 0x83, 0x84, 0x84, 0x86, 0x87, 0x89, 0x87, 0x85, 0x80, 0x7b, 0x74, 0x6c, 0x68, 0x65, 0x68, 0x6b, 0x71, 0x7a, 0x80, 0x85, 0x87, 0x84, 0x80, 0x78, 0x6f, 0x67, 0x61, 0x62, 0x68, 0x70, 0x79, 0x7f, 0x81, 0x82, 0x82, 0x83, 0x83, 0x84, 0x84, 0x84, 0x85, 0x86, 0x87, 0x89, 0x88, 0x89, 0x88, 0x87, 0x87, 0x88, 0x8b, 0x8b, 0x8e, 0x8e, 0x8e, 0x8e, 0x8e, 0x8e, 0x8b, 0x88, 0x86, 0x85, 0x84, 0x84, 0x84, 0x86, 0x86, 0x87, 0x87, 0x87, 0x84, 0x7f, 0x77, 0x6e, 0x67, 0x61, 0x60, 0x62, 0x67, 0x6a, 0x71, 0x76, 0x79, 0x7e, 0x80, 0x80, 0x7c, 0x77, 0x71, 0x6e, 0x6e, 0x70, 0x73, 0x7b, 0x80, 0x82, 0x87, 0x89, 0x8b, 0x8c, 0x8b, 0x8b, 0x89, 0x88, 0x88, 0x87, 0x88, 0x88, 0x89, 0x8c, 0x8c, 0x8e, 0x8f, 0x8f, 0x8f, 0x8f, 0x8c, 0x8b, 0x87, 0x84, 0x80, 0x7f, 0x7d, 0x7d, 0x7d, 0x7f, 0x7e, 0x7c, 0x7b, 0x7c, 0x7e, 0x7f, 0x80, 0x81, 0x81, 0x80, 0x7b, 0x75, 0x71, 0x6c, 0x69, 0x6c, 0x6e, 0x74, 0x7a, 0x80, 0x84, 0x87, 0x87, 0x84, 0x80, 0x7a, 0x77, 0x75, 0x75, 0x79, 0x7b, 0x80, 0x82, 0x85, 0x88, 0x89, 0x88, 0x88, 0x87, 0x84, 0x80, 0x7f, 0x7b, 0x7b, 0x7c, 0x80, 0x80, 0x83, 0x83, 0x85, 0x86, 0x87, 0x87, 0x87, 0x87, 0x87, 0x87, 0x87, 0x87, 0x84, 0x83, 0x83, 0x82, 0x81, 0x81, 0x83, 0x83, 0x86, 0x86, 0x87, 0x87, 0x86, 0x83, 0x80, 0x7a, 0x75, 0x71, 0x6e, 0x6e, 0x6f, 0x72, 0x77, 0x7c, 0x80, 0x81, 0x81, 0x80, 0x7d, 0x75, 0x70, 0x6b, 0x6b, 0x6e, 0x72, 0x78, 0x7d, 0x80, 0x80, 0x83, 0x80, 0x80, 0x80, 0x80 };

		public override void Initialize()
		{
			MixerPort = Device.Resources.GetIOPortWrite(0, 0);
			MixerDataPort = Device.Resources.GetIOPortWrite(0, 1);
			Reset = Device.Resources.GetIOPortWrite(0, 2);
			Read = Device.Resources.GetIOPortRead(0, 6);
			Write = Device.Resources.GetIOPortWrite(0, 8);

			ChannelControl = HAL.GetWriteIOPort(Play16BitSongs ? (ushort)0xD4 : (ushort)0x0A);
			FlipFlop = HAL.GetWriteIOPort(Play16BitSongs ? (ushort)0xD8 : (ushort)0x0C);
			TransferMode = HAL.GetWriteIOPort(Play16BitSongs ? (ushort)0xD6 : (ushort)0x0B);
			PageNumber = HAL.GetWriteIOPort(Play16BitSongs ? (ushort)0x8B : (ushort)0x83);
			Position = HAL.GetWriteIOPort(Play16BitSongs ? (ushort)0xC4 : (ushort)0x02);
			DataLength = HAL.GetWriteIOPort(Play16BitSongs ? (ushort)0xC6 : (ushort)0x03);

			Reset.Write8(1);
			HAL.Pause();
			Reset.Write8(0);

			_ = Read.Read8();

            // Turn the speaker on
            Write.Write8((byte)Options.TurnOn);
            (this as IAudioDevice).SetVolume(0xF); // Max volume
        }

		void IAudioDevice.SetVolume(byte v)
        {
	        MixerPort.Write8((byte)Options.MasterVolume);
	        MixerDataPort.Write8(v);
        }

        void IAudioDevice.Play(ConstrainedPointer data)
        {
            var a = (uint)data.Address;

            // Channel 1
            byte channel = 0x01;

            // Program DMA
            ChannelControl.Write8((byte)(channel + 0x04));
            FlipFlop.Write8(1); // Any value can be written to it
            TransferMode.Write8((byte)(channel + 0x48)); // Single mode
            PageNumber.Write8((byte)(a >> 16 & 0xFF));
            Position.Write8((byte)(a & 0xFF));
            Position.Write8((byte)(a >> 8 & 0xFF));
            DataLength.Write8((byte)(data.Size & 0xFF));
            DataLength.Write8((byte)(data.Size >> 8 & 0xFF));
            ChannelControl.Write8(channel);

            // Program Sound Blaster 16
            Write.Write8((byte)Options.SetTimeConstant);
            Write.Write8(64);

            Write.Write8(0xC0);
            Write.Write8(0);

            Write.Write8((byte)((data.Size - 1) & 0xFF));
            Write.Write8((byte)((data.Size - 1) >> 8 & 0xFF));
        }
	}
}
