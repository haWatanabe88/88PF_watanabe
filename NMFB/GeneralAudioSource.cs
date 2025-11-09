using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAudioSource : MonoBehaviour
{
    [SerializeField] AudioSource GeneralAudioSrc;
    [SerializeField] AudioClip HitSE;
    [SerializeField] AudioClip ProtectedSE;
    [SerializeField] AudioClip ShotSE;
    [SerializeField] AudioClip MuscleItemGetSE;
    [SerializeField] AudioClip FoodItemGetSE;
    [SerializeField] AudioClip PowerUpSE;
    [SerializeField] AudioClip LaserPowerUpSE;
    [SerializeField] AudioClip damage_s;
    [SerializeField] AudioClip damage_L;
    [SerializeField] AudioClip AllOutSE;
    [SerializeField] AudioClip ExerciseSE;
    [SerializeField] AudioClip BuzzerSE;
    [SerializeField] AudioClip resultSE;

    [SerializeField] AudioClip PlatinumSE;
    [SerializeField] AudioClip GoldSE;
    [SerializeField] AudioClip SilverSE;
    [SerializeField] AudioClip BronzeSE;

    public static GeneralAudioSource instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        GeneralAudioSrc = GetComponent<AudioSource>();
    }

    public void ShotSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(ShotSE, 0.5f);
    }

    public void HitSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(HitSE, 0.5f);
    }

    public void ProtectedSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(ProtectedSE, 0.5f);
    }

    public void MuscleItemGetSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(MuscleItemGetSE);
    }

    public void FoodItemGetSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(FoodItemGetSE);
    }
    public void PowerUpSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(PowerUpSE);
    }
    public void LaserPowerUpSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(LaserPowerUpSE);
    }
    public void DamageSSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(damage_s);
    }
    public void DamageLSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(damage_L);
    }
    public void AlloutSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(AllOutSE);
    }
    public void ExerciseSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(ExerciseSE);
    }
    public void BuzzerSEFunc()
    {
        GeneralAudioSrc.PlayOneShot(BuzzerSE);
    }
    public void PlatinumMedalSE()
    {
        GeneralAudioSrc.PlayOneShot(PlatinumSE);
    }
    public void GoldMedalSE()
    {
        GeneralAudioSrc.PlayOneShot(GoldSE);
    }
    public void SilverMedalSE()
    {
        GeneralAudioSrc.PlayOneShot(SilverSE);
    }
    public void BronzeMedalSE()
    {
        GeneralAudioSrc.PlayOneShot(BronzeSE);
    }
}
