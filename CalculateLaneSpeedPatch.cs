using CitiesHarmony.API;
using HarmonyLib;
using System;

namespace BusOnlyRoadEnabler
{
	[HarmonyPatch(typeof(PathFind), "CalculateLaneSpeed")]
	class IsCategoryValidPatch
	{
		///
		/// Returns TRUE if the road segment has at least 1 "normal" car lane in the given direction.
		///
		private static bool HasCarLanes(NetInfo.Direction direction, NetSegment segment)
        {
			foreach(var lane in segment.Info.m_lanes)
            {
				if(lane.m_laneType == NetInfo.LaneType.Vehicle && 
				  (lane.m_vehicleType & VehicleInfo.VehicleType.Car) != 0 &&
			      (lane.m_direction & direction) != 0)
                {
					return true;
                }
            }

			return false;
        }

		[HarmonyPostfix]
		public static void Postfix(byte startOffset, byte endOffset, ref NetSegment segment, NetInfo.Lane laneInfo, bool ___m_transportVehicle, ref float __result)
		{
			if (!___m_transportVehicle && // we are a bus
				laneInfo.m_laneType == NetInfo.LaneType.TransportVehicle && // this is a bus lane
				!HasCarLanes(laneInfo.m_direction, segment)) // and this is a bus-only road, at least in this direction
            {
				__result = 0.02f;
            }
		}
	}
}