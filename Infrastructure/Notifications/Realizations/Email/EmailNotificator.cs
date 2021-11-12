using DAL.Models;
using Infrastructure.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.EmailNotifications
{
    class EmailNotificator : EmailNotificatorBase, INotificator<bool>
    {
        protected readonly Dictionary<RegistrationReason, Func<string, Task<bool>>> _notifyAboutRegistration;
        protected readonly Dictionary<SecurityReason, Func<string, Task<bool>>> _notifyAboutSecurity;
        protected readonly Dictionary<BetReason, Func<string, UsersBets, Task<bool>>> _notifyAboutBet;
        protected readonly Dictionary<TransactionReason, Func<string, Transactions, Task<bool>>> _notifyAboutTransaction;

        public EmailNotificator(EmailSender sender) : base(sender) {

            _notifyAboutRegistration = new Dictionary<RegistrationReason, Func<string, Task<bool>>> {
                { RegistrationReason.Succeeded, base.AboutRegistrationSucceeded}
            };

            _notifyAboutBet = new Dictionary<BetReason, Func<string, UsersBets, Task<bool>>> {
                { BetReason.Applyed, base.AboutBetApplyed },
                { BetReason.Winned, base.AboutBetWinned},
                { BetReason.Loosed, base.AboutBetLoosed }
            };

            _notifyAboutSecurity = new Dictionary<SecurityReason, Func<string, Task<bool>>>
            {
                { SecurityReason.PassportUpdated, base.AboutPassportUpdated},
                { SecurityReason.ProfileUpdated, base.AboutProfileUpdated},
                { SecurityReason.PasswordUpdated, base.AboutPasswordUpdated}
            };

            _notifyAboutTransaction = new Dictionary<TransactionReason, Func<string, Transactions, Task<bool>>>
            {
                { TransactionReason.Passed, base.AboutTransactionPassed }
            };

        }

        public Task<bool> AboutRegistrationAsync(RegistrationReason reason, string email) =>
            _notifyAboutRegistration[reason].Invoke(email);

        public Task<bool> AboutSecurityAsync(SecurityReason reason, string email) =>
            _notifyAboutSecurity[reason].Invoke(email);

        public Task<bool> AboutBetAsync(BetReason reason, string email, UsersBets bet) =>
            _notifyAboutBet[reason].Invoke(email, bet);

        public Task<bool> AboutTransactionAsync(TransactionReason reason, string email, Transactions transaction) =>
            _notifyAboutTransaction[reason].Invoke(email, transaction);
    }
}
