using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBehavior : MonoBehaviour
{
    
    
    [SerializeField]
    private float _enemySpeed = 2.4f;
    [SerializeField]
    private int _enemyDamage = 1;
    [SerializeField]
    private Player player;
    private int points;
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _enemyLaser;

    private float _fireRate = 3.0f;
    private float _canFire = 1;
    


    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        points = Random.Range(5, 10);
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is null");
        }

        if (_anim == null)
        {
            Debug.LogError("Animator is missing");
        }
    }
    public void Update()
    {
        EnemyMovement();
        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(8f, 10f);
            _canFire = Time.time + _fireRate;
            var enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            
            LaserBehavior[] laser = enemyLaser.GetComponentsInChildren<LaserBehavior>();
            
            for(int i = 0; i < laser.Length; i++)
            {
                laser[i].AssignEnemyLaser();
            }
            
        }

    }


   
    public void EnemyMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if(this.transform.position.y <= -7)
        {
            var randomXPos = Random.Range(-9, 9);
            this.transform.position = new Vector3(randomXPos, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Laser"))
        {
            _anim.SetTrigger("OnEnemyDeath");
            
            GetComponent<BoxCollider2D>().enabled = false;
            _enemySpeed = 0;
            Destroy(other.gameObject);
           
            if (player != null)
            {
                player.AddScore(points);
            }
            _audioSource.Play();
            Destroy(this.gameObject, 2.5f);
            
        }
        if(other.CompareTag("Player"))
        {
            _anim.SetTrigger("OnEnemyDeath");

            if (player != null)
            {
                player.OnDamage(_enemyDamage);
                
                GetComponent<BoxCollider2D>().enabled = false;
                _enemySpeed = 0;
                _audioSource.Play();
                Destroy(this.gameObject,2.5f);
            }
            
            
        }


    }
   
}
