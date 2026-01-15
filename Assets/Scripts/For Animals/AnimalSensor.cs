using UnityEngine;

public class AnimalSensor : MonoBehaviour
{
    private AnimalStats stats;
    private Transform threat;

    void Awake()
    {
        stats = GetComponent<AnimalStats>();
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
            threat = go.transform;
    }

    public bool ThreatDetected()
    {
        return Vector3.Distance(transform.position, threat.position)
               <= stats.DetectionRange;
    }

    public bool LostThreat()
    {
        return Vector3.Distance(transform.position, threat.position)
               >= stats.LoseRange;
    }

    public Vector3 ThreatPosition()
    {
        return threat.position;
    }
}
