﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace NAudioTests
{
    [TestFixture]
    public class AudioClientTests
    {
        [Test]
        public void CanGetMixFormat()
        {
            // don't need to initialize before asking for MixFormat
            Console.WriteLine("Mix Format: {0}", GetAudioClient().MixFormat);
        }

        [Test]
        public void CanInitializeInSharedMode()
        {
            InitializeClient(AudioClientShareMode.Shared);        
        }

        [Test]
        public void CanInitializeInExclusiveMode()
        {
            InitializeClient(AudioClientShareMode.Exclusive);
        }

        [Test]
        public void CanGetAudioRenderClient()
        {
            Assert.IsNotNull(InitializeClient(AudioClientShareMode.Shared).AudioRenderClient);
        }


        [Test]
        public void CanGetBufferSize()
        {
            Console.WriteLine("Buffer Size: {0}", InitializeClient(AudioClientShareMode.Shared).BufferSize);
        }

        [Test]
        public void CanGetCurrentPadding()
        {
            Console.WriteLine("CurrentPadding: {0}", InitializeClient(AudioClientShareMode.Shared).CurrentPadding);
        }

        [Test]
        public void CanGetDefaultDevicePeriod()
        {
            // should not need initialization
            Console.WriteLine("DefaultDevicePeriod: {0}", GetAudioClient().DefaultDevicePeriod);
        }

        [Test]
        public void CanGetMinimumDevicePeriod()
        {
            // should not need initialization
            Console.WriteLine("MinimumDevicePeriod: {0}", GetAudioClient().MinimumDevicePeriod);
        }

        [Test]
        public void DefaultFormatIsSupportedInSharedMode()
        {
            AudioClient client = GetAudioClient();
            WaveFormat defaultFormat = client.MixFormat;
            Assert.IsTrue(client.IsFormatSupported(AudioClientShareMode.Shared, defaultFormat), "Is Format Supported");
        }

        [Test]
        public void DefaultFormatIsSupportedInExclusiveMode()
        {
            AudioClient client = GetAudioClient();
            WaveFormat defaultFormat = client.MixFormat;
            Assert.IsTrue(client.IsFormatSupported(AudioClientShareMode.Exclusive, defaultFormat), "Is Format Supported");
        }


        [Test]
        public void CanRequestIfFormatIsSupportedExtensible44100SharedMode()
        {
            WaveFormatExtensible desiredFormat = new WaveFormatExtensible(44100, 32, 2);
            Console.Write(desiredFormat);
            GetAudioClient().IsFormatSupported(AudioClientShareMode.Shared, desiredFormat);
        }

        [Test]
        public void CanRequestIfFormatIsSupportedExtensible44100ExclusiveMode()
        {
            WaveFormatExtensible desiredFormat = new WaveFormatExtensible(44100, 32, 2);
            Console.Write(desiredFormat);
            GetAudioClient().IsFormatSupported(AudioClientShareMode.Exclusive, desiredFormat);
        }


        [Test]
        public void CanRequestIfFormatIsSupportedExtensible48000()
        {
            WaveFormatExtensible desiredFormat = new WaveFormatExtensible(48000, 32, 2);
            Console.Write(desiredFormat);
            GetAudioClient().IsFormatSupported(AudioClientShareMode.Shared, desiredFormat);
        }

        [Test]
        public void CanRequestIfFormatIsSupportedExtensible48000_16bit()
        {
            WaveFormatExtensible desiredFormat = new WaveFormatExtensible(48000, 16, 2);
            Console.Write(desiredFormat);
            GetAudioClient().IsFormatSupported(AudioClientShareMode.Shared, desiredFormat);
        }

        


        [Test]
        public void CanRequestIfFormatIsSupportedPCMStereo()
        {
            GetAudioClient().IsFormatSupported(AudioClientShareMode.Shared, new WaveFormat(44100, 16, 2));
        }

        [Test]
        public void CanRequestIfFormatIsSupported8KHzMono()
        {
            GetAudioClient().IsFormatSupported(AudioClientShareMode.Shared, new WaveFormat(8000, 16, 1));
        }

        [Test]
        public void CanRequest48kHz16BitStereo()
        {
            GetAudioClient().IsFormatSupported(AudioClientShareMode.Shared, new WaveFormat(48000, 16, 2));

        }

        [Test]
        public void CanRequest48kHz16BitMono()
        {

            GetAudioClient().IsFormatSupported(AudioClientShareMode.Shared, new WaveFormat(48000, 16, 1));

        }

        [Test]
        public void CanRequestIfFormatIsSupportedIeee()
        {
            GetAudioClient().IsFormatSupported(AudioClientShareMode.Shared, WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));
        }

        [Test]
        public void CanPopulateABuffer()
        {
            AudioClient audioClient = InitializeClient(AudioClientShareMode.Shared);
            AudioRenderClient renderClient = audioClient.AudioRenderClient;
            int bufferFrameCount = audioClient.BufferSize;
            IntPtr buffer = renderClient.GetBuffer(bufferFrameCount);
            // TODO put some stuff in
            // will tell it it has a silent buffer
            renderClient.ReleaseBuffer(bufferFrameCount, AudioClientBufferFlags.Silent);

        }

        private AudioClient InitializeClient(AudioClientShareMode shareMode)
        {
            AudioClient audioClient = GetAudioClient();
            WaveFormat waveFormat = audioClient.MixFormat;
            long refTimesPerSecond = 10000000;
            audioClient.Initialize(shareMode,
                AudioClientStreamFlags.None,
                refTimesPerSecond,
                0,
                waveFormat,
                Guid.Empty);
            return audioClient;
        }

        private AudioClient GetAudioClient()
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDevice defaultAudioEndpoint = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            AudioClient audioClient = defaultAudioEndpoint.AudioClient;
            Assert.IsNotNull(audioClient);
            return audioClient;
        }
    
    }
}
