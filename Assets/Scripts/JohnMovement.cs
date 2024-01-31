using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JohnMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Hozirontal;
    public float Speed;
    public float JumpForce;
    private bool Grounded;
    public GameObject BulletPrefabs;
    private float LastShoot;
    private int Health = 20;
    public AudioClip HurtSound;
    public AudioClip ExploreSound;
    public AudioClip JumpSound;
    private AudioSource AudioSource;
    public HPProcess hPProcess;
    public float hpNow, hpMax;
    public TextMeshProUGUI TextMeshProUGUI;
    private int countBarrel = 0;
    public AudioClip missionFailed;
    public GameObject panelRestartGame;
    public GameObject panelVictoryGame;
    public GameObject panelPauseGame;
    void Start()
    {
        Time.timeScale = 1;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        // panelRestartGame.SetActive(false);
        // panelVictoryGame.SetActive(false);
        // panelPauseGame.SetActive(false);
        hpMax = Health;
        hpNow = hpMax;
    }

    // Update is called once per frame
    void Update()
    {
        Hozirontal = Input.GetAxisRaw("Horizontal");
        Animator.SetBool("isRunning", Hozirontal != 0.0f);

        if (Hozirontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Hozirontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Debug.DrawRay(transform.position, Vector3.down * 0.6f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.6f))
        { // 
            Grounded = true;
            // Debug.Log("Grounded");
        }
        else
        {
            Grounded = false;
            // Debug.Log("Above Grounded");
        }

        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0) && Time.time > LastShoot + 0.20f)
        {
            Shoot();
            LastShoot = Time.time;
        }
        else if (Input.GetMouseButton(0) && Time.time > LastShoot + 0.15f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Red Barrel")
        {
            countBarrel++;
            TextMeshProUGUI.SetText("" + countBarrel);
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "Tale Gate")
        {
            Time.timeScale = 0;
            panelVictoryGame.SetActive(true);
        }

        if(other.gameObject.tag == "skullpanel")
        {
            Hit(5);
            AudioSource.PlayOneShot(ExploreSound);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "death ray"){
            AudioSource.PlayOneShot(HurtSound);
            GameOver();
        }
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        AudioSource.PlayOneShot(JumpSound);
        Time.timeScale = 1;
    }

    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector3.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefabs, transform.position + direction + new Vector3(0, -1, 0) * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Hozirontal * Speed, Rigidbody2D.velocity.y);
        // if(Rigidbody2D.velocity.x != 0){
        //     StartCoroutine(RunningSoundMatching());
        // }
    }

    // IEnumerator RunningSoundMatching(){
    //     AudioSource.PlayOneShot(runninSound);
    //     yield return new WaitForSeconds(runninSound.length + 5.0f);
    // }

    public void Hit(float damage)
    {
        hpNow -= damage;
        hPProcess.updateHPProcess(hpNow, hpMax);
        AudioSource.PlayOneShot(HurtSound);
        if (hpNow <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        hPProcess.updateHPProcess(0, hpMax);
        panelRestartGame.SetActive(true);
        // StartCoroutine(PlayAudioWhenGameOver());
    }

    // IEnumerator PlayAudioWhenGameOver()
    // {
    //     AudioSource.PlayOneShot(missionFailed);
    //     yield return new WaitForSeconds(missionFailed.length);
    // }

    public void RestartGame()
    {
        Debug.Log("RestartGame called!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Debug.Log("NextLevel called!");
        SceneManager.LoadScene("LV2");
    }

    public void PauseGame()
    {
        Debug.Log("Pause Game called!");
        Time.timeScale = 0;
        panelPauseGame.SetActive(true);
    }

    public void ContinuteGame()
    {
        Debug.Log("Continus called!");
        Time.timeScale = 1;
        panelPauseGame.SetActive(false);
    }
}