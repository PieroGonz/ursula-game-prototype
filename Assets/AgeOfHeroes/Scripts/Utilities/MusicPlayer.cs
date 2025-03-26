using UnityEngine;
using System.Collections;
using AgeOfHeroes.ScriptableObjects;
namespace AgeOfHeroes
{
    public class MusicPlayer : MonoBehaviour
    {

        public static MusicPlayer Current;

        public AudioClip[] Musics;

        [HideInInspector]
        public AudioSource MyAudio;

        [SerializeField, Space]
        private Contents m_Contents;
        [SerializeField, Space]
        private DataStorage m_DataStorage;

        void Awake()
        {
            Current = this;
            MyAudio = GetComponent<AudioSource>();
        }

        void Start()
        {
            //StartMusic(0);
            //MyAudio.clip = Musics[0];
            if (m_DataStorage != null)
            {

            }

            Invoke("HandleMute", .1f);
            //HandleMute();

            MyAudio.clip = Musics[Random.Range(0, Musics.Length)];
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void HandleMute()
        {
            if (m_DataStorage.MuteMusic)
            {
                GetComponent<AudioSource>().Stop();
                AudioListener.volume = 0;
            }
            else
            {
                GetComponent<AudioSource>().Play();
                AudioListener.volume = 1;
            }
        }

        public void StopMusic()
        {
            GetComponent<AudioSource>().Stop();
        }

        public void StartMusic(int num)
        {
            MyAudio.Stop();
            MyAudio.clip = Musics[num];
            MyAudio.Play();
        }
    }
}