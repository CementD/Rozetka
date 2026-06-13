using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.Repositories;
using Domain;
using BLL.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BLL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(
            IUserRepository userRepo,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();

            return users.Select(u => new UserReadDto
            {
                Id = u.Id,
                Email = u.Email ?? string.Empty,
                FullName = $"{u.FirstName} {u.LastName}".Trim(),
                PhoneNumber = u.PhoneNumber ?? string.Empty,
                OrdersCount = u.Orders?.Count ?? 0
            });
        }

        public async Task<UserDetailsDto?> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var user = await _userRepo.GetByIdWithOrdersAsync(id);
            if (user == null) return null;

            return new UserDetailsDto
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                Address = user.Address ?? string.Empty,
                CardNumber = user.CardNumber,
                CardExpiry = user.CardExpiry,
                CardCvv = user.CardCvv,
                Orders = user.Orders?.Select(o => new OrderReadDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    UserId = o.UserId,
                    Total = o.OrderItems?.Sum(i => i.UnitPrice * i.Quantity) ?? 0m
                }).ToList() ?? new List<OrderReadDto>()
            };
        }

        public async Task<bool> RegisterAsync(UserRegisterDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };

            user.CardNumber = dto.CardNumber;
            user.CardExpiry = dto.CardExpiry;
            user.CardCvv = dto.CardCvv;

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return true;
            }

            return false;
        }

        public async Task UpdatePaymentInfoAsync(string userId, string? cardNumber, string? cardExpiry, string? cardCvv)
        {
            if (string.IsNullOrEmpty(userId)) return;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;

            user.CardNumber = cardNumber;
            user.CardExpiry = cardExpiry;
            user.CardCvv = cardCvv;

            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> LoginAsync(UserLoginDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var result = await _signInManager.PasswordSignInAsync(
                dto.Email,
                dto.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<bool> IsAdminAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
    }
}