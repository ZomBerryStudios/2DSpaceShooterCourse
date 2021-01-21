using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _SpeedBoost = 2f;
    private float x;
    private float y;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.25f;
    private float _canFire = -1f;
    [SerializeField]
    private int _life = 3;
    [SerializeField]
    private GameObject _tripleshotPrefab;
    [SerializeField]
    private bool _isShieldActive = false;
    private bool _isTripleshotActive = false;
    private bool _isSpeedUpActive = false;
    [SerializeField]
    private GameObject _shieldOverlayPrefab;
    private GameObject _shield;


    private int _score;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _rightEngineDamagedPrefab;
    [SerializeField]
    private GameObject _leftEngineDamagedPrefab;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserShot;
    

    [SerializeField]
    private GameObject _explosionPrefab;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is null");
        }else
        {
            _audioSource.clip = _laserShot;
        }


        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null");
        }
    }

    
    void Update()
    {
        CalculateMovement();
        FireLaser();
       

    }

     void CalculateMovement()
    {
       
       
            x = Input.GetAxisRaw("Horizontal") * _speed * Time.deltaTime;
            y = Input.GetAxisRaw("Vertical") * _speed * Time.deltaTime;
        
            

        transform.Translate(new Vector3(x, y, 0));

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

     void FireLaser()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            if (_isTripleshotActive == false)
            {
                var Laser = Instantiate(_laserPrefab);
                Laser.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                Laser.transform.rotation = Quaternion.identity;
            }

            if(_isTripleshotActive == true)
            {
                Instantiate(_tripleshotPrefab, transform.position + new Vector3(-1.3f, 0, 0), Quaternion.identity);
                
            }
            _audioSource.Play();
        }
        
    }

    public void OnDamage(int Damage)
    {
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            Destroy(_shield);
            return;
        }
        else
        _life -= Damage;

        _uiManager.UpdateHealth(_life);
        
        if(_life == 2)
        {
            _rightEngineDamagedPrefab.SetActive(true);
        }
        if (_life == 1)
        {
            _leftEngineDamagedPrefab.SetActive(true);
        }
        if (_life < 1)
        {


            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.OnPlayerDeath();
            
            Destroy(this.gameObject,1f);
            
        }
    }

    public void OnTriplePickUp()
    {
        _isTripleshotActive = true;
        StartCoroutine(PickupTimer());

    }
   
    private IEnumerator PickupTimer()
    {
        while (_isTripleshotActive == true)
        {
            yield return new WaitForSeconds(7f);
            _isTripleshotActive = false;
        }
        while (_isSpeedUpActive == true)
        {
            yield return new WaitForSeconds(7f);
            _isSpeedUpActive = false;
            _speed /= _SpeedBoost;
        }
        

    }
  
    public void OnSpeedPickup()
    {
        _isSpeedUpActive = true;
        _speed *= _SpeedBoost;
        StartCoroutine(PickupTimer());
    }
    public void OnShieldPickup()
    {
        _isShieldActive = true;
        ApplyShield();
        
    }

    private void ApplyShield()
    {

        _shield = Instantiate(_shieldOverlayPrefab, transform.position, Quaternion.identity);

        _shield.transform.SetParent(this.transform);

    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

     
    
}
