using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class ImpactSound : MonoBehaviour
{
    public AudioSource impactSound;
    public AudioSource sustainedContactSound;
    public float scaleVolume = 0.1f;
    public float scalePitch = 0.1f;
    public float minPitch = 0.5f;
    public float maxPitch = 2.0f;

    private float _pitch;
    private float sustainedContactPitch
    {
        get
        {
            return _pitch;
        }
        set
        {
            _pitch = value;
            sustainedContactSound.pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
        }
    }

    struct SoundSettings
    {
        public float volume;
        public float pitch;
    }

    private Dictionary<Collider, SoundSettings> colliderVolumeContribution = new Dictionary<Collider, SoundSettings>();

    // Use this for initialization
    void Start()
    {
        _pitch = sustainedContactSound.pitch;
    }

    // Update is called once per frame
    void Update()
    {

    }

    SoundSettings createSettings(float magnitude)
    {
        SoundSettings settings;
        settings.volume = scaleVolume * magnitude;
        settings.pitch = scalePitch * magnitude;

        return settings;
    }

    void OnCollisionEnter(Collision col)
    {
        SoundSettings settings = createSettings(col.relativeVelocity.magnitude);
        
        impactSound.volume = settings.volume;
        
        sustainedContactSound.volume += settings.volume;
        sustainedContactPitch += settings.pitch;

        colliderVolumeContribution[col.collider] = settings;

        impactSound.Play();

        sustainedContactSound.time = Random.Range(0, sustainedContactSound.clip.length * 0.999f);
        sustainedContactSound.Play();
    }

    void OnCollisionStay(Collision col)
    {
        if (!sustainedContactSound.isPlaying)
        {
            sustainedContactSound.Play();
            sustainedContactSound.time = Random.Range(0.0f, sustainedContactSound.clip.length * 0.999f);
        }

        SoundSettings newSettings = createSettings(col.relativeVelocity.magnitude);
        SoundSettings oldSettings;

        colliderVolumeContribution.TryGetValue(col.collider, out oldSettings);

        sustainedContactSound.volume += newSettings.volume - oldSettings.volume;
        sustainedContactPitch += newSettings.pitch - oldSettings.pitch;

        colliderVolumeContribution[col.collider] = newSettings;
    }

    void OnCollisionExit(Collision col)
    {
        sustainedContactSound.Stop();

        SoundSettings oldSettings = colliderVolumeContribution[col.collider];
        sustainedContactSound.volume -= oldSettings.volume;
        sustainedContactPitch -= oldSettings.pitch;

        colliderVolumeContribution.Remove(col.collider);
    }

}
