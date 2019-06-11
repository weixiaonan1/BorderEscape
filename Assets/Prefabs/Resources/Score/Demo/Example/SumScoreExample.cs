using UnityEngine;

/// <summary>
/// Examples for usage of SumScore and SumScoreManager
/// </summary>
public class SumScoreExample : MonoBehaviour
{

    bool timed = false;

    /// <summary>
    /// Example of how to add points from a game object.
    /// </summary>
    /// <remarks>Can call from button in inspector</remarks>
    /// <param name="points">Number of points to add (negative to subtract)</param>
	public void AddPoints(int points)
    {
        SumScore.Add(points);
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Example of how to subtract points from a game object.
    /// </summary>
    /// <remarks>Can call from button in inspector</remarks>
    /// <param name="points">Number of points to subtract from score</param>
    public void SubtractPoints(int points)
    {
        SumScore.Add(-points);
        // NOTE - You can also use SumScore.Subtract(points) if you like typing
    }

    /// <summary>
    /// Toggles adding score based on time
    /// </summary>
    /// <remarks>Can call from button in inspector.</remarks>
    public void ToggleTimed()
    {
        timed = !timed;
    }

    /// <summary>Resets score to zero</summary>
    /// <remarks>Can call from button in inspector</remarks>
    public void ResetPoints()
    {
        SumScore.Reset();
    }

    /// <summary>Save if current score is greater than high score</summary>
    public void CheckHighScore()
    {
        if (SumScore.Score > SumScore.HighScore)
            SumScore.SaveHighScore();
    }

    /// <summary>Resets high score to zero</summary>
    /// <remarks>Can call from button in inspector</remarks>
    public void ClearHighScore()
    {
        SumScore.ClearHighScore();
    }

    void Update()
    {
        if (timed)
            // Use Time.deltaTime to create a steady addition of points.
            // This example would add 100 points per second
            SumScore.Add(Mathf.RoundToInt(Time.deltaTime * 100));
    }
}
