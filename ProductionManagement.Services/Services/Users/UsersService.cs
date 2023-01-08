using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.Common.Enums;
using ProductionManagement.Common.Exceptions;
using ProductionManagement.Common.Extensions;
using ProductionManagement.Model;
using ProductionManagement.Services.Services.Users.Models;
using Serilog;

namespace ProductionManagement.Services.Services.Users
{
    public interface IUsersService
    {
        Task<IEnumerable<UsersModel>> GetUsersAsync();

        Task<LoginUserModel?> LoginAsync(string userName, string password);

        Task<UsersModel> AddUserAsync(UsersModel usersModel);

        Task<LoginUserModel?> ChangePasswordAsync(string userName, string password, string repeatPassword);
    }

    public class UsersService : IUsersService
    {
        private readonly ProductionManagementContext _context;
        private readonly IMapper _mapper;

        public UsersService(ProductionManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsersModel> AddUserAsync(UsersModel usersModel)
        {
            var dbModel = new ProductionManagement.Model.DbSets.Users()
            {
                Email = usersModel.Email,
                FirstName = usersModel.FirstName,
                LastName = usersModel.LastName,
                Password = usersModel.Password.ComputeSha256Hash(),
                RegisteredDate = DateTime.UtcNow,
                Status = Common.Enums.UserStatusEnum.New,
                UserRoles = usersModel.Roles.Select(x => new Model.DbSets.UserRoles()
                {
                    RoleId = (RolesEnum)x.Id,
                }).ToList(),
            };

            _context.Users.Add(dbModel);
            await _context.SaveChangesAsync();

            usersModel.Id = dbModel.Id;
            usersModel.RegisteredDate = DateTime.UtcNow;
            return usersModel;
        }

        public async Task<LoginUserModel?> LoginAsync(string userName, string password)
        {
            var user = await _context.Users
                .Where(x => x.Email == userName)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }
            else if (user.Password.Equals(password.ComputeSha256Hash()))
            {
                user.TimeBlockCount = 0;
            }
            else
            {
                user.TimeBlockCount += 1;
                user.Status = user.TimeBlockCount >= 3 && (user.Status == UserStatusEnum.Active || user.Status == UserStatusEnum.New)
                    ? UserStatusEnum.TimeBlocked : user.Status;
            }

            await _context.SaveChangesAsync();
            return new LoginUserModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = user.Status,
                UserRoles = user.UserRoles.Select(x => x.RoleId).ToList(),
                TimeBlockCount = user.TimeBlockCount,
            };
        }

        public async Task<LoginUserModel?> ChangePasswordAsync(string userName, string password, string repeatPassword)
        {
            var user = await _context.Users
                .Include(x => x.UserRoles)
                .Where(x => x.Email == userName)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                Log.Error($"ChangePassword only for existing users. Email: {userName}");
                throw new ProductionManagementException($"ChangePasswordAsync: No user for login/email {userName}");
            }
            else if (!password.Equals(repeatPassword) || password.Length < 12)
            {
                Log.Warning($"ChangePasswordAsync - User: {userName}, Pass: {password}, RepeaPass: {repeatPassword}");
                return null;
            }

            user.Password = password.ComputeSha256Hash();
            user.Status = UserStatusEnum.Active;
            user.TimeBlockCount = 0;
            user.ActivationDate = user.ActivationDate.HasValue ? user.ActivationDate : DateTime.UtcNow.Date;

            await _context.SaveChangesAsync();

            return new LoginUserModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = UserStatusEnum.Active,
                UserRoles = user.UserRoles.Select(x => x.RoleId).ToList(),
                TimeBlockCount = 0,
            };
        }

        public async Task<IEnumerable<UsersModel>> GetUsersAsync()
        {
            var result = await _context.Users
                .Where(x => x.Id > 1)
                .Select(x => new UsersModel
                {
                    Id = x.Id,
                    Status = x.Status,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    ActivationDate = x.ActivationDate,
                    RegisteredDate = x.RegisteredDate,
                    Roles = x.UserRoles.Select(y => new Shared.Models.DictModel()
                    {
                        Id = (int)y.RoleId,
                        Value = y.Role.Name,
                    }).ToList(),
                })
                .ToListAsync();

            return result;
        }
    }
}