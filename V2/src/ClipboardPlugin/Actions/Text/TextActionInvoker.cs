namespace ClipboardPlugin.Actions.Text;

internal class TextActionInvoker(IEnumerable<IAction<TextAction, ClipboardArguments>> actions) : ActionInvokerBase<TextAction, ClipboardArguments>(actions);
