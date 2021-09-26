using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GitHubSearchWebApp.Data;
using GitHubSearchWebApp.Models;
using System.Text.Json;

namespace GitHubSearchWebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email")] IdentityUser user)
        {
            if (ModelState.IsValid)
            {
                await userManager.CreateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email")] IdentityUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpPut("updateRoles")]
        public async Task<IActionResult> Update([FromBody] JsonElement userRoles)
        {
            var id = userRoles.GetProperty("id").GetString();
            var user = userManager.FindByIdAsync(id).GetAwaiter().GetResult();
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine(userRoles.GetProperty("isUser").GetInt32());
                    var isUser = userRoles.GetProperty("isUser").GetInt32();
                    if (userManager.IsInRoleAsync(user, "User").GetAwaiter().GetResult() && isUser == 0)
                    {
                        await userManager.RemoveFromRoleAsync(user, "User");
                    }
                    else if (!userManager.IsInRoleAsync(user, "User").GetAwaiter().GetResult() && isUser == 1)
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }

                    var isTeamLead = userRoles.GetProperty("isTeamLead").GetInt32();
                    Console.WriteLine(userRoles.GetProperty("isTeamLead").GetInt32());
                    if (userManager.IsInRoleAsync(user, "TeamLead").GetAwaiter().GetResult() && isTeamLead == 0)
                    {
                        await userManager.RemoveFromRoleAsync(user, "TeamLead");
                    }
                    else if (!userManager.IsInRoleAsync(user, "TeamLead").GetAwaiter().GetResult() && isTeamLead == 1)
                    {
                        await userManager.AddToRoleAsync(user, "TeamLead");
                    }

                    Console.WriteLine(userRoles.GetProperty("isAdministrator").GetInt32());
                    var isAdministrator = userRoles.GetProperty("isAdministrator").GetInt32();
                    if (userManager.IsInRoleAsync(user, "Administrator").GetAwaiter().GetResult() && isAdministrator == 0)
                    {
                        await userManager.RemoveFromRoleAsync(user, "Administrator");
                    }
                    else if (!userManager.IsInRoleAsync(user, "Administrator").GetAwaiter().GetResult() && isAdministrator == 1)
                    {
                        await userManager.AddToRoleAsync(user, "Administrator");
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok();
            }
            return Ok();
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.Users
               .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            await userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return userManager.Users.Any(e => e.Id == id);
        }
    }
}
