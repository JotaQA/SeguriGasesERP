using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeguriGasesERP.Models;
using System.Web.Security;

namespace SeguriGasesERP.Controllers
{
    [Authorize(Roles="Administrador")]
    public class UsuariosAdminController : Controller
    {
        SeguriGasesEntities db = new SeguriGasesEntities();
        //
        // GET: /UsuariosAdmin/

        public ActionResult Index()
        {
            var userProfiles = db.PerfilUsuarios.OrderBy(g => g.Username);

            return View(userProfiles);
        }

        public ActionResult Create()
        {
            var Perfil = new PerfilUsuario();

            MembershipUserCollection Users = Membership.GetAllUsers();

            MembershipUser[] arr = new MembershipUser[Users.Count];

            Users.CopyTo(arr, 0);

            List<MembershipUser> Usuarios =  arr.ToList();
            
            List<PerfilUsuario> Perfiles = db.PerfilUsuarios.ToList();

            foreach (var item in Perfiles)
            {
                Usuarios.Remove(Membership.GetUser(item.Username));
            }

            ViewBag.Usuarios = Usuarios;

            var Sucursales = db.Sucursales.OrderBy(s => s.Nombre);

            ViewBag.Sucursales = Sucursales;

            return View(Perfil);     
        }
        
        // POST
        [HttpPost]
        public ActionResult Create(PerfilUsuario perfil)
        {
            if (ModelState.IsValid)
            {
                int sucursal = int.Parse(Request.Form["IdSucursal"]);

                var Pfil = perfil;

                var Suc = db.Sucursales.Where(s => s.ID == sucursal).First();

                Pfil.Sucursales = new List<Sucursal>();
                
                Pfil.Sucursales.Add(Suc);

                db.PerfilUsuarios.Add(Pfil);

                db.SaveChanges();

                TryUpdateModel(Pfil);

                Suc.Usuarios.Add(Pfil);

                return RedirectToAction("Index");                
            }
            var Perfil = new PerfilUsuario();

            MembershipUserCollection Users = Membership.GetAllUsers();

            MembershipUser[] arr = new MembershipUser[Users.Count];

            Users.CopyTo(arr, 0);

            List<MembershipUser> Usuarios = arr.ToList();

            List<PerfilUsuario> Perfiles = db.PerfilUsuarios.ToList();

            foreach (var item in Perfiles)
            {
                Usuarios.Remove(Membership.GetUser(item.Username));
            }

            ViewBag.Usuarios = Usuarios;

            var Sucursales = db.Sucursales.OrderBy(s => s.Nombre);

            ViewBag.Sucursales = Sucursales;

            return View(perfil);
        }

        public ActionResult AsignarRol(int id)
        {
            int IdUsuario = id;
            var  user = db.PerfilUsuarios.Find(id);
            List<string> rolesUsuario = Roles.GetRolesForUser(user.Username).ToList();
            List<string> roles = Roles.GetAllRoles().ToList();

            ViewBag.RolesUsuario = rolesUsuario;
            ViewBag.Roles = roles;
            /*Se asigna una sucursal a un usuario!!!*/
            //Recuperamos el usuario
            var User = db.PerfilUsuarios.Find(IdUsuario);

            return View(User);

        }
        [HttpPost]
        public ActionResult AsignarRol(int id, string rol)
        {
            int IdUsuario = id;
            var user = db.PerfilUsuarios.Find(id);
            List<string> rolesUsuario = Roles.GetRolesForUser(user.Username).ToList();
            List<string> roles = Roles.GetAllRoles().ToList();

            ViewBag.RolesUsuario = rolesUsuario;
            ViewBag.Roles = roles;
            /*Se asigna una sucursal a un usuario!!!*/
            //Recuperamos el usuario

            try
            {
                Roles.AddUserToRole(user.Username, rol);
            }
            catch
            {
                return View(user);
            }
            return RedirectToAction("Index");
           
        }
        public ActionResult QuitarRol(int id, string rol)
        {
            var user = db.PerfilUsuarios.Find(id);

            try
            {
                Roles.RemoveUserFromRole(user.Username, rol);
                return RedirectToAction("AsignarRol", "UsuariosAdmin", new { id = id });
            }
            catch
            {
                return RedirectToAction("AsignarRol", "UsuariosAdmin", new { id = id });
            }

        }
        public ActionResult AsignarSucursal(int id)
        {
            int IdUsuario = id;
            /*Se asigna una sucursal a un usuario!!!*/
            //Recuperamos el usuario
            var User = db.PerfilUsuarios.Find(IdUsuario); 
            //Sacamos la ista de sucursales a las que estamos enrolados
            List<Sucursal> SU = User.Sucursales.ToList();
            //Sacamos las sucursales existentes
            List<Sucursal> Sucursales = db.Sucursales.OrderBy(s => s.Nombre).ToList();

            Sucursales = Sucursales.Except(SU).ToList();
                
                //db.Sucursales.Include("Usuarios").Where(s => !s.Usuarios.Where(u => u.Username == HttpContext.User.Identity.Name).Any());
          
            ViewBag.Sucursales = Sucursales;

            return View(User);

        }

        [HttpPost]
        public ActionResult AsignarSucursal()
        {
            int IdUsuario = int.Parse(Request.Form["IdUsuario"]);
            int IdSucursal = int.Parse(Request.Form["IdSucursal"]);

            var User = db.PerfilUsuarios.Find(IdUsuario);
            var Sucursal = db.Sucursales.Find(IdSucursal);

            User.Sucursales.Add(Sucursal);
            Sucursal.Usuarios.Add(User);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        
    }
}
