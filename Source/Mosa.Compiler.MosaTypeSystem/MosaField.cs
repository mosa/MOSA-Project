using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaField
	{
		public MosaType FieldType { get; internal set; }

		public MosaType DeclaringType { get; internal set; }

		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public IList<MosaAttribute> CustomAttributes { get; internal set; }

		public bool IsLiteralField { get; internal set; }

		public bool IsStaticField { get; internal set; }

		public int RVA { get; internal set; }

		public MosaField()
		{
			CustomAttributes = new List<MosaAttribute>();
		}

		public override string ToString()
		{
			return FieldType.Name + " " + FullName;
		}

		public MosaField Clone(MosaType declaringType)
		{
			var cloneField = Clone();
			cloneField.DeclaringType = declaringType;

			if (this.FieldType.IsVarFlag || this.FieldType.IsMVarFlag)
			{
				cloneField.FieldType = declaringType.GenericTypes[this.FieldType.VarOrMVarIndex];
			}

			return cloneField;
		}

		private MosaField Clone()
		{
			var field = new MosaField();
			field.FieldType = this.FieldType;
			field.DeclaringType = this.DeclaringType;
			field.Name = this.Name;
			field.FullName = this.FullName;
			field.CustomAttributes = this.CustomAttributes;
			field.IsLiteralField = this.IsLiteralField;
			field.IsStaticField = this.IsStaticField;
			field.RVA = field.RVA;

			return field;
		}
	}
}