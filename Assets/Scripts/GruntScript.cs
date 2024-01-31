using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntScript : MonoBehaviour
{
    public GameObject BulletPrefabs;
    private float LastShoot;
    public GameObject John;
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private int Health = 5;
    public AudioClip ExploreSound;
    private AudioSource AudioSource;
    // Start is called before the first frame update
    private void Start() {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if(John == null) return ;
        Vector3 direction = John.transform.position - transform.position;
        if(direction.x >= 0.0f) transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        else transform.localScale = new Vector3(-1.0f,1.0f,1.0f);

        float distance = Mathf.Abs(John.transform.position.x - transform.position.x);
        if ( distance < 4.0f && Time.time > LastShoot + 0.40f)
        {
            Shoot();
            LastShoot = Time.time;
        }

        if ( distance < 4.0f)
        {
        Animator.SetBool("catchEnemy",true);
        } else Animator.SetBool("catchEnemy",false);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "death ray"){
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        Vector3 direction;
        if(transform.localScale.x == 1.0f) direction = Vector3.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefabs,transform.position + direction  + new Vector3(0,-1,0)  * 0.1f,Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    public void Hit()
    {
        Health -= 1;
        if(Health == 0) {
            StartCoroutine(PlaySoundThenDestroy());
        }
    }

    IEnumerator PlaySoundThenDestroy()
    {
        AudioSource.PlayOneShot(ExploreSound);

        // chờ âm thanh kết thúc này
        yield return new WaitForSeconds(ExploreSound.length);

        // Xong mới xóa mệt ghia
        Destroy(gameObject);
    }
}
