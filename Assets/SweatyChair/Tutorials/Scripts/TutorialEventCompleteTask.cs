using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Reflection;

namespace SweatyChair
{
	
	/// <summary>
	/// Complete when a event fired.
	/// </summary>
    public class TutorialEventCompleteTask : TutorialTask
	{

		public string targetPath;
		public string targetComponentName;
		public string fieldName;
		public string eventName;

		public override bool Init()
		{
			if (string.IsNullOrEmpty(targetPath)) {
				Debug.LogErrorFormat("{0}:{1}:Init - Target path not set", name, GetType());
				return false;
			}

			Transform targetTF = TransformUtils.GetTransformByPath(targetPath);
			if (targetTF == null) {
				Debug.LogErrorFormat("{0}:{1}:Init - Target transform not found, targetPath={2}", name, GetType(), targetPath);
				return false;
			}

			if (string.IsNullOrEmpty(targetComponentName)) {
				Debug.LogErrorFormat("{0}:{1}:Init - Target component not set", name, GetType());
				return false;
			}

			System.Type targetComponentType = System.Type.GetType(targetComponentName);

			if (targetComponentType == null) {
				Debug.LogErrorFormat("{0}:{1}:Init - Target component type not found, targetComponentName={2}", name, GetType(), targetComponentName);
				return false;
			}

			Component targetC = targetTF.GetComponent(targetComponentName) as Component;
			if (targetC == null) {
				Debug.LogErrorFormat("{0}:{1}:Init - Target component not found, targetComponentType={2}", name, GetType(), targetComponentType);
				return false;
			}

			UnityAction handler = DoComplete;

			if (string.IsNullOrEmpty(fieldName)) {

				try {
					EventInfo ei = targetComponentType.GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
					ei.AddEventHandler(targetC, handler);
				} catch {
					Debug.LogWarningFormat("{0}:{1}:Init - Action not found, targetC={2}, actionName={3}", name, GetType(), targetC, eventName);
					return false;
				}

			} else {

				object obj;

				try {
					obj = (object)targetComponentType.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(targetC);
				} catch {
					Debug.LogWarningFormat("{0}:{1}:Init - Target field not found, targetC={2}, fieldName={3}", name, GetType(), targetC, fieldName);
					return false;
				}

				try {
					EventInfo ei = obj.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance);
					ei.AddEventHandler(obj, handler);
				} catch {
					Debug.LogWarningFormat("{0}:{1}:Init - Event not found, targetC={2}, obj={3}, eventName={4}", name, GetType(), targetC, obj, eventName);
					return false;
				}

			}

			return true;
		}

	}

}