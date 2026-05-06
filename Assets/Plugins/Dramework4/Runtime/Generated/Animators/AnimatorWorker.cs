using System.Collections.Generic;

using System.Linq;

namespace IG.HappyCoder.Dramework4.Runtime.Generated
{
	public static class AnimatorWorker
	{

		public static class Layer
		{
			public const int BaseLayer = 0;
		}

		public static class State
		{
			public const int Idle1 = 244403810;
			public const int Walking = 1724194506;
		}

		public enum States
		{
			Idle1 = 244403810,
			Walking = 1724194506,
		}

		public static class Parameters
		{
			public const int Speed = -823668238;
			public const int IdleID = -1698620609;
			public const int IsCarrying = 2126089053;
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
			(0, "Idle 1", 13.93333f),
			(0, " Walking", 2.866667f),
		};

		 private static readonly IReadOnlyList<(int layerIndex, int stateNameHash, float length)> ClipLengthByNameHash = new List<(int, int, float)>
		{
			(0, 244403810, 13.93333f),
			(0, 1724194506, 2.866667f),
		};
	}
}