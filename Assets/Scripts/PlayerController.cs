using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float movementVelocity = 4f;
    public float depthVelocityModifier = 1f;
    public float jumpVelocity = 200f;
    public BoxCollider myCollide;
    public Text ScoreText;
    public int lifeRemaining = 3;
    public float lookSpeed = 10;
    public NavMeshAgent agent;

    bool up = false;
    bool running = false;
    Vector3 prevLoc;
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

        ScoreText.text = string.Format("Points: {0}", Score);
        
        if (transform.position.y < -1)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    private void checkMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
          //  agent.enabled = true;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);
                if (agent.isOnNavMesh)
                {
                    running = true;
                    agent.SetDestination(hit.point);
                }
            }            
        }
        if (agent.remainingDistance <= 0.1f)
        {
            running = false;
        }
    }

    void FixedUpdate()
    {
        doMove();
        checkMouseClick();
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


        #region Rotation and animations
        if (transform.position != prevLoc || running)
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

        if (inputV != 0 || inputH != 0)
        {
            //  agent.enabled = false;
            if (agent.isOnNavMesh)
            {
                agent.ResetPath();
            }
        }

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