using Assets.Scripts;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private CharacterName _characterName;

    public CharacterType CharacterType;

    public void Zombify()
    {
        // Change the character type to zombie
        CharacterType = CharacterType.Zombie;
    }

    public CharacterName GetCharacterName()
    {
        return _characterName;
    }
}
