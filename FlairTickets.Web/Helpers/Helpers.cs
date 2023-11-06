using FlairTickets.Web.Helpers.Interfaces;

namespace FlairTickets.Web.Helpers
{
    public class Helpers : IHelpers
    {
        public Helpers(IConverterHelper converterHelper, IUserHelper userHelper)
        {
            Converter = converterHelper;
            User = userHelper;
        }

        public IConverterHelper Converter { get; }
        public IUserHelper User { get; }
    }
}
