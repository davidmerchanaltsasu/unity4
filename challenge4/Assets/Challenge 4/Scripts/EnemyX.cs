using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que controla el comportamiento de un enemigo en el juego
public class EnemyX : MonoBehaviour
{
    // Velocidad a la que se mueve el enemigo
    public float speed = 10f;

    // Componente Rigidbody del enemigo para aplicar física
    private Rigidbody enemyRb;

    // Referencia al objeto que representa la meta del jugador
    private GameObject playerGoal;

    //Referencia al componente Spawnmanager
    private SpawnManagerX SpawnManager;

    // Start is called before the first frame update
    void Start()
    {
        // Obtiene el componente Rigidbody del enemigo
        enemyRb = GetComponent<Rigidbody>();

        // Busca el objeto llamado "player Goal" en la escena y lo asigna a la variable playerGoal
        playerGoal = GameObject.Find("player Goal");

        SpawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManagerX>();

        speed = SpawnManager.enemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la dirección hacia el objetivo del jugador y normaliza el vector
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;

        // Aplica fuerza al enemigo en la dirección del objetivo, multiplicada por la velocidad y el tiempo
        enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
    }

    // Método que se llama al colisionar con otro objeto
    private void OnCollisionEnter(Collision other)
    {
        // Si el enemigo colisiona con "Enemy Goal", destruye el objeto enemigo
        if (other.gameObject.name == "Enemy Goal")
        {
            Destroy(gameObject);
        }
        // Si el enemigo colisiona con "Player Goal", también lo destruye
        else if (other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }
    }
}