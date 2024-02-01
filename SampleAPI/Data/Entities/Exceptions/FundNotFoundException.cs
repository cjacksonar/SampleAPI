using Data.Entities.Exceptions;

namespace Entities.Exceptions
{
    public sealed class FundNotFoundException : NotFoundException
    {
        public FundNotFoundException(int fundId)
            : base($"The fund with id: {fundId} was not found.")
        {
        }
    }

}
