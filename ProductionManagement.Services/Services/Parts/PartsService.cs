using AutoMapper;
using ProductionManagement.Model;
using ProductionManagement.Services.Services.Parts.Models;

namespace ProductionManagement.Services.Services.Parts
{
    public interface IPartsService
    {
        Task<int> AddPartAsync(PartModel model);
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

    }
}
