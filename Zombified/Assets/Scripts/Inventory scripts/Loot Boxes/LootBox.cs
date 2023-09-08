using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    public ItemManager itemManager;

    private void Start()
    {

        droppedLoot();

    }
    List<ScriptableObject> droppedLoot()
    {
        int amountDropped = Random.Range(1, 5);

        List<ScriptableObject> droppingLoot = new List<ScriptableObject>();

        for (int i = 0; i < amountDropped; i++)
        {
            int dropChance = Random.Range(1, 101);

            if (dropChance <= 15)
            {
                medPackStats med = itemManager.GetItem<medPackStats>("MedPack");
                droppingLoot.Add(med);
            }
            else if(dropChance <= 25)
            {
                ammoBoxStats ammo = itemManager.GetItem<ammoBoxStats>("Ammo");
                droppingLoot.Add(ammo);
            }
            else if (dropChance <= 30)
            {
                WeaponStats snipe = itemManager.GetItem<WeaponStats>("Sniper Rifle");
                droppingLoot.Add(snipe);
            }
            else if (dropChance <= 40)
            {
                WeaponStats shotgun = itemManager.GetItem<WeaponStats>("Shotgun");
                droppingLoot.Add(shotgun);
            }
            else if (dropChance <= 50)
            {
                WeaponStats assualtRifle = itemManager.GetItem<WeaponStats>("Assualt Rifle");
                droppingLoot.Add(assualtRifle);
            }
            else if (dropChance <= 60)
            {
                WeaponStats ak = itemManager.GetItem<WeaponStats>("AK-47");
                droppingLoot.Add(ak);
            }
        }
        return droppingLoot;
    }
}
