using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSoundController : MonoBehaviour
{
    void Start()
    {
        AttackController.AttackEvent += AttackSoundHandler;
    }

    void AttackSoundHandler(AudioSource audioSource) {
        audioSource.Play();
    }
}
