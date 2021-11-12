using DAL.Models;
using System.Threading.Tasks;

namespace Infrastructure.Notifications
{
    public enum RegistrationReason
    {
        Succeeded
    }

    public enum SecurityReason
    {
        PassportUpdated,
        PasswordUpdated,
        ProfileUpdated
    }

    public enum BetReason
    {
        Applyed,
        Winned,
        Loosed
    }

    public enum TransactionReason
    {
        Passed
    }

    public interface INotificator<TResult>
    {
        public Task<TResult> AboutRegistrationAsync(RegistrationReason reson, string email);

        public Task<TResult> AboutSecurityAsync(SecurityReason reson, string email);

        public Task<TResult> AboutBetAsync(BetReason reson, string email, UsersBets bet);

        public Task<TResult> AboutTransactionAsync(TransactionReason reson, string email, Transactions transaction);

    }
}
