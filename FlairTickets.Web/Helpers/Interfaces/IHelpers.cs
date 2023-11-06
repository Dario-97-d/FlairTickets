namespace FlairTickets.Web.Helpers.Interfaces
{
    public interface IHelpers
    {
        IConverterHelper Converter { get; }
        IUserHelper User { get; }
    }
}