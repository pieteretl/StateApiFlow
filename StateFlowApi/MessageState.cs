namespace StateFlowApi;

public enum MessageState
{
    Reject,
    Receive,
    Defer,
    Process,
    Isolate,
    Cancel,
    Fail,
    Retry,
    Start,
    Success,
    Unknown
}