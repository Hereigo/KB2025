```cs
public enum UserRolesEnum
{
    Admin,
    User,
    Manager,
    Guest
}

public class User
{
    public string Name { get; set; } = "Default User";
    public UserRolesEnum UserRole { get; set; }
}

internal class Program
{
    public static void Test()
    {
        List<User> sortedUsers = 
            SomeService.GetUsers()
            .OrderByDescending(user => GetRankByRole(user.UserRole))
            .ThenBy(user => user.Name)
            .ToList();
    }

    private static int GetRankByRole(UserRolesEnum userRole) => userRole switch
    {
        UserRolesEnum.Admin => 4,
        UserRolesEnum.Manager => 3,
        UserRolesEnum.User => 2,
        UserRolesEnum.Guest => 1,
        _ => 0
    };
}
```