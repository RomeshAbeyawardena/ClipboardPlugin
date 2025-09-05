namespace ClipboardPlugin.Actions.Copying;

public class CopyActionInvoker(IEnumerable<IAction<CopyAction, ClipboardArguments>> actions) : ActionInvokerBase<CopyAction, ClipboardArguments>(actions);