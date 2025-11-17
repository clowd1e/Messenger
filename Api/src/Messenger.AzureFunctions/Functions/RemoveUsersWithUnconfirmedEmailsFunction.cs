using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Identity;
using Messenger.AzureFunctions.Settings;
using Messenger.Domain.Aggregates.Users;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Messenger.AzureFunctions.Functions
{
    internal sealed class RemoveUsersWithUnconfirmedEmailsFunction
    {
        private readonly ILogger _logger;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RemoveUsersWithUnconfirmedEmailsSettings _settings;

        public RemoveUsersWithUnconfirmedEmailsFunction(
            ILoggerFactory loggerFactory,
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IOptions<RemoveUsersWithUnconfirmedEmailsSettings> settings)
        {
            _logger = loggerFactory.CreateLogger<RemoveUsersWithUnconfirmedEmailsFunction>();
            _identityService = identityService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _settings = settings.Value;
        }

        [Function("remove-users-with-unconfirmed-emails")]
        public async Task Run(
            [TimerTrigger($"%{nameof(TimeTriggerSettings)}:{nameof(TimeTriggerSettings.UsersWithUnconfirmedEmailsCleanupSchedule)}%")] TimerInfo myTimer)
        {
            _logger.LogInformation("Starting remove-users-with-unconfirmed-emails function: {executionTime}", DateTime.UtcNow);

            var registrationCutoffDate = DateTime.UtcNow.AddDays(-_settings.RegistrationLookbackDays);

            var usersWithUnconfirmedEmails = await _userRepository.GetUsersWithUnconfirmedEmailsAsync(registrationCutoffDate);

            if (usersWithUnconfirmedEmails.Any())
            {
                _logger.LogInformation("Found {count} users with unconfirmed emails to remove.", usersWithUnconfirmedEmails.Count());

                var usersIds = usersWithUnconfirmedEmails.Select(u => u.Id).ToList();
                var applicationUsers = await _identityService.GetUsersByIds(usersIds);

                await _identityService.DeleteAsync(applicationUsers);
                await _userRepository.RemoveAsync(usersWithUnconfirmedEmails);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Removed {count} users with unconfirmed emails.", usersWithUnconfirmedEmails.Count());
            }
            else
            {
                _logger.LogInformation("No users with unconfirmed emails found to remove.");
            }

            _logger.LogInformation("Finished remove-users-with-unconfirmed-emails function: {executionTime}", DateTime.UtcNow);
        }
    }
}
