using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto1_IF2.Models;

namespace Projeto1_IF2.Controllers
{
    public class TbProfissionalsController : Controller
    {
        private readonly db_IF2Context _context;

        public TbProfissionalsController(db_IF2Context context)
        {
            _context = context;
        }

        // GET: TbProfissionals
        public async Task<IActionResult> Index()
        {
            var db_IF2Context = _context.TbProfissional.Include(t => t.IdCidadeNavigation).Include(t => t.IdContratoNavigation).Include(t => t.IdTipoAcessoNavigation);
            return View(await db_IF2Context.ToListAsync());
        }

        // GET: TbProfissionals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TbProfissional? tbProfissional = await _context.TbProfissional
                .Include(t => t.IdCidadeNavigation)
                .Include(t => t.IdContratoNavigation)
                .Include(t => t.IdTipoAcessoNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdProfissional == id);
            if (tbProfissional == null)
            {
                return NotFound();
            }

            return View(tbProfissional);
        }

        // GET: TbProfissionals/Create
        public IActionResult Create()
        {
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome");
            ViewData["IdPlano"] = new SelectList(_context.TbPlano, "IdPlano", "Nome");
            ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome");
            return View();
        }

        // POST: TbProfissionals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoProfissional,IdTipoAcesso,IdCidade,IdUser,Nome,Cpf,CrmCrn,Especialidade,Logradouro,Numero,Bairro,Cep,Ddd1,Ddd2,Telefone1,Telefone2,Salario")] TbProfissional tbProfissional, [Bind("IdPlano")] TbContrato IdContratoNavigation)
        {
            try
            {
                ModelState.Remove("IdUser");
                ModelState.Remove("IdContrato");
            
                if (ModelState.IsValid)
                {
                    IdContratoNavigation.DataInicio = DateTime.Now;
                    IdContratoNavigation.DataFim = IdContratoNavigation.DataInicio.Value.AddMonths(1);
                    _context.Add(IdContratoNavigation);
                    await _context.SaveChangesAsync();

                    var userManager = HttpContext.RequestServices.GetService<UserManager<IdentityUser>>();
                    if (userManager != null)
                    {
                        var email = User.Identity?.Name;
                        if (email != null)
                        {
                            var user = await userManager.FindByEmailAsync(User.Identity.Name);
                            if (user != null)
                            {
                                tbProfissional.IdUser = user.Id;
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

                    tbProfissional.IdContrato = IdContratoNavigation.IdContrato;
                    _context.Add(tbProfissional);
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
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
            ViewData["IdPlano"] = new SelectList(_context.TbPlano, "IdPlano", "Nome",IdContratoNavigation.IdPlano);
            ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome", tbProfissional.IdTipoAcesso);
            return View(tbProfissional);
        }

        // GET: TbProfissionals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var tbProfissional = await _context.TbProfissional.Include(t => t.IdContratoNavigation).FirstOrDefaultAsync(p => p.IdProfissional == id);
            if (tbProfissional == null)
            {
                return NotFound();
            }
            ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "Nome", tbProfissional.IdCidade);
            ViewData["IdPlano"] = new SelectList(_context.TbPlano, "IdPlano", "Nome", tbProfissional.IdContratoNavigation.IdPlano);
            ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome", tbProfissional.IdTipoAcesso);
            return View(tbProfissional);
        }

        // POST: TbProfissionals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbProfissional = await _context.TbProfissional.Include(t => t.IdContratoNavigation).FirstOrDefaultAsync(p => p.IdProfissional == id);

            if (tbProfissional == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<TbProfissional>(
                tbProfissional,
                "",
                p => p.IdTipoProfissional, p => p.IdTipoAcesso, p => p.IdCidade, p => p.Nome,
                p => p.Cpf, p => p.CrmCrn, p => p.Especialidade, p => p.Logradouro, p => p.Numero,
                p => p.Bairro, p => p.Cep, p => p.Ddd1, p => p.Ddd2,
                p => p.Telefone1, p => p.Telefone2, p => p.Salario))
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
                ViewData["IdCidade"] = new SelectList(_context.TbCidade, "IdCidade", "IdCidade", tbProfissional.IdCidade);
                ViewData["IdContrato"] = new SelectList(_context.TbContrato, "IdContrato", "IdContrato", tbProfissional.IdContrato);
                ViewData["IdTipoAcesso"] = new SelectList(_context.TbTipoAcesso, "IdTipoAcesso", "Nome", tbProfissional.IdTipoAcesso);
                return View(tbProfissional);

            }


        // GET: TbProfissionals/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbProfissional = await _context.TbProfissional
                .Include(t => t.IdCidadeNavigation)
                .ThenInclude(t => t.IdEstadoNavigation)
                .Include(t => t.IdTipoAcessoNavigation)
                .Include(t => t.IdContratoNavigation)
                .ThenInclude(s => s.IdPlanoNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdProfissional == id);
            if (tbProfissional == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete falhou. Tente novamente, e se o problema persistir " +
                    "consulte o administrador do sistema.";
            }

            return View(tbProfissional);
        }

        // POST: TbProfissionals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbProfissional = await _context.TbProfissional.FindAsync(id);
            if (tbProfissional == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try 
            { 
                _context.TbProfissional.Remove(tbProfissional);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool TbProfissionalExists(int id)
        {
            return _context.TbProfissional.Any(e => e.IdProfissional == id);
        }
    }
}
