using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que controla el comportamiento del jugador en el juego
public class PlayerControllerX : MonoBehaviour
{
    // Componente Rigidbody del jugador para aplicar física
    private Rigidbody playerRb;

    // Velocidad de movimiento del jugador
    private float speed = 500;

    // Referencia al objeto que representa el punto focal
    private GameObject focalPoint;

    // Indica si el jugador tiene un potenciador activo
    public bool hasPowerup;

    // Indicador visual del potenciador activo
    public GameObject powerupIndicator;

    // Duración del potenciador
    public int powerUpDuration = 5;

    // Fuerza de impacto normal y con potenciador
    private float normalStrength = 10; // Fuerza normal al golpear enemigos
    private float powerupStrength = 25; // Fuerza al golpear enemigos con potenciador

    // Fuerza del impulso turbo
    private float turboBoost = 20f;

    // Sistema de partículas para el efecto de humo del turbo
    public ParticleSystem turboSmoke;

    // Start is called before the first frame update
    void Start()
    {
        // Obtiene el componente Rigidbody del jugador
        playerRb = GetComponent<Rigidbody>();
        // Busca el objeto llamado "Focal Point" en la escena
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        // Captura la entrada vertical del usuario (adelante/atrás)
        float verticalInput = Input.GetAxis("Vertical");

        // Aplica fuerza al jugador en dirección del punto focal
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime);

        // Actualiza la posición del indicador del potenciador, colocándolo debajo del jugador
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        // Si se presiona la tecla espacio, aplica un impulso turbo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRb.AddForce(focalPoint.transform.forward * turboBoost, ForceMode.Impulse);
            turboSmoke.Play(); // Reproduce el efecto de humo del turbo
        }
    }

    // Si el jugador colisiona con un potenciador, activa el potenciador
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto tiene la etiqueta "Powerup"
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject); // Destruye el objeto de potenciador
            hasPowerup = true; // Activa el potenciador
            powerupIndicator.SetActive(true); // Muestra el indicador del potenciador
            StartCoroutine(PowerupCooldown()); // Inicia la cuenta regresiva del potenciador
        }
    }

    // Coroutine para contar la duración del potenciador
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration); // Espera el tiempo de duración
        hasPowerup = false; // Desactiva el potenciador
        powerupIndicator.SetActive(false); // Oculta el indicador del potenciador
    }

    // Si el jugador colisiona con un enemigo
    private void OnCollisionEnter(Collision other)
    {
        // Verifica si el objeto colisionado es un enemigo
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>(); // Obtiene el Rigidbody del enemigo
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position; // Calcula la dirección de empuje

            // Aplica fuerza al enemigo dependiendo de si el jugador tiene un potenciador
            if (hasPowerup) // Si tiene un potenciador, usa la fuerza del potenciador
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // Si no tiene potenciador, usa la fuerza normal
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }
}