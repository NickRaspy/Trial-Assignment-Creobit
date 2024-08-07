using UnityEngine;
namespace CB_TA.Adventure
{
    public class PlayerBehavior : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private float speed = 3f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private Animator animator;
        [Header("Camera")]
        [SerializeField] private Transform followingCamera;
        [SerializeField] private float cameraOffsetX;

        private Rigidbody rb;
        private bool camJump;
        public bool CanMove { get; set; }

        void Start()
        {
            CanMove = true;
            rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && camJump && CanMove) Jump();
        }
        void FixedUpdate()
        {
            followingCamera.position = new Vector3(transform.position.x + cameraOffsetX, followingCamera.position.y, transform.position.z);
            Vector3 movement;
            if (CanMove)
            {
                movement = new(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
                rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * movement);
            }
            else movement = Vector3.zero;
            if (movement != Vector3.zero)
            {
                animator.SetBool("isRunning", true);
                Quaternion rot = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.fixedDeltaTime);
            }
            else animator.SetBool("isRunning", false);
        }
        void Jump()
        {
            animator.SetBool("isLanded", false);
            animator.Play("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            camJump = false;
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                camJump = true;
                animator.SetBool("isLanded", true);
            }
        }
    }
}

