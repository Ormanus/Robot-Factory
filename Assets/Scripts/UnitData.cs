using UnityEngine;

[CreateAssetMenu()]
public class UnitData : ScriptableObject
{
    [System.Serializable]
    public struct Unit
    {
        public string name;
        public int metalCost;
        public int padsCost;
        public int electricityCost;
        public int constructionTime;
        public GameObject prefab;
    }

    public Unit[] units;
}
