using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;
using UnityEngine.EventSystems;

public class VoteButton : MonoBehaviour, IPointerEnterHandler
{
    [HideInInspector] public ulong target;
    [HideInInspector] public GameController game;
    private bool currentEvidence = false;
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
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Evidence != null && currentEvidence == false)
        {
            currentEvidence = true;
            int e = (int)Evidence.GetComponent<VoterEvidence>().myEvidence;
            if (e == 1)
            {
                Debug.Log("EVIDENCE 1 cursor.");

                byte[] ba = Evidence.GetComponent<VoterEvidence>().ba;
                SendEvidenceEvent sendEvidenceEvent = new SendEvidenceEvent();
                sendEvidenceEvent.byteArray = ba;
                sendEvidenceEvent.Evidence = e;
                sendEvidenceEvent.positionOfTarget = transform.position;
                EventCallbacks.EventSystem.Current.FireEvent(EVENT_TYPE.SHOW_EVIDENCE, sendEvidenceEvent);
            }
        }       
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        currentEvidence = false;
    }

    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Setup || pc.phase == GamePhase.Main)
        {
            currentEvidence = false;
        }
    }
}
