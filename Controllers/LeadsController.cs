using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assignment5.Data;
using assignment5.Models;
using Microsoft.AspNetCore.Authorization;

namespace assignment5.Controllers
{
    


    public class LeadsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeadsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Leads

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUserEmail = User.Identity.Name;

                if (User.IsInRole("Admin"))
                {
                    // Admin can see all users
                    var allUsers = await _context.SalesLead.ToListAsync();
                    return View(allUsers);
                }
                else if (User.IsInRole("Manager"))
                {
                    // Manager can see all users
                    var allUsers = await _context.SalesLead.ToListAsync();
                    return View(allUsers);
                }
                else
                {
                    // Regular user can only view all users
                    var allUsers = await _context.SalesLead.ToListAsync();
                    return View(allUsers);
                }
            }
            else
            {
                return Challenge(); // Redirect unauthenticated users to the login page
            }
        }






        // GET: Leads/Details/5




        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesLeadEntity = await _context.SalesLead
                .FirstOrDefaultAsync(m => m.ID == id);

            if (salesLeadEntity == null)
            {
                return NotFound();
            }

            return View(salesLeadEntity);
        }




        // GET: Leads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Mobile,Email,Source")] UsersInfo salesLeadEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesLeadEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesLeadEntity);
        }

        // GET: Leads/Edit/5
        


        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SalesLead == null)
            {
                return NotFound();
            }

            var userEntity = await _context.SalesLead.FindAsync(id);
            if (userEntity == null)
            {
                return NotFound();
            }
            return View(userEntity);
        }

        // POST: Leads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,Mobile,Email,Source")] UsersInfo salesLeadEntity)
        {
            if (id != salesLeadEntity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesLeadEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesLeadEntityExists(salesLeadEntity.ID))
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
            return View(salesLeadEntity);
        }

        // GET: Leads/Delete/5
        



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SalesLead == null)
            {
                return NotFound();
            }

            var userEntity = await _context.SalesLead.FirstOrDefaultAsync(m => m.ID == id);
            if (userEntity == null)
            {
                return NotFound();
            }

            return View(userEntity);
        }
        // POST: Leads/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SalesLead == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SalesLead'  is null.");
            }
            var salesLeadEntity = await _context.SalesLead.FindAsync(id);
            if (salesLeadEntity != null)
            {
                _context.SalesLead.Remove(salesLeadEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesLeadEntityExists(int id)
        {
          return (_context.SalesLead?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
