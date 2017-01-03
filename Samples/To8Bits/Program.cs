using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace To8Bits
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                System.Console.WriteLine("Usage: To8Bits.exe input.wav output.wav");
                System.Environment.Exit(1);
            }
            string file = args[0];
            byte[] data = System.IO.File.ReadAllBytes(file);

            Ratchet.IO.Format.Waveform.Sound<UInt16> sound = Ratchet.IO.Format.Waveform.Read<UInt16>(data);
            Ratchet.IO.Format.Waveform.Sound<byte> _8bitssound = new Ratchet.IO.Format.Waveform.Sound<byte>(8000);

            float sampleRatio = sound.SampleRate / _8bitssound.SampleRate;
            int newLength = (int)((float)sound.Channels[0].Length / sampleRatio);



            for (int c = 0; c < sound.Channels.Count; c++)
            {
                byte[] channelData = new byte[newLength];
                for (int n = 0; n < newLength - 1; n++)
                {
                    int targetSample = (int)((float)n * sampleRatio);
                    int nextTargetSample = (int)((float)(n + 1) * sampleRatio);

                    if (targetSample >= sound.Channels[c].Length) { targetSample = sound.Channels[c].Length - 1; }
                    if (nextTargetSample >= sound.Channels[c].Length) { nextTargetSample = sound.Channels[c].Length - 1; }

                    if ((nextTargetSample - targetSample) <= 1)
                    {
                        channelData[n] = (byte)(sound.Channels[c].Samples[targetSample] >> 8);
                    }
                    else
                    {

                        ulong sum = 0;
                        for (int x = targetSample; x < nextTargetSample; x++) { sum += sound.Channels[c].Samples[x]; }
                        sum /= (ulong)(nextTargetSample - targetSample);

                        channelData[n] = (byte)(sum >> 8);
                    }
                }
                Ratchet.IO.Format.Waveform.Channel<byte> _8bitsChannel = new Ratchet.IO.Format.Waveform.Channel<byte>(channelData);
                _8bitssound.Channels.Add(_8bitsChannel);

            }
            System.IO.Stream stream = System.IO.File.OpenWrite(args[1]);

            _8bitssound.Write(stream);
            stream.Close();
        }
    }
}
