namespace TheHossGame.Web.Endpoints;

public abstract record Command
{
  public Guid Id { get; set; }
}