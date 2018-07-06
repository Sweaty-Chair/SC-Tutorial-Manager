using UnityEngine;
using System.Collections.Generic;

namespace SweatyChair
{
	
	public abstract class Panel : MonoBehaviour
	{

		// First come, last serve
		private static Stack<Panel> _shownPanels = new Stack<Panel>();

		[SerializeField] private bool _hasAndroidBackClick = false;

		protected bool _initialized = false;

		public static Stack<Panel> shownPanels {
			get { return _shownPanels; }
		}

		public bool isShown {
			get { return gameObject.activeSelf; }
		}

		public static bool IsNoPanelShownExcept<T>()
		{
			foreach (Panel panel in _shownPanels) {
				if (panel is T)
					continue;
				return false;
			}
			return true;
		}

		public static bool IsPanelShown<T>()
		{
			foreach (Panel panel in _shownPanels) {
				if (panel is T)
					return true;
			}
			return false;
		}

		public static bool IsPanelOnTop<T>()
		{
			return _shownPanels.Peek() is T;
		}

		protected virtual void Awake()
		{
			StateManager.stateChangedEvent += OnStateChange;
		}

		protected virtual void OnDestroy()
		{
			StateManager.stateChangedEvent -= OnStateChange;
		}

		public virtual void Init()
		{
		}

		public virtual void Reset()
		{
		}

		protected virtual void OnStateChange(State state)
		{
		}

		public virtual void OnAndroidBackClick()
		{
			if (_hasAndroidBackClick)
				OnBackClick();
		}

		public virtual void OnBackClick()
		{
			Hide();
		}

		public virtual void Show()
		{
			Toggle(true);
		}

		public virtual void Hide()
		{
			Toggle(false);
		}

		public virtual void Toggle(bool doShow)
		{
			gameObject.SetActive(doShow);

			if (_hasAndroidBackClick) {
				if (doShow) {
					if (_shownPanels.Count == 0 || _shownPanels.Peek() != this)
						_shownPanels.Push(this);
				} else if (_shownPanels.Count > 0 && _shownPanels.Peek() == this) {
					_shownPanels.Pop();
				}
			}
		}

		public static void HideCurrent()
		{
			if (_shownPanels.Count > 0)
				_shownPanels.Pop().Hide();
		}

		#if UNITY_EDITOR

		[UnityEditor.MenuItem("Debug/Panel/Print Current Panels", false, 100)]
		public static void PrintCurrentPanels()
		{
			DebugUtils.LogCollection(_shownPanels);
		}

		[ContextMenu("Print Path")]
		public void PrintPath()
		{
			Debug.Log(transform.ToPath());
		}

		[ContextMenu("Show")]
		public void DebugShow()
		{
			Show();
		}

		[ContextMenu("Hide")]
		public void DebugHide()
		{
			Hide();
		}

		#endif

	}

}