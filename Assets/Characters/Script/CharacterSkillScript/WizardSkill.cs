using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WizardSkill : MonoBehaviour
{
    [HideInInspector] public PlayerMovement PlayerMove;
    [HideInInspector] public PlayerBody Body;
    [HideInInspector] public PlayerAttack AttackCollider;
    [SerializeField] private CharactersSO MyCharacter;
    [SerializeField] private Image Cooltime1;
    [SerializeField] private Image Activetime1;
    [SerializeField] private Image Cooltime2;
    [SerializeField] private Image Activetime2;
    [SerializeField] private Collider2D SkillCollider;
    private SkillTimeSystem Active1;
    private SkillTimeSystem Active2;
    private SkillTimeSystem Active3;
    private bool Skill2Press;
    public GameObject NearestEnemy;
    public GameObject TempNearestEnemy;

    private void Start()
    {
        PlayerMove = GetComponent<PlayerMovement>();
        Body = GetComponentInChildren<PlayerBody>();
        AttackCollider = GetComponentInChildren<PlayerAttack>();
        Active1 = new SkillTimeSystem();
        Active2 = new SkillTimeSystem();
        Active3 = new SkillTimeSystem();
        NearestEnemy = null;
        TempNearestEnemy = null;
    }

    private void Update()
    {
        Active1Time();
        Active2Time();
        Active3Time();
        Skill2SelectedCharacter();
    }

    void OnSkill1()
    {
        if (Active1.CoolDown <= 0)
        {
            Skill1Performed();
            Active1.CoolDown = MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillCoolDown;
            Active1.ActiveTime = MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillActiveTime;
        }
    }
    void OnSkill2(InputValue Value)
    {
        if (Active2.CoolDown <= 0)
            if (Value.isPressed)
            {
                Skill2Press = true;
            }
            else
            {
                Skill2Press = false;
                Skill2Performed();
                Active2.CoolDown = MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillCoolDown;
                Active2.ActiveTime = MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillActiveTime;
                Active2.WorkOnce = false;
            }
    }
    void OnSkill3()
    {

    }

    private void Active1Time()
    {
        if (Active1.CoolDown > 0)
        {
            Active1.CoolDown -= Time.deltaTime;
            Cooltime1.fillAmount = Active1.CoolDown / MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillCoolDown;
        }
    }

    private void Active2Time()
    {
        if (Active2.ActiveTime > 0)
        {
            Active2.ActiveTime -= Time.deltaTime;
            Activetime2.fillAmount = Active2.ActiveTime / MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillActiveTime;
        }
        else
        {
            if (!Active2.WorkOnce)
            {
                //Skill2Performed(FinishSkill);
                Active2.ActiveTime = 0;
                Active2.WorkOnce = true;
            }
            if (Active2.CoolDown > 0)
            {
                Active2.CoolDown -= Time.deltaTime;
                Cooltime2.fillAmount = Active2.CoolDown / MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillCoolDown;
            }
        }
    }

    private void Active3Time()
    {
        if (Active3.ActiveTime > 0)
        {
            Active3.ActiveTime -= Time.deltaTime;
            Activetime1.fillAmount = Active3.ActiveTime / MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillActiveTime;
        }
        else
        {
            if (!Active3.WorkOnce)
            {
                Skill3Performed();
                Active3.ActiveTime = 0;
                Active3.WorkOnce = true;
            }
            if (Active3.CoolDown > 0)
            {
                Active3.CoolDown -= Time.deltaTime;
                Cooltime1.fillAmount = Active3.CoolDown / MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillCoolDown;
            }
        }
    }



    void Skill1Performed()
    {
        Vector2 SkillPosition = new Vector2(transform.position.x + (PlayerMove.PlayerDirection.x * 5f), transform.position.y + 1.5f);
        GameObject Skill1 = Instantiate(MyCharacter.SkillTree.Active1.SkillPrefabs, SkillPosition, Quaternion.identity);
        Skill1.GetComponent<SkillShotSC>().Power = MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillPower * 20;
        Skill1.GetComponent<SkillShotSC>().SkillSpeed = Skill1.GetComponent<SkillShotSC>().SkillSpeed * PlayerMove.PlayerDirection.x;
        Skill1.GetComponent<SkillShotSC>().SkillDirection = PlayerMove.PlayerDirection;
    }

    void Skill2Performed()
    {
        Debug.Log("Test");
    }
    void Skill3Performed()
    {

    }

    void Skill2SelectedCharacter()
    {
        if (Skill2Press)
        {
            float NearestDistance = float.MaxValue;
            Collider2D[] HitColliders;
            NearestEnemy = null;
            HitColliders = new Collider2D[25];
            ContactFilter2D ContactFilter = new ContactFilter2D();
            ContactFilter.useTriggers = true;
            ContactFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
            int HitCount = Physics2D.OverlapCollider(SkillCollider, ContactFilter, HitColliders);
            if (TempNearestEnemy != null && NearestEnemy != TempNearestEnemy)
            {
                TempNearestEnemy.GetComponent<SpriteRenderer>().color = Color.white;
                TempNearestEnemy = null;
            }

            if (HitCount > 0)
            {
                for (int i = 0; i < HitCount; i++)
                {
                    if (Vector2.Distance(transform.position, HitColliders[i].transform.position) < NearestDistance)
                    {
                        NearestDistance = Vector2.Distance(transform.position, HitColliders[i].transform.position);
                        NearestEnemy = HitColliders[i].gameObject;
                        TempNearestEnemy = NearestEnemy;
                    }
                }
                if (NearestEnemy != null)
                {
                    NearestEnemy.GetComponent<SpriteRenderer>().color = Color.red;
                    for (int i = 0; i < HitCount; i++)
                    {
                        if (HitColliders[i].gameObject != NearestEnemy)
                            HitColliders[i].GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }
        else
        {
            if (NearestEnemy != null)
            {
                NearestEnemy.GetComponent<SpriteRenderer>().color = Color.white;
                NearestEnemy = null;
                TempNearestEnemy = null;
            }
        }

    }
}
