using System.Collections.Generic;
using UnityEngine;

public class PlayerDataModel : MonoBehaviour
{
    public Hero hero;
    List<Hero> heroList;
    public Animator animator;
    public Rigidbody rb;
    [SerializeField][Range(0, 2)] int heroNum;

    void Awake()
    {
        Initailze();
    }

    /// <summary>
    /// ĳ���� ����
    /// </summary>
    /// <param name="num"></param>
    /// <returns>���� ���� ����</returns>
    public bool SelectHero(int num)
    {
        if(num >= 0 && num < heroList.Count)
        {
            foreach (var hero in heroList)
                hero.gameObject.SetActive(false);
            heroList[num].gameObject.SetActive(true);
            hero = heroList[num];
            animator = hero.animator;
            hero.playerDataModel = this;
            animator.SetInteger("Hero", num);
            return true;
        }
        return false;
    }

    /// <summary>
    /// ĳ���� �ı�
    /// </summary>
    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }

    public float hitPoint;
    public float moveSpeed, highSpeed, jumpPower;
    public float attackCoolTime, skillCoolTime;
    public int jumpLimit, jumpCount;
    public bool isJump, attackCooldown;

    public Vector3 moveDir, prevDir;
    public Dictionary<Vector3, bool> climable;
    public float climbCheckLowHeight, climbCheckHighHeight, climbCheckLength;

    void Initailze()
    {
        heroList = new List<Hero>();
        heroList.AddRange(GetComponentsInChildren<Hero>());
        foreach (var hero in heroList)
            hero.gameObject.SetActive(false);

        rb = GetComponent<Rigidbody>();
        climable = new Dictionary<Vector3, bool>();

        SelectHero(heroNum);

        DontDestroyOnLoad(gameObject);
    }
}
