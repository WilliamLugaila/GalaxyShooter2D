using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

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
    private int _score;

    private UIManager _uiManager;
    private float horizontalInput; // move left right
    private float verticalInput; // move up and down

    void Start()
    {
        // take current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        //_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_spawnManager == null)
            Debug.LogError("The Spawn Manager is Null");
        //_spawnManager.OnPlayerDeath

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        Firelaser();

    }
    private void Firelaser()
    {
        //if I hit space key
        //spawn gameObject
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
       
            _canFire = Time.time + _fireRate;
            if (_isTripleShotActive == true)
            {
                Instantiate(_TripleShotPreFab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_LaserPrefab, transform.position, Quaternion.identity);
            }


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
    }
    public void Damage()
    {
        if (_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives -= 1;
       // _uiManager.UpdateLives(_lives);

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
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        
        _speed /= _speedMultiplier;
    }
    public void ShieldsActive()
    { _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }
    public void Addscore()
    {
        _score += 10;
        //_uiManager.UpdateScore(_score);
    }
}
