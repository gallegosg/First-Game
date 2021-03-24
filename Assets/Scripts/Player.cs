using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]public Transform groundCheckTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidBodyComponent;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Text superJumpsRemainingText;
    bool jumpKeyWasPressed = false;
    bool sprintKeyPressed = false;
    float horizontalInput;
    int jumpsLeft = 2;
    int superJumpsRemaining;
    int isWalkingHash;

    // Start is called before the first frame update
    void Start()
    {
        rigidBodyComponent = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("IsWalking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        if(Input.GetKeyDown(KeyCode.R)){
            transform.position = new Vector3(0, 1, 0);
        }

        horizontalInput = Input.GetAxis("Horizontal");

        //set player running animation
        if(horizontalInput != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            sprintKeyPressed = true;
            animator.SetBool("IsSprinting", true);
        }       
        else
        {
            sprintKeyPressed = false;
            animator.SetBool("IsSprinting", false);
        }

        //set player walking animation
        if(!isWalking && horizontalInput != 0)
            animator.SetBool("IsWalking", true);
        else if (isWalking && horizontalInput == 0)
            animator.SetBool("IsWalking", false);

        //turn player towards direction walking
        if(horizontalInput > 0)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (horizontalInput < 0)
            transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    void FixedUpdate()
    {
        if(jumpKeyWasPressed)
        {
            if(jumpsLeft > 0)
            {
                animator.SetBool("IsJumping", true);
                jumpPlayer();
            } 
        }

        float moveSpeed = horizontalInput * (sprintKeyPressed ? 3 : 1.5f);
        rigidBodyComponent.velocity = new Vector3(moveSpeed, rigidBodyComponent.velocity.y, 0);
    }

    void jumpPlayer()
    {
        float jumpForce = 5 * (jumpsLeft == 1 ? 0.5f : 1);
        if(superJumpsRemaining > 0)
        {
            jumpForce *= 1.5f;
            superJumpsRemaining--;
            superJumpsRemainingText.text = "Super Jumps: " + superJumpsRemaining.ToString();
        }
        rigidBodyComponent.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpKeyWasPressed = false;
        jumpsLeft--;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            superJumpsRemaining++;
            superJumpsRemainingText.text = "Super Jumps: " + superJumpsRemaining.ToString();
        } 
        if (other.gameObject.tag == "Floor")
        {
            animator.SetBool("IsJumping", false);
            jumpsLeft = 2;
        }
    }
}