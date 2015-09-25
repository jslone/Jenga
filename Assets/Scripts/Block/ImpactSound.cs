using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class ImpactSound : MonoBehaviour
{
    public AudioSource impactSound;
    public AudioSource sustainedContactSound;
    public float scaleVolume = 0.1f;
    public float pitchVariance = 0.05f;
    private Dictionary<Collider, float> colliderVolumeContribution = new Dictionary<Collider, float>();

    // Use this for initialization
    void Start()
    {
        sustainedContactSound.pitch = Random.Range(1 - pitchVariance, 1 + pitchVariance);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        float volume = scaleVolume * col.relativeVelocity.magnitude;
        impactSound.volume = volume;
        sustainedContactSound.volume += volume;

        colliderVolumeContribution[col.collider] = volume;

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

        float newVolume = scaleVolume * col.relativeVelocity.magnitude;
        float oldVolume;

        colliderVolumeContribution.TryGetValue(col.collider, out oldVolume);

        sustainedContactSound.volume += newVolume - oldVolume;

        colliderVolumeContribution[col.collider] = newVolume;
    }

    void OnCollisionExit(Collision col)
    {
        sustainedContactSound.Stop();

        sustainedContactSound.volume -= colliderVolumeContribution[col.collider];

        colliderVolumeContribution[col.collider] = 0;
    }

}
