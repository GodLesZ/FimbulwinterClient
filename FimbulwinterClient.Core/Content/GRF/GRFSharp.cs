namespace FimbulwinterClient.Core.Content.GRF
{
    #region Event Delegates
    public delegate void ExtractCompleteEventHandler(object sender, GRFEventArg e);
    public delegate void FileReadCompleteEventHandler(object sender, GRFEventArg e);
    public delegate void FileAddCompleteEventHandler(object sender, GRFEventArg e);
    public delegate void GRFMetaWriteCompleteEventHandler(object sender);
    public delegate void FileBodyWriteCompleteEventHandler(object sender,GRFEventArg e);
    public delegate void FileTableWriteCompleteEventHandler(object sender, GRFEventArg e);
    public delegate void SaveCompleteEventHandler(object sender);
    public delegate void FileCountReadCompleteEventHandler(object sender);
    public delegate void GRFOpenCompleteEventHandler(object sender);
    #endregion
}

