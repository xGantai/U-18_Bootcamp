using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Instantiate(CurrentCharacter.Character.CharacterPrefab, new Vector3(0, -2f, 0), Quaternion.identity);
    }
}
