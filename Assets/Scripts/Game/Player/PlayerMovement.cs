using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public DynamicJoystick joystick;

    private Animator anim;
    private BoxCollider2D col;

    private string currentAnimState = "";
    private string lastDirection = "Down"; // 🔥 마지막 방향 저장

    public Vector2 LastInput { get; private set; } // 🔥 외부에서 사용 (파티클용)

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }

    public void HandleMove(bool canMove)
    {
        if (!canMove) return;

        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (input.magnitude > 0.1f)
        {
            LastInput = input.normalized; // 🔥 방향 저장
        }

        transform.Translate(input * speed * Time.deltaTime);

        UpdateAnimation(input);
        ClampToScreen();
    }

    private void UpdateAnimation(Vector2 input)
    {
        string nextState = "Idle";

        if (input.magnitude > 0.1f)
        {
            if (Mathf.Abs(input.y) >= Mathf.Abs(input.x))
                nextState = (input.y > 0) ? "Up" : "Down";
            else
                nextState = (input.x > 0) ? "Right" : "Left";

            lastDirection = nextState; // 🔥 방향 기억
        }

        if (currentAnimState != nextState)
        {
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Left", false);
            anim.SetBool("Right", false);

            if (nextState != "Idle")
            {
                anim.SetBool(nextState, true);
                UpdateCollider(nextState);
            }
            else
            {
                // 🔥 Idle이어도 마지막 방향 기준으로 콜라이더 유지
                UpdateCollider("Up");
            }

            currentAnimState = nextState;
        }
    }

    private void UpdateCollider(string state)
    {
        if (state == "Up" || state == "Down")
        {
            col.offset = new Vector2(0, 0.05f);
            col.size = new Vector2(0.58f, 1.43f);
        }
        else
        {
            col.offset = new Vector2(0.01f, -0.09f);
            col.size = new Vector2(1.79f, 0.67f);
        }
    }

    private void ClampToScreen()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}