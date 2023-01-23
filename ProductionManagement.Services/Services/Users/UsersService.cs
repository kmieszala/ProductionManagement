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

        Task<UsersModel> EditUserAsync(UsersModel usersModel);

        Task<LoginUserModel?> ChangePasswordAsync(string userName, string password, string repeatPassword);

        Task<bool> CheckUniqueLoginAsync(string login);

        Task<bool> UnlockUserAsync(int userId, string newPassword);

        Task<bool> DeactiveUserAsync(int userId);

        /// <summary>
        /// User change own password.
        /// </summary>
        /// <param name="changePasswordModel">Old and new passwords.</param>
        /// <returns>False if old password is incorrect.</returns>
        Task<ChangePasswordStatusEnum> ChangePasswordAsync(ChangePasswordModel changePasswordModel);
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
                Password = usersModel.Password!.ComputeSha256Hash(),
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
                .Include(x => x.UserRoles)
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
                Log.Warning($"ChangePasswordAsync - User: {userName}, Pass: {password}, RepeatPassword: {repeatPassword}");
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

        public async Task<bool> CheckUniqueLoginAsync(string login)
        {
            var result = await _context.Users
                .AnyAsync(x => x.Email == login);

            return result;
        }

        public async Task<UsersModel> EditUserAsync(UsersModel usersModel)
        {
            var user = await _context.Users
                .Include(x => x.UserRoles)
                .Where(x => x.Id == usersModel.Id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                Log.Error($"UnlockUserAsync only for existing users. UserId: {usersModel.Id}");
                throw new ProductionManagementException($"UnlockUserAsync: No user for id {usersModel.Id}");
            }

            user.Email = usersModel.Email;
            user.FirstName = usersModel.FirstName;
            user.LastName = usersModel.LastName;

            var rolesToDelete = user.UserRoles.Where(x => !usersModel.Roles.Any(y => y.Id == (int)x.RoleId)).ToList();
            _context.UserRoles.RemoveRange(rolesToDelete);
            var rolesToAdd = usersModel.Roles.Where(x => !user.UserRoles.Any(y => (int)y.RoleId == x.Id)).ToList();
            rolesToAdd.ForEach(x => user.UserRoles.Add(new Model.DbSets.UserRoles() { RoleId = (RolesEnum)x.Id }));

            await _context.SaveChangesAsync();

            return new UsersModel
            {
                Id = usersModel.Id,
                Status = usersModel.Status,
                FirstName = usersModel.FirstName,
                LastName = usersModel.LastName,
                Email = usersModel.Email,
                ActivationDate = user.ActivationDate,
                RegisteredDate = user.RegisteredDate,
                Roles = user.UserRoles.Select(y => new Shared.Models.DictModel()
                {
                    Id = (int)y.RoleId,
                    Value = y.RoleId.ToString(),
                }).ToList(),
            };
        }

        public async Task<bool> UnlockUserAsync(int userId, string newPassword)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                Log.Error($"UnlockUserAsync only for existing users. UserId: {userId}");
                throw new ProductionManagementException($"UnlockUserAsync: No user for id {userId}");
            }

            user.Status = UserStatusEnum.New;
            user.Password = newPassword.ComputeSha256Hash();
            user.TimeBlockCount = 0;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactiveUserAsync(int userId)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                Log.Error($"DeactiveUserAsync only for existing users. UserId: {userId}");
                throw new ProductionManagementException($"DeactiveUserAsync: No user for id {userId}");
            }

            user.Status = UserStatusEnum.TimeBlocked;
            user.TimeBlockCount = 0;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ChangePasswordStatusEnum> ChangePasswordAsync(ChangePasswordModel model)
        {
            var user = await _context.Users
                .Where(x => x.Id == model.UserId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                Log.Error($"ChangePasswordAsync only for existing users. UserId: {model.UserId}");
                throw new ProductionManagementException($"ChangePasswordAsync: No user for id {model.UserId}");
            }

            var result = model switch
            {
                var d when (!user.Password.Equals(d.OldPassword.ComputeSha256Hash())) => ChangePasswordStatusEnum.WrongOldPassword,
                var d when (d.Password.Equals(d.OldPassword)) => ChangePasswordStatusEnum.PasswordEqualOldPass,
                var d when (!d.Password.Equals(d.RepeatPassword) || d.Password.Length < 12) => ChangePasswordStatusEnum.PasswordNotEqualRepetPass,
                _ => ChangePasswordStatusEnum.Ok,
            };

            if (result == ChangePasswordStatusEnum.Ok)
            {
                user.Password = model.Password.ComputeSha256Hash();
                user.TimeBlockCount = 0;

                await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}