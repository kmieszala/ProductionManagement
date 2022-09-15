using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Model;
using ProductionManagement.Services.Services.Tanks.Models;

namespace ProductionManagement.Services.Services.Tanks
{
    public interface ITanksService
    {
        Task<int> AddTankAsync(TankModel model);

        Task<IEnumerable<TankModel>> GetTanksAsync();

        Task<bool> EditTankAsync(TankModel model);
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

        public async Task<IEnumerable<TankModel>> GetTanksAsync()
        {
            var result = await _context.Tanks.Select(x => new TankModel()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                ProductionDays = x.ProductionDays,
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
                .Include(x=> x.TankParts)
                .ThenInclude(x=> x.Parts)
                .Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if(dbModel == null)
            {
                return false;
            }

            dbModel.Description = model.Description;
            dbModel.Name = model.Name;
            dbModel.ProductionDays = model.ProductionDays;
            dbModel.Active = model.Active;

            var newParts = model.Parts.Where(x => x.Id == 0).ToList();

            newParts.ForEach(n =>
            {
                dbModel.TankParts.Add(new Model.DbSets.TankParts
                {
                    PartsId = n.PartsId,
                    PartsNumber = n.PartsNumber,
                });
            });

            var deleted = dbModel.TankParts.Where(x => !model.Parts.Any(y => y.Id == x.Id)).ToList();

            deleted.ForEach(d =>
            {
                dbModel.TankParts.Remove(d);
            });

            var updated = model.Parts.Where(x => x.Id != 0 && !deleted.Any(y => y.Id == x.Id)).ToList();
            updated.ForEach(x =>
            {
                var tmp = dbModel.TankParts.FirstOrDefault(x => x.Id == x.Id);
                if (tmp != null)
                {
                    tmp.PartsNumber = x.PartsNumber;
                }
            });

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
