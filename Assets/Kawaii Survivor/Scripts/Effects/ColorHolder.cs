using System;
using UnityEngine;

public class ColorHolder : MonoBehaviour
{
    public static ColorHolder Instance;
   
    [Header("Elements")]
    [SerializeField] private PaletteSO palette;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Color GetColor(int level)
    {
        level = Mathf.Clamp(level, 0, Instance.palette.LevelColors.Length);
        return Instance.palette.LevelColors[level];
    }
    
    public Color GetOutlineColor(int level)
    {
        level = Mathf.Clamp(level, 0, Instance.palette.LevelOutlineColors.Length);
        return Instance.palette.LevelOutlineColors[level];
    }
}
