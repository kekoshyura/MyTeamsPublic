namespace MyTeams.Client;

public class UserInput{

    public string UserName {get; set;}
    public string Password { get; set;}

    public UserInput(string userName, string password) {
        UserName = userName;
        Password = password;
    }

    public UserInput() {
        
    }
}
