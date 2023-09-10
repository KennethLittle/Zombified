using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Buying 
{
    void BuyingItem(ItemType type);
    bool TrytoBuy(int coins);
}
