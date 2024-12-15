using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class PlayerStats : UnitStats
{
    //GUI Art
    public Texture hpBarTexture;
    public Texture manaBarTexture;
    public Texture barsBackgroundTexture;

    public GameObject pnlDeath;
    public Button btnRespawn;

    float tarCurHp = 0, tarMaxHp = 0, tarCurMana = 0, tarMaxMana = 0;

    // Start is called before the first frame update
    void Start()
    {
        Type = Constants.GameobjectType.Creature;
    }

    void OnGUI()
    {
        //Player
        GUI.DrawTexture(new Rect(20, 30, 120, 70), barsBackgroundTexture);
        GUI.DrawTexture(new Rect(30, 40, unitHpBarLength, 20), hpBarTexture);
        GUI.DrawTexture(new Rect(30, 65, unitManaBarLength, 20), manaBarTexture);

        GUI.Label(new Rect(50, 40, 200, 20), $"{curHp} / {maxHp}");
        GUI.Label(new Rect(50, 65, 200, 20), $"{curMana} / {maxMana}");


        //Target
        if (HasTarget())
        {
            GUI.DrawTexture(new Rect(220, 30, 120, 70), barsBackgroundTexture);
            GUI.DrawTexture(new Rect(230, 40, unitHpBarLength, 20), hpBarTexture);
            GUI.DrawTexture(new Rect(230, 65, unitManaBarLength, 20), manaBarTexture);

            GUI.Label(new Rect(250, 40, 200, 20), $"{tarCurHp} / {tarMaxHp}");
            GUI.Label(new Rect(250, 65, 200, 20), $"{tarCurMana} / {tarMaxMana}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.ManagePowers();

        if (Input.GetMouseButtonDown(0))
        {
            SelectTarget("enemy", TargetSelectionType.SelectOnly);
        }
        if( Input.GetMouseButtonDown(1))
        {
            SelectTarget("enemy", TargetSelectionType.SelectAndAttack);
        }

        base.ManageSelectedUnit();

        if (Input.GetKeyDown("1"))
        {
            base.TryAttackAbility();
        }
        else if( Input.GetKeyDown("2"))
        {
            base.RangedSpell();
        }
        else
        {
            base.TryAutoAttack();
        }

        if( selectedUnit != null )
        {
            var ai = selectedUnit.GetComponent<CreatureAI>();
            if( ai )
            {
                ai.isSelectedByPlayer = true;
                tarCurHp = ai.curHp;
                tarMaxHp = ai.maxHp;
                tarCurMana = ai.curMana;
                tarMaxMana = ai.maxMana;
            }
            
        }
        else
        {
            tarCurHp = 0;
            tarMaxHp = 0;
            tarCurMana = 0;
            tarMaxMana = 0;
        }
    }

    //Funzione solo per il player perchè utilizza il mouse.
    void SelectTarget(string target_tag, TargetSelectionType selectionType)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        base.SelectTarget(ray, out hit, target_tag, selectionType);

        if( hit.transform.tag == target_tag && selectedUnit != null )
        {
            var creatureAI = selectedUnit.transform.GetComponent<CreatureAI>();
            //unitStatsScript.Selected();

            if( creatureAI != null )
            {
                Debug.Log($"Ho selezionato la creatura con GUID: " + creatureAI.creatureDbInfo.GUID);
            }
        }
        else if( selectedUnit != null )
        {
            if( didDoubleClick)
            {
                selectedUnit.transform.GetComponent<UnitStats>().Deselected();
            }
        }
    }

    public bool HasTarget()
    {
        return selectedUnit != null;
    }

    public void SetTarget(GameObject target)
    {
        selectedUnit = target;
    }

    internal override void OnDeath()
    {
        base.OnDeath();
        GetComponent<PlayerMovement>().SetMovementEnabled(false);
        pnlDeath.SetActive(true);
    }

    internal override void OnRespawn()
    {
        base.OnRespawn();
        pnlDeath.SetActive(false);
        GetComponent<PlayerMovement>().SetMovementEnabled(true);
    }
}
