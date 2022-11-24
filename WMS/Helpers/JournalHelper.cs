using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WMS.Enums;
using WMS.Exceptions;
using WMS.Models;
using WMS.Models.Entities;

namespace WMS.Helpers
{
    public interface IJournalHelper
    {
        Task<bool> CreateJournalAsync(OperationTypeEnum type, string operationTarget, int operationId, string userId);
    }

    public class JournalHelper : IJournalHelper
    {
        private readonly WMSDbContext _dbContext;
        private readonly AuthorizationHandlerContext _context;

        public JournalHelper(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateJournalAsync(OperationTypeEnum type, string operationTarget,  int operationId, string userId)
        {

            var journal = new Journal()
            {
                OperationDate = DateTime.Now,
                OperationType = type.ToString(),
                OperationTarget = operationTarget,
                TargetId = operationId,
                UserId = userId != null ? userId : string.Empty
            };

            await _dbContext.AddAsync(journal);
            var isCreated = await _dbContext.SaveChangesAsync();
            var result  = isCreated > 0 ? true : false;

            if (!result)
                throw new NotFoundException("journal not created");

            return result;
        }

    }
}
