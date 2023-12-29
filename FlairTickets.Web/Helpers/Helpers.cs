using FlairTickets.Web.Helpers.Interfaces;

namespace FlairTickets.Web.Helpers
{
    public class Helpers : IHelpers
    {
        public Helpers(
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper)
        {
            Blob = blobHelper;
            Converter = converterHelper;
            User = userHelper;
        }

        public IBlobHelper Blob { get; }
        public IConverterHelper Converter { get; }
        public IUserHelper User { get; }
    }
}
