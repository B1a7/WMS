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
        void CreateJournal(OperationTypeEnum type, string operationTarget, int operationId, string userId);
    }

    public class JournalHelper : IJournalHelper
    {
        private readonly WMSDbContext _dbContext;
        private readonly AuthorizationHandlerContext _context;

        public JournalHelper(WMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateJournal(OperationTypeEnum type, string operationTarget,  int operationId, string userId)
        {

            var journal = new Journal()
            {
                OperationDate = DateTime.Now,
                OperationType = type.ToString(),
                OperationTarget = operationTarget,
                TargetId = operationId,
                UserId = userId
                };

                _dbContext.Add(journal);
        }

    }
}
