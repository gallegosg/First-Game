using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Text superJumpsRemainingText;
    [SerializeField] private GameObject winCoin;
    private Animator animator;
    private Rigidbody rigidBodyComponent;
    private AudioSource audioSource;
    public Transform groundCheckTransform;
    bool jumpKeyWasPressed = false;
    bool sprintKeyPressed = false;
    float horizontalInput;
    int jumpsLeft = 2;
    int superJumpsRemaining;
    int isWalkingHash;

    public GameObject coinPrefab;
    int coinsTotal;
    int coinsGotten;

    // Start is called before the first frame update
    void Start()
    {
        coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
        superJumpsRemainingText.text = "Coins: " + coinsGotten.ToString() + '/' + coinsTotal.ToString();

        rigidBodyComponent = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isWalkingHash = Animator.StringToHash("IsWalking");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
            resetScene();

        horizontalInput = Input.GetAxis("Horizontal");

        //set player running animation
        if (horizontalInput != 0 && Input.GetKey(KeyCode.LeftShift))
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
        if (!isWalking && horizontalInput != 0)
            animator.SetBool("IsWalking", true);
        else if (isWalking && horizontalInput == 0)
            animator.SetBool("IsWalking", false);

        //turn player towards direction walking
        if (horizontalInput > 0)
            transform.rotation = Quaternion.Euler(0, 90, 0);
        else if (horizontalInput < 0)
            transform.rotation = Quaternion.Euler(0, -90, 0);

        //check if player falls to a certain hight, then refresh scene
        if (transform.position.y <= -10)
            resetScene();
    }

    // update once every physics frame
    void FixedUpdate()
    {
        if (jumpKeyWasPressed)
        {
            if (jumpsLeft > 0)
            {
                animator.SetBool("IsJumping", true);
                audioSource.Play();
                jumpPlayer();
            }
        }

        float moveSpeed = horizontalInput * (sprintKeyPressed ? 3 : 1.5f);
        rigidBodyComponent.velocity = new Vector3(moveSpeed, rigidBodyComponent.velocity.y, 0);
    }

    void jumpPlayer()
    {
        float jumpForce = 2 * (jumpsLeft == 1 ? 0.5f : 1);
        // if (superJumpsRemaining > 0)
        // {
        //     jumpForce *= 1.5f;
        //     superJumpsRemaining--;
        //     superJumpsRemainingText.text = "Super Jumps: " + superJumpsRemaining.ToString();
        // }
        rigidBodyComponent.AddForce(Vector3.up * 7, ForceMode.Impulse);

        jumpKeyWasPressed = false;
        jumpsLeft--;
    }

    void resetScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    GameObject CreateText(Transform canvas_transform, float x, float y, string text_to_print, int font_size, Color text_color)
    {
        GameObject UItextGO = new GameObject("Text2");
        UItextGO.transform.SetParent(canvas_transform);
        UItextGO.transform.localScale = new Vector3(1, 1, 1);

        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.sizeDelta = new Vector2(300, 200);
        trans.anchoredPosition3D = new Vector3(x, y, 0);

        Text text = UItextGO.AddComponent<Text>();
        text.text = text_to_print;
        text.fontSize = font_size;
        text.color = text_color;
        text.alignment = TextAnchor.MiddleCenter;
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        return UItextGO;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            coinsGotten++;
            Destroy(other.gameObject);
            superJumpsRemaining++;
            superJumpsRemainingText.text = "Coins: " + coinsGotten.ToString() + '/' + coinsTotal.ToString();
            if(coinsGotten == coinsTotal)
            {
                winCoin.SetActive(true);
            }
        }
        else if (other.gameObject.layer == 9)
        {
            //win, stop game
            Time.timeScale = 0.4f;
            Destroy(other.gameObject);
            CreateText(mainCanvas.transform, 0, 0, "Game Over", 50, Color.white);
            CreateText(mainCanvas.transform, 0, -50, "Press R to play again", 24, Color.white);
        }
        if (other.gameObject.tag == "Floor")
        {
            animator.SetBool("IsJumping", false);
            jumpsLeft = 2;
        }
    }
}