namespace System.DirectoryServices.ActiveDirectory;

public delegate bool SyncUpdateCallback(SyncFromAllServersEvent eventType, string? targetServer, string? sourceServer, SyncFromAllServersOperationException? exception);
