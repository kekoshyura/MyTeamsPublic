using MyTeamsCore;

namespace MyTeams.Client;

public static class 
AppDispatcherHelper {

    public static bool 
    CanDispatch(this IAppAction action) => action.CanReduce(AppDispatcher.Instance.State);

    public static void
    Dispatch(this IAppAction action) => AppDispatcher.Instance.Dispatch(action);

}