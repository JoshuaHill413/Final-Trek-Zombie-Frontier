using UnityEngine;

public class Josh_GasRewardSystem : MonoBehaviour
{
    public int gasMin = 15;
    public int gasMax = 30;
    private Josh_DifficultyScaler difficultyScaler;

    [System.Serializable]
    public class LossInfo
    {
        public int amount;
        public string resource;
    }

    void Awake()
    {
        difficultyScaler = GetComponent<Josh_DifficultyScaler>();
    }

    public int GetGasReward()
    {
        int gasGained = Random.Range(gasMin, gasMax + 1);
        Debug.Log("Player gained " + gasGained + " gas!");
        return gasGained;
    }

    public LossInfo GetScaledLoss()
{
    float progression = difficultyScaler.progression;
    int lossAmount = Mathf.RoundToInt(Mathf.Lerp(10, 50, progression));

    Debug.Log("Player lost " + lossAmount + " Gas!");

    return new LossInfo { amount = lossAmount, resource = "Gas" };
}

    // Keep old methods for compatibility
    public void GiveGasReward() { GetGasReward(); }
    public void ApplyScaledLoss() { GetScaledLoss(); }
}