using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SweatyChair
{

	// Author: RV Sarmiento 23/05/2017
	// Version 1.0
	public static class RandomUtils
	{
		#region Object

		public class SCRandom
		{
			public int seed { get; private set; }

			public Random.State state { get; private set; }

			public SCRandom(int seed)
			{
				this.seed = seed;
				Random.InitState(this.seed);
				state = Random.state;
			}

			public void Init()
			{
				Random.InitState(seed);
				SaveState();
			}

			public void SaveState()
			{
				state = Random.state;
			}
		}

		#endregion

		private static Dictionary<int, SCRandom> _randomStateDict = new Dictionary<int, SCRandom>();
		private static SCRandom _current = null;

		#region Init

		/// <summary>
		/// Initialize the random seed back to first step. 
		/// </summary>
		/// <param name="seed">Seed.</param>
		public static void InitState(int seed)
		{
			if (!CheckAddSeed(seed))
				_randomStateDict[seed].Init();
		}

		#endregion

		#region Random Range Integer

		/// <summary>
		/// Generates a random int number from minimum to maximum using a seed.
		/// </summary>
		/// <param name="seed">Seed.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public static int Range(int seed, int min, int max)
		{
			CheckAddSeed(seed);
			return Range(_randomStateDict[seed], min, max);
		}

		// Generates a random int number without using any seed.
		public static int Range(int min, int max)
		{
			return Range(null, min, max);
		}

		// Returns a random float number
		private static int Range(SCRandom scRandom, int min, int max)
		{
			int randNumber = 0;

			// Opt not to use random seed
			if (scRandom == null) {
				// Save the current Random Seed state being use
				if (_current != null)
					_current.SaveState();

				_current = null;
				// Use the time as a seed to generate a pseudo true random number
				Random.InitState((int)System.DateTime.Now.Ticks);
				randNumber = Random.Range(min, max);
			} else {
				// Set the state for the random number
				Random.state = scRandom.state;

				// Set as current random seed
				_current = scRandom;
				randNumber = Random.Range(min, max);
				_current.SaveState();
			}

			return randNumber;
		}

		#endregion

		#region Random Range Float

		/// <summary>
		/// Generates a random float number from minimum to maximum using a seed.
		/// </summary>
		/// <param name="seed">Seed.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public static float Range(int seed, float min, float max)
		{
			CheckAddSeed(seed);
			return Range(_randomStateDict[seed], min, max);
		}

		// Generates a random float number without using any seed.
		public static float Range(float min, float max)
		{
			return Range(null, min, max);
		}

		// Returns a random float number
		private static float Range(SCRandom scRandom, float min, float max)
		{
			float randNumber = 0;

			// Opt not to use random seed
			if (scRandom == null) {
				// Save the current Random Seed state being use
				if (_current != null)
					_current.SaveState();

				_current = null;
				// Use the time as a seed to generate a pseudo true random number
				Random.InitState((int)System.DateTime.Now.Ticks);
				randNumber = Random.Range(min, max);
			} else {
				// Set the state for the random number
				Random.state = scRandom.state;

				// Set as current random seed
				_current = scRandom;
				randNumber = Random.Range(min, max);
				_current.SaveState();
			}

			return randNumber;
		}

		#endregion

		#region Random Float Value from 0.0 to 1.0

		/// <summary>
		/// Generates a random float number from 0.0 to 1.0 using a seed.
		/// </summary>
		/// <param name="seed">Seed.</param>
		/// <param name="min">Minimum.</param>
		/// <param name="max">Maximum.</param>
		public static float Value(int seed)
		{
			CheckAddSeed(seed);
			return Value(_randomStateDict[seed]);
		}

		public static float Value()
		{
			return Value(null);
		}

		private static float Value(SCRandom scRandom)
		{
			float randNumber = 0;

			// Opt not to use random seed
			if (scRandom == null) {
				// Save the current Random Seed state being use
				if (_current != null)
					_current.SaveState();

				_current = null;
				// Use the time as a seed to generate a pseudo true random number
				Random.InitState((int)System.DateTime.Now.Ticks);
				randNumber = Random.value;
			} else {
				// Set the state for the random number
				Random.state = scRandom.state;

				// Set as current random seed
				_current = scRandom;
				randNumber = Random.value;
				_current.SaveState();
			}

			return randNumber;
		}

		#endregion

		// Returns true if it successfully added a SCRandom otherwise false if it has already been added before.
		private static bool CheckAddSeed(int seed)
		{
			if (!_randomStateDict.ContainsKey(seed)) {
				_randomStateDict.Add(seed, new SCRandom(seed));
				return true;
			}

			return false;
		}

		public static void SaveCurrentState()
		{
			if (_current == null)
				return;
			_current.SaveState();
		}

		#region Random Name

		private static NameGenerator _firstNameNG, _lastNameNG;

		public static string GetRandomName(int length)
		{
			if (_firstNameNG == null)
				_firstNameNG = new NameGenerator(
					"mr,dr,lord,phd,lazy,lonely,happy,chessy,easy,cyan,thesis,banana,potato," +
					"creative,sweaty,chair,zhu,simple,test,galaxy,best,great,awesome,fanta,pudge,cpu,smart,clever,handsome,doctor," +
					"brian,richard,john,derek,rey,saymiento,zhang,michael,kinky,lovely,cute,killer,master,pro,love,boring,ex,pink,dark,trump,hilary," +
					"thank,you,yester,morning,crazy,insane,fancy,mr,load,go,war,tank,battle,money,13,25,66,87,47,90,2312,chicka,cow,milk,baby,fat," +
					"slow,poke,harambe,gorilla,stupid,king,wings,~~,--,***,o_O,Orz,@_@,air,monkey,jelly,miss,lady,ashley,chloe,jessie,mike,wonder," +
					"ipeng,jasmine",
					2, 6);
			if (_lastNameNG == null)
				_lastNameNG = new NameGenerator(
					"boy,girl,player,dota,man,mon,meth,69,86,2016,2015,777,888,444,1337,orange,apple," +
					"tofu,ski,bomber,king,ghost,rider,001,chair,pants,mayor,killer,flagger,carry,sadist,pianist,^_^,nose,armpit,nails,hair,wood," +
					"missy,chibi,misty,lily,koala,helen,tina,wang,wrong,connie,jojo,peanut,butter,jam,exam,pizza,zero,extra",
					2, 3);
			string fName = _firstNameNG.NextName;
			string lName = Random.value > 0.5f ? _lastNameNG.NextName : "";
			if ((fName + lName).Length > length)
				return fName;
			return fName + lName;
		}

		#endregion

		#region Random List

		public static List<int> GetRandomIntList(int length, int min = 0, int max = 100)
		{
			List<int> result = new List<int>();
			for (int i = 0; i < length; i++)
				result.Add(Random.Range(min, max));
			return result;
		}

		public static List<int> GetRandomIntListRandomLength(int minLength, int maxLength, int min = 0, int max = 100)
		{
			return GetRandomIntList(Random.Range(minLength, maxLength));
		}

		#endregion

		#region Random Dictionary

		public static Dictionary<int, int> GetRandomIntDictionary(int length, int minKey = 0, int maxKey = 100, int minValue = 0, int maxValue = 100)
		{
			Dictionary<int, int> result = new Dictionary<int, int>();
			for (int i = 0; i < length; i++) {
				int key = Random.Range(minKey, maxKey);
				if (result.ContainsKey(key)) {
					i--;
					continue;
				}
				result.Add(key, Random.Range(minValue, maxValue));
			}
			return result;
		}

		public static Dictionary<int, int> GetRandomIntDictionaryRandomLength(int minLength, int maxLength, int minKey = 0, int maxKey = 100, int minValue = 0, int maxValue = 100)
		{
			return GetRandomIntDictionary(Random.Range(minLength, maxLength), minKey, maxKey, minValue, maxValue);
		}

		#endregion

		public static T WeightedRandom<T>(Dictionary<T, int> dict) { return WeightedRandom(dict, System.Environment.TickCount); }
		public static T WeightedRandom<T>(Dictionary<T, int> dict, int seed)
		{
			System.Random _Random = new System.Random(seed);
			int sum = dict.Sum(x => x.Value);
			int random = _Random.Next(sum);
			int count = 0;
			foreach (var dictEntry in dict) {
				count += dictEntry.Value;
				if (random <= count)
					return dictEntry.Key;
			}
			return default(T);
		}

		public static void FlipACoin(System.Action OnHeadsAction, System.Action OnTailsAction, float headsFavour = 0.5f)
		{
			if (headsFavour > 1 || headsFavour < 0) {
				Debug.LogError("[RandomUtils] headsFavour should be a number between 0 and 1.");
			}

			if (Random.Range(0f, 1f) <= headsFavour) {
				OnHeadsAction();
			} else {
				OnTailsAction();
			}
		}

		#region RandomRange Vectors

		public static Vector3 Range(Vector3 minRange, Vector3 maxRange)
		{
			return new Vector3(Random.Range(minRange.x, maxRange.x), Random.Range(minRange.y, maxRange.y), Random.Range(minRange.z, maxRange.z));	
		}

		public static Vector2 Range(Vector2 minRange, Vector2 maxRange)
		{
			return new Vector2(Random.Range(minRange.x, maxRange.x), Random.Range(minRange.y, maxRange.y));
		}

		#endregion

	}

}