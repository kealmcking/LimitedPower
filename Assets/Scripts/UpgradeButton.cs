using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public List<Upgrade> upgrades;
    public Upgrade buttonUpgrade;
    public GameManager manager;

    public TMP_Text nameText, descriptionText;

    bool hasRandomizedUpgrade;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetRandomUpgrade() {
        int randomUpgrade = Random.Range(0, upgrades.Count);
        buttonUpgrade = upgrades[randomUpgrade];
        nameText.text = buttonUpgrade.m_name;
        descriptionText.text = buttonUpgrade.m_description;
        hasRandomizedUpgrade = true;
    }

    public void SetSelectedUpgrade() {
        manager.selectedUpgrade = buttonUpgrade;
    }
}
