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

            tbPaciente? tbPaciente = await _context.TbPaciente
                .Include(t => t.IdCidadeNavigation)
                .AsNoTracking()
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
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPaciente = await _context.TbPaciente.FirstOrDefaultAsync(p => p.IdPaciente == id);

            if (tbPaciente == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<TbPaciente>(
                tbPaciente,
                "",
                p => p.IdPaciente, p => p.Nome, p => p.Rg, p => p.Cpf,
                p => p.DataNascimento, p => p.NomeResponsavel, p => p.Sexo, p => p.Etnia,
                p => p.Endereco,p => p.Bairro, p => p.IdCidade, p => p.TelResidencial, p => p.TelComercial,
                p => p.TelCelular, p => p.Profissao, p => p.FlgAtleta, p => p.FlgGestante))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Não foi possível salvar as alterações. " +
                        "Tente novamente, e se o problema persistir, " +
                        "consulte o administrador do sistema." + ex.ToString());
                }
            }
            
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "IdCidade", tbPaciente.IdCidade);
            return View(tbPaciente);
        }

        // GET: TbPacientes/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
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
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete falhou. Tente novamente, e se o problema persistir " +
                    "consulte o administrador do sistema.";
            } 

            return View(tbPaciente);
        }

        // POST: TbPacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbPaciente = await _context.TbPaciente.FindAsync(id);
            if (tbPaciente == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try 
            { 
                _context.TbPaciente.Remove(tbPaciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool TbPacienteExists(int id)
        {
            return _context.TbPaciente.Any(e => e.IdPaciente == id);
        }
    }
}
