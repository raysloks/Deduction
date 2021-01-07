using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;
using UnityEngine.EventSystems;

public class VoteButton : MonoBehaviour, IPointerEnterHandler
{
    [HideInInspector] public ulong target;
    [HideInInspector] public GameController game;
    [HideInInspector] public bool currentEvidence = false;
    public Button button;
    public Image targetImage;
    public TMP_Text nameText;
    public TMP_Text votesReceivedCountText;
    public GameObject finishedVotingIndicator;
    public Transform votesReceivedLayoutGroup;
    public GameObject deadIndicator;
    public GameObject confirmedIndicator;
    public GameObject Evidence;

    private int votesReceivedCount = 0;
    private int votesCastCount = 0;
    byte[] picture = null;

    void Start()
    {
        EventCallbacks.EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }

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
        if (confirmedIndicator != null)
            confirmedIndicator.SetActive(false);
    }

    public void ConfirmVote()
    {
        Button confirmButton = game.CreateConfirmPopup("Vote for " + nameText.text + "?");
        confirmButton.onClick.AddListener(Vote);
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
    

    //if pointer enters send event to CurrentlyVisibleEvidence
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Evidence != null && currentEvidence == false)
        {
            Vector2 smaller = new Vector2(1f, 1f);
            Vector2 bigger = new Vector2(1.2f, 1.2f);

            foreach (Transform child in transform.parent)
            {
                child.GetComponent<VoteButton>().currentEvidence = false;
                child.localScale = smaller;
            }
            gameObject.transform.localScale = bigger;

            currentEvidence = true;
            VoterEvidence.Evidence e = Evidence.GetComponent<VoterEvidence>().myEvidence;

            Evidence.GetComponent<VoterEvidence>().newEvidence.SetActive(false);

            SendEvidenceEvent sendEvidenceEvent = new SendEvidenceEvent();
            sendEvidenceEvent.Evidence = (int)e;
            sendEvidenceEvent.positionOfTarget = transform.position;
            sendEvidenceEvent.gc = game;

            switch (e)
            {
                case VoterEvidence.Evidence.None:
                    break;
                case VoterEvidence.Evidence.Picture:
                    sendEvidenceEvent.photoIndex = Evidence.GetComponent<VoterEvidence>().photoIndex;
                    break;
                case VoterEvidence.Evidence.MotionSensor:
                    sendEvidenceEvent.MotionSensorEvidence = Evidence.GetComponent<VoterEvidence>().ms;
                    break;
            }

            EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.SHOW_EVIDENCE, sendEvidenceEvent);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentEvidence = false;
    }

    //End of meeting cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Setup || pc.previous == GamePhase.EndOfMeeting)
        {
            currentEvidence = false;
        }
    }

}
