using Messenger.Application.Abstractions.Data;
using Messenger.AzureFunctions.Settings;
using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Messenger.AzureFunctions.Functions
{
    public sealed class RemoveExpiredResetPasswordTokensFunction
    {
        private readonly ILogger _logger;
        private readonly IResetPasswordTokenRepository _resetPasswordTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveExpiredResetPasswordTokensFunction(
            ILoggerFactory loggerFactory,
            IResetPasswordTokenRepository resetPasswordTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _logger = loggerFactory.CreateLogger<RemoveExpiredRefreshTokensFunction>();
            _resetPasswordTokenRepository = resetPasswordTokenRepository;
            _unitOfWork = unitOfWork;
        }

        [Function("remove-expired-reset-password-tokens")]
        public async Task Run(
            [TimerTrigger($"%{nameof(TimeTriggerSettings)}:{nameof(TimeTriggerSettings.ResetPasswordTokenCleanupSchedule)}%")] TimerInfo _)
        {
            _logger.LogInformation("Starting remove-expired-reset-password-tokens function: {executionTime}", DateTime.UtcNow);

            var expiredTokens = await _resetPasswordTokenRepository.GetExpiredTokensAsync();

            if (expiredTokens.Any())
            {
                _logger.LogInformation("Found {count} expired reset password tokens to remove.", expiredTokens.Count());

                await _resetPasswordTokenRepository.RemoveAsync(expiredTokens);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Removed {count} expired reset password tokens.", expiredTokens.Count());
            }
            else
            {
                _logger.LogInformation("No expired reset password tokens found to remove.");
            }

            _logger.LogInformation("Finished remove-expired-reset-password-tokens function: {executionTime}", DateTime.UtcNow);
        }
    }
}
