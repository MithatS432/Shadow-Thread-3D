using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float dayDuration = 300f;

    [Header("Skyboxes")]
    public Material daySkybox;
    public Material nightSkybox;

    [Header("Light")]
    public Light directionalLight;

    [Header("Fog Settings (Night Only)")]
    public Color nightFogColor = new Color(0.15f, 0.15f, 0.18f);
    public float nightFogDensity = 0.015f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip wolfClip;
    public AudioClip dayClip;
    public AudioClip nightClip;

    float timeOfDay = 0f;
    bool isNight = false;
    bool daySoundPlayed = false;
    Coroutine nightRoutine;

    void Start()
    {
        EnterDay();
    }

    void Update()
    {
        timeOfDay += Time.deltaTime;
        float normalizedTime = timeOfDay / dayDuration;

        directionalLight.transform.rotation =
            Quaternion.Euler(normalizedTime * 360f - 90f, 170f, 0f);

        if (normalizedTime >= 0.5f && !isNight)
            EnterNight();

        if (timeOfDay >= dayDuration)
        {
            timeOfDay = 0f;
            EnterDay();
        }
    }

    void EnterDay()
    {
        isNight = false;
        daySoundPlayed = false;

        if (nightRoutine != null)
            StopCoroutine(nightRoutine);

        audioSource.Stop();
        audioSource.loop = false;

        RenderSettings.skybox = daySkybox;
        DynamicGI.UpdateEnvironment();
        RenderSettings.fog = false;

        if (dayClip != null && !daySoundPlayed)
        {
            audioSource.clip = dayClip;
            audioSource.Play();
            daySoundPlayed = true;
        }
    }

    void EnterNight()
    {
        isNight = true;

        RenderSettings.skybox = nightSkybox;
        DynamicGI.UpdateEnvironment();

        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogColor = nightFogColor;
        RenderSettings.fogDensity = nightFogDensity;

        nightRoutine = StartCoroutine(NightAudioSequence());
    }

    IEnumerator NightAudioSequence()
    {
        audioSource.loop = false;

        if (wolfClip != null)
        {
            audioSource.clip = wolfClip;
            audioSource.Play();
            yield return new WaitForSeconds(wolfClip.length);
        }

        if (nightClip != null)
        {
            audioSource.clip = nightClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
