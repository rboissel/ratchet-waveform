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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.IO.Format
{
    static class WaveformPcm
    {
        static internal Waveform.Sound<byte> Read8BitsPCM(byte[]Data, int ChannelCount, uint SampleCount, uint SampleRate, int Offset)
        {
            byte[][] data = new byte[ChannelCount][];
            for (int n = 0; n < ChannelCount; n++) { data[n] = new byte[SampleCount]; }
            for (uint n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++, Offset++)
                {
                    data[c][n] = Data[Offset];
                }
            }
            Waveform.Sound<byte> sound = new Waveform.Sound<byte>(SampleRate);
            for (int n = 0; n < ChannelCount; n++)
            {
                Waveform.Channel<byte> channel = new Waveform.Channel<byte>();
                channel._Samples = data[n];
                sound.Channels.Add(channel);
            }

            return sound;
        }

        static internal Waveform.Sound<byte> Read16BitsPCMAs8BitsPCM(byte[] Data, int ChannelCount, uint SampleCount, uint SampleRate, int Offset)
        {
            byte[][] data = new byte[ChannelCount][];
            for (int n = 0; n < ChannelCount; n++) { data[n] = new byte[SampleCount]; }
            for (uint n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++, Offset += 2)
                {
                    data[c][n] = Data[Offset + 1];
                }
            }
            Waveform.Sound<byte> sound = new Waveform.Sound<byte>(SampleRate);
            for (int n = 0; n < ChannelCount; n++)
            {
                Waveform.Channel<byte> channel = new Waveform.Channel<byte>();
                channel._Samples = data[n];
                sound.Channels.Add(channel);
            }

            return sound;
        }

        static internal Waveform.Sound<byte> Read32BitsPCMAs8BitsPCM(byte[] Data, int ChannelCount, uint SampleCount, uint SampleRate, int Offset)
        {
            byte[][] data = new byte[ChannelCount][];
            for (int n = 0; n < ChannelCount; n++) { data[n] = new byte[SampleCount]; }
            for (uint n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++, Offset += 4)
                {
                    data[c][n] = Data[Offset + 3];
                }
            }
            Waveform.Sound<byte> sound = new Waveform.Sound<byte>(SampleRate);
            for (int n = 0; n < ChannelCount; n++)
            {
                Waveform.Channel<byte> channel = new Waveform.Channel<byte>();
                channel._Samples = data[n];
                sound.Channels.Add(channel);
            }

            return sound;
        }

        static internal Waveform.Sound<ushort> Read8BitsPCMAs16BitsPCM(byte[] Data, int ChannelCount, uint SampleCount, uint SampleRate, int Offset)
        {
            ushort[][] data = new ushort[ChannelCount][];
            for (int n = 0; n < ChannelCount; n++) { data[n] = new ushort[SampleCount]; }
            for (uint n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++, Offset++)
                {
                    data[c][n] = (ushort)((ushort)Data[Offset] * (ushort)0x100);
                }
            }
            Waveform.Sound<ushort> sound = new Waveform.Sound<ushort>(SampleRate);
            for (int n = 0; n < ChannelCount; n++)
            {
                Waveform.Channel<ushort> channel = new Waveform.Channel<ushort>();
                channel._Samples = data[n];
                sound.Channels.Add(channel);
            }

            return sound;
        }

        static internal Waveform.Sound<ushort> Read16BitsPCM(byte[] Data, int ChannelCount, uint SampleCount, uint SampleRate, int Offset)
        {
            ushort[][] data = new ushort[ChannelCount][];
            for (int n = 0; n < ChannelCount; n++) { data[n] = new ushort[SampleCount]; }
            for (uint n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++, Offset+=2)
                {
                    data[c][n] = (ushort)((ushort)Data[Offset] + (ushort)Data[Offset + 1] * (ushort)0x100);
                }
            }
            Waveform.Sound<ushort> sound = new Waveform.Sound<ushort>(SampleRate);
            for (int n = 0; n < ChannelCount; n++)
            {
                Waveform.Channel<ushort> channel = new Waveform.Channel<ushort>();
                channel._Samples = data[n];
                sound.Channels.Add(channel);
            }

            return sound;
        }

        static internal Waveform.Sound<ushort> Read32BitsPCMAs16BitsPCM(byte[] Data, int ChannelCount, uint SampleCount, uint SampleRate, int Offset)
        {
            ushort[][] data = new ushort[ChannelCount][];
            for (int n = 0; n < ChannelCount; n++) { data[n] = new ushort[SampleCount]; }
            for (uint n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++, Offset += 4)
                {
                    data[c][n] = (ushort)((ushort)Data[Offset + 2] + (ushort)Data[Offset + 3] * 0x100);
                }
            }
            Waveform.Sound<ushort> sound = new Waveform.Sound<ushort>(SampleRate);
            for (int n = 0; n < ChannelCount; n++)
            {
                Waveform.Channel<ushort> channel = new Waveform.Channel<ushort>();
                channel._Samples = data[n];
                sound.Channels.Add(channel);
            }

            return sound;
        }


        static internal Waveform.Sound<uint> Read8BitsPCMAs32BitsPCM(byte[] Data, int ChannelCount, uint SampleCount, uint SampleRate, int Offset)
        {
            uint[][] data = new uint[ChannelCount][];
            for (int n = 0; n < ChannelCount; n++) { data[n] = new uint[SampleCount]; }
            for (uint n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++, Offset++)
                {
                    data[c][n] = Data[Offset];
                }
            }
            Waveform.Sound<uint> sound = new Waveform.Sound<uint>(SampleRate);
            for (int n = 0; n < ChannelCount; n++)
            {
                Waveform.Channel<uint> channel = new Waveform.Channel<uint>();
                channel._Samples = data[n];
                sound.Channels.Add(channel);
            }

            return sound;
        }

        static internal Waveform.Sound<uint> Read16BitsPCMAs32BitsPCM(byte[] Data, int ChannelCount, uint SampleCount, uint SampleRate, int Offset)
        {
            uint[][] data = new uint[ChannelCount][];
            for (int n = 0; n < ChannelCount; n++) { data[n] = new uint[SampleCount]; }
            for (uint n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++, Offset += 2)
                {
                    data[c][n] = (uint)Data[Offset] + (uint)Data[Offset + 1] * (uint)0x100;
                }
            }
            Waveform.Sound<uint> sound = new Waveform.Sound<uint>(SampleRate);
            for (int n = 0; n < ChannelCount; n++)
            {
                Waveform.Channel<uint> channel = new Waveform.Channel<uint>();
                channel._Samples = data[n];
                sound.Channels.Add(channel);
            }

            return sound;
        }

        static internal Waveform.Sound<uint> Read32BitsPCM(byte[] Data, int ChannelCount, uint SampleCount, uint SampleRate, int Offset)
        {
            uint[][] data = new uint[ChannelCount][];
            for (int n = 0; n < ChannelCount; n++) { data[n] = new uint[SampleCount]; }
            for (uint n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++, Offset+=4)
                {
                    data[c][n] = (uint)Data[Offset] + (uint)Data[Offset + 1] * 0x100 + (uint)Data[Offset + 2] * 0x10000 + (uint)Data[Offset + 3] * 0x1000000;
                }
            }
            Waveform.Sound<uint> sound = new Waveform.Sound<uint>(SampleRate);
            for (int n = 0; n < ChannelCount; n++)
            {
                Waveform.Channel<uint> channel = new Waveform.Channel<uint>();
                channel._Samples = data[n];
                sound.Channels.Add(channel);
            }

            return sound;
        }

        static internal void Write8BitsPCM(List<Waveform.Channel<byte>> Channels, System.IO.BinaryWriter Output)
        {
            int ChannelCount = Channels.Count;
            int SampleCount = Channels[0].Length;

            for (int n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++)
                {
                    Output.Write(Channels[c].Samples[n]);
                }
            }
        }

        static internal void Write16BitsPCM(List<Waveform.Channel<ushort>> Channels, System.IO.BinaryWriter Output)
        {
            int ChannelCount = Channels.Count;
            int SampleCount = Channels[0].Length;

            for (int n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++)
                {
                    Output.Write(Channels[c].Samples[n]);
                }
            }
        }

        static internal void Write32BitsPCM(List<Waveform.Channel<uint>> Channels, System.IO.BinaryWriter Output)
        {
            int ChannelCount = Channels.Count;
            int SampleCount = Channels[0].Length;
 
            for (int n = 0; n < SampleCount; n++)
            {
                for (int c = 0; c < ChannelCount; c++)
                {
                    Output.Write(Channels[c].Samples[n]);
                }
            }
        }
    }
}
