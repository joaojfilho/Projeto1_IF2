using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto1_IF2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto1_IF2.Controllers
{
    [Authorize]
    public class TbProfissionalsController : Controller
    {
        private readonly db_IF2Context _context;

        public TbProfissionalsController(db_IF2Context context)
        {
            _context = context;
        }

        public enum Plano
        {
            MedicoTotal = 7,
            MedicoParcial = 8,
            Nutricionista = 9
        }


        // GET: TbProfissionals
        //[Authorize(Roles = "Medico,Nutricionista,GerenteNutricionista,GerenteMedico")] //Medico ou Nutricionista.
        //Nas duas linhas abaixo, ele teria que ser medico e nutricionista.
        //[Authorize(Roles = "Medico")]
        //[Authorize(Roles = "Nutricionista")]
        //[Authorize(Roles = "GerenteNutricionista")]
        public async Task<IActionResult> Index()
        {
            var userManager = HttpContext.RequestServices.GetService<UserManager<IdentityUser>>();
            var currentUserId = userManager?.GetUserId(User);
            //var db_IF2Context = _context.TbProfissional.Include(t => t.IdCidadeNavigation).
            //    Include(t => t.IdContratoNavigation).ThenInclude(t => t.IdPlanoNavigation).Include(t => t.IdTipoAcessoNavigation);
            //return View(await db_IF2Context.ToListAsync());

            ////Metodo 1
            //var db_IFContext = _context.TbProfissional
            //.Include(t => t.IdCidadeNavigation)
            //.Include(t => t.IdContratoNavigation)
            //.ThenInclude(s => s.IdPlanoNavigation)
            //.Include(t => t.IdTipoAcessoNavigation)
            //.Where(t => t.IdContratoNavigation.IdPlano == 1);

            ////Metodo 2
            //var db_IFContext = (from pro in _context.TbProfissional
            //                    where (Plano)pro.IdContratoNavigation.IdPlano == Plano.MedicoTotal
            //                    select pro)
            //.Include(t => t.IdTipoAcessoNavigation)
            //.Include(t => t.IdCidadeNavigation)
            //.Include(pro => pro.IdContratoNavigation)
            //.ThenInclude(contrato => contrato.IdPlanoNavigation);

            //Metodo 3
            //var db_IFContext = from pro in _context.TbProfissional
            //                   join contrato in _context.TbContrato on pro.IdContrato equals contrato.IdContrato
            //                   join plano in _context.TbPlano on contrato.IdPlano equals plano.IdPlano
            //                   where plano.IdPlano == 1
            //                   select pro;
            if (User.IsInRole("GerenteGeral"))
            {
                var db_IFContext = (from pro in _context.TbProfissional
                                    select new TbProfissionalResumido
                                    {
                                        Nome = pro.Nome,
                                        NomeCidade = pro.IdCidadeNavigation.Nome,
                                        NomePlano = pro.IdContratoNavigation.IdPlanoNavigation.Nome,
                                        IdProfissional = pro.IdProfissional.ToString(),
                                        Cpf = pro.Cpf,
                                        CrmCrn = pro.CrmCrn,
                                        Especialidade = pro.Especialidade,
                                        Logradouro = pro.Logradouro,
                                        Numero = pro.Numero,
                                        Bairro = pro.Bairro,
                                        Cep = pro.Cep,
                                        Ddd1 = pro.Ddd1,
                                        Ddd2 = pro.Ddd2,
                                        Telefone1 = pro.Telefone1,
                                        Telefone2 = pro.Telefone2,
                                        Salario = pro.Salario,
                                    });
                return View(db_IFContext);
            }
            else if (User.IsInRole("GerenteMedico"))
            {
                var db_IFContext = (from pro in _context.TbProfissional
                                    where (Plano)pro.IdContratoNavigation.IdPlano == Plano.MedicoTotal
                                    || (Plano)pro.IdContratoNavigation.IdPlano == Plano.MedicoParcial
                                    select new TbProfissionalResumido
                                    {
                                        Nome = pro.Nome,
                                        NomeCidade = pro.IdCidadeNavigation.Nome,
                                        NomePlano = pro.IdContratoNavigation.IdPlanoNavigation.Nome,
                                        IdProfissional = pro.IdProfissional.ToString(),
                                        Cpf = pro.Cpf,
                                        CrmCrn = pro.CrmCrn,
                                        Especialidade = pro.Especialidade,
                                        Logradouro = pro.Logradouro,
                                        Numero = pro.Numero,
                                        Bairro = pro.Bairro,
                                        Cep = pro.Cep,
                                        Ddd1 = pro.Ddd1,
                                        Ddd2 = pro.Ddd2,
                                        Telefone1 = pro.Telefone1,
                                        Telefone2 = pro.Telefone2,
                                        Salario = pro.Salario,
                                    });
                return View(db_IFContext);
            }
            else if (User.IsInRole("GerenteNutricionista"))
            {
                var db_IFContext = (from pro in _context.TbProfissional
                                    where (Plano)pro.IdContratoNavigation.IdPlano == Plano.Nutricionista
                                    select new TbProfissionalResumido
                                    {
                                        Nome = pro.Nome,
                                        NomeCidade = pro.IdCidadeNavigation.Nome,
                                        NomePlano = pro.IdContratoNavigation.IdPlanoNavigation.Nome,
                                        IdProfissional = pro.IdProfissional.ToString(),
                                        Cpf = pro.Cpf,
                                        CrmCrn = pro.CrmCrn,
                                        Especialidade = pro.Especialidade,
                                        Logradouro = pro.Logradouro,
                                        Numero = pro.Numero,
                                        Bairro = pro.Bairro,
                                        Cep = pro.Cep,
                                        Ddd1 = pro.Ddd1,
                                        Ddd2 = pro.Ddd2,
                                        Telefone1 = pro.Telefone1,
                                        Telefone2 = pro.Telefone2,
                                        Salario = pro.Salario,
                                    });
                return View(db_IFContext);
            }
            else if (User.IsInRole("Medico"))
            {
                // Metodo Otimizado
                var db_IFContext = (from pro in _context.TbProfissional
                                    where (Plano)pro.IdContratoNavigation.IdPlano == Plano.MedicoTotal
                                    && pro.IdUser == currentUserId
                                    select new TbProfissionalResumido
                                    {
                                        Nome = pro.Nome,
                                        NomeCidade = pro.IdCidadeNavigation.Nome,
                                        NomePlano = pro.IdContratoNavigation.IdPlanoNavigation.Nome,
                                        IdProfissional = pro.IdProfissional.ToString(),
                                        Cpf = pro.Cpf,
                                        CrmCrn = pro.CrmCrn,
                                        Especialidade = pro.Especialidade,
                                        Logradouro = pro.Logradouro,
                                        Numero = pro.Numero,
                                        Bairro = pro.Bairro,
                                        Cep = pro.Cep,
                                        Ddd1 = pro.Ddd1,
                                        Ddd2 = pro.Ddd2,
                                        Telefone1 = pro.Telefone1,
                                        Telefone2 = pro.Telefone2,
                                        Salario = pro.Salario,
                                    });


                return View(db_IFContext);
            }else if (User.IsInRole("Nutricionista"))
            {
                var db_IFContext = (from pro in _context.TbProfissional
                                    where (Plano)pro.IdContratoNavigation.IdPlano == Plano.Nutricionista
                                    && pro.IdUser == currentUserId
                                    select new TbProfissionalResumido
                                    {
                                        Nome = pro.Nome,
                                        NomeCidade = pro.IdCidadeNavigation.Nome,
                                        NomePlano = pro.IdContratoNavigation.IdPlanoNavigation.Nome,
                                        IdProfissional = pro.IdProfissional.ToString(),
                                        Cpf = pro.Cpf,
                                        CrmCrn = pro.CrmCrn,
                                        Especialidade = pro.Especialidade,
                                        Logradouro = pro.Logradouro,
                                        Numero = pro.Numero,
                                        Bairro = pro.Bairro,
                                        Cep = pro.Cep,
                                        Ddd1 = pro.Ddd1,
                                        Ddd2 = pro.Ddd2,
                                        Telefone1 = pro.Telefone1,
                                        Telefone2 = pro.Telefone2,
                                        Salario = pro.Salario,
                                    });
                return View(db_IFContext);
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: TbProfissionals/Details/5
        [AllowAnonymous]
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
