using UnityEngine;

[CreateAssetMenu()]
public class UnitData : ScriptableObject
{
    [System.Serializable]
    public struct ResCost
    {
        public string resource;
        public int cost;
    }

    [System.Serializable]
    public struct Unit
    {
        public string name;
        public ResCost[] resourceCosts;
        public float constructionTime;
        public Sprite icon;
        public GameObject prefab;
    }

    public Unit[] units;
}