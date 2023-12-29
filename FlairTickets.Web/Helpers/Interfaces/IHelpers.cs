namespace FlairTickets.Web.Helpers.Interfaces
{
    public interface IHelpers
    {
        IBlobHelper Blob { get; }
        IConverterHelper Converter { get; }
        IUserHelper User { get; }
    }
}