
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskList.Data;
using TaskList.Models;

namespace TaskList.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TaskController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            return View(await _context.TaskList.Where(x => x.User.Id == userId).ToListAsync());
        }

     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskViewModel = await _context.TaskList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskViewModel == null)
            {
                return NotFound();
            }

            return View(taskViewModel);
        }

      
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserEmail,TaskDescription,DueDate,IsComplete")] TaskViewModel taskViewModel)
        {
            if (ModelState.IsValid)
            {
                taskViewModel.User = await _userManager.GetUserAsync(User);
                _context.Add(taskViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskViewModel);
        }

     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskViewModel = await _context.TaskList.FindAsync(id);
            if (taskViewModel == null)
            {
                return NotFound();
            }
            return View(taskViewModel);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserEmail,TaskDescription,DueDate,IsComplete")] TaskViewModel taskViewModel)
        {
            if (id != taskViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskViewModelExists(taskViewModel.Id))
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
            return View(taskViewModel);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskViewModel = await _context.TaskList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskViewModel == null)
            {
                return NotFound();
            }

            return View(taskViewModel);
        }

  
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskViewModel = await _context.TaskList.FindAsync(id);
            _context.TaskList.Remove(taskViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskViewModelExists(int id)
        {
            return _context.TaskList.Any(e => e.Id == id);
        }
    }
}
