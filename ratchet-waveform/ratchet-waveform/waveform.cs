/*                                                                           *
 * Copyright © 2017, Raphaël Boissel                                         *
 * Permission is hereby granted, free of charge, to any person obtaining     *
 * a copy of this software and associated documentation files, to deal in    *
 * the Software without restriction, including without limitation the        *
 * rights to use, copy, modify, merge, publish, distribute, sublicense,      *
 * and/or sell copies of the Software, and to permit persons to whom the     *
 * Software is furnished to do so, subject to the following conditions:      *
 *                                                                           *
 * - The above copyright notice and this permission notice shall be          *
 *   included in all copies or substantial portions of the Software.         *
 * - The Software is provided "as is", without warranty of any kind,         *
 *   express or implied, including but not limited to the warranties of      *
 *   merchantability, fitness for a particular purpose and noninfringement.  *
 *   In no event shall the authors or copyright holders. be liable for any   *
 *   claim, damages or other liability, whether in an action of contract,    *
 *   tort or otherwise, arising from, out of or in connection with the       *
 *   software or the use or other dealings in the Software.                  *
 * - Except as contained in this notice, the name of Raphaël Boissel shall   *
 *   not be used in advertising or otherwise to promote the sale, use or     *
 *   other dealings in this Software without prior written authorization     *
 *   from Raphaël Boissel.                                                   *
 *                                                                           */

using System;
using System.Collections.Generic;

namespace Ratchet.IO.Format
{
    /// <summary>
    /// This class provides a set of feature to manipulate Waveform audio files
    /// </summary>
    public static class Waveform
    {

        public class Channel<T>
        {
            internal T[] _Samples;
            public int Length { get { return _Samples.Length; } }
            public T[] Samples { get { return _Samples; } }
            internal Channel() { }
            public Channel(T[] Samples)
            {
                _Samples = (T[])Samples.Clone();
            }
        }

        public class Sound<T>
        {
            uint _SampleRate = 0;
            public uint SampleRate { get { return _SampleRate; } }
            List<Channel<T>> _Channels = new List<Channel<T>>();
            public List<Channel<T>> Channels { get { return _Channels; } }
            public void Write(System.IO.Stream Stream)
            {
                Waveform.Write<T>(Stream, this);
            }
            public Sound(uint SampleRate)
            {
                if (typeof(T) != typeof(Byte) && typeof(T) != typeof(UInt16) && typeof(T) != typeof(UInt32)) { throw new System.IO.InvalidDataException("Sample type must be: Byte, Uint16, UInt32"); }
                _SampleRate = SampleRate;
            }
        }

        static void Write8BitsSound(System.IO.BinaryWriter output, Sound<byte> Sound)
        {
            uint size = 44 + (uint)Sound.Channels.Count * (uint)Sound.Channels[0].Length - 8;
            output.Write(size);
            output.Write('W'); output.Write('A'); output.Write('V'); output.Write('E');
            output.Write('f'); output.Write('m'); output.Write('t'); output.Write(' ');
            output.Write((uint)16);
            output.Write((ushort)1);
            output.Write((ushort)Sound.Channels.Count);
            output.Write((uint)Sound.SampleRate);
            output.Write((uint)Sound.SampleRate * (uint)Sound.Channels.Count);
            output.Write((ushort)Sound.Channels.Count);
            output.Write((ushort)8);
            output.Write('d'); output.Write('a'); output.Write('t'); output.Write('a');
            output.Write((uint)Sound.Channels[0].Length * (uint)Sound.Channels.Count);
            WaveformPcm.Write8BitsPCM(Sound.Channels, output);
            output.Close();

        }

        static void Write16BitsSound(System.IO.BinaryWriter output, Sound<ushort> Sound)
        {
            uint size = 44 + (uint)Sound.Channels.Count * (uint)Sound.Channels[0].Length * 2 - 8;
            output.Write(size);
            output.Write('W'); output.Write('A'); output.Write('V'); output.Write('E');
            output.Write('f'); output.Write('m'); output.Write('t'); output.Write(' ');
            output.Write((uint)16);
            output.Write((ushort)1);
            output.Write((ushort)Sound.Channels.Count);
            output.Write((uint)Sound.SampleRate);
            output.Write((uint)Sound.SampleRate * (uint)Sound.Channels.Count * 2);
            output.Write((ushort)(Sound.Channels.Count * 2));
            output.Write((ushort)16);
            output.Flush();
            output.Write('d'); output.Write('a'); output.Write('t'); output.Write('a');
            output.Write((uint)Sound.Channels[0].Length * (uint)Sound.Channels.Count * 2);
            WaveformPcm.Write16BitsPCM(Sound.Channels, output);
            output.Close();
        }

        static void Write32BitsSound(System.IO.BinaryWriter output, Sound<uint> Sound)
        {
            uint size = 44 + (uint)Sound.Channels.Count * (uint)Sound.Channels[0].Length * 4 - 8;
            output.Write(size);
            output.Write('W'); output.Write('A'); output.Write('V'); output.Write('E');
            output.Write('f'); output.Write('m'); output.Write('t'); output.Write(' ');
            output.Write((uint)16);
            output.Write((ushort)1);
            output.Write((ushort)Sound.Channels.Count);
            output.Write((uint)Sound.SampleRate);
            output.Write((uint)Sound.SampleRate * (uint)Sound.Channels.Count * 4);
            output.Write((ushort)(Sound.Channels.Count * 4));
            output.Write((ushort)32);
            output.Write('d'); output.Write('a'); output.Write('t'); output.Write('a');
            output.Write((uint)Sound.Channels[0].Length * (uint)Sound.Channels.Count * 4);
            WaveformPcm.Write32BitsPCM(Sound.Channels, output);
            output.Close();

        }

        static void Write<T>(System.IO.Stream Stream, Sound<T> Sound)
        {
            System.IO.BinaryWriter output = new System.IO.BinaryWriter(Stream);
            output.Write('R'); output.Write('I'); output.Write('F'); output.Write('F');
            if (typeof(T) == typeof(Byte)) { Write8BitsSound(output, Sound as Sound<byte>); }
            else if (typeof(T) == typeof(UInt16)) { Write16BitsSound(output, Sound as Sound<ushort>); }
            else if (typeof(T) == typeof(UInt32)) { Write32BitsSound(output, Sound as Sound<uint>); }
        }

        public static Sound<T> Read<T>(byte[] Waveform)
        {
            return Read<T>(Waveform, 0);
        }

        public static Sound<T> Read<T>(byte[] Waveform, int Offset)
        {
            if (typeof(T) != typeof(Byte) && typeof(T) != typeof(UInt16) && typeof(T) != typeof(UInt32)) { throw new System.IO.InvalidDataException("Sample type must be: Byte, Uint16, UInt32"); }
            if (Waveform.Length + Offset < 22) { throw new System.IO.InvalidDataException(); }

            if (Waveform[Offset] == 'R')
            {
                if (Waveform[Offset + 1] != 'I' || Waveform[Offset + 2] != 'F' || Waveform[Offset + 3] != 'F')
                {
                    throw new System.IO.InvalidDataException("Invalid Magik number");
                }
                Offset += 4;
                uint size = BitConverter.ToUInt32(Waveform, Offset);
                Offset += 4;

            }

            if (Waveform[Offset] == 'W')
            {
                if (Waveform[Offset + 1] != 'A' || Waveform[Offset + 2] != 'V' || Waveform[Offset + 3] != 'E')
                {
                    throw new System.IO.InvalidDataException("Invalid Magik number");
                }
                Offset += 4;

            }
            else { throw new System.IO.InvalidDataException("Invalid Magik number"); }

            Offset += 10; // Jmpt to the channel count
            int channelCount = (int)BitConverter.ToUInt16(Waveform, Offset); Offset += 2;
            uint SampleRate = BitConverter.ToUInt32(Waveform, Offset); Offset += 4;
            uint ByteRate = BitConverter.ToUInt32(Waveform, Offset); Offset += 4;
            uint BlockAlign = (uint)BitConverter.ToUInt16(Waveform, Offset); Offset += 2;
            uint BitPerSample = BitConverter.ToUInt16(Waveform, Offset); Offset += 2;
            uint BytePerSample = BitPerSample / 8;
            if (BytePerSample * 8 != BitPerSample) { throw new System.IO.InvalidDataException("Invalid BitPerSample: " + BitPerSample.ToString()); }
            if (BytePerSample != 1 && BytePerSample != 2 && BytePerSample != 4) { throw new System.IO.InvalidDataException("Invalid BitPerSample: " + BitPerSample.ToString()); }

            if (Waveform[Offset] != 'd' || Waveform[Offset + 1] != 'a' || Waveform[Offset + 2] != 't' || Waveform[Offset + 3] != 'a') { throw new System.IO.InvalidDataException("Invalid Data chunk"); }
            Offset += 4;
            uint dataSize = BitConverter.ToUInt32(Waveform, Offset); Offset += 4;
            uint sampleCount = dataSize / ((uint)channelCount * BytePerSample);
            if (sampleCount * BytePerSample * channelCount + Offset > Waveform.Length) { throw new System.IO.InvalidDataException("Truncated file"); }

            if (typeof(T) == typeof(Byte))
            {
                switch (BytePerSample)
                {
                    case 1: return WaveformPcm.Read8BitsPCM(Waveform, channelCount, sampleCount, SampleRate, Offset) as Sound<T>;
                    case 2: return WaveformPcm.Read16BitsPCMAs8BitsPCM(Waveform, channelCount, sampleCount, SampleRate, Offset) as Sound<T>;
                    case 4: return WaveformPcm.Read32BitsPCMAs8BitsPCM(Waveform, channelCount, sampleCount, SampleRate, Offset) as Sound<T>;
                }
            }
            else if (typeof(T) == typeof(UInt16))
            {
                switch (BytePerSample)
                {
                    case 1: return WaveformPcm.Read8BitsPCMAs16BitsPCM(Waveform, channelCount, sampleCount, SampleRate, Offset) as Sound<T>;
                    case 2: return WaveformPcm.Read16BitsPCM(Waveform, channelCount, sampleCount, SampleRate, Offset) as Sound<T>;
                    case 4: return WaveformPcm.Read32BitsPCMAs16BitsPCM(Waveform, channelCount, sampleCount, SampleRate, Offset) as Sound<T>;
                }
            }
            else if (typeof(T) == typeof(UInt32))
            {
                switch (BytePerSample)
                {
                    case 1: return WaveformPcm.Read8BitsPCMAs32BitsPCM(Waveform, channelCount, sampleCount, SampleRate, Offset) as Sound<T>;
                    case 2: return WaveformPcm.Read16BitsPCMAs32BitsPCM(Waveform, channelCount, sampleCount, SampleRate, Offset) as Sound<T>;
                    case 4: return WaveformPcm.Read32BitsPCM(Waveform, channelCount, sampleCount, SampleRate, Offset) as Sound<T>;
                }
            }


            return null;
        }
    }
}
