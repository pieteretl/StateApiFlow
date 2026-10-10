namespace StateFlowApi;

public enum MessageState
{
    Reject,
    Received,
    Differ,
    Process,
    Isolate,
    Cancelled,
    Fail,
    Retry,
    Start,
    Success
}