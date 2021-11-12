using System;
using System.Threading.Tasks;
using DAL.Models;
using Infrastructure.Notifications;

namespace Infrastructure.EmailNotifications
{
    public abstract class EmailNotificatorBase:
        IRegistrationNotificator<bool>,
        IBettingNotificator<bool>,
        ISecurityNotificator<bool>,
        ITransactionsNotificator<bool>
    {
        private protected EmailSender _sender;

        public EmailNotificatorBase(EmailSender sender) =>
            _sender = sender;

        public async Task<bool> AboutBetApplyed(string email, UsersBets bet) =>
           await SendNotify(email, $"Your bet from {ToPretty(bet.Time)} on {bet.Bet.Match.Team1} vs {bet.Bet.Match.Team2} for ${bet.Money} x{bet.Coef} ({bet.Bet.Description}) has been applyed!");

        public async Task<bool> AboutBetLoosed(string email, UsersBets bet) =>
           await SendNotify(email, $"Your bet from {ToPretty(bet.Time)} on {bet.Bet.Match.Team1} vs {bet.Bet.Match.Team2} for ${bet.Money} x{bet.Coef} ({bet.Bet.Description})  has been loosed!");

        public async Task<bool> AboutBetWinned(string email, UsersBets bet) =>
           await SendNotify(email, $"Your bet from {ToPretty(bet.Time)}  on {bet.Bet.Match.Team1} vs {bet.Bet.Match.Team2} for ${bet.Money} x{bet.Coef} ({bet.Bet.Description})  has been winned!");

        public async Task<bool> AboutPassportUpdated(string email) =>
           await SendNotify(email, $"Your passport has been updated!");

        public async Task<bool> AboutPasswordUpdated(string email) =>
           await SendNotify(email, $"Your password has been updated!");

        public async Task<bool> AboutProfileUpdated(string email) =>
           await SendNotify(email, $"Your profile has been updated!");

        public async Task<bool> AboutRegistrationSucceeded(string email) =>
           await SendNotify(email, $"Dear {email}, thank you for registration!");

        public async Task<bool> AboutTransactionPassed(string email, Transactions transaction) =>
           await SendNotify(email, $"Your {transaction.Type} transaction from {ToPretty(transaction.Date)} for ${transaction.Money} has been passed!");

        private async Task<bool> SendNotify(string email, string message) =>
            await _sender.SendEmailAsync(email, "Notification", message);

        private string ToPretty(DateTime time) =>
            time.ToString("MMMM dd, yyyy");
    }
}
