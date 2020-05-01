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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(20, 30, 120, 70), barsBackgroundTexture);
        GUI.DrawTexture(new Rect(30, 40, unitHpBarLength, 20), hpBarTexture);
        GUI.DrawTexture(new Rect(30, 65, unitManaBarLength, 20), manaBarTexture);

        GUI.Label(new Rect(50, 40, 200, 20), $"{curHp} / {maxHp}");
        GUI.Label(new Rect(50, 65, 200, 20), $"{curMana} / {maxMana}");
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
        else
        {
            base.TryAutoAttack();
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
            selectedUnit.transform.GetComponent<UnitStats>().Selected();
        }
        else if( selectedUnit != null )
        {
            if( didDoubleClick)
            {
                selectedUnit.transform.GetComponent<UnitStats>().Deselected();
            }
        }
    }

    internal override void OnDeath()
    {
        base.OnDeath();
        GetComponent<CharacterController>().SetMovementEnabled(false);
        pnlDeath.SetActive(true);
    }

    internal override void OnRespawn()
    {
        base.OnRespawn();
        pnlDeath.SetActive(false);
        GetComponent<CharacterController>().SetMovementEnabled(true);
    }
}
