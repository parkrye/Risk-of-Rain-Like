using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    PlayerDataModel playerDataModel;

    public Vector3 moveDir, dirModifier;
    [SerializeField] Vector3 curVelocity;
    [SerializeField] float slopeDegree;
    [SerializeField] bool isSlope;
    RaycastHit slopeHit;
    bool descending;

    void Awake()
    {
        playerDataModel = GetComponent<PlayerDataModel>();
    }

    void Update()
    {
        CheckGround();
        CheckSlope();
    }

    void CheckGround()
    {
        if (playerDataModel.rb.velocity.y > 0f)
        {
            descending = true;
        }

        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, 0.2f, LayerMask.GetMask("Ground")))
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
        if(Physics.Raycast(ray, out slopeHit, 0.2f, LayerMask.GetMask("Ground")))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
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
        Move();
    }

    void Move()
    {
        float animatorFoward = playerDataModel.animator.GetFloat("Foward");
        float animatorSide = playerDataModel.animator.GetFloat("Side");

        // ���� ����
        float gravity = Mathf.Abs(playerDataModel.rb.velocity.y) - Physics.gravity.y * Time.deltaTime;
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
        playerDataModel.rb.velocity = new Vector3(curVelocity.x, 0f, curVelocity.z) * playerDataModel.moveSpeed + Vector3.down * gravity + dirModifier;
        if (dirModifier != Vector3.zero)
            dirModifier = Vector3.Lerp(dirModifier, Vector3.zero, 0.1f);

        // �ִϸ��̼�
        playerDataModel.animator.SetFloat("Foward", Mathf.Lerp(animatorFoward, moveDir.z, 0.1f));
        playerDataModel.animator.SetFloat("Side", Mathf.Lerp(animatorSide, moveDir.x, 0.1f));
    }

    void OnMove(InputValue inputValue)
    {
        Vector2 tmp = inputValue.Get<Vector2>();

        if (playerDataModel.controlleable)
            moveDir = new Vector3(tmp.x, 0f, tmp.y);
        else
            moveDir = Vector3.zero;
    }

    void OnJump(InputValue inputValue)
    {
        if (playerDataModel.controlleable)
        {
            if (playerDataModel.jumpCount < playerDataModel.jumpLimit)
            {
                playerDataModel.hero.Jump(inputValue.isPressed);
            }
            else
            {
                playerDataModel.hero.Jump(false);
            }
        }
    }
}
