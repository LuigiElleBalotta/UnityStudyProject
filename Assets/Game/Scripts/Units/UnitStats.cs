using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class UnitStats : MonoBehaviour
{
    public string unitName;
    public int level;
    public string unitClass;

    public float curHp;
    public float maxHp;
    public float curMana;
    public float maxMana;

    public float baseAttackPower;
    public float curAttackPower;
    public float baseAttackSpeed;
    public float curAttackSpeed;
    public float baseDodge;
    public float curDodge;
    public float baseHitPercent;
    public float curHitPercent;

    public float hpRegenTimer;
    public float hpRegenAmount;
    public float manaRegenTimer;
    public float manaRegenAmount;

    public float curXp;
    public float maxXp;

    public bool isDead;

    public GameObject selectedUnit;
    public UnitStats enemyStatsScript;

    public bool behindEnemy;
    public bool canAttack;

    public float autoAttackCooldown;
    public float autoAttackCurTime;
    public bool canAutoAttack;

    public float doubleClickTimer;
    public bool didDoubleClick;

    public LayerMask RaycastLayers;
    public bool inLineOfSight;

    public bool hoverOverActive;
    public string hoverName;

    public float TickTime;

    public GameObject RangedSpellPrefab;

    //USER GUI Bars stats
    public float unitHpBarLength;
    public float percentOfUnitHp;
    public float unitManaBarLength;
    public float percentOfUnitMana;

    //Shaders - non sono sicuro vada bene qui
    //public Shader shader1;
    //public Shader shader2;
    //public Renderer rend;

    public GameobjectType Type;

    //Positions: non sono sicuro vada bene qui
    public Vector3 respawnPosition = Vector3.zero;

    public bool _isInCombat {  get
        {
            return selectedUnit != null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"THIS particular script is on {gameObject.name}");

        //rend = GetComponent<Renderer>();
        //shader1 = Shader.Find("Legacy Shaders/Diffuse");
        //shader2 = Shader.Find("Legacy Shaders/Self-Illumin/Diffuse");

        if (respawnPosition == Vector3.zero)
        {
            respawnPosition = GetComponent<Rigidbody>().position;
            Debug.Log("Saved respawn position.");
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        ManagePowers();


        ManageSelectedUnit();
    }

    internal void SelectTarget(Ray ray, out RaycastHit hit, string target_tag, TargetSelectionType selectionType)
    {
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.transform.tag == target_tag)
            {
                selectedUnit = hit.transform.gameObject;
                enemyStatsScript = selectedUnit.transform.gameObject.transform.GetComponent<UnitStats>();

                if( selectionType == TargetSelectionType.SelectOnly && selectedUnit == null)
                {
                    canAutoAttack = false;
                }
                else if( selectionType == TargetSelectionType.SelectAndAttack )
                {
                    canAutoAttack = true;
                }
            }
            else
            {
                if( selectedUnit != null )
                {
                    if( !didDoubleClick )
                    {
                        didDoubleClick = true;
                        doubleClickTimer = 0.3f;
                    }
                    else
                    {
                        Debug.Log("DESELECT");
                        selectedUnit = null;
                        didDoubleClick = false;
                        doubleClickTimer = 0;
                        autoAttackCurTime = 0;
                    }
                }
            }
        }
    } 

    internal void BasicAttack()
    {
        if( enemyStatsScript )
            enemyStatsScript.ReceiveDamage(10);
    }

    internal void ManagePowers()
    {
        //Unit bars calculated
        if (curHp <= maxHp)
        {
            percentOfUnitHp = curHp / maxHp;
            unitHpBarLength = percentOfUnitHp * 100;
            percentOfUnitMana = curMana / maxMana;
            unitManaBarLength = percentOfUnitMana * 100;
        }

        //Make sure mana and hp doesn't exceed max values
        if (curHp > maxHp)
            curHp = maxHp;

        if (curMana > maxMana)
            curMana = maxMana;

        //Make sure values can't go below 0
        if (curHp < 0)
            curHp = 0;

        if (curMana < 0)
            curMana = 0;
    }

    internal void ManageSelectedUnit()
    {
        if (selectedUnit != null)
        {
            Vector3 toTarget = (selectedUnit.transform.position - transform.position).normalized;

            //check if unit is behind his enemy (calculate dodge, parry, extra damage ecc.. )
            if (Vector3.Dot(toTarget, selectedUnit.transform.forward) < 0)
                behindEnemy = false;
            else
                behindEnemy = true;

            //Calculate if the unit is facing the enemy and is within attack distance.
            float distance = Vector3.Distance(this.transform.position, selectedUnit.transform.position);
            Vector3 targetDir = selectedUnit.transform.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(targetDir, forward);

            if (angle > 60.0)
            {
                //TODO canAngleAttack change
                canAttack = false;
                autoAttackCurTime = 0;
            }
            else
            {
                if( distance < 60 )
                {
                    canAttack = true;
                }
                else
                {
                    canAttack = false;
                    autoAttackCurTime = 0;
                }
            }

            //double click detect
            if( doubleClickTimer > 0)
            {
                doubleClickTimer -= Time.deltaTime;
            }
            else
            {
                didDoubleClick = false;
            }
        }

        TryAutoAttack();
    }

    //L'autoattack viene eseguito se non è stato premuto nessun bottone ma è possibile attaccare.
    internal void TryAutoAttack()
    {
        if( selectedUnit != null && canAttack && canAutoAttack )
        {
            if( autoAttackCurTime < autoAttackCooldown )
            {
                autoAttackCurTime += Time.deltaTime;
            }
            else
            {
                BasicAttack();
                autoAttackCurTime = 0;
            }
        }
    }

    //Per i giocatori viene usato quando si preme un bottone per una spell.
    internal void TryAttackAbility()
    {
        if (selectedUnit != null && canAttack)
        {
            BasicAttack();
            canAutoAttack = true;
        }
            
    }

    public void ReceiveDamage( float damage )
    {
        curHp -= damage;

        Debug.Log($"[{gameObject.name}] Current HP: { curHp }");
    }

    public void Selected()
    {
        //rend.material.shader = shader2;
    }

    public void Deselected()
    {
        //rend.material.shader = shader1;
    }

    public bool IsAlive()
    {
        return curHp > 0;
    }

    internal virtual void OnDeath()
    {

    }

    internal virtual void OnRespawn()
    {

    }
}
