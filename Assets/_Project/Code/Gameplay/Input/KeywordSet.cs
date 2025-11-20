using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeywordSet", menuName = "Scriptable Objects/KeywordSet")]
public class KeywordSet : ScriptableObject
{
    [System.Serializable]
    public class Set
    {
        public string Onomatopoeia;
        public List<string> Words;

        [HideInInspector]
        public HashSet<string> matchLookup;

        public void SetupLookup()
        {
            matchLookup = new HashSet<string>();

            foreach (string word in Words)
                matchLookup.Add(word.ToLower());
        }
    }

    public List<Set> keywordSets;


    public void Initialize()
    {
        if (keywordSets.Count == 0) { Debug.LogError("KeywordSet Empty"); return; }

        if (keywordSets[0].matchLookup != null) return;

        foreach (Set set in keywordSets)
            set.SetupLookup();
    }

    public string GetOnomatopoeia(string word)
    {
        Initialize();

        string lowerCase = word.ToLower();

        foreach(Set set in keywordSets)
        {
            if (set.matchLookup.Contains(lowerCase))
                return set.Onomatopoeia;
        }

        return null;
    }

    public string GetOnomatopoeia(int index)
    {
        Initialize();

        if (index < 0 || index >= keywordSets.Count)
            return null;

        return keywordSets[index].Onomatopoeia;
    }

    public int GetIndex(string word)
    {
        Initialize();

        string lowerCase = word.ToLower();

        for (int i = 0; i < keywordSets.Count; i++)
        {
            if (keywordSets[i].matchLookup.Contains(lowerCase))
                return i;
        }

        return -1;
    }
}
