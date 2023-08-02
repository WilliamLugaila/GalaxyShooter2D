using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to Text
    [SerializeField]
    private Text _scoreText;
    // Start is called before the first frame update
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;
    [SerializeField]
    private TMP_Text _ammoDisplay;
    public Slider _ThrusterStatus;
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Image _thrusterSliderFill;

    void Start()
    {
        //_liveSprites[CurrentPlayerLives = 3];
        _scoreText.text = "score:" + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
       
        if (_gameManager == null)
        {
            
        }
    }
    void Update()
    {
        
    }

    public void UpdateThrustersSlider(float _thrustValue) // Method to update the Thursters slide.
    {
        if (_thrustValue >= 0 && _thrustValue <= 100)
        {
            _thrusterSlider.value = _thrustValue;
        }
    }
    public void ThursterSliderUsableColor(bool _usableThrusters)
    {
        if (_usableThrusters)
        {
            _thrusterSliderFill.color = Color.green;
        }
        else if (!_usableThrusters)
        {
            _thrusterSliderFill.color = Color.red;
        }

    }
    public void UpdateAmmoDisplay( int currentAmmo, int maxAmmo)
    {
        _ammoDisplay.text = currentAmmo + " " + maxAmmo;
    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }
    // Update is called once per frame
    
    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 0) 
        {
        
        _LivesImg.sprite = _liveSprites[currentLives];
        }

        if (currentLives == 0)
        {
            GameOverSequence();

        }
    }
    void GameOverSequence()
    {
  
        //_gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
