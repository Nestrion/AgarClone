using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class LeaderboardTable : MonoBehaviour
{
    /// <summary>
    /// The entry container
    /// </summary>
    private Transform entryContainer;
    /// <summary>
    /// The entry template
    /// </summary>
    private Transform entryTemplate;
    /// <summary>
    /// The leaderboard entry transform list
    /// </summary>
    private List<Transform> leaderboardEntryTransformList;

    /// <summary>
    /// The template offset
    /// </summary>
    private float templateOffset = 2f;
    /// <summary>
    /// The template height
    /// </summary>
    private float templateHeight = 22f;

    // TODO: should be a reference to a manager which stores this number
    /// <summary>
    /// The base score
    /// </summary>
    public int baseScore = 0;

    /// <summary>
    /// The local player entry name
    /// </summary>
    public string LocalPlayerEntryName = null;

    // cached from AddLocalPlayerEntryTransform
    /// <summary>
    /// The local player entry transform
    /// </summary>
    private Transform localPlayerEntryTransform;

    /// <summary>
    /// The maximum highscores
    /// </summary>
    private static int maximumHighscores = 10;

    /// <summary>
    /// The local player added
    /// </summary>
    private bool localPlayerAdded = false;

    /// <summary>
    /// Updates this instance.
    /// </summary>
    private void Update()
    {
        if (LocalPlayerEntryName != null && !localPlayerAdded)
        {
            AddLocalPlayerEntry();
            localPlayerAdded = true;
        }
        UpdateLocalPlayerEntryTransformPositionBelowTop();
    }

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    private void Awake()
    {
        entryContainer = transform.Find("LeaderboardEntryContainer");
        entryTemplate = entryContainer.Find("LeaderboardEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        leaderboardEntryTransformList = new List<Transform>();
    }

    /// <summary>
    /// Updates the name of the entry by player.
    /// </summary>
    /// <param name="playerName">Name of the player.</param>
    /// <param name="newScore">The new score.</param>
    public void UpdateEntryByPlayerName(string playerName, int newScore)
    {
        Transform entry = leaderboardEntryTransformList.Find(x => x.GetComponent<LeaderboardEntry>().PlayerName == playerName);
        if (entry != null)
        {
            UpdateEntry(entry, newScore);
        }
    }

    /// <summary>
    /// Updates the entry for local player.
    /// </summary>
    /// <param name="newScore">The new score.</param>
    public void UpdateEntryForLocalPlayer(int newScore)
    {
        UpdateEntryByPlayerName(LocalPlayerEntryName, newScore);
    }

    /// <summary>
    /// Updates the entry.
    /// </summary>
    /// <param name="entryTransform">The entry transform.</param>
    /// <param name="newScore">The new score.</param>
    private void UpdateEntry(Transform entryTransform, int newScore)
    {
        int idx = leaderboardEntryTransformList.IndexOf(entryTransform);
        int maxIdx = leaderboardEntryTransformList.Count - 1;
        int minIdx = 0;
        List<Transform> list = leaderboardEntryTransformList;
        LeaderboardEntry entry = entryTransform.GetComponent<LeaderboardEntry>();
        int oldScore = entry.Score;
        entry.Score = newScore;

        int swapIdx;
        if (oldScore > newScore)
        {
            if (idx + 1 <= maxIdx)
            {
                while (newScore < list[idx + 1].GetComponent<LeaderboardEntry>().Score)
                {
                    swapIdx = idx + 1;
                    Transform cached = list[idx];
                    list[idx] = list[swapIdx];
                    list[swapIdx] = cached;
                    UpdateTransformPosition(list[idx]);
                    UpdateTransformPosition(list[swapIdx]);
                    SetEntryVisiblity(list[idx]);
                    SetEntryVisiblity(list[swapIdx]);

                    if (idx + 1 < maxIdx)
                    {
                        idx++;
                    }
                    else
                        break;
                }
            }

        }
        else if (oldScore < newScore)
        {
            if (idx - 1 >= minIdx)
            {
                while (newScore > list[idx - 1].GetComponent<LeaderboardEntry>().Score)
                {
                    swapIdx = idx - 1;
                    Transform cached = list[idx];
                    list[idx] = list[swapIdx];
                    list[swapIdx] = cached;
                    UpdateTransformPosition(list[idx]);
                    UpdateTransformPosition(list[swapIdx]);
                    SetEntryVisiblity(list[idx]);
                    SetEntryVisiblity(list[swapIdx]);

                    if (idx - 1 > minIdx)
                    {
                        idx--;
                    }
                    else
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Adds the entry.
    /// </summary>
    /// <param name="name">The name.</param>
    public void AddEntry(string name)
    {
        Transform entryTransform = CreateLeaderboardEntryTransform(baseScore, name);
        SetNewEntryTransformPosition(entryTransform);
        SetEntryVisiblity(entryTransform);
    }

    /// <summary>
    /// Adds the local player entry.
    /// </summary>
    private void AddLocalPlayerEntry()
    {
        Transform entryTransform = CreateLeaderboardEntryTransform(baseScore, LocalPlayerEntryName);
        SetNewEntryTransformPosition(entryTransform);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("PlaceText").GetComponent<TMP_Text>().color = Color.yellow;
        entryTransform.Find("PlayerNameText").GetComponent<TMP_Text>().color = Color.yellow;

        localPlayerEntryTransform = entryTransform;
    }

    /// <summary>
    /// Creates the leaderboard entry transform.
    /// </summary>
    /// <param name="score">The score.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    private Transform CreateLeaderboardEntryTransform(int score, string name)
    {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        LeaderboardEntry leaderboardEntry = entryTransform.GetComponent<LeaderboardEntry>();
        leaderboardEntry.Score = score;
        leaderboardEntry.PlayerName = name;
        return entryTransform;
    }

    // new entries are added to the back of the list
    /// <summary>
    /// Sets the new entry transform position.
    /// </summary>
    /// <param name="entryTransform">The entry transform.</param>
    private void SetNewEntryTransformPosition(Transform entryTransform)
    {
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * leaderboardEntryTransformList.Count - templateOffset);
        entryTransform.gameObject.SetActive(true);

        int rank = leaderboardEntryTransformList.Count + 1;
        string rankString = rank.ToString() + ".";

        entryTransform.Find("PlaceText").GetComponent<TMP_Text>().text = rankString;
        entryTransform.Find("PlayerNameText").GetComponent<TMP_Text>().text = entryTransform.GetComponent<LeaderboardEntry>().PlayerName;

        leaderboardEntryTransformList.Add(entryTransform);
    }

    /// <summary>
    /// Updates the transform position.
    /// </summary>
    /// <param name="entryTransform">The entry transform.</param>
    private void UpdateTransformPosition(Transform entryTransform)
    {
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * leaderboardEntryTransformList.IndexOf(entryTransform) - templateOffset);

        int rank = leaderboardEntryTransformList.IndexOf(entryTransform) + 1;
        string rankString = rank.ToString() + ".";

        entryTransform.Find("PlaceText").GetComponent<TMP_Text>().text = rankString;
        entryTransform.Find("PlayerNameText").GetComponent<TMP_Text>().text = entryTransform.GetComponent<LeaderboardEntry>().PlayerName;
    }

    /// <summary>
    /// Updates the local player entry transform position below top.
    /// </summary>
    private void UpdateLocalPlayerEntryTransformPositionBelowTop()
    {
        if (localPlayerEntryTransform != null)
        {
            if (leaderboardEntryTransformList.IndexOf(localPlayerEntryTransform) > maximumHighscores)
            {
                localPlayerEntryTransform.gameObject.SetActive(true);
                RectTransform entryRectTransform = localPlayerEntryTransform.GetComponent<RectTransform>();
                entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * maximumHighscores - templateOffset);
            }
        }
    }

    /// <summary>
    /// Sets the entry visiblity.
    /// </summary>
    /// <param name="entry">The entry.</param>
    private void SetEntryVisiblity(Transform entry)
    {
        if (leaderboardEntryTransformList.IndexOf(entry) >= maximumHighscores)
        {
            entry.gameObject.SetActive(false);
        }
        else
        {
            entry.gameObject.SetActive(true);
        }
    }
}
