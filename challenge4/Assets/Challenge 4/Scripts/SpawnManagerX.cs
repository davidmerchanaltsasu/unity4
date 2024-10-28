using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que gestiona la generación de enemigos y potenciadores en el juego
public class SpawnManagerX : MonoBehaviour
{
    // Prefab del enemigo que se generará
    public GameObject enemyPrefab;

    // Prefab del potenciador que se generará
    public GameObject powerupPrefab;

    // Rango de posición en el eje X para generar enemigos
    private float spawnRangeX = 10;

    // Rango mínimo y máximo en el eje Z para generar enemigos
    private float spawnZMin = 15; // Z mínimo
    private float spawnZMax = 25; // Z máximo

    // Contador de enemigos activos en la escena
    public int enemyCount;

    // Contador de la ola actual de enemigos
    public int waveCount = 1;

    // Referencia al objeto del jugador
    public GameObject player;

    //Referencia la velocidad del enemigo
    public float enemySpeed = 50;


    void Start()
    {
        // Llama a la función para iniciar la primera ola de enemigos
        SpawnEnemyWave(waveCount);
    }


    // Update is called once per frame
    void Update()
    {
        // Cuenta el número actual de enemigos en la escena
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Si no hay enemigos, genera una nueva ola
        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }
    }

    // Genera una posición aleatoria de spawn para potenciadores y enemigos
    Vector3 GenerateSpawnPosition()
    {
        // Genera una posición X aleatoria dentro del rango permitido
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        // Genera una posición Z aleatoria dentro del rango mínimo y máximo
        float zPos = Random.Range(spawnZMin, spawnZMax);
        // Devuelve un nuevo vector de posición
        return new Vector3(xPos, 0, zPos);
    }

    // Método que genera una ola de enemigos
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        // Offset para la posición del potenciador
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15); // Hacer que los potenciadores aparezcan cerca de la meta del jugador

        // Si no hay potenciadores en la escena, genera uno nuevo
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0) // Verifica que no haya potenciadores
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        // Genera el número de enemigos basado en la ola actual
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        // Incrementa el contador de la ola
        waveCount++;
        // Resetea la posición del jugador
        ResetPlayerPosition(); // Coloca al jugador de vuelta en la posición inicial
        
        enemySpeed += 25;// Aumenta la velocidad del enemigo
    }

    // Método que mueve al jugador de vuelta a su posición inicial
    void ResetPlayerPosition()
    {
        // Establece la posición del jugador
        player.transform.position = new Vector3(0, 1, -7);
        // Resetea la velocidad y la velocidad angular del Rigidbody del jugador
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}