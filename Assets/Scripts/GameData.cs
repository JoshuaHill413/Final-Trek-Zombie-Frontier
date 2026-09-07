public static class GameData
{
    // Trail progress
    public static int   CityIndex        = 0;
    public static float ScrollOffset     = 0f;

    // Resources
    public static int Food = 100;
    public static int Gas  = 100;

    // Special items
    public static bool HasSuperPart      = false;
    public static int  LastBreakdownCity = -99;
    public static int  LastChoicesCity   = -1;

    public static void Reset()
    {
        CityIndex        = 0;
        ScrollOffset     = 0f;
        Food             = 100;
        Gas              = 100;
        HasSuperPart     = false;
        LastBreakdownCity = -99;
        LastChoicesCity   = -1;
    }

    public static void Save()
    {
        UnityEngine.PlayerPrefs.SetInt  ("cityIndex",         CityIndex);
        UnityEngine.PlayerPrefs.SetFloat("scrollOffset",      ScrollOffset);
        UnityEngine.PlayerPrefs.SetInt  ("food",              Food);
        UnityEngine.PlayerPrefs.SetInt  ("gas",               Gas);
        UnityEngine.PlayerPrefs.SetInt  ("hasSuperPart",      HasSuperPart ? 1 : 0);
        UnityEngine.PlayerPrefs.SetInt  ("lastBreakdownCity", LastBreakdownCity);
        UnityEngine.PlayerPrefs.SetInt  ("lastChoicesCity",   LastChoicesCity);
        UnityEngine.PlayerPrefs.Save();
    }

    public static void Load()
    {
        CityIndex         = UnityEngine.PlayerPrefs.GetInt  ("cityIndex",         0);
        ScrollOffset      = UnityEngine.PlayerPrefs.GetFloat("scrollOffset",      0f);
        Food              = UnityEngine.PlayerPrefs.GetInt  ("food",              100);
        Gas               = UnityEngine.PlayerPrefs.GetInt  ("gas",              100);
        HasSuperPart      = UnityEngine.PlayerPrefs.GetInt  ("hasSuperPart",      0) == 1;
        LastBreakdownCity = UnityEngine.PlayerPrefs.GetInt  ("lastBreakdownCity", -99);
        LastChoicesCity   = UnityEngine.PlayerPrefs.GetInt  ("lastChoicesCity",   -1);
    }

    public static bool HasSave()
    {
        return UnityEngine.PlayerPrefs.HasKey("cityIndex");
    }
}