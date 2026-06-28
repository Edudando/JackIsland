/**
 * @author Eduardo Ortega
 * @email eduardoortega@live.com.ar
 * @create date 27-06-2026 23:59:41
 * @modify date 27-06-2026 23:59:41
 * @desc [description]
 */
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public CompanionDialogue dialogoDelLoro;
    public bool estaHablando = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        dialogoDelLoro = GetComponent<CompanionDialogue>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        if (estaHablando)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        rb.linearVelocity = moveInput * MoveSpeed;
    }

    void Update()
    {
        if (animator != null)
        {
            if (estaHablando)
            {
                animator.SetFloat("Velocity", 0);
                return;
            }

            animator.SetFloat("Velocity", moveInput.magnitude);

            if (moveInput.x > 0)
                spriteRenderer.flipX = false;
            else if (moveInput.x < 0)
                spriteRenderer.flipX = true;

            if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
            {
                string[] frasesDePrueba = {
                    "¡Squawk! ¡Cuidado por donde pisas, Jack!",
                    "Esa calavera de piedra no me da buena espina."
                };
                dialogoDelLoro.ShowDialogue(frasesDePrueba);
            }
        }
    }

    public void ReproducirPaso()
    {
        RuntimeManager.PlayOneShot("event:/PasosJack", transform.position);
    }
}