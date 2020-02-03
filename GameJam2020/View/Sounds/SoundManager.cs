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

        private Dictionary<string, AudioFileReader> soundsDictionary;

        private Dictionary<string, AudioFileReader> musicsDictionary;

        public SoundManager()
        {
            this.mappingPathWaveOutEvent = new Dictionary<string, WaveOutEvent>();

            this.soundsDictionary = new Dictionary<string, AudioFileReader>();

            this.musicsDictionary = new Dictionary<string, AudioFileReader>();
        }

        public void PlaySound(string path)
        {
            AudioFileReader reader = this.soundsDictionary[path];
            reader.CurrentTime = new TimeSpan(0);

            WaveOutEvent outputDevice = new WaveOutEvent();
            
            if (this.mappingPathWaveOutEvent.ContainsKey(path))
            {
                this.StopSound(path);
            }

            outputDevice.Init(reader);

            outputDevice.Volume = 0.75f;

            outputDevice.Play();
        }

        public void StopAllSounds()
        {
            if(this.currentMusic != null)
            {
                this.currentMusic.Stop();
                this.currentMusic.Dispose();
            }
            this.currentMusic = null;

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

        public void PlayMusic(string path)
        {
            if(this.currentMusic != null)
            {
                this.currentMusic.Stop();
                this.currentMusic.Dispose();
            }

            AudioFileReader reader = this.musicsDictionary[path];
            WaveOutEvent outputDevice = new WaveOutEvent();
            this.currentMusic = outputDevice;

            outputDevice.Init(reader);

            outputDevice.Play();
        }

        public void LoadSounds(HashSet<string> soundsToLoad)
        {
            HashSet<string> soundNotToLoad = new HashSet<string>();
            List<string> soundToRemove = new List<string>();

            foreach (KeyValuePair<string, AudioFileReader> keyValuePair in this.soundsDictionary)
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
                    this.soundsDictionary.Add(pathToLoad, new AudioFileReader(pathToLoad));
                }
            }
        }

        public void LoadMusics(HashSet<string> musicsToLoad)
        {
            HashSet<string> musicNotToLoad = new HashSet<string>();
            List<string> musicToRemove = new List<string>();

            foreach (KeyValuePair<string, AudioFileReader> keyValuePair in this.musicsDictionary)
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
                this.musicsDictionary.Remove(keyToRemove);
            }

            foreach (string pathToLoad in musicsToLoad)
            {
                if (musicNotToLoad.Contains(pathToLoad) == false)
                {
                    this.musicsDictionary.Add(pathToLoad, new AudioFileReader(pathToLoad));
                }
            }
        }
    }
}
