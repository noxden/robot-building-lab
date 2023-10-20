using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
public class RobotSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource robotAudioSource;
    [SerializeField] private AudioClip[] robotSounds;

    private Rigidbody robotRigidbody;
    private bool stopSoundIsPlayed;

    private void Awake()
    {
        robotRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (robotRigidbody.velocity.magnitude == 0) return;
        RobotSoundManagement();
    }

    private void RobotSoundManagement()
    {
        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0 )
        {
            PlayRobotDriveSound();
            stopSoundIsPlayed = false;
        }

        if (Input.GetAxis("Vertical") == 0 && !stopSoundIsPlayed)
        {
            PlayRobotStopSound();
            stopSoundIsPlayed = true;
        }
    }
    
    private void PlayRobotAccelerationSound()
    {
        robotAudioSource.Stop();
        robotAudioSource.PlayOneShot(robotSounds[0]);
    }

    private void PlayRobotDriveSound()
    {
        if (!robotAudioSource.isPlaying)
        {
            robotAudioSource.Stop();
            robotAudioSource.PlayOneShot(robotSounds[1]);
        }
    }

    private void PlayRobotStopSound()
    {
        robotAudioSource.Stop();
        robotAudioSource.PlayOneShot(robotSounds[2]);
    }
}
