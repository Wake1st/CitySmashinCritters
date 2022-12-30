using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSoundController : MonoBehaviour
{
    private AudioSource soundEffect;

    void Start()
    {
        soundEffect = this.GetComponent<AudioSource>();

        AttackController.AttackEvent += AttackSoundHandler;
    }

    void AttackSoundHandler() {
        soundEffect.Play();
    }
}
