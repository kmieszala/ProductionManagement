using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Model;
using ProductionManagement.Services.Services.Tanks.Models;

namespace ProductionManagement.Services.Services.Tanks
{
    public interface ITanksService
    {
        Task<int> AddTankAsync(TankModel model);

        Task<IEnumerable<TankModel>> GetTanksAsync(bool active);

        Task<bool> EditTankAsync(TankModel model);

        Task<bool> ChangeTankStatusAsync(int model, bool status);
    }

    public class TanksService : ITanksService
    {
        private readonly ProductionManagementContext _context;
        private readonly IMapper _mapper;

        public TanksService(ProductionManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TankModel>> GetTanksAsync(bool active)
        {
            var result = await _context.Tanks
                .Where(x => x.Active == active)
                .Select(x => new TankModel()
                {
                    Id = x.Id,
                    Active = active,
                    Description = x.Description,
                    Name = x.Name,
                    ProductionDays = x.ProductionDays,
                    ProductionLines = x.LineTank.Select(y => new ProductionLineTankModel()
                    {
                        LineTankId = y.Id,
                        ProductionLineId = y.ProductionLineId,
                        ProductionLineName = y.ProductionLine.Name,
                    }).ToList(),
                    Parts = x.TankParts.Select(y => new TankPartsModel()
                    {
                        Id = y.Id,
                        PartsId = y.PartsId,
                        PartsName = y.Parts.Name,
                        PartsNumber = y.PartsNumber,
                    }).ToList()
                }).ToListAsync();

            return result;
        }

        public async Task<int> AddTankAsync(TankModel model)
        {
            var dbModel = _mapper.Map<Model.DbSets.Tanks>(model);
            dbModel.CreationDate = DateTime.Now;

            await _context.Tanks.AddAsync(dbModel);

            await _context.SaveChangesAsync();

            return dbModel.Id;
        }

        public async Task<bool> EditTankAsync(TankModel model)
        {
            var dbModel = await _context.Tanks
                .Include(x => x.TankParts)
                    .ThenInclude(x => x.Parts)
                .Include(x => x.LineTank)
                .Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if(dbModel == null)
            {
                return false;
            }

            dbModel.Description = model.Description;
            dbModel.Name = model.Name;
            dbModel.ProductionDays = model.ProductionDays;
            dbModel.Active = model.Active;

            var deleted = dbModel.TankParts.Where(x => !model.Parts.Any(y => y.PartsId == x.PartsId)).ToList();

            deleted.ForEach(d =>
            {
                dbModel.TankParts.Remove(d);
            });

            model.Parts.ForEach(n =>
            {
                var tmp = dbModel.TankParts.FirstOrDefault(x => x.PartsId == n.PartsId);
                if (tmp != null)
                {
                    tmp.PartsNumber = tmp.PartsNumber;
                }
                else
                {
                    dbModel.TankParts.Add(new Model.DbSets.TankParts
                    {
                        PartsId = n.PartsId,
                        PartsNumber = n.PartsNumber,
                    });
                }
            });

            var deletedLineProd = dbModel.LineTank.Where(x => !model.ProductionLines.Any(y => y.ProductionLineId == x.ProductionLineId)).ToList();

            deletedLineProd.ForEach(d =>
            {
                dbModel.LineTank.Remove(d);
            });

            model.ProductionLines.ForEach(n =>
            {
                var tmp = dbModel.LineTank.FirstOrDefault(x => x.ProductionLineId == n.ProductionLineId);
                if (tmp == null)
                {
                    dbModel.LineTank.Add(new Model.DbSets.LineTank
                    {
                        ProductionLineId = n.ProductionLineId,
                    });
                }
            });

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeTankStatusAsync(int id, bool status)
        {
            var model = await _context.Tanks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                return false;
            }

            model.Active = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
