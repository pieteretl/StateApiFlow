namespace StateFlowApi;

public enum MessageState
{
    Reject,
    Receive,
    Differ,
    Process,
    Isolate,
    Cancel,
    Fail,
    Retry,
    Start,
    Success,
    Unknown
}