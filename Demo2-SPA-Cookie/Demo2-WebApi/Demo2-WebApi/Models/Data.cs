using Bogus;
namespace Demo2_WebApi.Models;

public class Data
{
  #region People
  private static readonly List<PersonDto> people;
  private static readonly Faker<PersonDto> faker =
    new Faker<PersonDto>()
        .RuleFor(p => p.FirstName, f => f.Name.FirstName())
        .RuleFor(p => p.LastName, f => f.Name.LastName())
        .RuleFor(p => p.Address, f => f.Address.FullAddress())
        .RuleFor(p => p.Birthday, f => f.Date.Between(new DateTime(1900, 1, 1), DateTime.Now));
  static Data()
  {
    people = faker.Generate(10);
  }

  public static List<PersonDto> People => people;
  #endregion
  private static readonly List<UserInfo> users = new()
  {
    new UserInfo("rade", "Rade", "Adminić", true),
    new UserInfo("ivo", "Ivo", "Korisniković", false)
  };
  public static List<UserInfo> Users => users;
  #region Users

  #endregion
}