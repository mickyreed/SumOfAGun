using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounterGUIControl : MonoBehaviour
{
    public TMP_Text weaponNameText;
    public TMP_Text ammoTypeText;
    public TMP_Text currentAmmoText;
    int currentMaxCapacity = 0;

    public void InitialiseGUI(GunData weapon, int currentAmmo)
    {
        weaponNameText.text = weapon.name;
        ammoTypeText.text = weapon.ammoType.name;
        currentMaxCapacity = weapon.ammoType.maximumCapacity;
        UpdateAmmo(currentAmmo);
    }

    public void UpdateAmmo(int currentAmmo)
    {
        string currentAmmoString = currentAmmo.ToString();
        string currentMaxAmmoString = currentMaxCapacity.ToString();

        // 099/100
        // 05/10
        // 3 - 2
        // 1
        // ('0' *1) + "99" = "099"

        if(currentMaxAmmoString.Length > currentAmmoString.Length)
        {
            currentAmmoString = new string('0', currentAmmoString.Length - currentAmmoString.Length) + currentAmmoString;
        }

        currentAmmoText.text = currentAmmoString + "/" + currentMaxAmmoString;
    }

}
