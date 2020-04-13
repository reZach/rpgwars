﻿namespace GameCreator.Behavior
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Core;

	[AddComponentMenu("")]
	public class ActionForceUpdateBehavior : IAction
	{
        public TargetGameObject behavior = new TargetGameObject(TargetGameObject.Target.GameObject);

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            Behavior _behavior = this.behavior.GetComponent<Behavior>(target);
            if (_behavior != null) _behavior.Execute();

            return true;
        }

		#if UNITY_EDITOR
        public static new string NAME = "Behavior/Update Behavior";
        public const string CUSTOM_ICON_PATH = "Assets/Plugins/GameCreator/Behavior/Icons/Actions/";
        private const string NODE_TITLE = "Force update Behavior {0}";

        public override string GetNodeTitle()
        {
            return string.Format(
                NODE_TITLE,
                this.behavior
            );
        }
        #endif
    }
}
