namespace System.ServiceModel.Syndication;

public delegate bool TryParseDateTimeCallback(XmlDateTimeData data, out DateTimeOffset dateTimeOffset);
