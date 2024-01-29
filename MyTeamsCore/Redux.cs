namespace MyTeamsCore;

/// <summary>
/// Represents either a command or an action that changes the model
/// </summary>
public interface IAction {}
public interface IEvent {}

public interface ITranslatedAction : IAction{
    public IAction Translate();
}

public interface
IDispatcher {
    void Dispatch(IAction action);
    bool CanDispatch(IAction action);
}

/// <summary>
/// Represents a command or an action that changes the model of type TState
/// </summary>
public interface 
IAction<TState>: IAction {
    TState Reduce(TState state);
    bool CanReduce(TState state) => true;
}

public interface 
IAsyncAction<TState>: IAction {
    TState Reduce(TState state);
    Task<TState> ReduceAsync(TState state);
    bool CanReduce(TState state) => true;
}

public class 
Dispatcher : IDispatcher {
    private readonly Action<IAction> _dispatch;
    public Dispatcher(Action<IAction> dispatch) => _dispatch = dispatch;
    public void Dispatch(IAction action) => _dispatch(action);
    public bool CanDispatch(IAction action) => true;
}

/// <summary>
/// Use it for simple commands to avoid excess entities
/// </summary>
/*public record 
Command<T>(Func<T, T> Execute, Func<T, bool>? CanExecute = null) : IAction<T> {
    public T Reduce(T report) => Execute(report);
    public bool CanReduce(T app) => CanExecute?.Invoke(app) ?? true;

    public static Command<T>
    Empty => new Command<T>(state => state);
}

public record 
Event<T>(Func<T, T> Reduce) : IAction<T> {
    T IAction<T>.Reduce(T @object) => Reduce(@object);
}*/

public record 
DispatchEvent<TState, TCommand> {
    public TState State { get; init; }
    public TCommand Command { get; init; }
    public DateTime DateTime { get; init; }
}

public class 
Store<TState, TAction> : IDisposable {
    public TState State { get; private set; }
    public DateTime InitDate { get; }
    public event Action<TState>? StateChanged; 
    
    public Store(TState initialState) {
        InitDate = DateTime.Now;
        State = initialState;
    }

    public void 
    UpdateState(TState newState) {
        State = newState;
        StateChanged?.Invoke(newState);
    }

    public void Dispose() { }
}

public static class 
ReduxHelper {

    public static void
    Dispatch(this IAction action, IDispatcher dispatcher) {
        if (action is ITranslatedAction translatedAction)
            dispatcher.Dispatch(translatedAction.Translate());
        else
            dispatcher.Dispatch(action);
    }

    public static bool 
    CanDispatch(this IAction command, IDispatcher dispatcher) => dispatcher.CanDispatch(command);

}