using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;
    [SerializeField]
    private Animator _anim;
    private AudioSource _audioSource;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("The Player is Null");
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        //Move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //if bottom of screen
        //respawn at top with new random x position
        if (transform.position.y < -5f)
        {
                float randomX = Random.Range(-8f, 8f);
                transform.position = new Vector3(randomX, 7,0);

        }
                
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Player player = other.transform.GetComponent<Player>();
        if (other.tag == "Player")
        {
            
            other.transform.GetComponent<Player>().Damage();
            _anim.SetTrigger("OnEnemyDeath");

            Destroy(this.gameObject, 2.8f);
            _audioSource.Play();
        }

        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);
           
            if(_player != null)
            {
                _player.Addscore();
            }
            //add 10 to score
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);

        }
        //method to add 10 to score!
        //communicate with UI to update the score!
    }
}
