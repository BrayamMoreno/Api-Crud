using Crud7._1.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

namespace Crud7._1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContactosController : ControllerBase
	{
		private readonly ILogger<ContactosController> _logger;
		private readonly ContactoContext _db;

		public ContactosController(ContactoContext db, ILogger<ContactosController> logger){
			_db = db;
			_logger = logger;
		}


		[HttpGet (Name ="AllContatos")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<Contacto>> GetAllContactos() {
			return Ok(_db.Contactos.ToList());
		}

		[HttpGet("Id:int" , Name = "Contato")]
		[ProducesResponseType(StatusCodes.Status200OK)] //Ok
		[ProducesResponseType(StatusCodes.Status400BadRequest)] //Respuesta Negativa
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<Contacto> GetContacto(int id){

			if(id <= 0) 
			{
				return BadRequest();
			}

			var Contacto = _db.Contactos.FirstOrDefault(x => x.Id == id);

			if(Contacto == null){
				return NotFound();
			}
			else{
				return Ok(Contacto);
			}

		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)] //Respuesta Negativa
		public ActionResult<ContactoDto> AddContacto([FromBody] ContactoDto contacto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if(_db.Contactos.FirstOrDefault(x => x.Cedula.ToLower() == contacto.Cedula.ToLower()) != null){
				ModelState.AddModelError("Cedula Existente", "La Cedula Ya Existe");
				return BadRequest(ModelState);
			}

			Contacto Model = new()
			{

				Nombre = contacto.Nombre,
				Apellido = contacto.Apellido,
				Telefono = contacto.Telefono,
				Cedula = contacto.Cedula,
				Email = contacto.Email
			};

			_db.Contactos.Add(Model);
			_db.SaveChanges();

			return NoContent();
		}

		[HttpDelete ("id:int")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult DeleteContacto(int id){

			if(id <= 0)
			{
				return BadRequest();
			}

			var contacto = _db.Contactos.FirstOrDefault(x => x.Id == id);

			if(contacto == null)
			{
				return NotFound();
			}

			_db.Contactos.Remove(contacto);
			_db.SaveChanges();
			return NoContent();
		}

		[HttpPut ("id:int")]
		public ActionResult<ContactoDto> UpdateContacto(int id, [FromBody] ContactoDto contactoDto)
		{
			if (id <= 0)
			{
				return BadRequest();
			}

			var Contacto = _db.Contactos.FirstOrDefault(x => x.Id == id);

			if (Contacto == null)
			{
				return NotFound();
			}

			Contacto.Nombre = contactoDto.Nombre;
			Contacto.Apellido = contactoDto.Apellido;
			Contacto.Telefono = contactoDto.Telefono;
			Contacto.Cedula = contactoDto.Cedula;
			Contacto.Email = contactoDto.Email;

			_db.SaveChanges();

			return NoContent();
		}

	}	
}
