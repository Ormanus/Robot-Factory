using Outloud.Common;
using UnityEngine;

public class ExplosionFactory : MonoBehaviour
{
    static ExplosionFactory _instance;

    public GameObject explosionPrefab;
    public string[] sounds;

    private void Awake()
    {
        _instance = this;
    }

    public static void CreateBoom(Vector2 pos, float scale = 1f, string soundID = "explosion")
    {
        AudioManager.PlaySound(soundID);

        var obj = Instantiate(_instance.explosionPrefab, pos, Quaternion.identity);
        obj.transform.localScale = Vector3.one * scale;
    }
}
