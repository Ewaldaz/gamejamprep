using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float movementVelocity = 4f;
    public float depthVelocityModifier = 1f;
    public float jumpVelocity = 200f;
    public BoxCollider myCollide;
    public Text ScoreText;
    public int lifeRemaining = 3;
    public float lookSpeed = 10;
    public GameObject mainCamera;

    bool up = false;
    Vector3 prevLoc;
    float cameraOffset = 12;
    float inputV, inputH, jump = 0;

    public int Score
    {
        get; set;
    }

    
    void Start()
    {
        Score = 0;
        GetComponent<Animator>().Play("Idle");
    }
    
    void Update()
    {
        checkInput();

        ScoreText.text = $"Points: {Score}";
        
        if (transform.position.y < -1)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    void FixedUpdate()
    {
        doMove();
    }

    void OnCollisionEnter(Collision col)
    {
        
    }

    private void doMove()
    {
        prevLoc = transform.position;

        #region Move
        float x = transform.position.x;
        float z = transform.position.z;
        if (inputV != 0)
        {
            var move = inputV * movementVelocity * Time.deltaTime * depthVelocityModifier;
            x -= move;
            z -= move;
        }
        if (inputH != 0)
        {
            var move = inputH * movementVelocity * Time.deltaTime;
            x -= move;
            z += move;
        }

        transform.position = new Vector3
        (
            x,
            transform.position.y,
            z
        );
        #endregion Move

        #region Camera Follow
        mainCamera.transform.position = new Vector3
        (
            x + cameraOffset,
            mainCamera.transform.position.y,
            z + cameraOffset
        );
        #endregion Camera Follow

        #region Rotation and animations
        if (transform.position != prevLoc)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - prevLoc), Time.fixedDeltaTime * lookSpeed);
            GetComponent<Animator>().Play("Running");
        }
        else
        {
            GetComponent<Animator>().Play("Idle");
        }
        #endregion Rotation and animations

        #region Jump
        if (up)
        {
            if (GetComponent<Rigidbody>().velocity.y <= 0 && GetComponent<Rigidbody>().velocity.y > -0.01)
            {
                GetComponent<Rigidbody>().AddForce(0, jumpVelocity, 0);
            }
            up = false;
        }
        #endregion Jump
    }
    
    private void checkInput()
    {
        inputV = Input.GetAxis("Vertical");
        inputH = Input.GetAxis("Horizontal");

        jump = Input.GetAxis("Jump");
        if (jump != 0)
        {
            up = true;
        }
    }

    public void loseLife()
    {
        lifeRemaining--;
    }
}