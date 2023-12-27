using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject _pivot;

    [Header("Respawn")]
    [SerializeField] private Vector2 resetPosition;
    [SerializeField] private float timeBeforeReset = 3f;
    [SerializeField] private float minCollisionForce = 10f;
    [SerializeField] private float velocityMagnitude;
    [SerializeField] private int maxRespawns = 3;

    [Header("Camera")]
    [SerializeField] private float cameraFollowSpeed = 5f;
    [SerializeField] private Vector2 minCameraPosition;
    [SerializeField] private Vector2 maxCameraPosition;

    [SerializeField] GameObject pig;

    [SerializeField] GameObject health;


    private bool _isDragging;
    private bool _canDrag;
    private Camera _main;
    private Rigidbody2D _rigidbody2D;
    private SpringJoint2D _springJoint2D;
    private float _timeSinceLastAction;
    public int _respawnsLeft = 3;

    private bool canRespawn;
    private bool isDefeated;
    private bool isWin;
    public GameObject defeatPanel;
    public GameObject winPanel;

    private int ultimoHijoDesactivado = 0;


    private void Start()
    {
        _main = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _springJoint2D = GetComponent<SpringJoint2D>();
        _canDrag = true;

        isDefeated = false;
        isWin = false;
        defeatPanel.SetActive(false);
        winPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        DetectTouch();
        DetachBall();

        if (!_isDragging)
        {
            Vector3 targetPosition = new Vector3(
                Mathf.Clamp(transform.position.x, minCameraPosition.x, maxCameraPosition.x),
                Mathf.Clamp(transform.position.y, minCameraPosition.y, maxCameraPosition.y),
                _main.transform.position.z);

            _main.transform.position = Vector3.Lerp(_main.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
        }
        
        if (!_isDragging && _rigidbody2D.velocity.magnitude < velocityMagnitude)
        {
            if (_timeSinceLastAction >= timeBeforeReset && _respawnsLeft > 0)
            {
                ResetPlayer();
                _timeSinceLastAction = 0f; // Resetea el temporizador después de la reaparición
                canRespawn = false;

            }
            else
            {
                if (canRespawn == true){
                _timeSinceLastAction += Time.deltaTime;
                }
            }
        }

        if (_respawnsLeft <= 0 && !isDefeated)
        {
            PauseGame();
            ShowDefeat();
        }

        if (pig.transform.childCount == 0 && !isWin)
        {
            PauseGame();
            ShowWin();
        }
    }

    private void DetectTouch()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (_isDragging)
            {
                LaunchPlayer();
                canRespawn = true;
            }
            _isDragging = false;
            return;
        }

        if (_canDrag && Time.timeScale != 0f)
        {
            // Obtener la posición del toque en el mundo
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = _main.ScreenToWorldPoint(touchPosition);

            // Verificar si el toque está dentro del Collider2D del jugador
            Collider2D playerCollider = GetComponent<Collider2D>();
            if (playerCollider == Physics2D.OverlapPoint(worldPosition))
            {
                _isDragging = true;
                _rigidbody2D.isKinematic = true;

                worldPosition.z = transform.position.z;
                transform.position = worldPosition;
            }

        }
    }

    private void LaunchPlayer()
    {
        _rigidbody2D.isKinematic = false;
        _canDrag = false;
    }

    private void DetachBall()
    {
        if (transform.position.x > _pivot.transform.position.x && !_isDragging)
        {
            _springJoint2D.enabled = false;
        }
    }

    private void ResetPlayer()
    {
        _respawnsLeft--;
        transform.position = resetPosition;
        _springJoint2D.enabled = true;
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        _rigidbody2D.isKinematic = true;
        _canDrag = true;
        BajarVida();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto que colisiona está por encima del pájaro y la fuerza es suficiente
        if (collision.relativeVelocity.magnitude >= minCollisionForce && collision.contacts[0].normal.y < -0.8f)
        {
            ResetPlayer(); // O también puedes destruir el pájaro si lo prefieres: Destroy(gameObject);
        }
    }

    private void ShowDefeat()
    {
        isDefeated = true;
        // Aquí puedes activar el panel de derrota y realizar cualquier otra acción necesaria
        defeatPanel.SetActive(true);

        Debug.Log("¡Has sido derrotado!");
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pausa el juego
    }

    private void ShowWin()
    {
        isWin = true;
        winPanel.SetActive(true);
        Debug.Log("¡Has ganado!");
    }

    private void BajarVida()
    {
        if (ultimoHijoDesactivado < health.transform.childCount)
        {
            // Desactivar el próximo hijo
            health.transform.GetChild(ultimoHijoDesactivado).gameObject.SetActive(false);
            ultimoHijoDesactivado++;
        }
    }
}
