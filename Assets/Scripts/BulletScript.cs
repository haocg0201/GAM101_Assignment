using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D Rigidbody2D;
    public float Speed;
    private Vector2 Direction;
    private float lastShotTime = 0f;
    public float timeBetweenShots = 0.3f;

    public AudioClip bulletShot;
    private AudioSource AudioSource;
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame

    private void FixedUpdate() {
        Rigidbody2D.velocity = Direction * Speed;
        if (Time.time - lastShotTime >= timeBetweenShots) {
            AudioSource.PlayOneShot(bulletShot);
            // Cập nhật thời điểm của lần bắn cuối cùng
            lastShotTime = Time.time;
        }
    }

    public void SetDirection(Vector2 direction){
        Direction = direction;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
       JohnMovement john = other.collider.GetComponent<JohnMovement>();
       GruntScript grunt = other.collider.GetComponent<GruntScript>();
       if(john != null)
       {
            john.Hit(1.0f); // dmg = 1
            DestroyBullet();
       }
       if(grunt != null)
       {
        grunt.Hit();
        DestroyBullet();
       }
       DestroyBullet();
    }
}
