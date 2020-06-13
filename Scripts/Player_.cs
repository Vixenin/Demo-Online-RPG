using UnityEngine;
using Mirror;
using TMPro;

public class Player_ : NetworkBehaviour
{
    private readonly float moveSpeed = 8f;

    private Rigidbody2D rb;
    private Animator animator;

    Vector2 movement;

    private bool enable_player;

    //User Name 
    [SerializeField] private TMP_Text playerNameBubble = null;
    
    public static string userName;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string userNameST;
    void OnNameChanged(string _Old, string _New)
    {
        playerNameBubble.text = _New;
    }

    [Client]
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enable_player = true;
    }

    //Cam Follow Part
    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<FollowCam>().setTarget(gameObject.transform);

        CmdPlayerName(userName);
    }

    [Command]
    private void CmdPlayerName(string _New)
    {
        userNameST = _New;
    }

    [Client]
    private void Update()
    {
        if (enable_player == true)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movement = new Vector2(0,0);
        }

        PlayerMovement();
        PlayerAnimation();
    }

    [Client]
    private void PlayerMovement()
    {
        if (!base.hasAuthority)
            return;

        if (Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") !=0 )
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    [Client]
    private void PlayerAnimation()
    {
        if (!base.hasAuthority)
            return;

        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    [Client]
    public void Enable_Movement()
    {
        enable_player = true;
    }

    [Client]
    public void Disable_Movement()
    {
        enable_player = false;
    }
}