using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VoteButton : MonoBehaviour
{
    [HideInInspector] public ulong target;
    [HideInInspector] public GameController game;

    public Button button;
    public Image targetImage;
    public TMP_Text nameText;
    public TMP_Text votesReceivedCountText;
    public GameObject finishedVotingIndicator;
    public Transform votesReceivedLayoutGroup;

    private int votesReceivedCount = 0;
    private int votesCastCount = 0;

    public void ResetVoteCountAndState()
    {
        votesReceivedCount = 0;
        votesReceivedCountText.text = votesReceivedCount.ToString();
        votesReceivedCountText.gameObject.SetActive(game.settings.anonymousVotes);
        votesCastCount = 0;
        if (finishedVotingIndicator)
            finishedVotingIndicator.SetActive(false);
        if (votesReceivedLayoutGroup != null)
            foreach (Transform child in votesReceivedLayoutGroup)
                Destroy(child.gameObject);
        button.interactable = false;
    }

    public void Vote()
    {
        game.handler.link.Send(new PlayerVoted { target = target });
    }

    public void VoteReceived(GameObject prefab, SpriteRenderer sprite)
    {
        votesReceivedCountText.text = (++votesReceivedCount).ToString();
        if (votesReceivedLayoutGroup != null)
        {
            GameObject go = Instantiate(prefab, votesReceivedLayoutGroup);
            Image image = go.GetComponent<Image>();
            image.sprite = sprite.sprite;
            image.color = sprite.color;
        }
    }

    public void VoteCast()
    {
        if (++votesCastCount == game.settings.votesPerPlayer)
            if (finishedVotingIndicator)
                finishedVotingIndicator.SetActive(true);
    }
}
