using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _RestartText;

    private GameManager _gameManager;
    



    public void Start()
    {
        _scoreText.text = "Score : " + 0;
        _GameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is null");
        }
    }

   

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score : " + playerScore.ToString();
    }

    public void UpdateHealth(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _GameOverText.gameObject.SetActive(true);
        _RestartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    

    private IEnumerator GameOverFlicker()
    {
        while(true)
        {

            _GameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _GameOverText.text = "";
            yield return new WaitForSeconds(0.5f);

            

        }
    }
    
    
}
