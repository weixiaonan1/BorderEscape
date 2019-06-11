using UnityEngine;

/// <summary>
/// Simple score manager. Requires SumScoreManager attached to game object in scene.
/// </summary>
public class SumScore {

    public static int Score { get; protected set; }
    public static int HighScore { get; set; }

    private static SumScoreManager mgr; // Easy reference to manager instance

    // Private constructor to ensure only one copy exists
    private SumScore () { }
	
    /// <summary>Adds points to total score</summary>
    /// <remarks>
    /// You can also use a negative number as a shortcut to the Subtract method
    /// </remarks>
    /// <param name="pointsToAdd">Number of points to add</param>
    public static void Add (int pointsToAdd) {
        Debug.Log(pointsToAdd + " points " + ((pointsToAdd > 0) ? "added" : "removed"));
        Score += pointsToAdd; // Add points to current score
        if (MgrSet()) {
            // Make sure we don't go negative unless we're supposed to
            if (Score < 0 && !mgr.allowNegative)
                Score = 0; // Reset score to 0
            mgr.Updated(); // Let the manager know we've changed the score
        }
    }

    /// <summary>Removes points from total score</summary>
    /// <param name="pointsToSubtract">Number of points to remove</param>
    public static void Subtract (int pointsToSubtract) {
        Add(-pointsToSubtract);
    }

    /// <summary>Sets Score to 0 and updates manager</summary>
    public static void Reset () {
        Debug.Log("Reset score");
        Score = 0;
        if(MgrSet()) {
            mgr.Updated();
        }
    }

    /// <summary>Checks and sets references needed for the script</summary>
    /// <returns>True if successful, false if failed</returns>
    static bool MgrSet () {
        if (mgr == null) {
            mgr = SumScoreManager.instance; // Set instance reference
            if (mgr == null) {
                // Throw error message if we can't link
                Debug.LogError("<b>SumScoreManager.instance</b> cannot be found. Make sure object is active in inspector.");
                return false;
            }
        }
        return true;
    }

    /// <summary>Checks score against high score and saves if higher</summary>
    public static void SaveHighScore () {
        if (Score > HighScore) {
            Debug.Log("New high score " + Score);
            HighScore = Score;
            PlayerPrefs.SetInt("sumHS", Score); // Store high score in player prefs
            if (MgrSet())
                mgr.UpdatedHS(); // Notify manager of change
        }
    }

    /// <summary>Reset high score and clear from player prefs</summary>
    public static void ClearHighScore () {
        Debug.Log("Deleting high score");
        PlayerPrefs.DeleteKey("sumHS");
        HighScore = 0;
        if (MgrSet())
            mgr.UpdatedHS(); // Notify manager of change
    }

}
