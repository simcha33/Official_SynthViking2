using System;

namespace RayFire
{
	[Serializable]
	public class RFFragmentProperties
	{
		public RFColliderType colliderType;
		public float sizeFilter;
		public bool decompose;
		public bool removeCollinear;
		public string layer;

		/// /////////////////////////////////////////////////////////
        /// Constructor
        /// /////////////////////////////////////////////////////////
		
		// Constructor
		public RFFragmentProperties()
		{
			colliderType = RFColliderType.Mesh;
			sizeFilter   = 0;
			decompose    = false;

			removeCollinear = false;
			layer           = "";
		}

		// Copy from
		public void CopyFrom (RFFragmentProperties fragmentProperties)
		{
			colliderType = fragmentProperties.colliderType;
			sizeFilter   = fragmentProperties.sizeFilter;
			decompose    = false;

			removeCollinear = fragmentProperties.removeCollinear;
			layer           = fragmentProperties.layer;
		}
	}
}