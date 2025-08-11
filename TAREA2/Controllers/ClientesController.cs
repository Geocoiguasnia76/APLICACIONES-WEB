using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tipo_Datos.Data;
using Tipo_Datos.Models.Entidades;

namespace TiposDatos.Controllers
{
    public class ClientesController : Controller
    {
        private readonly DatosDbContext _dbContext;
        public ClientesController(DatosDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var clientesActivos = await _dbContext.Clientes
                .Where(c => c.isDelete == false)
                .ToListAsync();
            return View(clientesActivos);
        }

        public IActionResult Nuevo()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult>
            Nuevo([Bind("Nombres,Email,Telefono,Direccion,Cedula_Ruc")] ClientesModel cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Create_At = DateTime.Now;
                cliente.isDelete = false;
                _dbContext.Add(cliente);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var cliente = await _dbContext.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult>
            Editar([Bind("Id,Nombres,Email,Telefono,Direccion,Cedula_Ruc")] ClientesModel cliente)
        {
            if (ModelState.IsValid)
            {
                var clienteGuardado = await _dbContext.Clientes.FindAsync(cliente.Id);
                if (clienteGuardado == null)
                    return NotFound();
                clienteGuardado.Update_At = DateTime.Now;
                clienteGuardado.Nombres = cliente.Nombres;
                clienteGuardado.Telefono = cliente.Telefono;
                clienteGuardado.Direccion = cliente.Direccion;
                clienteGuardado.Email = cliente.Email;
                clienteGuardado.Cedula_RUC = cliente.Cedula_RUC;
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }


        public async Task<IActionResult> Detalle(int id)
        {
            var cliente = await _dbContext.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }


        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = await _dbContext.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }


        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminacionConfirmada(int id)
        {
            var cliente = await _dbContext.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.isDelete = true;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}


