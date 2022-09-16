
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Model;
using ProductionManagement.Services.Services.ProductionLine.Models;

namespace ProductionManagement.Services.Services.ProductionLine
{
    public interface IProductionLineService
    {
        Task<IEnumerable<ProductionLineModel>> GetLinesAsync();

        Task<int> AddLineAsync(ProductionLineModel productionLineModel);

        Task<bool> EditLineAsync(ProductionLineModel productionLineModel);
    }

    public class ProductionLineService : IProductionLineService
    {
        private readonly ProductionManagementContext _context;
        private readonly IMapper _mapper;

        public ProductionLineService(ProductionManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddLineAsync(ProductionLineModel productionLineModel)
        {
            var model = _mapper.Map<Model.DbSets.ProductionLine>(productionLineModel);

            await _context.ProductionLine.AddAsync(model);
            await _context.SaveChangesAsync();

            return model.Id;
        }

        public async Task<bool> EditLineAsync(ProductionLineModel productionLineModel)
        {
            var model = _mapper.Map<Model.DbSets.ProductionLine>(productionLineModel);
            var dbModel = await _context.ProductionLine.Where(x => x.Id == model.Id)
                .Include(x => x.LineTank)
                .FirstOrDefaultAsync();

            if(dbModel == null)
            {
                return false;
            }

            dbModel.Active = model.Active;
            dbModel.Name = model.Name;

            var newParts = model.LineTank.Where(x => x.Id == 0).ToList();

            newParts.ForEach(n =>
            {
                dbModel.LineTank.Add(new Model.DbSets.LineTank
                {
                    TankId = n.TankId,
                });
            });

            var deleted = dbModel.LineTank.Where(x => !model.LineTank.Any(y => y.Id == x.Id)).ToList();

            deleted.ForEach(d =>
            {
                dbModel.LineTank.Remove(d);
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductionLineModel>> GetLinesAsync()
        {
            var result = await _context.ProductionLine
                .Select(x => new ProductionLineModel()
                {
                    Active = x.Active,
                    Id = x.Id,
                    Name = x.Name,
                    Tanks = x.LineTank.Select(y => new LineTankModel()
                    {
                        Id = y.Id,
                        TankId = y.TankId,
                        TankName = y.Tank.Name,
                    }).ToList(),
                })
                .ToListAsync();

            return result;
        }
    }
}
