namespace StateFlowApi;

public enum MessageState
{
    Reject,
    Receive,
    Defer,
    Send,
    Isolate,
    Cancel,
    Fail,
    Retry,
    Start,
    Finish,
    Unknown
}