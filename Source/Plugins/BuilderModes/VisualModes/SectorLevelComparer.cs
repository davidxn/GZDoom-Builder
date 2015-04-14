﻿#region === Copyright (c) 2010 Pascal van der Heiden ===

using System;
using System.Collections.Generic;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	internal class SectorLevelComparer : IComparer<SectorLevel>
	{
		// Center of sector to use for plane comparison
		public Vector2D center;
		
		// Constructor
		public SectorLevelComparer(Sector s)
		{
			this.center = new Vector2D(s.BBox.Left + s.BBox.Width / 2, s.BBox.Top + s.BBox.Height / 2);
		}

		// Comparer. -1 = x is less than y.
		public int Compare(SectorLevel x, SectorLevel y)
		{
			if(x == y) return 0; //mxd
			float diff = (float)Math.Round(x.plane.GetZ(center) - y.plane.GetZ(center), 3);
			if(diff == 0)
			{
				bool xislight = (x.type == SectorLevelType.Light || x.type == SectorLevelType.Glow);
				bool yislight = (y.type == SectorLevelType.Light || y.type == SectorLevelType.Glow);
				
				//mxd. Push light levels above floor and ceiling levels when height is the same
				if(!xislight) return (yislight ? -1 : 0);
				if(!yislight) return 1;

				//mxd. Push light levels without lighttype (those should be lower levels of type 1 Transfer Brightness effect) above other ones
				if(x.lighttype == y.lighttype) return 0; //TODO: how this should be handled?
				if(x.lighttype == LightLevelType.TYPE1_BOTTOM) return 1;
				return -1;
			}

			return Math.Sign(diff);
		}
	}
}
