using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SW_Interfaces : MonoBehaviour
{
    public interface IDamageable
    {
        void Damage(float damageTaken);
    }
}
