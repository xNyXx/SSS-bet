using DAL.Models;
using System.Threading.Tasks;

namespace Infrastructure.Notifications
{
    public interface IRegistrationNotificator<TResult>
    {
        public Task<TResult> AboutRegistrationSucceeded(string email);
    }

    public interface IBettingNotificator<TResult>
    {
        public Task<TResult> AboutBetApplyed(string email, UsersBets bet);
        public Task<TResult> AboutBetWinned(string email, UsersBets bet);
        public Task<TResult> AboutBetLoosed(string email, UsersBets bet);
    }

    public interface ISecurityNotificator<TResult>
    {
        public Task<TResult> AboutPassportUpdated(string email);
        public Task<TResult> AboutProfileUpdated(string email);
        public Task<TResult> AboutPasswordUpdated(string email);
    }

    public interface ITransactionsNotificator<TResult>
    {
        public Task<TResult> AboutTransactionPassed(string email, Transactions transaction);
    }
}
