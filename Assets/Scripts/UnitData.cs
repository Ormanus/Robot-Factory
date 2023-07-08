using UnityEngine;

[CreateAssetMenu()]
public class UnitData : ScriptableObject
{
    [System.Serializable]
    public struct Unit
    {
        public string name;
        public (string, int)[] resourceCosts;
        public float constructionTime;
        public Sprite icon;
        public GameObject prefab;
    }

    public Unit[] units;
}
