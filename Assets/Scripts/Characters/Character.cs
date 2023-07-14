using Assets.Scripts;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private CharacterName _characterName;
    [SerializeField]
    private Levels Level;
    public CharacterType CharacterType;

    public CharacterName GetCharacterName()
    {
        return _characterName;
    }
}
