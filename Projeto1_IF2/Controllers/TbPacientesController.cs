using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto1_IF2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto1_IF2.Controllers
{
    public class TbPacientesController : Controller
    {
        private readonly db_IF2Context _context;

        public TbPacientesController(db_IF2Context context)
        {
            _context = context;
        }

        // GET: TbPacientes
        public async Task<IActionResult> Index()
        {
            var db_IF2Context = _context.TbPaciente.Include(t => t.IdCidadeNavigation);
            return View(await db_IF2Context.ToListAsync());
        }

        // GET: TbPacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPaciente = await _context.TbPaciente
                .Include(t => t.IdCidadeNavigation)
                .FirstOrDefaultAsync(m => m.IdPaciente == id);
            if (tbPaciente == null)
            {
                return NotFound();
            }

            return View(tbPaciente);
        }

        // GET: TbPacientes/Create
        public IActionResult Create()
        {
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome");
            return View();
        }

        // POST: TbPacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPaciente,Nome,Rg,Cpf,DataNascimento,NomeResponsavel,Sexo,Etnia,Endereco,Bairro,IdCidade,TelResidencial,TelComercial,TelCelular,Profissao,FlgAtleta,FlgGestante")] TbPaciente tbPaciente)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(tbPaciente);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            try
            {
                if (ModelState.IsValid)
                {
                    var userManager = HttpContext.RequestServices.GetService<UserManager<IdentityUser>>();
                    if (userManager != null)
                    {
                        var email = User.Identity?.Name;
                        if (email != null)
                        {
                            var user = await userManager.FindByEmailAsync(User.Identity.Name);
                            if (user != null)
                            {
                                tbPaciente.IdPaciente = int.Parse(user.Id);
                                //tbPaciente.IdUser = user.Id;
                            }
                            else
                            {
                                return NotFound("Usuário não encontrado.");

                            }
                        }
                        else
                        {
                            return NotFound("Email do usuário não encontrado.");
                        }
                    }
                    else
                    {
                        return NotFound("UserManager não disponível.");
                    }
                    _context.Add(tbPaciente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException dex)
            {
                ModelState.AddModelError("", "Erro ao Salvar Modelo." + dex.ToString());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro." + ex.ToString());
            }
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbPaciente.IdCidade);
            return View(tbPaciente);
        }

        // GET: TbPacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var tbPaciente = await _context.TbPaciente.FindAsync(id);
            if (tbPaciente == null)
            {
                return NotFound();
            }
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "IdCidade", tbPaciente.IdCidade);
            return View(tbPaciente);
        }

        // POST: TbPacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaciente,Nome,Rg,Cpf,DataNascimento,NomeResponsavel,Sexo,Etnia,Endereco,Bairro,IdCidade,TelResidencial,TelComercial,TelCelular,Profissao,FlgAtleta,FlgGestante")] TbPaciente tbPaciente)
        {
            if (id != tbPaciente.IdPaciente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbPaciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbPacienteExists(tbPaciente.IdPaciente))
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
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "IdCidade", tbPaciente.IdCidade);
            return View(tbPaciente);
        }

        // GET: TbPacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPaciente = await _context.TbPaciente
                .Include(t => t.IdCidadeNavigation)
                .FirstOrDefaultAsync(m => m.IdPaciente == id);
            if (tbPaciente == null)
            {
                return NotFound();
            }

            return View(tbPaciente);
        }

        // POST: TbPacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbPaciente = await _context.TbPaciente.FindAsync(id);
            if (tbPaciente != null)
            {
                _context.TbPaciente.Remove(tbPaciente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbPacienteExists(int id)
        {
            return _context.TbPaciente.Any(e => e.IdPaciente == id);
        }
    }
}
