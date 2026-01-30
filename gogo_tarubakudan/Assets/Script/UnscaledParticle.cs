using UnityEngine;

public class UnscaledParticle : MonoBehaviour
{
    ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Time.timeScale >= 0f)
        {
            ps.Simulate(Time.unscaledDeltaTime, true, false);
        }
    }
}