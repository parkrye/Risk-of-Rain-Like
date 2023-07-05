using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    PlayerDataModel playerDataModel;

    public Vector3 moveDir, dirModifier;
    [SerializeField] Vector3 curVelocity;
    [SerializeField] float slopeDegree;
    [SerializeField] bool isSlope;
    [SerializeField] AudioSource jumpAudio;
    RaycastHit slopeHit;
    bool descending;

    void Awake()
    {
        playerDataModel = GetComponent<PlayerDataModel>();
    }

    void Update()
    {
        if (!playerDataModel.onESC)
        {
            CheckGround();
            CheckSlope();
        }
    }

    void CheckGround()
    {
        if (playerDataModel.rb.velocity.y > 0f)
        {
            descending = true;
        }

        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, 0.5f, LayerMask.GetMask("Ground")))
        {
            if (descending && playerDataModel.rb.velocity.y < 0f)
            {
                playerDataModel.jumpCount = 0;
                descending = false;
            }

            playerDataModel.animator.SetBool("IsGround", true);
            return;
        }
        playerDataModel.animator.SetBool("IsGround", false);
    }

    /// <summary>
    /// �ٴڸ��� ���� ���͸� �̿��Ͽ� ���� ���ִ� �ٴ��� �������� Ȯ���ϴ� �޼ҵ�
    /// </summary>
    void CheckSlope()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if(Physics.Raycast(ray, out slopeHit, 0.3f, LayerMask.GetMask("Ground")))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            isSlope = angle != 0f && angle < slopeDegree;
            return;
        }
        isSlope = false;
    }

    /// <summary>
    /// ������ ���� ����� ���ϰ� �ʿ��� ���� ���� ������ ��ȯ�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    void FixedUpdate()
    {
        if (!playerDataModel.onESC)
        {
            Move();
        }
    }

    void Move()
    {
        float animatorFoward = playerDataModel.animator.GetFloat("Foward");
        float animatorSide = playerDataModel.animator.GetFloat("Side");

        // ���� ����
        float gravity = (playerDataModel.rb.velocity.y < 0 ? -playerDataModel.rb.velocity.y : playerDataModel.rb.velocity.y) - Physics.gravity.y * Time.deltaTime * playerDataModel.TimeScale;
        if(playerDataModel.animator.GetBool("IsGround") && isSlope)
        {
            curVelocity = AdjustDirectionToSlope(moveDir);
            gravity = 0f;
        }
        else
            curVelocity = moveDir;
        if (!playerDataModel.rb.useGravity)
            gravity = 0f;
        curVelocity = transform.right * curVelocity.x + transform.forward * curVelocity.z;

        // �̵�
        playerDataModel.rb.velocity = (new Vector3(curVelocity.x, 0f, curVelocity.z) * playerDataModel.MoveSpeed * playerDataModel.TimeScale + Vector3.down * gravity + dirModifier);
        if (Vector3.SqrMagnitude(dirModifier - Vector3.zero) > 0.1f)
            dirModifier = Vector3.Lerp(dirModifier, Vector3.zero, Time.deltaTime * 5f);

        // �ִϸ��̼�
        playerDataModel.animator.SetFloat("Foward", Mathf.Lerp(animatorFoward, moveDir.z, Time.deltaTime * playerDataModel.TimeScale * 5f));
        playerDataModel.animator.SetFloat("Side", Mathf.Lerp(animatorSide, moveDir.x, Time.deltaTime * playerDataModel.TimeScale * 5f));
    }

    void OnMove(InputValue inputValue)
    {
        Vector2 tmp = inputValue.Get<Vector2>();

        if (playerDataModel.controllable)
            moveDir = new Vector3(tmp.x, 0f, tmp.y);
        else
            moveDir = Vector3.zero;
    }

    void OnJump(InputValue inputValue)
    {
        if (playerDataModel.controllable)
        {
            if (playerDataModel.jumpCount < playerDataModel.jumpLimit)
            {
                if (playerDataModel.hero.Jump(inputValue.isPressed))
                    jumpAudio.Play();
            }
            else
            {
                playerDataModel.hero.Jump(false);
            }
        }
    }
}
