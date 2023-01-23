using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Model;
using ProductionManagement.Services.Services.Parts.Models;

namespace ProductionManagement.Services.Services.Parts
{
    public interface IPartsService
    {
        Task<int> AddPartAsync(PartModel model);

        Task<IEnumerable<PartModel>> GetPartsAsync();

        Task<bool> EditPartAsync(PartModel model);
    }

    public class PartsService : IPartsService
    {
        private readonly ProductionManagementContext _context;
        private readonly IMapper _mapper;

        public PartsService(ProductionManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddPartAsync(PartModel model)
        {
            var dbModel = _mapper.Map<Model.DbSets.Parts>(model);

            await _context.Parts.AddAsync(dbModel);

            await _context.SaveChangesAsync();

            return dbModel.Id;
        }

        public async Task<bool> EditPartAsync(PartModel model)
        {
            var dbModel = await _context.Parts.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (dbModel == null)
            {
                return false;
            }

            dbModel.Description = model.Description;
            dbModel.Name = model.Name;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<PartModel>> GetPartsAsync()
        {
            var result = await _context.Parts.Select(x => new PartModel()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
            }).ToListAsync();

            return result;
        }
    }
}