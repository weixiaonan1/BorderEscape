using UnityEngine;
using UnityEngine.UI;

/// <summary>Manager for SumScore accessible from inspector</summary>
/// <remarks>
/// Attach to game object in scene. 
/// This is a singleton so only one instance can be active at a time.
/// </remarks>
public class SumScoreManager : MonoBehaviour {

    public static SumScoreManager instance = null;  // Static instance for singleton

    public int initialScore = 0;
    public bool storeHighScore = true, allowNegative = true;
    public Text field; // Text field displaying current score
    public Text highScoreField; // Text field displaying high score

    void Awake() {
        // Ensure only one instance is running
        if (instance == null)
            instance = this; // Set instance to this object
        else
            Destroy(gameObject); // Kill yo self
        // Make sure the linked references didn't go missing
        if (field == null)
            Debug.LogError("Missing reference to 'field' on <b>SumScoreManager</b> component");
        if (storeHighScore && highScoreField == null)
            Debug.LogError("Missing reference to 'highScoreField' on <b>SumScoreManager</b> component");
    }

    void Start() {
        SumScore.Reset(); // Ensure score is 0 when object loads
        if (initialScore != 0)
            SumScore.Add(initialScore);  // Set initial score
        if (storeHighScore) {
            if (PlayerPrefs.HasKey("sumHS")) { 
                // Set high score value and tell manager
                SumScore.HighScore = PlayerPrefs.GetInt("sumHS");
                UpdatedHS();
            }
            else
                SumScore.HighScore = 0;
        }

        Updated(); // Set initial score in UI
    }

    /// <summary>Notify this manager of a change in score</summary>
    public void Updated () {
        field.text = SumScore.Score.ToString("0"); // Post new score to text field
    }

    /// <summary>Notify this manager of a change in high score</summary>
    public void UpdatedHS () {
        if(storeHighScore)
            highScoreField.text = SumScore.HighScore.ToString("0"); // Post new high score to text field
    }


}
