using NAudio.Wave;
using SFML.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam2020.View.Sounds
{
    public class SoundManager
    {
        private Dictionary<string, WaveOutEvent> mappingPathWaveOutEvent;
        private WaveOutEvent currentMusic;
        private string currentMusicPath;

        private Dictionary<string, LoopAudioFileReader> soundsDictionary;

        private Dictionary<string, LoopAudioFileReader> musicsDictionary;

        public SoundManager()
        {
            this.mappingPathWaveOutEvent = new Dictionary<string, WaveOutEvent>();

            this.soundsDictionary = new Dictionary<string, LoopAudioFileReader>();

            this.musicsDictionary = new Dictionary<string, LoopAudioFileReader>();

            this.currentMusic = null;
            this.currentMusicPath = string.Empty;
        }

        public void PlaySound(string path, bool loop)
        {
            LoopAudioFileReader reader = this.soundsDictionary[path];
            reader.CurrentTime = new TimeSpan(0);

            WaveOutEvent outputDevice = new WaveOutEvent();
            
            if (this.mappingPathWaveOutEvent.ContainsKey(path))
            {
                this.StopSound(path);
            }

            reader.EnableLooping = loop;

            this.mappingPathWaveOutEvent.Add(path, outputDevice);

            outputDevice.Init(reader);

            outputDevice.Volume = 0.75f;

            outputDevice.Play();
        }

        public void StopAllSounds()
        {
            /*if(this.currentMusic != null)
            {
                this.currentMusic.Stop();
                this.currentMusic.Dispose();
            }
            this.currentMusic = null;*/

            foreach(WaveOutEvent device in this.mappingPathWaveOutEvent.Values)
            {
                device.Stop();
                device.Dispose();
            }

            this.mappingPathWaveOutEvent.Clear();
        }

        public void StopSound(string path)
        {
            if (this.mappingPathWaveOutEvent.ContainsKey(path))
            {
                WaveOutEvent device = this.mappingPathWaveOutEvent[path];

                device.Stop();
                device.Dispose();

                this.mappingPathWaveOutEvent.Remove(path);
            }
        }

        public void SetVolumeMusic(float volume)
        {
            if (this.currentMusic != null)
            {
                /*this.currentMusic.Volume = volume;
                this.currentMusic.Play();*/
            }
        }

        public void PlayMusic(string path)
        {
            if (this.currentMusic == null || path.Equals(this.currentMusicPath) == false)
            {
                if (this.currentMusic != null)
                {
                    this.currentMusic.Stop();
                    this.currentMusic.Dispose();
                }

                LoopAudioFileReader reader = this.musicsDictionary[path];
                WaveOutEvent outputDevice = new WaveOutEvent();
                outputDevice.Volume = 0.75f;

                reader.EnableLooping = true;

                this.currentMusic = outputDevice;
                this.currentMusicPath = path;

                outputDevice.Init(reader);

                outputDevice.Play();
            }
        }

        public void LoadSounds(HashSet<string> soundsToLoad)
        {
            HashSet<string> soundNotToLoad = new HashSet<string>();
            List<string> soundToRemove = new List<string>();

            foreach (KeyValuePair<string, LoopAudioFileReader> keyValuePair in this.soundsDictionary)
            {
                if (soundsToLoad.Contains(keyValuePair.Key) == false)
                {
                    keyValuePair.Value.Dispose();

                    soundToRemove.Add(keyValuePair.Key);
                }
                else
                {
                    soundNotToLoad.Add(keyValuePair.Key);
                }
            }

            foreach (string keyToRemove in soundToRemove)
            {
                this.soundsDictionary.Remove(keyToRemove);
            }

            foreach (string pathToLoad in soundsToLoad)
            {
                if (soundNotToLoad.Contains(pathToLoad) == false)
                {
                    this.soundsDictionary.Add(pathToLoad, new LoopAudioFileReader(pathToLoad));
                }
            }
        }

        public void LoadMusics(HashSet<string> musicsToLoad)
        {
            HashSet<string> musicNotToLoad = new HashSet<string>();
            List<string> musicToRemove = new List<string>();

            foreach (KeyValuePair<string, LoopAudioFileReader> keyValuePair in this.musicsDictionary)
            {
                if (musicsToLoad.Contains(keyValuePair.Key) == false)
                {
                    keyValuePair.Value.Dispose();

                    musicToRemove.Add(keyValuePair.Key);
                }
                else
                {
                    musicNotToLoad.Add(keyValuePair.Key);
                }
            }

            foreach (string keyToRemove in musicToRemove)
            {
                if(this.currentMusicPath.Equals(string.Empty) != false && this.currentMusicPath.Equals(keyToRemove))
                {
                    this.currentMusic.Stop();
                    this.currentMusic.Dispose();

                    this.currentMusic = null;
                    this.currentMusicPath = string.Empty;
                }

                this.musicsDictionary.Remove(keyToRemove);
            }

            foreach (string pathToLoad in musicsToLoad)
            {
                if (musicNotToLoad.Contains(pathToLoad) == false)
                {
                    this.musicsDictionary.Add(pathToLoad, new LoopAudioFileReader(pathToLoad));
                }
            }
        }
    }

    /// <summary>
    /// Stream for looping playback
    /// </summary>
    public class LoopAudioFileReader : AudioFileReader
    {

        /// <summary>
        /// Creates a new Loop stream
        /// </summary>
        /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
        /// or else we will not loop to the start again.</param>
        public LoopAudioFileReader(string sourceStream):base(sourceStream)
        {
            this.EnableLooping = false;
        }

        /// <summary>
        /// Use this to turn looping on or off
        /// </summary>
        public bool EnableLooping { get; set; }


        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = base.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (this.Position == 0 || !EnableLooping)
                    {
                        // something wrong with the source stream
                        break;
                    }
                    // loop
                    base.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
