using UnityEngine;
using UnityEngine.Rendering;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    public bool VFXEnabled { get; private set; } = true;


    [Header("Particles")]
    public ParticleSystem[] allParticles;

    [Header("Post Processing")]
    public Volume postProcessVolume;

    [Header("Fog")]
    public bool controlFog = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetVFX(bool enabled)
    {
        VFXEnabled = enabled;

        foreach (var ps in allParticles)
        {
            if (ps == null) continue;

            if (enabled)
                ps.Play();
            else
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (postProcessVolume != null)
            postProcessVolume.enabled = enabled;

        if (controlFog)
            RenderSettings.fog = enabled;
    }
}
