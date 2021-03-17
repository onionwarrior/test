using System;
using System.Collections.Generic;
using Enemy;
using SceneManagement;
using UnityEngine;
using NonPlayableCharacters;
namespace QuestSystem
{
    public enum QuestStatus
    {
        NotStarted,
        Started,
        Done,
        Finished
    }
    public delegate void QuestReward(GameObject player);
    public class Quest
    {
        private readonly int _id;
        private string Description { get; set; }
        private GameObject _questGiver;
        public QuestStatus Status
        {
            get;
            private set;
        } = QuestStatus.NotStarted;
        public Quest(string description, GameObject questGiver, QuestReward questReward,int id)
        {
            this._id = id;
            Description = description;
            this._questGiver = questGiver;
            QuestReward = questReward;
        }
        private QuestReward QuestReward { get; set; }
        public static void TakeQuest( GameObject questOwner,Quest newQuest)
        {
            
            SceneManager.Instance.currentPlayerQuests.Add(newQuest._id,newQuest);
            newQuest.Status = QuestStatus.Started;
        }

        public static void DoQuest(GameObject questHolder)
        {
            var qId = questHolder.GetComponent<BotsAI>().questId;
            SceneManager.Instance.currentPlayerQuests[qId].Status = QuestStatus.Done;
        }

        public static void CompleteQuest( GameObject questOwner)
        {
            int thisQuestId = questOwner.GetComponent<NPC>().associatedQuestId;
            Dictionary<int, Quest> quests = SceneManager.Instance.currentPlayerQuests;
            if (quests[thisQuestId].Status != QuestStatus.Done)
                throw new NotImplementedException("Add some ui or dialogue");
            else
            {
                quests[thisQuestId].QuestReward(SceneManager.Instance.player.gameObject);
            }
            quests[thisQuestId].Status = QuestStatus.Finished;
        }
    }
}