using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
   
    
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _isDead = false;
    [SerializeField]
    private GameObject[] powerups;
    
    



    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }
    public IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3.0f);
        while(_isDead == false)
        {
            var Enemy = Instantiate(_enemyPrefab);
            
            var randomXPos = Random.Range(-9, 9);
            Enemy.transform.position = new Vector3(randomXPos, 7.3f, 0);
            Enemy.transform.rotation = Quaternion.identity;
            Enemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.5f);
        }
        
    }

    public IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(3.0f);
        while (_isDead == false)
        {
            var RandomPowerupID = Random.Range(0, 3);
            var Powerup = Instantiate(powerups[RandomPowerupID]);

            var randomXPos = Random.Range(-9, 9);
            Powerup.transform.position = new Vector3(randomXPos, 12, 0);
            Powerup.transform.rotation = Quaternion.identity;
            //last number in random.range is exclusive
            yield return new WaitForSeconds(Random.Range(8f, 11f));
        }
    }

    public void OnPlayerDeath()
    {
        
        _isDead = true;
    }
}
