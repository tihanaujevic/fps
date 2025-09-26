
using UnityEngine;

public static class AudioManager
{
    public static void PlaySound(Sounds sound, float volume = 1.0f, float pitch = 1.0f, bool loop = false)
    {
        AudioClip audioClip = Resources.Load<AudioClip>($"SoundEffects/{sound}");

        if (audioClip == null)
        {
            Debug.LogError($"Sound {sound} not found");
            return;
        }

        GameObject soundGameObject = new GameObject(sound.ToString());
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        audioSource.Play();

        if (loop == false)
            Object.Destroy(soundGameObject, audioClip.length);

    }
}

public enum Sounds
{
    Shot
}
