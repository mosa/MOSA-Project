// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Collections.Generic
{
	[Serializable]
	public class CollectionsException : Exception
	{
		public byte ErrorCode { get; }
		public string ErrorText { get; }
		public string SourceCodeFileName { get; }
		public string ClassName { get; }
		public string FunctionName { get; }
		public string ParameterName { get; }

		public CollectionsException()
		{
		}

		public CollectionsException(string Message) : base(Message)
		{
		}

		public CollectionsException(string Message, Exception Inner) : base(Message, Inner)
		{
		}

		public CollectionsException(byte ErrorCode, string ErrorText, string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(Message)
		{
			this.ErrorCode = ErrorCode;
			this.ErrorText = ErrorText;
			this.SourceCodeFileName = SourceCodeFileName;
			this.ClassName = ClassName;
			this.FunctionName = FunctionName;
			this.ParameterName = ParameterName;
		}
	}

	[Serializable]
	public class CollectionsUnknownErrorException : CollectionsException
	{
		public CollectionsUnknownErrorException() : base()
		{
		}

		public CollectionsUnknownErrorException(string Message) : base(Message)
		{
		}

		public CollectionsUnknownErrorException(string Message, Exception Inner) : base(Message, Inner)
		{
		}

		public CollectionsUnknownErrorException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x01, "Unknown Error", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message)
		{
		}
	}

	[Serializable]
	public class CollectionsDataNullException : CollectionsException
	{
		public CollectionsDataNullException() : base()
		{
		}

		public CollectionsDataNullException(string Message) : base(Message)
		{
		}

		public CollectionsDataNullException(string Message, Exception Inner) : base(Message, Inner)
		{
		}

		public CollectionsDataNullException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x02, "Data Is Null", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message)
		{
		}
	}

	[Serializable]
	public class CollectionsDataExistsException : CollectionsException
	{
		public CollectionsDataExistsException() : base()
		{
		}

		public CollectionsDataExistsException(string Message) : base(Message)
		{
		}

		public CollectionsDataExistsException(string Message, Exception Inner) : base(Message, Inner)
		{
		}

		public CollectionsDataExistsException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x03, "Data Exists", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message)
		{
		}
	}

	[Serializable]
	public class CollectionsDataNotFoundException : CollectionsException
	{
		public CollectionsDataNotFoundException() : base()
		{
		}

		public CollectionsDataNotFoundException(string Message) : base(Message)
		{
		}

		public CollectionsDataNotFoundException(string Message, Exception Inner) : base(Message, Inner)
		{
		}

		public CollectionsDataNotFoundException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x04, "Data Not Found", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message)
		{
		}
	}

	[Serializable]
	public class CollectionsDataNotValidException : CollectionsException
	{
		public CollectionsDataNotValidException() : base()
		{
		}

		public CollectionsDataNotValidException(string Message) : base(Message)
		{
		}

		public CollectionsDataNotValidException(string Message, Exception Inner) : base(Message, Inner)
		{
		}

		public CollectionsDataNotValidException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x05, "Data Not Valid", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message)
		{
		}
	}

	[Serializable]
	public class CollectionsDataOverflowException : CollectionsException
	{
		public CollectionsDataOverflowException() : base()
		{
		}

		public CollectionsDataOverflowException(string Message) : base(Message)
		{
		}

		public CollectionsDataOverflowException(string Message, Exception Inner) : base(Message, Inner)
		{
		}

		public CollectionsDataOverflowException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x06, "Data Overflow", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message)
		{
		}
	}

	[Serializable]
	public class CollectionsDataUnderflowException : CollectionsException
	{
		public CollectionsDataUnderflowException() : base()
		{
		}

		public CollectionsDataUnderflowException(string Message) : base(Message)
		{
		}

		public CollectionsDataUnderflowException(string Message, Exception Inner) : base(Message, Inner)
		{
		}

		public CollectionsDataUnderflowException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x07, "Data Underflow", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message)
		{
		}
	}

	[Serializable]
	public class CollectionsDataOutOfRangeException : CollectionsException
	{
		public CollectionsDataOutOfRangeException() : base()
		{
		}

		public CollectionsDataOutOfRangeException(string Message) : base(Message)
		{
		}

		public CollectionsDataOutOfRangeException(string Message, Exception Inner) : base(Message, Inner)
		{
		}

		public CollectionsDataOutOfRangeException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x08, "Data Out Of Range", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message)
		{
		}
	}
}
