using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private List<Character> _charactersPrefabs;

    public static CharactersManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // Loop over all CharacterNames enum and set the values of those to null
    }

    public GameObject GetPrefab(CharacterName characterName)
    {
        var prefab = _charactersPrefabs.Find(character => character.GetCharacterName() == characterName).gameObject;
        return prefab.gameObject;
    }
}