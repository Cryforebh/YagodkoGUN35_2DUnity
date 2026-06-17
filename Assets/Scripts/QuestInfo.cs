using Netologia.Quest.Characters;

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Netologia.Quest
{
	[Serializable]
	public class QuestInfo
	{
		public enum Status : byte
		{
			New,
			Progress,
			Complete
		}

        public uint[] AllStageQuest = {0, 10, 20};
		private uint _currentStageQuest;
		private uint _maxStage = 0;

		[HideInInspector]
		public Status State;

		public bool СonditionLaser = false;
		public Receiver ReceiverTarget;
        public Character Target;
		public List<MovementPointData> NewIdlePointForTargetOnQuestComplete;
		public string Short;
		public string Question;
		public string Answer;

		public override string ToString()
			=> $"{Target.Name} Status: {State}";

		public bool IsContitionComplete
		{
			get
			{
				if (ReceiverTarget)
					return ReceiverTarget.IsActive;
				return true;
			}
		}

        public void SetStage(uint numStage)
		{
			if (State == Status.Complete) return;

            uint maxStage = SerchMaxStage();

            if (numStage >= maxStage)
            {
                _currentStageQuest = maxStage;
                State = Status.Complete;
            }
            else
            {
                foreach (uint stage in AllStageQuest)
                {
                    if (stage == numStage)
					{
                        _currentStageQuest = numStage;
                        State = _currentStageQuest == 0 ? Status.New : Status.Progress;
						return;
                    }
                }
            }
        }

		public void NextStage()
		{
            if (State == Status.Complete) return;

            uint currentStage = GetStage();

			foreach (uint stage in AllStageQuest)
			{
                if (stage > GetStage())
				{
					if (СonditionLaser && State == Status.Progress)
						if (!IsContitionComplete)
							return;
                    SetStage(stage);
					return;
                }
            }
        }

		public void SetComplete() => SetStage(SerchMaxStage());

		public uint GetStage() => _currentStageQuest;

		private uint SerchMaxStage()
		{
			if (_maxStage > 0) return _maxStage;

            _maxStage = AllStageQuest[0];
            foreach (var stage in AllStageQuest)
            {
                if (_maxStage < stage)
                    _maxStage = stage;
            }
			return _maxStage;
        }
    }
}