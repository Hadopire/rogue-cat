using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Oracle : Unit {

    public void initialize(Cart startPos, int health, int damage)
    {
        base.unitInitialize(startPos, health, damage);
    }
}
