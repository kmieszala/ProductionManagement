using AutoMapper;
using ProductionManagement.Common.Enums;
using ProductionManagement.Model;

namespace ProductionManagement.Services.Services.Shared.Log;

public interface ILogService
{
    Task AddLogAsync(LogCodeEnum logCode, string desc, int? userId);
}

public class LogService : ILogService
{
    private readonly ProductionManagementContext _context;

    public LogService(ProductionManagementContext context, IMapper mapper)
    {
        _context = context;
    }

    public async Task AddLogAsync(LogCodeEnum logCode, string desc, int? userId)
    {
        Model.DbSets.Log log = new Model.DbSets.Log()
        {
            LogCode = logCode,
            Description = desc,
            CreationDate = DateTime.Now,
            UserId = userId,
        };

        await AddLog(log);
    }

    private async Task AddLog(Model.DbSets.Log log)
    {
        _context.Log.Add(log);
        await _context.SaveChangesAsync();
    }
}