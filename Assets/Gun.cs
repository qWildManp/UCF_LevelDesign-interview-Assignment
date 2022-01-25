using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public AnimationController animationController;
    public playerInteractBehavior playerInteractBehavior;
    public GameObject gunPartical;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootFinish()
    {
        animationController.ShootFinish();
    }

    public void PlayGunSound()
    {
        GetComponent<AudioSource>().Play();
    }
    public void GenerateGunFire()
    {
        gunPartical.GetComponent<ParticleSystem>().Play();
    }
    public void SetCanGenerateBullet()
    {
        playerInteractBehavior.bullectGenerated = false;
    }
}
