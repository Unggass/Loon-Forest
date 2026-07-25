using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header ("Audio Source")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource runSfxSource;

    [Space(10)]

    [Header("Audio Clip")]
    public AudioClip Background;
    public AudioClip ballonHit;
    public AudioClip playerRun;

    private void Start()
    {
        bgmSource.clip = Background;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip, float pitch)
    {
        sfxSource.pitch = pitch;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayRunSFX(float pitch = 1f)
    {
        if (runSfxSource.isPlaying) return;

        runSfxSource.pitch = pitch;
        runSfxSource.Play();
    }

    public void StopRunSFX()
    {
        runSfxSource.Stop();
    }
}
