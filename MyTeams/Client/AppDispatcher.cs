using MyTeamsCore.Common;
using System.Collections.Concurrent;
using MyTeamsCore;

namespace MyTeams.Client;

public interface 
IAppDispatcher: IDispatcher {
    void Dispatch(IAppAction action);
    void IDispatcher.Dispatch(IAction action) => Dispatch(action.VerifyType<IAppAction>());
}

public interface 
IAppAction: IAction<App> {
}

public interface 
IAppTranslator<T> {
    IAppAction Translate(IAction<T> scopedAction);
}

public record 
CommandTranslator<T>(Func<IAction<T>, IAppAction> TranslatorFunction) : IAppTranslator<T> {
    public IAppAction Translate(IAction<T> scopedAction) => TranslatorFunction(scopedAction);
}

/// <summary>
/// Use it for simple commands to avoid excess entities
/// </summary>
public record 
AppCommand(Func<App, App> Execute, Func<App, bool>? CanExecute = null, string? Name = null) : IAppAction {
    public App Reduce(App app) => Execute(app);
    public bool CanReduce(App app) => CanExecute?.Invoke(app) ?? true;

     public async Task<App> ReduceAsync(App app) => throw new InvalidOperationException("Use reduce");

    public static AppCommand
    Empty => new AppCommand(app => app);
}

public record 
AppEvent(Func<App, App> Reducer, CancellationToken? CancellationToken = null, string? Name = null) : IAppAction, IAsyncAction<App> {
    public App Reduce(App app) {
        if (CancellationToken is { IsCancellationRequested: true })
            return app;
        var newApp = Reducer(app);
        return CancellationToken is { IsCancellationRequested: true } ? app :newApp;
    }
    public async Task<App> ReduceAsync(App app) => throw new InvalidOperationException("Use reduce");
}

public class 
AppStore : Store<App, IAppAction> {
    public static AppStore Instance {get; set;}
    public AppStore(App initialState) : base(initialState) {}
}

public class 
AppDispatcher: IAppDispatcher {
    public static AppDispatcher Instance {get;set; }

    public AppStore AppStore {get;}
    public App State => AppStore.State;
    
    private readonly Thread _uiThread = Thread.CurrentThread;

    public AppDispatcher(AppStore store) {
        AppStore = store;
    }

    public ConcurrentQueue<IAppAction> DispatchedActionQueue {get;} = new();

    private int _isDispatching = 0;
    public void 
    Dispatch(IAppAction action) {
        if (Interlocked.Exchange(ref _isDispatching , 1) == 0){
            DispatchInUiThread(action); 
            while (DispatchedActionQueue.TryDequeue(out var actionFromQueue)) 
                DispatchInUiThread(actionFromQueue);

            Interlocked.Exchange(ref _isDispatching , 0);
        }
        else
            DispatchedActionQueue.Enqueue(action);
    }

    public async Task
    DispatchInUiThread(IAppAction action) {
        try {
            
            if (!CanDispatch(action))
                return;

            var newState = action.Reduce(State);

            if (newState == State) 
                return;

            AppStore.UpdateState(newState);
            
        }
        catch (Exception exception) {
            #if DEBUG
            throw;      
            #endif
        }
    }

    public bool 
    CanDispatch(IAction action) => action.VerifyType<IAppAction>().CanReduce(state: AppStore.State);
}