using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    private Player _player;
    [SerializeField]
    private Animator _anim;
    private AudioSource _audioSource;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private float _enemyStartPosition;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("The Player is Null");
        }
        //_enemyStartPosition = transform.position.x;
        EnemyMovement();
    }


    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
        EnemyMovement();


    }

    void EnemyStartPosition()
    {
        transform.position = new Vector3(Random.Range(-11.4f, 11.4f), 7.5f, 0f);
        _enemyStartPosition = transform.position.x;
    }
    void EnemyMovementDown()
    {
        if (transform.position.y < -8.5f)
        {
            EnemyStartPosition();
        }
    }

    void EnemyMovementLeft()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        if (transform.position.y < -11.2f || transform.position.y < -8.5f)
        {
            EnemyStartPosition();
        }
    }

    void EnemyMovementRight()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        transform.Translate(Vector3.left * _speed * Time.deltaTime);

        if (transform.position.y > -11.2f || transform.position.y < -8.5f)
        {
            EnemyStartPosition();
        }
    }

    void EnemyMovement()
    {
        if (_enemyStartPosition <= -9.5f)
        {
            EnemyMovementRight();
        }
        else if (_enemyStartPosition >= -9.5f)
        {
            EnemyMovementLeft();
        }
        else
        {
            EnemyMovementDown();
        }
    }
    void CalculateMovement()
    {
        //Move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //if bottom of screen
        //respawn at top with new random x position
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Player player = other.transform.GetComponent<Player>();
        if (other.tag == "Player")
        {

            other.transform.GetComponent<Player>().Damage();
            _anim.SetTrigger("OnEnemyDeath");

            Destroy(this.gameObject, 2.0f);
            _audioSource.Play();
        }

        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.Addscore();
            }
            //add 10 to score
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.0f);
        }


    }

}
