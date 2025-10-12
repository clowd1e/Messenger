using Messenger.Application.Abstractions.Data;
using Messenger.AzureFunctions.Settings;
using Messenger.Domain.Aggregates.RefreshTokens;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Messenger.AzureFunctions.Functions
{
    public sealed class RemoveExpiredRefreshTokensFunction
    {
        private readonly ILogger _logger;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveExpiredRefreshTokensFunction(
            ILoggerFactory loggerFactory,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _logger = loggerFactory.CreateLogger<RemoveExpiredRefreshTokensFunction>();
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        [Function("remove-expired-refresh-tokens")]
        public async Task Run(
            [TimerTrigger($"%{nameof(TimeTriggerSettings)}:{nameof(TimeTriggerSettings.RefreshTokenCleanupSchedule)}%")] TimerInfo myTimer)
        {
            _logger.LogInformation("Starting remove-expired-refresh-tokens function: {executionTime}", DateTime.UtcNow);

            var expiredTokens = await _refreshTokenRepository.GetExpiredRefreshTokensAsync();

            if (expiredTokens.Any())
            {
                _logger.LogInformation("Found {count} expired refresh tokens to remove.", expiredTokens.Count());

                await _refreshTokenRepository.RemoveAsync(expiredTokens);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Removed {count} expired refresh tokens.", expiredTokens.Count());
            }
            else
            {
                _logger.LogInformation("No expired refresh tokens found to remove.");
            }

            _logger.LogInformation("Finished remove-expired-refresh-tokens function: {executionTime}", DateTime.UtcNow);
        }
    }
}
