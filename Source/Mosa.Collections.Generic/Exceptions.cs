// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Collections.Generic
{
    [System.Serializable]
    public class CollectionsException : Exception
    {
        public DateTime DateTimeStamp { get; }
        public byte ErrorCode { get; }
        public string ErrorText { get; }
        public string SourceCodeFileName { get; }
        public string ClassName { get; }
        public string FunctionName { get; }
        public string ParameterName { get; }

        public CollectionsException() 
        {
            this.DateTimeStamp = DateTime.Now;
        }

        public CollectionsException(string Message) : base(Message) 
        {
            this.DateTimeStamp = DateTime.Now;
        }

        public CollectionsException(string Message, Exception Inner) : base(Message, Inner) 
        {
            this.DateTimeStamp = DateTime.Now;
        }

        protected CollectionsException(System.Runtime.Serialization.SerializationInfo Info, System.Runtime.Serialization.StreamingContext Context) : base(Info, Context)
        {
            this.DateTimeStamp = DateTime.Now;
        }

        public CollectionsException(byte ErrorCode, string ErrorText, string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message): base(Message)
        {
            this.DateTimeStamp = DateTime.Now;
            this.ErrorCode = ErrorCode;
            this.ErrorText = ErrorText;
            this.SourceCodeFileName = SourceCodeFileName;
            this.ClassName = ClassName;
            this.FunctionName = FunctionName;
            this.ParameterName = ParameterName;
        }
    }

    [System.Serializable]
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

        protected CollectionsUnknownErrorException(System.Runtime.Serialization.SerializationInfo Info, System.Runtime.Serialization.StreamingContext Context) : base(Info, Context) 
        { 
        }

        public CollectionsUnknownErrorException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x01, "Unknown Error", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message) 
        { 
        }      
    }

    [System.Serializable]
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

        protected CollectionsDataNullException(System.Runtime.Serialization.SerializationInfo Info, System.Runtime.Serialization.StreamingContext Context) : base(Info, Context) 
        { 
        }

        public CollectionsDataNullException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x02, "Data Is Null", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message) 
        { 
        }
    }

    [System.Serializable]
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
        
        protected CollectionsDataExistsException(System.Runtime.Serialization.SerializationInfo Info, System.Runtime.Serialization.StreamingContext Context) : base(Info, Context) 
        { 
        }

        public CollectionsDataExistsException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x03, "Data Exists", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message) 
        { 
        }
    }

    [System.Serializable]
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

        protected CollectionsDataNotFoundException(System.Runtime.Serialization.SerializationInfo Info, System.Runtime.Serialization.StreamingContext Context) : base(Info, Context) 
        { 
        }

        public CollectionsDataNotFoundException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x04, "Data Not Found", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message) 
        { 
        }
    }

    [System.Serializable]
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

        protected CollectionsDataNotValidException(System.Runtime.Serialization.SerializationInfo Info, System.Runtime.Serialization.StreamingContext Context) : base(Info, Context) 
        { 
        }

        public CollectionsDataNotValidException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x05, "Data Not Valid", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message) 
        { 
        }
    }

    [System.Serializable]
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

        protected CollectionsDataOverflowException(System.Runtime.Serialization.SerializationInfo Info, System.Runtime.Serialization.StreamingContext Context) : base(Info, Context) 
        { 
        }

        public CollectionsDataOverflowException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x06, "Data Overflow", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message) 
        { 
        }
    }

    [System.Serializable]
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

        protected CollectionsDataUnderflowException(System.Runtime.Serialization.SerializationInfo Info, System.Runtime.Serialization.StreamingContext Context) : base(Info, Context) 
        { 
        }

        public CollectionsDataUnderflowException(string SourceCodeFileName, string ClassName, string FunctionName, string ParameterName, string Message) : base(0x07, "Data Underflow", SourceCodeFileName, ClassName, FunctionName, ParameterName, Message) 
        { 
        }
    }
}
