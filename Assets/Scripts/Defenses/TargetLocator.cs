using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 20f;
    Transform target;

    Animator m_Animator;

    private AudioSource audioSource;
    public AudioClip shootingNoise;

    private void Start() {
        m_Animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        // Check enemies exist
        if (enemies.Length < 1)
        {
            return;
        }
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(Enemy enemy in enemies)
        {
            // Ignore enemy if it is still being purchased
            if(!enemy.canBeShot)
            {
                return;
            }
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if(targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }
    void AimWeapon()
    {
        // Check target exists
        if(target == null)
        {
            return;
        }

        float targetDistance = Vector3.Distance(transform.position, target.position);

        weapon.LookAt(target);
        
        if(targetDistance < range){
            Attack(true);
        }else{
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
        if (isActive)
        {
            // For towers with animators
            if(m_Animator != null)
            {
                m_Animator.SetBool("isShooting", true);
            }

            // Play audio once
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(shootingNoise);
            }
        }
    }
}
