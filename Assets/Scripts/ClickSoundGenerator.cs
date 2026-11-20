using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ClickSoundGenerator : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip clickClip;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        clickClip = GenerateClickSound();
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickClip);
    }

    AudioClip GenerateClickSound()
    {
        int sampleRate = 44100;
        float duration = 0.03f; // 30 ms click
        int samples = Mathf.CeilToInt(sampleRate * duration);

        float[] data = new float[samples];

        float frequency = 600f; // click pitch

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)sampleRate;

            // Fast decay envelope
            float envelope = Mathf.Exp(-40f * t);

            // Sine wave + tiny noise for "click feel"
            data[i] =
                Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope
                + Random.Range(-0.02f, 0.02f) * envelope;
        }

        AudioClip clip = AudioClip.Create(
            "ClickSound",
            samples,
            1,
            sampleRate,
            false
        );

        clip.SetData(data, 0);
        return clip;
    }
}
