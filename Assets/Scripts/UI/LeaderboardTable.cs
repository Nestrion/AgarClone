using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> leaderboardEntryTransformList;

    private float templateOffset = 2f;
    private float templateHeight = 22f;

    // TODO: should be a reference to a manager which stores this number
    public int baseScore = 0;

    public string LocalPlayerEntryName = null;

    // cached from AddLocalPlayerEntryTransform
    private Transform localPlayerEntryTransform;

    private static int maximumHighscores = 10;

    private bool localPlayerAdded = false;

    private void Update()
    {
        if (LocalPlayerEntryName != null && !localPlayerAdded)
        {
            AddLocalPlayerEntry();
            localPlayerAdded = true;
        }
        UpdateLocalPlayerEntryTransformPositionBelowTop();
    }

    private void Awake()
    {
        entryContainer = transform.Find("LeaderboardEntryContainer");
        entryTemplate = entryContainer.Find("LeaderboardEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        leaderboardEntryTransformList = new List<Transform>();
    }

    public void UpdateEntryByPlayerName(string playerName, int newScore)
    {
        Transform entry = leaderboardEntryTransformList.Find(x => x.GetComponent<LeaderboardEntry>().PlayerName == playerName);
        if (entry != null)
        {
            UpdateEntry(entry, newScore);
        }
    }

    public void UpdateEntryForLocalPlayer(int newScore)
    {
        UpdateEntryByPlayerName(LocalPlayerEntryName, newScore);
    }

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

    public void AddEntry(string name)
    {
        Transform entryTransform = CreateLeaderboardEntryTransform(baseScore, name);
        SetNewEntryTransformPosition(entryTransform);
        SetEntryVisiblity(entryTransform);
    }

    private void AddLocalPlayerEntry()
    {
        Transform entryTransform = CreateLeaderboardEntryTransform(baseScore, LocalPlayerEntryName);
        SetNewEntryTransformPosition(entryTransform);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("PlaceText").GetComponent<TMP_Text>().color = Color.yellow;
        entryTransform.Find("PlayerNameText").GetComponent<TMP_Text>().color = Color.yellow;

        localPlayerEntryTransform = entryTransform;
    }

    private Transform CreateLeaderboardEntryTransform(int score, string name)
    {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        LeaderboardEntry leaderboardEntry = entryTransform.GetComponent<LeaderboardEntry>();
        leaderboardEntry.Score = score;
        leaderboardEntry.PlayerName = name;
        return entryTransform;
    }

    // new entries are added to the back of the list
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

    private void UpdateTransformPosition(Transform entryTransform)
    {
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * leaderboardEntryTransformList.IndexOf(entryTransform) - templateOffset);

        int rank = leaderboardEntryTransformList.IndexOf(entryTransform) + 1;
        string rankString = rank.ToString() + ".";

        entryTransform.Find("PlaceText").GetComponent<TMP_Text>().text = rankString;
        entryTransform.Find("PlayerNameText").GetComponent<TMP_Text>().text = entryTransform.GetComponent<LeaderboardEntry>().PlayerName;
    }

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
