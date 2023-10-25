using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    //public or private reference
    //data type (int, float, bool, string)
    // every variable has a name
    //Optional value assigned
    [SerializeField]
    private float _speed = 3.5f; //f is float and required
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _LaserPrefab;
    [SerializeField]
    private GameObject _TripleShotPreFab;
    [SerializeField]
    private GameObject _HealthPreFab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isShieldsActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;
    [SerializeField] 
    private int _currentAmmo;
    [SerializeField]
    private int _maxAmmo;
    [SerializeField] 
    private bool _hasAmmo = true;
    [SerializeField]
    private TMP_Text _ammoDisplay;
    [SerializeField]
    private TMP_Text _Out_Of_Ammo;

    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _LaserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
    private float horizontalInput; // move left right
    private float verticalInput; // move up and down
    [SerializeField]
    private GameObject _shieldStregthGO;
    [SerializeField]
    private GameObject _shieldStregthTextGo;
    private GameObject _playerShieldGO;
    [SerializeField]
    private int _shieldPower = 0;
    [SerializeField]
    private bool _ShieldsActive = false;
    [SerializeField]
    private GameObject _secondaryPreFab;
    [SerializeField]
    private bool _isSecondaryActive = false;
    [SerializeField]
    private UnityEngine.UI.Slider _thrustSlider;
    bool _canThrust = true;
    [SerializeField]
    float _refillThrusterSpeed = 0.1f;

    [SerializeField]
    private float _powerupThrustersWaitTimeLimit = 3.0f;
    [SerializeField]
    private float _thrusterChargeLevelMax = 10.0f;
    [SerializeField]
    private float _thrusterChargeLevel;
    [SerializeField]
    private float _changeDecreaseThrusterChargeBy = 1.5f;
    [SerializeField]
    private float _changeIncreaseThrusterChargeBy = 0.01f;
    [SerializeField]
    private bool _canUseThrusters = true;
    [SerializeField]
    private bool _thrustersInUse = false;
    private ScreenShake screenShake;


    void Start()
    {

        screenShake = Camera.main.GetComponent<ScreenShake>();
        if (screenShake == null)
        {
            Debug.Log("screenshake is null");
        }
            // take current position = new position (0, 0, 0)
            transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        if (_spawnManager == null)
            Debug.LogError("The Spawn Manager is Null");

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null");
        }
        if (_audioSource == null)
            Debug.LogError("AudioSource on the player is null!");
        else
        {
            _audioSource.clip = _LaserSoundClip;
        }
        if (_shieldStregthGO == null)
        {
            Debug.Log("Player:: DoNullChecks - _shieldStreghtGO is Null");
        }
        _currentAmmo = _maxAmmo;
        _uiManager.UpdateAmmoDisplay(_currentAmmo, _maxAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        EngineStatus();

        Firelaser();
        
        
    }
    private void Firelaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_currentAmmo > 0)
            {
                _canFire = Time.time + _fireRate;
                _currentAmmo -= 1;
                _uiManager.UpdateAmmoDisplay(_currentAmmo, _maxAmmo);
                if (_isTripleShotActive == true)
                {
                    Instantiate(_TripleShotPreFab, transform.position, Quaternion.identity);
                }
                else if (_isSecondaryActive == true)
                {
                    //fire twinkie, wait, fire twinkie
                    //Coroutine called
                    Instantiate(_secondaryPreFab, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(_LaserPrefab, transform.position, Quaternion.identity);
                    
                }
                _audioSource.Play();
                if (_currentAmmo <= 0)
                {
                    _hasAmmo = false;
                    _Out_Of_Ammo.gameObject.SetActive(true);
                }
            }

        }
    }

    IEnumerator ThrusterPowerReplenisRoutine()
    {
        yield return new WaitForSeconds(_powerupThrustersWaitTimeLimit);
        while (_thrusterChargeLevel <= _thrusterChargeLevelMax && !_thrustersInUse)
        {
            yield return null;
            _thrusterChargeLevel += Time.deltaTime * _changeIncreaseThrusterChargeBy;
            _uiManager.UpdateThrustersSlider(_thrusterChargeLevel);
        }
        if (_thrusterChargeLevel >= _thrusterChargeLevelMax)
        {
            _thrusterChargeLevel = _thrusterChargeLevelMax;
            _uiManager.ThursterSliderUsableColor(true);
            _canUseThrusters = true;
        }
    }
    public void PlayerReload()
    {
        
        _currentAmmo = _maxAmmo;
        _uiManager.UpdateAmmoDisplay(_currentAmmo, _maxAmmo);
        
    }
    public void PlayerHealth()
    {
        if (_lives < 3)
        {
            _lives = _lives + 1;
            _uiManager.UpdateLives(_lives);
        }

    }
    
  

    void CalculateMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
   

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        if (Input.GetKey(KeyCode.LeftShift) && _canThrust)
        {

            transform.Translate(direction * (_speed * _speedMultiplier * Time.deltaTime));
            //_engineHeat += Time.deltaTime * 3;
            _thrusterChargeLevel -= Time.deltaTime * 3;
            _uiManager.UpdateThrustersSlider(_thrusterChargeLevel);
        }
    }



    private void EngineStatus()
    {


        if (_thrusterChargeLevel <= 0)
        {
            _canThrust = false;
            _thrusterChargeLevel = 0;
            StartCoroutine(ThrusterPowerReplenisRoutine());

        }
    }

    public void Damage()
    {
        screenShake.StartShake();     
        if (_isShieldsActive == true)
        {
            
            if (_shieldPower > 0)
            {
                _shieldStregthTextGo.GetComponent<TMP_Text>().text = _shieldPower.ToString();
            }
            if (_shieldPower < 0)
            {
                _isShieldsActive = false;
                _shieldVisualizer.SetActive(false);
            }
            return;
        }

        _lives -= 1;
        if (_lives == 2)
            {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

    }

    public void tripleShotActive()
    { _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    { 
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void SpeedBoostActive()
    {
        
        _speed *= _speedMultiplier;
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }
    public void SecondaryActive()
    { 
        _isSecondaryActive = true;
        StartCoroutine(SecondaryPowerDownRoutine());
    }

    IEnumerator SecondaryPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSecondaryActive = false;
    }
   
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        
        _speed /= _speedMultiplier;
    }

    public void Addscore()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }

    public void decreasescore()
    {
        _score = 0;
        _uiManager.UpdateScore(_score);
    }


    public void ShieldActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldPower = 3;
        _shieldStregthTextGo.GetComponent<TMP_Text>().text = _shieldPower.ToString();
        _shieldStregthGO.SetActive(true);
    }
    public void ShieldDeactivate()
    {
        if (_isShieldsActive == true)
        {
            _isShieldsActive = false;
            if (_playerShieldGO != null)
            {
                _shieldVisualizer.SetActive(false);

            }
            _shieldStregthGO.SetActive(false);
        }
    }


}
