using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Nanodogs.Nanobox.Enemy
{
    /// <summary>
    /// Defines a nextbot enemy
    /// </summary>
    public class NbNextbot : MonoBehaviourPun
    {
        public float speed = 5;
        public AudioSource audioSource;
        public AudioClip idleSound;
        public AudioClip deathSound;

        public bool canJump = true;
        public AudioClip jumpSound;
        public float jumpHeight = 5;
        public float jumpTime = 1;

        public float detectionRadius = Mathf.Infinity;

        private NavMeshAgent agent;
        private GameObject closestPlayer;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            audioSource = GetComponent<AudioSource>();

            audioSource.clip = idleSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        private void Update()
        {
            // spherecast
            if (closestPlayer == null)
            {
                Collider[] cols = Physics.OverlapSphere(this.transform.position, detectionRadius);
                if (cols.Length > 0)
                {
                    closestPlayer = cols[0].gameObject;
                }
            }
            if (closestPlayer == null)
            {
                Debug.LogWarning("No closest player found!!");
            }

            agent.destination = closestPlayer.transform.position;
            agent.speed = speed;

            if (canJump)
            {
                if (closestPlayer.transform.position.y > transform.position.y)
                {
                    // decide when to jump
                    float randJumpTimer = Random.Range(1, 6);
                    Invoke(nameof(Jump), randJumpTimer);
                }
            }
        }

        public void Jump()
        {
            // apply upward force, not using physics though
            if (!canJump)
            {
                return;
            }

            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            Vector3.Lerp(transform.position, new Vector3(
                transform.position.x,
                transform.position.y + jumpHeight,
                transform.position.z),
                jumpTime);
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == closestPlayer.gameObject)
            {
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
                
                // kill the player
            }

        }
    }
}
