using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Rune Sprite Disambiguator", fileName = "newRuneSpriteDisambiguator.asset")]
public class RuneSpriteDisambiguator : ScriptableObject
{
    public Sprite GeboSprite, JeraSprite, OthalaSprite, AnsuzSprite;

    public Sprite GetSpriteByType (RuneType type)
    {
        switch (type)
        {
            case RuneType.Gebo:
                return GeboSprite;

            case RuneType.Jera:
                return JeraSprite;

            case RuneType.Othala:
                return OthalaSprite;

            case RuneType.Ansuz:
                return AnsuzSprite;

            default:
                throw new ArgumentException($"unexpected rune type {type}");
        }
    }
}
