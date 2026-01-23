using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorOverrideController NoWeaponOverride;
    [SerializeField] private AnimatorOverrideController GunlanceOverride;
    [SerializeField] private AnimatorOverrideController OneHSwordOverride;
    [SerializeField] private AnimatorOverrideController HammerOverride;
    [SerializeField] private AnimatorOverrideController TwoHSwordOverride;

    public PlayerMove playerMove;

    public float moveDuration;
    public float rotationSpeed;

    public bool isMoving = false;

    public GameObject PlayerLeftHand;
    public GameObject PlayerRightHand;

    public GameObject LShield;

    public GameObject ROneHandSword;
    public GameObject RTwoHandSword;
    public GameObject RGunlance;
    public GameObject RHammer;

    public PlayerWeapon currentPlayerWeapon;

    public Transform granadeSpawnTransform;

    public GameObject Granade;
    public GameObject CampFire;

    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsDownHash = Animator.StringToHash("isDown");

    void Update()
    {
        UpdateIsMoving();
    }

    public void ChangeWeapon(PlayerWeapon playerWeapon)
    {
        Debug.Log("changeWeapon");
        RemovePlayerWeapon();
        currentPlayerWeapon = playerWeapon;

        switch (playerWeapon)
        {
            case PlayerWeapon.NoWeapon:

                animator.runtimeAnimatorController = NoWeaponOverride;
                break;

            case PlayerWeapon.TwoHandSword:
                RTwoHandSword.SetActive(true);
                animator.runtimeAnimatorController = TwoHSwordOverride;
                break;

            case PlayerWeapon.OneHandSword:
                ROneHandSword.SetActive(true);
                animator.runtimeAnimatorController = OneHSwordOverride;
                break;

            case PlayerWeapon.Gunlance:
                LShield.SetActive(true);
                RGunlance.SetActive(true);

                animator.runtimeAnimatorController = GunlanceOverride;
                break;

            case PlayerWeapon.Hammer:
                RHammer.SetActive(true);
                animator.runtimeAnimatorController = HammerOverride;
                break;

        }
    }

    private void RemovePlayerWeapon()
    {
        LShield.SetActive(false);

        ROneHandSword.SetActive(false);
        RTwoHandSword.SetActive(false);
        RGunlance.SetActive(false);
        RHammer.SetActive(false);
    }

    private void ShowPlayerWeapon()
    {
        ChangeWeapon(currentPlayerWeapon);
    }

    // ===== 플레이어 이동 애니메이션

    private void UpdateIsMoving()
    {
        if (animator != null)
            animator.SetBool(IsWalkingHash, isMoving);
    }

    // ==== 플레이어 회전

    public void RotateToTile(HexTile tile)
    {
        playerMove.RotateToTile(tile);
    }


    // ===== 스킬 로직

    public void PlayAnimation(int index)
    {
        switch (index)
        {
            case 1:
                animator.SetTrigger("Skill1");
                break;

            case 2:
                animator.SetTrigger("Skill2");
                break;

            case 3:
                animator.SetTrigger("Skill3");
                break;

            default: break;
        }
    }

    // ===== 포션 로직

    public IEnumerator DrinkPotion()
    {
        RemovePlayerWeapon();
        animator.SetTrigger("DrinkPotion");
        yield return new WaitForSeconds(2.3f);
        ShowPlayerWeapon();

        yield return null;
    }

    // ===== 플레이어 다운 로직

    public void SetPlayerDown(bool down)
    {
        animator.SetBool("isPlayerDown", down);
    }

    // ===== 수류탄 로직

    private GameObject currentGranade;

    public void ThrowItem(GranadeType type, HexTile targetTile = null)
    {
        RemovePlayerWeapon();
        RotateToTile(targetTile);
        currentGranade = SpawnGranade(type);
        MoveGranade(targetTile);
        StartCoroutine(ThrowAnimation());
    }

    private GameObject SpawnGranade(GranadeType type)
    {
        Vector3 granadePos = granadeSpawnTransform.transform.position;

        return Instantiate(Granade, new Vector3(granadePos.x, granadePos.y + 1, granadePos.z), quaternion.identity);
    }

    private void MoveGranade(HexTile tile)
    {
        Vector3 granadePos = granadeSpawnTransform.transform.position;
        Vector3 moveVector = tile.transform.position - granadePos;

        moveVector = moveVector.normalized;

        float power = Vector3.Distance(tile.transform.position, playerMove.GetCurrentTile().transform.position);

        currentGranade.GetComponent<Rigidbody>().AddForce(1.5f * power * moveVector, ForceMode.Impulse);

    }

    private IEnumerator ThrowAnimation()
    {
        animator.SetTrigger("Throw");
        yield return new WaitForSeconds(1.5f);
        ShowPlayerWeapon();
        yield return null;
    }

    // ===== 특수 아이템 로직

    private GameObject currentSpecialItem;

    public void UseSpecialItem(SpecialType type, HexTile tile)
    {
        RemovePlayerWeapon();
        RotateToTile(tile);

        StartCoroutine(PlaceItem());
    }

    private IEnumerator PlaceItem()
    {
        animator.SetTrigger("PlaceItem");
        yield return new WaitForSeconds(1.5f);
        ShowPlayerWeapon();
        yield return null;
    }



}
