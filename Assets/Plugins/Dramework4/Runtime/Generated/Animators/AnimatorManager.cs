using System.Collections.Generic;

using System.Linq;

namespace IG.HappyCoder.Dramework4.Runtime.Generated
{
	public static class AnimatorManager
	{

		public static class Layer
		{
			public const int BaseLayer = 0;
			public const int TestLayer = 1;
		}

		public static class State
		{
			public const int Idle = 2081823275;
			public const int UseTablet2 = -353194247;
			public const int UseTablet1 = 1945894723;
			public const int Walk = 765711723;
		}

		public enum States
		{
			Idle = 2081823275,
			UseTablet2 = -353194247,
			UseTablet1 = 1945894723,
			Walk = 765711723,
		}

		public static class Parameters
		{
			public const int Speed = -823668238;
		}

		 public static float GetClipLength(int layerIndex, string stateName)
		{
			return ClipLengthByName.FirstOrDefault(i => i.layerIndex == layerIndex && i.stateName == stateName).length;
		}

		 public static float GetClipLength(int layerIndex, int stateNameHash)
		{
			return ClipLengthByNameHash.FirstOrDefault(i => i.layerIndex == layerIndex && i.stateNameHash == stateNameHash).length;
		}

		 private static readonly IReadOnlyList<(int layerIndex, string stateName, float length)> ClipLengthByName = new List<(int, string, float)>
		{
			(0, "Idle", 6.666667f),
			(0, "Use Tablet 2", 6.666667f),
			(0, "Use Tablet 1", 6.666667f),
			(0, "Walk", 0.9666667f),
		};

		 private static readonly IReadOnlyList<(int layerIndex, int stateNameHash, float length)> ClipLengthByNameHash = new List<(int, int, float)>
		{
			(0, 2081823275, 6.666667f),
			(0, -353194247, 6.666667f),
			(0, 1945894723, 6.666667f),
			(0, 765711723, 0.9666667f),
		};
	}
}