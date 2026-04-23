using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("MUSIC SOURCE")]
    public AudioSource musicSource;
    [Header("MUSIC")]
    [SerializeField] private AudioClip[] musicSound;
    [Space]
    [Header("SOUND SOURCE")]
    public AudioSource[] soundSources;
    public AudioSource[] loopSources;
    public AudioSource m_pitchSource;

    private Queue<AudioSource> _queueSources, _queueLoops;
    private Dictionary<AudioClip, AudioSource> _dicLoop = new();

    [Header("GAME_AUDIO")]
    [SerializeField] private AudioClip buttonSound;
    [SerializeField]
    private AudioClip m_footStepSound, m_windSound, m_fallImpactSound, m_popupSound,
        m_coinCollectSound, m_eggCrackSound, m_divine, m_petUnlocked, m_jumpSound, m_congrats, m_luckySpin, m_notify;
    [SerializeField] protected AudioClip[] punchs;
    public AudioClip swing;

    [Header("FLOOR_IN_AUDIO")]
    [SerializeField] protected AudioClip treadMillSound;
    [SerializeField] protected AudioClip tsunamiSound, fireDamageSound, floorLavaSound, deadSound;

    public List<AudioSource> m_temporalSources = new();

    public AudioClip m_upgradeSound;

#if LIT_MOTION
    private CompositeMotionHandle m_handle = new CompositeMotionHandle();   
#endif
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _queueSources = new Queue<AudioSource>(soundSources);
        _queueLoops = new Queue<AudioSource>(loopSources);
    }
    public void PlayPitch(AudioClip clip, float pitch)
    {
        if (!DataController.Setting_SFX) return;
        m_pitchSource.pitch = pitch;
        m_pitchSource.PlayOneShot(clip);
    }
    public AudioClip m_plateSound;
    public void PlayPlateSound(float pitch)
    {
        PlayPitch(m_plateSound, pitch);
    }


    public void SetMusic(AudioClip clip)
    {
        if (clip == null)
        {
            musicSource.Stop();
            return;
        }

        if (!DataController.Setting_Music)
        {
            return;
        }
        if (musicSource != null && musicSource.clip != clip)
        {
            var volume = musicSource.volume;
#if PRIMETWEEN
            musicSource.DOKill();
            Tween.AudioVolume(musicSource, 0, .5f).OnComplete(() =>
            {
                musicSource.clip = clip;
                musicSource.Play();
                Tween.AudioVolume(musicSource, volume, .5f);
            });
#elif DOTWEEN
			musicSource.DOFade(0, .5f).OnComplete(() =>
			{
		        musicSource.clip = clip;
		        musicSource.Play();
			    musicSource.DOFade(volume, .5f);
			});
#elif LIT_MOTION
	        m_handle.Cancel();
	        m_handle.Clear();
	        LMotion.Create(volume, 0, .5f).WithOnComplete(() => {
		        musicSource.clip = clip;
		        musicSource.Play();
		        LMotion.Create(0f, volume, .5f).BindToVolume(musicSource);
	        }).BindToVolume(musicSource).AddTo(m_handle);
#endif
        }
    }

    private void Start()
    {
        UpdateMute();

    }
    public void AddTemporalSource(AudioSource source)
    {
        if (m_temporalSources.Contains(source)) return;
        m_temporalSources.Add(source);
        source.mute = !DataController.Setting_SFX;
    }
    public void RemoveTemporalSource(AudioSource source)
    {
        if (!m_temporalSources.Contains(source)) return;
        m_temporalSources.Remove(source);
    }
    public void UpdateMute()
    {
        ChangeMusic(!DataController.Setting_Music);
        ChangeSound(!DataController.Setting_SFX);

    }

    private void ChangeMusic(bool isMute)
    {
        if (musicSource != null)
        {
            if (isMute)
            {
                musicSource.Stop();
            }
            else
            {
                musicSource.Play();
            }
        }
    }

    void ChangeSound(bool isMute)
    {
        if (isMute)
        {
            foreach (var sound in soundSources)
            {
                sound.Stop();
            }
            foreach (var sound in loopSources)
            {
                sound.Stop();
            }
            foreach (var sound in m_temporalSources)
            {
                sound.Stop();
            }
        }
        else
        {
            foreach (var sound in m_temporalSources)
            {
                sound.Play();
            }
        }
    }

    public virtual void PlayShot(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            return;
        }
        //this.Log("audio ");
        if (DataController.Setting_SFX)
        {
            //this.Log("Count " + _queueSources.Count);
            var source = _queueSources.Dequeue();
            source.volume = volume;
            source.PlayOneShot(clip);
            _queueSources.Enqueue(source);
        }
    }

    public void PlayLoop(AudioClip clip, float volume = 1f)
    {
        if (clip == null || _dicLoop.ContainsKey(clip))
        {
            return;
        }

        if (DataController.Setting_SFX)
        {

            var loopSource = _queueLoops.Dequeue();
            loopSource.volume = volume;
            loopSource.clip = clip;
            loopSource.Play();
            _dicLoop.Add(clip, loopSource);
        }
    }

    public void StopLoop(AudioClip clip)
    {
        if (!clip || !_dicLoop.ContainsKey(clip))
        {
            return;
        }
        var loopSource = _dicLoop[clip];
        if (loopSource)
        {
            loopSource.Stop();
            _queueLoops.Enqueue(loopSource);
            _dicLoop.Remove(clip);
        }
    }
    public void ChangeBackgroundMusic(int index)
    {
        if (index == -1)
        {
            musicSource.clip = null;
            musicSource.Stop();
            return;
        }


        if (musicSource.clip != musicSound[index])
        {
            var volume = musicSource.volume;

#if PRIMETWEEN
            musicSource.DOKill();
            Tween.AudioVolume(musicSource, 0, .5f).OnComplete(() =>
            {
                musicSource.clip = musicSound[index];
                if (DataController.Setting_Music)
                {
                    musicSource.Play();
                }
                Tween.AudioVolume(musicSource, volume, .5f);
            });
#elif DOTWEEN
            musicSource.DOFade(0, .5f).OnComplete(() =>
            {
                musicSource.clip = musicSound[index];
                musicSource.Play();
                musicSource.DOFade(volume, .5f);
            });
#elif LIT_MOTION
            LMotion.Create(volume, 0, .5f).WithOnComplete(() =>
            {
                musicSource.clip = musicSound[index];
                musicSource.Play();
                LMotion.Create(0f, volume, .5f).BindToVolume(musicSource);
            }).BindToVolume(musicSource);
#endif
        }
        UpdateMute();
    }
    public void PlayButtonSound()
    {
        PlayShot(buttonSound);
    }
    public void PlayPopupSound()
    {
        PlayShot(m_popupSound);
    }
    public void PlayNotifySound()
    {
        PlayShot(m_notify);
    }
    public void SetLoopSoundState(AudioClip clip, bool state, float volume = 1)
    {
        if (state) PlayLoop(clip, volume);
        else StopLoop(clip);
    }
    public void SetFootStepSound(bool state)
    {
        SetLoopSoundState(m_footStepSound, state);
    }

    public void SetWindSound(bool state)
    {
        SetLoopSoundState(m_windSound, state, 2);
    }

    public void PlayImpactSound()
    {
        PlayShot(m_fallImpactSound);
    }
    public void PlayCollectSound()
    {
        PlayShot(m_coinCollectSound);
    }
    public void PlaySwing() => PlayShot(swing);
    public void PlaySlap() => PlayShot(punchs[Random.Range(0, punchs.Length)]);
    public void PlayEggCrackSound()
    {
        PlayShot(m_eggCrackSound);
    }
    public void PlayPetUnlocked()
    {
        PlayShot(m_petUnlocked);
    }
    public void PlayDivine()
    {
        PlayShot(m_divine);
    }
    public void PlayJumpSound()
    {
        PlayShot(m_jumpSound);
    }
    public void PlayCongrats()
    {
        PlayShot(m_congrats);
    }
    public void PlayLuckySpin()
    {
        PlayShot(m_luckySpin);
    }
    public void PlayUpgrade()
    {
        PlayShot(m_upgradeSound, .5f);
    }

    public void ToggleTreadMillSound(bool isActive)
    {
        if (isActive) PlayLoop(treadMillSound);
        else StopLoop(treadMillSound);
    }

    public void ToggleLavaSound(bool isActive)
    {
        if (isActive) PlayLoop(floorLavaSound);
        else StopLoop(floorLavaSound);
    }

    public void ToggleTsunamiSound(bool isActive)
    {
        if (isActive) PlayLoop(tsunamiSound);
        else StopLoop(tsunamiSound);
    }

    public void PlayFireDamageSound()
    {
        PlayShot(fireDamageSound);
    }
    public void PlayDeadSound()
    {
        PlayShot(deadSound);
    }
    public AudioClip platformActiveSound, breakSound, doorSound;
    public void PlayPlatformActiveSound()
    {
        PlayShot(platformActiveSound);
    }
    public void PlayBreakSound()
    {
        PlayShot(breakSound);
    }
    public void PlayDoorSound()
    {
        PlayShot(doorSound);
    }
    [Header("Bot Audio")]
    public AudioClip botCrawlLoop;
    public AudioClip[] botGrowlClips;
    public AudioClip botAttackClip;
    private Coroutine _randomGrowlRoutine;

    public void PlayBotCrawlLoop()
    {
        PlayLoop(botCrawlLoop);
    }

    public void StopBotCrawlLoop()
    {
        StopLoop(botCrawlLoop);

        if (_randomGrowlRoutine != null)
        {
            StopCoroutine(_randomGrowlRoutine);
            _randomGrowlRoutine = null;
        }
    }

    public void PlayBotGrowl()
    {
        if (botGrowlClips == null || botGrowlClips.Length == 0) return;
        PlayShot(botGrowlClips[Random.Range(0, botGrowlClips.Length)]);
    }

    public void PlayBotAttack()
    {
        PlayShot(botAttackClip);
    }
    private bool _isGrowling;
    public void StartRandomGrowl(float intervalMin = 4f, float intervalMax = 6f)
    {
        if (_randomGrowlRoutine != null) return;
        _isGrowling = true;
        _randomGrowlRoutine = StartCoroutine(RandomGrowlRoutine(intervalMin, intervalMax));
    }
    public void StopRandomGrowl()
    {
        _isGrowling = false;

        if (_randomGrowlRoutine != null)
        {
            StopCoroutine(_randomGrowlRoutine);
            _randomGrowlRoutine = null;
        }
    }
    private IEnumerator RandomGrowlRoutine(float min, float max)
    {
        while (_isGrowling)
        {
            yield return new WaitForSeconds(Random.Range(min, max));

            if (!_isGrowling) yield break;

            PlayBotGrowl();
        }
    }
    public void PlaySoundAtPosition(AudioSource tempSource, AudioClip clip, bool loop, float volume = 1f, float minDistance = 1f, float maxDistance = 15f)
    {
        if (clip == null || !DataController.Setting_SFX) { return; }
        tempSource.PlayOneShot(clip);
        tempSource.spatialBlend = 1f;
        tempSource.minDistance = minDistance;
        tempSource.maxDistance = maxDistance;
        tempSource.rolloffMode = AudioRolloffMode.Logarithmic;
        tempSource.loop = loop;
    }
}


public enum MusicType
{
    Home,
    Gameplay
}