using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public class PipelineStageOrder
	{
		/// <summary>
		/// 
		/// </summary>
		public enum Location
		{
			/// <summary>
			/// 
			/// </summary>
			Before,
			/// <summary>
			/// 
			/// </summary>
			After,
			/// <summary>
			/// 
			/// </summary>
			First,
			/// <summary>
			/// 
			/// </summary>
			Last,
			/// <summary>
			/// 
			/// </summary>
			Any
		};

		/// <summary>
		/// 
		/// </summary>
		public Location Position;
		/// <summary>
		/// 
		/// </summary>
		public Type StageType;

		/// <summary>
		/// Initializes a new instance of the <see cref="PipelineStageOrder"/> class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="stageType">Type of the stage.</param>
		public PipelineStageOrder(Location position, Type stageType)
		{
			Position = position;
			StageType = stageType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PipelineStageOrder"/> class.
		/// </summary>
		/// <param name="position">The position.</param>
		public PipelineStageOrder(Location position)
		{
			Position = position;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return Position.ToString() + " " + StageType.ToString();
		}

		/// <summary>
		/// Creates the pipeline order.
		/// </summary>
		/// <param name="after">The after.</param>
		/// <param name="before">The before.</param>
		/// <returns></returns>
		public static PipelineStageOrder[] CreatePipelineOrder(Type after, Type before)
		{
			PipelineStageOrder[] pipeline = new PipelineStageOrder[2];
			pipeline[0] = new PipelineStageOrder(Location.After, after);
			pipeline[1] = new PipelineStageOrder(Location.Before, before);
			return pipeline;
		}

	}
}
