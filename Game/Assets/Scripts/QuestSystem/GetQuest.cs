using SceneManage;
using UnityEngine;
using NonPlayableCharacters;
namespace QuestSystem
{
    [RequireComponent(typeof(NPC))]
    public class GetQuest : MonoBehaviour
    {
        private NPC _npc;
        private void Awake()
        {
            _npc = GetComponent<NPC>();
        }

        // Update is called once per frame
        private void OnMouseDown()
        {
            if (_npc.associatedQuestId < 0)
            {
                print("Квест уже взят и выполнен");
                return;
            }

            if (!SceneManager.Instance.currentPlayerQuests.ContainsKey(_npc.associatedQuestId))
            {
                Quest.TakeQuest(gameObject,
                    new Quest("MASAYA", gameObject,
                        (GameObject player) => print($"Хорошая работа,{player.name}"), _npc.associatedQuestId));
                print("Квест взят");
            }
            else 
                print("Квест уже взят");
            if (SceneManager.Instance.currentPlayerQuests.ContainsKey(_npc.associatedQuestId) &&
                SceneManager.Instance.currentPlayerQuests[_npc.associatedQuestId].Status == QuestStatus.Done)
            {
                Quest.CompleteQuest(gameObject);
                _npc.associatedQuestId = -1;
            }
        }
    }
}
